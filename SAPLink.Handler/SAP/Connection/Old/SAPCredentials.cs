namespace SAPLink.Handler.SAP.Connection.Old
{
    public class SAPCredentials
    {
        public string ServiceLayerUri { get; private set; }
        public string CompanyDb { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string AuthUserName { get; private set; }
        public string AuthPassword { get; private set; }
        public string Authorization { get; private set; }
        public string Cookie { get; private set; }

        private SAPCredentials(string serviceLayerUri, string companyDb, string userName,
            string password, string authorization)
        {
            ServiceLayerUri = serviceLayerUri;
            CompanyDb = companyDb;
            UserName = userName;
            Password = password;
            AuthUserName = $@"{{""UserName"" : ""{userName}"",""CompanyDB"" : ""{companyDb}""}}";
            AuthPassword = password;
            Authorization = authorization;

            ValidateParameters();
        }

        private void ValidateParameters()
        {
            if (string.IsNullOrEmpty(ServiceLayerUri) || string.IsNullOrEmpty(CompanyDb) ||
                string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(Authorization))
            {
                throw new ArgumentNullException();
            }
        }

        public static SAPCredentials LocalEnvironment()
        {
            return new SAPCredentials(
                "https://Localhost:50000/b1s/v1/",
                "SBODemoGB",
                "manager",
                "manager",
                "Basic eyJVc2VyTmFtZSI6ICJtYW5hZ2VyIiwgIkNvbXBhbnlEQiI6ICJTQk9EZW1vR0IifTptYW5hZ2Vy=");
        }

        public static SAPCredentials TestEnvironment()
        {
            return new SAPCredentials(
                "https://sap-test.alkaffary.com:50000/b1s/v1",
                "TESTDB",
                "manager",
                "Qw123654*",
                "Basic eyJVc2VyTmFtZSI6ICJtYW5hZ2VyIiwgIkNvbXBhbnlEQiI6ICJURVNUREIifTpRdzEyMzY1NCo=");
        }

        public static SAPCredentials ProductionEnvironment()
        {
            return new SAPCredentials("", "", "", "", "");
        }

        public static SAPCredentials Default()
        {
            return TestEnvironment();
        }
    }
}
