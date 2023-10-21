namespace SAPLink.Handler.Prism.Connection.Auth
{
    public class LoginResponse
    {
        public string AuthSession { get; set; }
    }


    public class LoginRequest
    {
        public string Username { get; set; } = "sysadmin";
        public string Password { get; set; } = "sysadmin";
        public bool NoCheck { get; set; }
    }

    public static class AuthService
    {
        private static RestClient client;

        public static LoginResponse Login(string username, string password)
        {
            client = new RestClient(""); // Replace with your API base URL

            // Step 1: Get the Auth-Nonce
            var authNonceRequest = new RestRequest("/v1/rest/auth", Method.GET);
            var authNonceResponse = client.Execute(authNonceRequest);

            if (authNonceResponse.Headers.GetValue("Auth-Nonce", out var authNonceValues))
            {
                string authNonce = authNonceValues.FirstOrDefault();

                if (!string.IsNullOrEmpty(authNonce))
                {
                    // Step 2: Perform the login request
                    var loginRequest = new RestRequest("/v1/rest/auth", Method.POST);
                    loginRequest.AddParameter("Auth-Nonce-Response", authNonce);
                    loginRequest.AddParameter("Auth-Nonce", authNonce);

                    var loginPayload = new LoginRequest
                    {
                        Username = username,
                        Password = password.CreateHash()
                    };

                    loginRequest.AddObject(loginPayload);

                    var loginResponse = client.Execute(loginRequest);

                    if (loginResponse.Headers.GetValue("Auth-Session", out var authSessionValues))
                    {
                        string authSession = authSessionValues.FirstOrDefault();

                        if (!string.IsNullOrEmpty(authSession))
                        {
                            // Step 3: Get the session information
                            client.AddDefaultHeader("Auth-Session", authSession);

                            var sessionRequest = new RestRequest("/v1/rest/session", Method.GET);
                            var sessionResponse = client.Execute(sessionRequest);

                            // Process the sessionResponse as needed
                            // ...

                            return new LoginResponse { AuthSession = authSession };
                        }
                    }
                    else
                    {
                        // Handle login error
                        // ...
                    }
                }
            }

            // Handle login error
            // ...
            return null;
        }

        private static bool GetValue(this IList<Parameter> header, string input, out IList<string> authSessionValues)
        {
            authSessionValues = (IList<string>)header
                .Where(x => x.Name == input)
                .Select(x => x.Value)
                .FirstOrDefault();

            if (authSessionValues.Any())
            {
                return true;
            }
            return false;
        }


    }

}
