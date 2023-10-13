using System.Runtime.InteropServices;
using SAPbobsCOM;
using SAPLink.Core.Utilities;

namespace SAPLink.Handler.SAP.Application
{
    public static class ActionHandler
    {
        public static void AddAlphaField(string tableName, string fieldName, string fieldDescription, int size, string validValues, string validDescriptions,
        string defaultValue, bool isMandatory, bool isSystemTable, string linkedTableName = "")
        {
            AddField(tableName, fieldName, fieldDescription, BoFieldTypes.db_Alpha, size,
                BoFldSubTypes.st_None, validValues, validDescriptions, defaultValue, isMandatory, isSystemTable, linkedTableName);
        }

        public static void AddField(string tableName, string fieldName, string fieldDescription, BoFieldTypes fieldType, int size,
            BoFldSubTypes subType, string validValues, string validDescriptions, string defaultValue, bool isMandatory, bool isSystemTable, string linkedTableName = "")
        {
            var oUserFieldsMd = (UserFieldsMD)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oUserFields);
            try
            {
                if (isSystemTable)
                    oUserFieldsMd.TableName = tableName;
                else
                    oUserFieldsMd.TableName = "@" + tableName;

                if (!IsFieldExist(tableName, fieldName, isSystemTable))
                {
                    GC.Collect();
                    string[] strValue, strDesc;
                    strValue = validValues.Split(Convert.ToChar(","));
                    strDesc = validDescriptions.Split(Convert.ToChar(","));
                    if (strValue.GetLength(0) != strDesc.GetLength(0))
                    {
                        throw new Exception("Invalid Valid Values");
                    }

                    oUserFieldsMd.Name = fieldName;
                    oUserFieldsMd.Description = fieldDescription;
                    oUserFieldsMd.Type = fieldType;

                    if (isMandatory)
                    {
                        oUserFieldsMd.Mandatory = BoYesNoEnum.tYES;
                    }

                    if (fieldType == BoFieldTypes.db_Numeric)
                    {
                        if (size > 11)
                        {
                            oUserFieldsMd.Size = 11;
                            oUserFieldsMd.SubType = subType;
                        }
                    }
                    else
                    {
                        if (linkedTableName.IsHasValue())
                        {
                            oUserFieldsMd.EditSize = 8;
                            oUserFieldsMd.TableName = tableName;
                            oUserFieldsMd.LinkedTable = linkedTableName;
                        }
                        else
                        {
                            oUserFieldsMd.EditSize = size;
                            oUserFieldsMd.SubType = subType;
                        }
                    }


                    if (strValue.Length > 1)
                    {
                        for (var i = 0; i < strValue.GetLength(0); i++)
                        {
                            if (!string.IsNullOrEmpty(strValue[i]) && !string.IsNullOrEmpty(strDesc[i]))
                            {
                                oUserFieldsMd.ValidValues.Value = strValue[i];
                                oUserFieldsMd.ValidValues.Description = strDesc[i];
                                oUserFieldsMd.ValidValues.Add();
                            }
                        }

                        oUserFieldsMd.DefaultValue = defaultValue;
                    }
                    else
                    {
                        if (defaultValue.Length > 0)
                        {
                            oUserFieldsMd.DefaultValue = defaultValue;
                        }
                    }

                    var sErrCode = oUserFieldsMd.Add();
                    if (sErrCode == 0)
                    {
                    }
                    else
                    {
                        var ex = ClientHandler.Company.GetLastErrorDescription();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                // Release the handle to the User Fields
                Marshal.ReleaseComObject(oUserFieldsMd);
                GC.Collect();
            }
        }

        private static bool IsFieldExist(string tableName, string fieldName, bool isSystemTable)
        {
            var oRecordset = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            try
            {
                if (!isSystemTable)
                    tableName = "@" + tableName;

                var query = "SELECT Count(*) FROM CUFD WHERE TableID = '" + tableName + "' AND AliasID = '" + fieldName + "'";
                oRecordset.DoQuery(query);
                var value = oRecordset.Fields.Item(0).Value;
                if (Convert.ToInt16(value) == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                // ClientHandler.Company.GetLastError(out sErrCode, out sErrMsg);
                //ClientHandler.ShowWarningMessage(sErrMsg);
                // SAPbouiCOM.Framework.Application.SetText("Error No: " + e.Message, BoMessageTime.bmt_Short);
            }
            finally
            {
                Marshal.ReleaseComObject(oRecordset);
                GC.Collect();
            }
            return false;
        }
        public static string GetValue(this Fields fields, string fieldName)
            => fields.Item(fieldName).Value.ToString();
    }
}
