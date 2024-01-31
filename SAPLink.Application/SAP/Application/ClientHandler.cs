using SAPbobsCOM;
using SAPLink.Domain.System;

namespace SAPLink.Application.SAP.Application
{
    public static class ClientHandler
    {
        public static Company Company;
        //private static Application Application;


        #region Old Login Client Objects

        //public static bool InitializeClientObjects(Credentials credential, out int errorCode, out string ErrorMessage)
        //{
        //    try
        //    {
        //        //Company = new CompanyClass();
        //        Company = new Company();
        //        //oCompany = (SAPbobsCOM.Company)Application.SBO_Application.Company.GetDICompany();


        //        // The actual database host
        //        // With HANA the single-tenancy port 30015 needs to be provided together with the host (not so with MSSQL)
        //        // When using HANA multi-tenancy the instance is prefixed and the port changed: INSTANCE@DB-HOST:30013
        //        // OR the correct instance port needs to be provided, eg. DB-HOST:30017
        //        Company.Server = credential.Server; //Company.Server = "DB-HOST:30015";
        //        Company.language = BoSuppLangs.ln_English;

        //        // The company database/schema name
        //        // With MSSQL the instance is provided here like: INSTANCE\DB_NAME
        //        Company.CompanyDB = credential.CompanyDb;

        //        // Hell knows why the version needs to be provided for MSSQL...
        //        Company.DbServerType = (BoDataServerTypes)credential.ServerTypes;

        //        // SLDServer is the new LicenseServer, don't forget the port with HANA
        //        // Be aware: use either hostname or IP of SLD everywhere
        //        //Company.SLDServer = "SLD-HOST:40000";
        //        Company.LicenseServer = $"{credential.Server}:30000";
        //        //Company.SLDServer = $"{credential.Server}:40000";


        //        // DB credentials when using MSSQL authentication or HANA
        //        Company.DbUserName = credential.DbUserName; // "SYSTEM";
        //        Company.DbPassword = credential.DbPassword;

        //        // SBO credentials
        //        Company.UserName = credential.UserName; 
        //        Company.Password = credential.Password;
        //        Company.UseTrusted = false;
        //        if (Company.Connect() == 0)
        //        {
        //            errorCode = 0;
        //            ErrorMessage = null;
        //            return true;
        //        }

        //        Company.GetLastError(out errorCode, out ErrorMessage);
        //    }
        //    catch
        //    {
        //        Company.GetLastError(out errorCode, out ErrorMessage);
        //    }
        //    return false;
        //}

        #endregion


        public static bool InitializeClientObjects(Clients client, out int errorCode, out string ErrorMessage)
        {
            try
            {
                var credential = client.Credentials.FirstOrDefault();

                Company = new Company
                {
                    DbServerType = (BoDataServerTypes)credential.ServerTypes,
                    DbUserName = credential.DbUserName,
                    DbPassword = credential.DbPassword,
                    CompanyDB = credential.CompanyDb,
                    UserName = credential.UserName,
                    Password = credential.Password,
                    Server = credential.Server,
                    language = BoSuppLangs.ln_English,
                    UseTrusted = false
                };

                if (Company.Connect() == 0)
                {
                    ErrorMessage = "Connected to " + Company.CompanyName;
                    errorCode = 0;
                    return true;
                }

                Company.GetLastError(out errorCode, out ErrorMessage);
            }
            catch
            {
                Company.GetLastError(out errorCode, out ErrorMessage);
            }
            return false;
        }

        public static void AddPrismFieldsToSAP()
        {
            try
            {
                ActionHandler.AddAlphaField("OITM", "SyncToPrism", "Prism Synced", 1, "Y,N", "Yes,No", "N", false, true);
                ActionHandler.AddAlphaField("OITB", "SyncToPrism", "Prism Synced", 1, "Y,N", "Yes,No", "N", false, true);
                ActionHandler.AddAlphaField("OCRD", "SyncToPrism", "Prism Synced", 1, "Y,N", "Yes,No", "N", false, true);
                ActionHandler.AddAlphaField("OINV", "SyncToPrism", "Prism Synced", 1, "Y,N", "Yes,No", "N", false, true);
                ActionHandler.AddAlphaField("OIQR", "SyncToPrism", "Prism Synced", 1, "Y,N", "Yes,No", "N", false, true);

                ActionHandler.AddAlphaField("OINV", "PrismSid", "Prism Doc No.", 200, "", "", "", false, true);
                ActionHandler.AddAlphaField("OIQR", "PrismSid", "Prism Doc No.", 200, "", "", "", false, true);
                ActionHandler.AddField("PDN1", "QTY", "Quntity", BoFieldTypes.db_Float, 20, BoFldSubTypes.st_Quantity, "", "", "", false, true, "");


                ActionHandler.AddAlphaField("OITM", "Active", "Prism Active", 1, "Y,N", "Yes,No", "N", false, true);
                ActionHandler.AddAlphaField("OITM", "ColorGrpN", "ColorGroup", 30, "", "", "", false, true);
                ActionHandler.AddAlphaField("OITM", "DesigGrpN", "DesigGroupName", 30, "", "", "", false, true);
                ActionHandler.AddAlphaField("OITM", "Size", "Size", 50, "", "", "", false, true);
                ActionHandler.AddAlphaField("OITM", "ProdGrpN", "ProductGroupName", 50, "", "", "", false, true);
                ActionHandler.AddAlphaField("OITM", "OrignGrpN", "OrignGroupName", 30, "", "", "", false, true);
                ActionHandler.AddAlphaField("OITM", "OrignGrpN", "OrignGroupName", 30, "", "", "", false, true);
                ActionHandler.AddAlphaField("OITM", "Sticker", "Sticker", 100, "", "", "", false, true);
                ActionHandler.AddAlphaField("OITM", "StickerF", "StickerForeign", 100, "", "", "", false, true);
                ActionHandler.AddAlphaField("OITM", "TypeGrpN", "TypeGroupName", 50, "", "", "", false, true);
                
                ActionHandler.AddAlphaField("OCRD", "PrismStoreCode", "Prism Store Code", 50, "", "", "", false, true);
                
                ActionHandler.AddAlphaField("OPDN", "WhsCode", "Warehouse Code", 50, "", "", "", false, true);
                ActionHandler.AddAlphaField("OINV", "EmpID", "Employee ID", 100, "", "", "", false, true);
            }
            catch (Exception ex)
            {
            }
        }


        public static string GetAccountCode(string FormatCode)
        {
            var query = $"SELECT T0.[AcctCode], T0.[AcctName], T0.[FormatCode] FROM OACT T0 WHERE T0.[FormatCode] = '{FormatCode}'";

            var oRecordSet = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            oRecordSet.DoQuery(query);
            return oRecordSet.Fields.Item("AcctCode").Value.ToString();
        }

        public static string GetFieldValueByQuery(string query)
        {
            var oRecordSet = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            oRecordSet.DoQuery(query);
            return oRecordSet.Fields.Item(0).Value.ToString();
        }
    }
}
