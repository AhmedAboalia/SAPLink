namespace PrismSync.Handler.Connection.Old
{
    public class PrismCredentials
    {
        public string Uri { get; private set; }
        public string BaseUri { get; private set; }
        public string AuthSession { get; private set; }
        public string Origin { get; private set; }
        public string Referer { get; private set; }

        private PrismCredentials(string uri, string baseUri, string authSession, string origin, string referer)
        {
            Uri = uri;
            BaseUri = baseUri;
            AuthSession = authSession;
            Origin = origin;
            Referer = referer;

            ValidateRequiredFields();
        }

        private void ValidateRequiredFields()
        {
            if (string.IsNullOrEmpty(Uri) || string.IsNullOrEmpty(AuthSession) || string.IsNullOrEmpty(Origin))
            {
                throw new ArgumentNullException();
            }
        }

        public static PrismCredentials Default()
        {
            return PublicEnvironment();
        }

        public static PrismCredentials PublicEnvironment()
        {
            return new PrismCredentials(
                "http://194.163.155.105/api/backoffice",
                "http://194.163.155.105",
                //"38B533E4FC554E529AFF6DCC0C01D5B2",
                "3AE6391AFFA54F9EAE8175C78FDC01D8",
                "http://194.163.155.105",
                "http://194.163.155.105/prism.shtml");
        }

        public static PrismCredentials TestEnvironment()
        {
            return new PrismCredentials(
                "http://postest.alkaffary.com:8080/api/backoffice",
                "http://postest.alkaffary.com:8080",
                "FA49855537C54DA1BD6C9EC89511E538",
                "http://postest.alkaffary.com:8080",
                "http://postest.alkaffary.com:8080/prism.shtml");
        }

        public static PrismCredentials ProductionEnvironment()
        {
            return new PrismCredentials(
                "http://postest.alkaffary.com:8080/api/backoffice",
                "http://postest.alkaffary.com:8080",
                "FA49855537C54DA1BD6C9EC89511E538",
                "http://postest.alkaffary.com:8080",
                "http://postest.alkaffary.com:8080/prism.shtml");
        }
    }
}
