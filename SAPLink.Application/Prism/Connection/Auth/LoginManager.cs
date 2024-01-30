using System.Security.Cryptography;
using System.Text;
using RestSharp.Serializers.SystemTextJson;

namespace SAPLink.Handler.Prism.Connection.Auth
{
    public static class LoginManager
    {
        private static RestClient client = new RestClient();

        public static async Task<string> GetAuthSessionAsync(string baseUrl, string userName, string password)
        {
            client.BaseUrl = new Uri(baseUrl);
            return await AuthAsync(userName, password);
        }

        public static async Task<string> AuthAsync(string userName, string password)
        {
            var request = new RestRequest("/v1/rest/auth", Method.GET);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json, version=2");
            request.AddHeader("Accept-Language", "en-US,en-SA;q=0.9");

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                string authNonce = GetValue(response.Headers, "Auth-Nonce");
                if (!string.IsNullOrEmpty(authNonce))
                {
                    string authNonceResponse = (double.Parse((double.Parse(authNonce) / 13).ToString("F0")) % 99999 * 17).ToString("F0");
                    return await AuthWithUserAndPasswordAsync(authNonce, authNonceResponse, userName, password);
                }
            }

            return null;
            //throw new Exception("Authentication failed.");
        }

        public static string GetValue(IList<Parameter> header, string input)
        {
            return (string)header
                .Where(x => x.Name == input)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        private static async Task<string> AuthWithUserAndPasswordAsync(string authNonce, string authNonceResponse, string user, string password)
        {
            var hashedPassword = password.CreateHash();
            var resource = $"?pwd={hashedPassword}&usr={user}";

            var request = new RestRequest($"/v1/rest/auth{resource}", Method.GET);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json, version=2");
            request.AddHeader("Auth-Nonce", authNonce);
            request.AddHeader("Auth-Nonce-Response", authNonceResponse);
            request.AddHeader("Accept-Language", "en-US,en-SA;q=0.9");

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                string authSession = GetValue(response.Headers, "Auth-Session");

                if (string.IsNullOrEmpty(authSession))
                {
                    return await AuthAsync(user, password);
                }
                else
                {
                    return await GetThirdSession(await GetSecondSession(await GetFirstSessionAsync(authSession)));
                }
            }
            return null;
            //throw new Exception("Authentication failed.");
        }

        private static async Task<string> GetFirstSessionAsync(string session)
        {
            return await GetSessionAsync("/v1/rest/session", session);
        }

        private static async Task<string> GetSecondSession(string session)
        {
            return await GetSessionAsync("/v1/rest/sit", session, new Parameter("ws", "webclient", ParameterType.QueryString));
        }

        private static async Task<string> GetThirdSession(string session)
        {
            return await GetSessionAsync("/v1/rest/session", session);
        }

        private static async Task<string> GetSessionAsync(string resource, string session, Parameter queryParam = null)
        {
            var request = new RestRequest(resource, Method.GET);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json, version=2");
            request.AddHeader("Auth-Session", session);
            request.AddHeader("Accept-Language", "en-US,en-SA;q=0.9");

            if (queryParam != null)
                request.AddParameter(queryParam);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return session;
            }

            return null;
            //throw new Exception("Failed to establish session.");
        }
    }

    public static class Md5HashGenerator
    {
        public static string CreateHash(this string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
