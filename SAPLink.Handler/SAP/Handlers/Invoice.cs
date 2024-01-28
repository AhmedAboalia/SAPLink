using SAPbobsCOM;
using SAPLink.Core;
using SAPLink.Core.Models;
using SAPLink.Core.Models.SAP;
using SAPLink.Core.Models.SAP.Sales;
using SAPLink.Core.Utilities;
using SAPLink.Handler.Connection;
using SAPLink.Handler.SAP.Application;
using SAPLink.Handler.SAP.Connection;
using ServiceLayerHelper.RefranceModels;
using static SAPLink.Core.Enums;
using DocumentLine = SAPLink.Core.Models.SAP.Sales.DocumentLine;
using PrismInvoice = SAPLink.Core.Models.Prism.Sales.Invoice;
using SAPInvoice = SAPLink.Core.Models.SAP.Sales.Invoice;
using TaxCodes = SAPLink.Core.Models.SAP.Sales.TaxCodes;

namespace SAPLink.Handler.SAP.Handlers;

partial class ServiceLayerHandler 
{
    public RequestResult<SAPInvoice> AddSalesInvoice(PrismInvoice PrismInvoice, string customerCode, string series, Enums.UpdateType updateType)
    {
        RequestResult<SAPInvoice> result = new RequestResult<SAPInvoice>();

        try
        {
            if (PrismInvoice.Items.Count > 0)
            {

                var taxCodes = GetTaxCodes();

                var body = CreateBody(PrismInvoice, taxCodes, customerCode, series, updateType);


                result.Response.Response = SAPHttpClientFactory.Initialize("Invoices", Method.POST, _client, LoginModel.LoginTypes.Basic, null, body);

                if (result.Response.Response.StatusCode == HttpStatusCode.OK || result.Response.Response.StatusCode == HttpStatusCode.Created)
                {
                    var invoice = JsonConvert.DeserializeObject<SAPInvoice>(result.Response.Response.Content);

                    if (invoice != null && invoice != null)
                    {
                        result.EntityList.Add(invoice);
                        result.Status = Enums.StatusType.Success;
                        result.Message = $"Invoice ID: {invoice.DocNum} is added.";
                    }

                    return result;
                }
                if (result.Response.StatusCode == HttpStatusCode.BadRequest || result.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    var message = "";
                    result.Status = Enums.StatusType.NotFound;
                    if (result.Response.Content.Contains("To generate this document, first define the numbering series in the Administration module"))
                        message = "Can`t find document series, Please make sure you define Whs series in Street/PO Box that Linked to document numbering (Series Name)";
                    else
                    {
                        ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(result.Response.Content);

                        message = errorResponse.Error.Message.Value;
                    }


                    result.Message = $"\r\nCan`t Add Invoice - Prism Invoice ({PrismInvoice.DocumentNumber})" +
                              $"\r\n\r\nErrors:" +
                              $"\r\n{message}" +
                              $"\r\n\r\nRequest Body:" +
                              $"\r\n{body}";

                    return result;
                }
              
            }
        }
        catch (Exception e)
        {
            {
                result.Status = Enums.StatusType.Failed;
                result.Message = $"Invoice ID: {PrismInvoice.DocumentNumber} is not added. - Exception: {e.Message}";
            }
        }
        //result.Status = Enums.StatusType.NotFound;
        //result.Message = $"Invoice ID: {PrismInvoice.DocumentNumber} is not found. \r\n {body}";

        return result;
    }
    public RequestResult<SAPInvoice> AddReturnInvoice(PrismInvoice PrismInvoice, string code, string series, Enums.UpdateType updateType)
    {
        RequestResult<SAPInvoice> result = new RequestResult<SAPInvoice>();

        try
        {
            if (PrismInvoice.Items.Count > 0)
            {
                var taxCodes = GetTaxCodes();


                var body = CreateReturnBody(PrismInvoice, taxCodes, code, series, updateType);


                result.Response.Response = SAPHttpClientFactory.Initialize("CreditNotes", Method.POST, _client, LoginModel.LoginTypes.Basic, null, body);

                if (result.Response.Response.StatusCode == HttpStatusCode.OK || result.Response.Response.StatusCode == HttpStatusCode.Created)
                {
                    var invoice = JsonConvert.DeserializeObject<SAPInvoice>(result.Response.Response.Content);

                    if (invoice != null && invoice != null)
                    {
                        result.EntityList.Add(invoice);
                        result.Status = Enums.StatusType.Success;
                        result.Message = $"Return Invoice ID: {invoice.DocNum} is added with Total {invoice.DocTotal}.";
                    }

                    return result;
                }
                if (result.Response.StatusCode == HttpStatusCode.BadRequest || result.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    var message = "";
                    result.Status = Enums.StatusType.NotFound;
                    if (result.Response.Content.Contains("To generate this document, first define the numbering series in the Administration module"))
                        message = "Can`t find document series, Please make sure you define Whs series in Street/PO Box that Linked to document numbring (Series Name)";
                    else
                        message = result.Response.Content;

                    result.Message = $"\r\nCan`t Add Return Invoice - Prism Invoice ({PrismInvoice.DocumentNumber})" +
                              $"\r\n\r\nErrors in Response Content:" +
                              $"\r\n{message}" +
                              $"\r\n\r\nRequest Body:" +
                              $"\r\n{body}";
                }
            }
        }
        catch (Exception e)
        {
            {
                result.Status = Enums.StatusType.Failed;
                if (code.IsNullOrEmpty() || e.Message.Contains("Index and length must refer to a location within the string. (Parameter 'length')"))
                    result.Message = "Business Partner Code is null or empty. Check Whs Code in Business Partner Master data 'U_PrismStoreCode'.";
                else
                    result.Message += $"Return Invoice ID: {PrismInvoice.DocumentNumber} is not added. - Exception: {e.Message}";

            }
        }
        //result.Status = Enums.StatusType.NotFound;
        //result.Message = $"Invoice ID: {PrismInvoice.DocumentNumber} is not found. \r\n {body}";

        return result;
    }
    public RequestResult<SAPInvoice> AddDownPayment(PrismInvoice PrismInvoice, string customerCode, string series)
    {
        RequestResult<SAPInvoice> result = new RequestResult<SAPInvoice>();

        try
        {
            if (PrismInvoice.Items.Count > 0)
            {
                var taxCodes = GetTaxCodes();

                var body = CreateDownPaymentBody(PrismInvoice, taxCodes, customerCode, series);


                result.Response.Response = SAPHttpClientFactory.Initialize("DownPayments", Method.POST, _client, LoginModel.LoginTypes.Basic, null, body);

                if (result.Response.Response.StatusCode == HttpStatusCode.OK || result.Response.Response.StatusCode == HttpStatusCode.Created)
                {
                    var invoice = JsonConvert.DeserializeObject<SAPInvoice>(result.Response.Response.Content);

                    if (invoice != null && invoice != null)
                    {
                        result.EntityList.Add(invoice);
                        result.Status = Enums.StatusType.Success;
                        result.Message = $"A/R Down Payment ID: {invoice.DocNum} is added.";
                    }

                    return result;
                }
                if (result.Response.StatusCode == HttpStatusCode.BadRequest || result.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    var message = "";
                    result.Status = Enums.StatusType.NotFound;
                    if (result.Response.Content.Contains("To generate this document, first define the numbering series in the Administration module"))
                        message = "Can`t find document series, Please make sure you define Whs series in Street/PO Box that Linked to document numbring (Series Name)";
                    else
                        message = result.Response.Content;

                    result.Message = $"\r\nCan`t Add A/R Down Payment - Prism Invoice ({PrismInvoice.DocumentNumber})" +
                              $"\r\n\r\nErrors in Response Content:" +
                              $"\r\n{message}" +
                              $"\r\n\r\nRequest Body:" +
                              $"\r\n{body}";
                }

            }
        }
        catch (Exception e)
        {
            {
                result.Status = Enums.StatusType.Failed;
                result.Message = $"Invoice ID: {PrismInvoice.DocumentNumber} is not added. - Exception: {e.Message}";
            }
        }
        //result.Status = Enums.StatusType.NotFound;
        //result.Message = $"Invoice ID: {PrismInvoice.DocumentNumber} is not found. \r\n {body}";

        return result;
    }

    public static string CreateBody(PrismInvoice invoice, List<TaxCodes> taxCodes, string customerCode, string series, UpdateType updateType)
    {

        var lines = new List<DocumentLine>();

        foreach (var item in invoice.Items)
        {
            var vatGroup = "X0";
            try { vatGroup = taxCodes.FirstOrDefault(x => x.Rate == -item.TaxPercent).Code; }
            catch (Exception e) { }

            
            string region, city, branch;

            if (updateType == UpdateType.SyncWholesale) 
            {
                region = GetRegion(customerCode);
                //if (region.IsHasValue())
                //    region = region.Substring(1, 1);

                city = GetCity(customerCode);
                //if (city.IsHasValue())
                //    city = city.Substring(1, 3);

                branch = "101003";
            }
            else if (updateType == UpdateType.SyncWholesaleRetail)
            {
                var WholesaleRetailCustomerCode = ActionHandler.GetStringValueByQuery($"SELECT T0.[AddID] FROM OCRD T0 WHERE T0.[CardCode] = '{customerCode}'");//invoice.WholesaleCustomerCode;


                region = WholesaleRetailCustomerCode.Substring(0, 1);
                city = WholesaleRetailCustomerCode.Substring(0, 3);
                branch = WholesaleRetailCustomerCode;
            }
            else
            {
                 region = customerCode.Substring(0, 1);
                 city = customerCode.Substring(0, 3);
                 branch = customerCode;
            }

            var line = new DocumentLine
            {
                ItemCode = item.Alu,
                Quantity = item.Quantity,
                VatGroup = vatGroup,
                UnitPrice = item.Price,

                CostCenter = GetCostCenter(item.Alu),

                Region = region,
                City = city,
                Branch = branch,
                //line.DiscountPercent = ((item.OriginalPrice - item.Price) / item.OriginalPrice) * 100;
                //line.DiscountPercent = item.TotalDiscountPercent;
                WarehouseCode = invoice.StoreCode
            };

            lines.Add(line);
        }


        var ShippingTaxCode = "X0";
        var FeeTaxCode = "X0";

        try { ShippingTaxCode = taxCodes.FirstOrDefault(x => x.Rate == -invoice.ShippingTaxPercentage).Code; }
        catch (Exception e) { }

        try { FeeTaxCode = taxCodes.FirstOrDefault(x => x.Rate == -invoice.FeeTaxPrecentage).Code; }
        catch (Exception e) { }

        var arInvoice = new Invoice
        {
            CardCode = customerCode,
            DocDueDate = invoice.CreatedDatetime,
            DocDate = invoice.CreatedDatetime,
            TaxDate = invoice.CreatedDatetime,
            //SalesEmployee = invoice.SalesEmpolyee,
            SalesEmployeeUDF = invoice.SalesEmpolyee,
            SyncdToSAP = "Y",
            PrismSid = invoice.Sid,
            Series = series,
            Remarks = $"Prism Transaction: {invoice.DocumentNumber} - Branch ({invoice.StoreCode} - {invoice.StoreName}) - Created By: {invoice.CreatedBy}",
            DocumentLines = lines,
            //InvoiceType = "جديده",
            DocumentAdditionalExpenses = new List<Core.Models.SAP.Sales.DocumentAdditionalExpense>
                {
                    new()
                    {
                        ExpenseCode = 1,
                        VatGroup = FeeTaxCode,
                        LineGross = invoice.FeeAmount,
                        Remarks = "Fee"
                    },
                    new()
                    {
                        ExpenseCode = 2,
                        VatGroup = ShippingTaxCode,
                        LineGross = invoice.TransactionTotalShippingAmtWithTax,
                        Remarks = "Shipping"
                    }
                }
        };

        return arInvoice.ToJson();
    }

    public static string CreateReturnBody(Core.Models.Prism.Sales.Invoice invoice, List<TaxCodes> taxCodes, string customerCode, string series, UpdateType updateType)
    {

        var lines = new List<DocumentLine>();

        foreach (var item in invoice.Items)
        {
            var vatGroup = "X0";
            try
            {
                vatGroup = taxCodes.FirstOrDefault(x => x.Rate == -item.TaxPercent).Code;
            }
            catch (Exception e)
            {
                //
            }

            string region, city, branch;

            if (updateType == UpdateType.SyncWholesale ||
                updateType == UpdateType.SyncWholesaleRetail
                )
            {
                var WholesaleCustomerCode = ActionHandler.GetStringValueByQuery($"SELECT T0.[AddID] FROM OCRD T0 WHERE T0.[CardCode] = '{customerCode}'");//invoice.WholesaleCustomerCode;

                region = WholesaleCustomerCode.Substring(0, 1);
                city = WholesaleCustomerCode.Substring(0, 3);
                branch = WholesaleCustomerCode;
            }
            else
            {
                region = customerCode.Substring(0, 1);
                city = customerCode.Substring(0, 3);
                branch = customerCode;
            }

            var line = new DocumentLine
            {
                ItemCode = item.Alu,
                Quantity = item.Quantity,
                VatGroup = vatGroup,
                UnitPrice = item.Price,

                CostCenter = GetCostCenter(item.Alu),

                Region = region,
                City = city,
                Branch = branch,

                //CostCenter = GetCostCenter(item.Alu),
                //Region = customerCode.Substring(0, 1),
                //City = customerCode.Substring(0, 3),
                //Branch = customerCode,

                //line.DiscountPercent = ((item.OriginalPrice - item.Price) / item.OriginalPrice) * 100;
                //line.DiscountPercent = item.TotalDiscountPercent;
                WarehouseCode = invoice.StoreCode
            };
            lines.Add(line);
        }


        var ShippingTaxCode = "X0";
        var FeeTaxCode = "X0";
        try
        {
            ShippingTaxCode = taxCodes.FirstOrDefault(x => x.Rate == -invoice.ShippingTaxPercentage).Code;
        }
        catch (Exception e)
        {
            //
        }
        try
        {
            FeeTaxCode = taxCodes.FirstOrDefault(x => x.Rate == -invoice.FeeTaxPrecentage).Code;
        }
        catch (Exception e)
        {
            //
        }

        var arInvoice = new SAPInvoice
        {
            CardCode = customerCode,
            DocDueDate = invoice.CreatedDatetime,
            DocDate = invoice.CreatedDatetime,
            TaxDate = invoice.CreatedDatetime,
            Series = series,
            //SalesEmployee = invoice.SalesEmpolyee,
            SalesEmployeeUDF = invoice.SalesEmpolyee,
            SyncdToSAP = "Y",
            PrismSid = invoice.Sid,
            Remarks = $"Prism Transaction: {invoice.DocumentNumber} - Branch ({invoice.StoreCode} - {invoice.StoreName}) - Created By: {invoice.CreatedBy}",
            DocumentLines = lines,

            DocumentAdditionalExpenses = new List<SAPLink.Core.Models.SAP.Sales.DocumentAdditionalExpense>
                {
                    new()
                    {
                        ExpenseCode = 1,
                        VatGroup = FeeTaxCode,
                        LineGross = Math.Abs(invoice.FeeAmount),
                        Remarks = "Fee"
                    },
                    new()
                    {
                        ExpenseCode = 2,
                        VatGroup = ShippingTaxCode,
                        LineGross = Math.Abs(invoice.TransactionTotalShippingAmtWithTax),
                        Remarks = "Shipping"
                    }
                }
        };

        return arInvoice.ToJson();
    }
    public static string CreateDownPaymentBody(Core.Models.Prism.Sales.Invoice invoice, List<TaxCodes> taxCodes, string customerCode, string series)
    {

        var lines = new List<DownPaymentLine>();

        foreach (var item in invoice.Items)
        {
            var vatGroup = "X0";
            try
            {
                vatGroup = taxCodes.FirstOrDefault(x => x.Rate == -item.TaxPercent).Code;
            }
            catch (Exception e)
            {
                //
            }
            var line = new DownPaymentLine
            {
                ItemCode = item.Alu,
                Quantity = item.Quantity,
                VatGroup = vatGroup,
                UnitPrice = item.Price,
                CostCenter = GetCostCenter(item.Alu),
                Region = customerCode.Substring(0, 1),
                City = customerCode.Substring(0, 3),
                Branch = customerCode,
                //line.DiscountPercent = ((item.OriginalPrice - item.Price) / item.OriginalPrice) * 100;
                //line.DiscountPercent = item.TotalDiscountPercent;
                WarehouseCode = invoice.StoreCode
            };

            lines.Add(line);
        }


        var ShippingTaxCode = "X0";
        var FeeTaxCode = "X0";
        try
        {
            ShippingTaxCode = taxCodes.FirstOrDefault(x => x.Rate == -invoice.ShippingTaxPercentage).Code;
        }
        catch (Exception e)
        {
            //
        }
        try
        {
            FeeTaxCode = taxCodes.FirstOrDefault(x => x.Rate == -invoice.FeeTaxPrecentage).Code;
        }
        catch (Exception e)
        {
            //
        }

        var downPayment = new DownPayment
        {
            CardCode = customerCode,
            DocDueDate = invoice.InvoicePostedDate,
            DocDate = invoice.InvoicePostedDate,
            TaxDate = invoice.InvoicePostedDate,
            SalesEmployeeUDF = invoice.SalesEmpolyee,
            SyncToPrism = "Y",
            PrismSid = invoice.Sid,
            Series = series,
            Remarks = $"Prism Transaction: {invoice.DocumentNumber} - Branch ({invoice.StoreCode} - {invoice.StoreName}) - Created By: {invoice.CreatedBy}",
            DocumentLines = lines,

            //DocumentAdditionalExpenses = new List<DocumentAdditionalExpense>
            //{
            //    new()
            //    {
            //        ExpenseCode = 1,
            //        VatGroup = FeeTaxCode,
            //        LineGross = invoice.FeeAmount,
            //        Remarks = "Fee"
            //    },
            //    new()
            //    {
            //        ExpenseCode = 2,
            //        VatGroup = ShippingTaxCode,
            //        LineGross = invoice.TransactionTotalShippingAmtWithTax,
            //        Remarks = "Shipping"
            //    }
            //}
        };

        return downPayment.ToJson();
    }
    public RequestResult<SAPInvoice> AddSalesOrder(PrismInvoice PrismInvoice)
    {
        RequestResult<SAPInvoice> result = new RequestResult<SAPInvoice>();

        try
        {
            if (PrismInvoice.Tenders.Count > 0)
            {
                var taxCodes = GetTaxCodes();
                var customerCode = Prism.Handlers.OutboundData.PointOfSale.Handler.GetCustomerCodeByStoreCode(PrismInvoice.StoreCode, out string message);

                var body = SAPInvoice.CreateOrderBody(PrismInvoice, taxCodes, customerCode);


                result.Response.Response = SAPHttpClientFactory.Initialize("Orders", Method.POST, _client, LoginModel.LoginTypes.Basic, null, body);

                if (result.Response.Response.StatusCode == HttpStatusCode.OK || result.Response.Response.StatusCode == HttpStatusCode.Created)
                {
                    var invoice = JsonConvert.DeserializeObject<SAPInvoice>(result.Response.Response.Content);

                    if (invoice != null && invoice != null)
                    {
                        result.EntityList.Add(invoice);
                        result.Status = Enums.StatusType.Success;
                        result.Message = $"Order No: {invoice.DocNum} is added.";
                    }

                    return result;
                }
                if (result.Response.Response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var message2 = "";
                    result.Status = Enums.StatusType.NotFound;
                    if (result.Response.Response.Content.Contains("To generate this document, first define the numbering series in the Administration module"))
                        message2 = "Can`t find document series, Please make sure you define Whs series in Street/PO Box that Linked to document numbring (Series Name)";
                    else
                        message2 = result.Response.Response.Content;

                    result.Message = $"\r\nCan`t Add Order - Prism Invoice ({PrismInvoice.DocumentNumber})" +
                              $"\r\n\r\nErrors in Response Content:" +
                              $"\r\n{message2}" +
                              $"\r\n\r\nRequest Body:" +
                              $"\r\n{body}";
                }
            }
        }
        catch (Exception e)
        {
            {
                result.Status = Enums.StatusType.Failed;
                result.Message = $"Invoice ID: {PrismInvoice.DocumentNumber} is not added. - Exception: {e.Message}";
            }
        }
        //result.Status = Enums.StatusType.NotFound;
        //result.Message = $"Invoice ID: {PrismInvoice.DocumentNumber} is not found. \r\n {body}";

        return result;
    }

    private static List<TaxCodes> GetTaxCodes()
    {
        var query = "SELECT T0.[Code], T0.[Name], T0.[Rate]  FROM OVTG T0 WHERE T0.[Category] = 'O' AND T0.[Inactive] = 'N'";


        if (!ClientHandler.Company.Connected)
        {
            ClientHandler.InitializeClientObjects(_client, out _, out _);
        }

        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        oRecordSet.DoQuery(query);

        var taxCodesList = new List<TaxCodes>();

        for (var i = 0; i < oRecordSet.RecordCount; i++)
        {
            var taxCode = new TaxCodes();
            taxCode.Code = oRecordSet.Fields.Item("Code").Value.ToString();
            taxCode.Name = oRecordSet.Fields.Item("Name").Value.ToString();
            taxCode.Rate = -Convert.ToDouble(oRecordSet.Fields.Item("Rate").Value.ToString());

            if (taxCode.Code is "X0" or "OA" or "OA1")
            {
                taxCodesList.Add(taxCode);
            }

            oRecordSet.MoveNext();
        }

        return taxCodesList;
    }

    private static string GetCostCenter(string itemCode)
    {
        var query = $"SELECT U_Dim1 As CostCenter FROM [@AIPG] WHERE Code = (SELECT U_ProdGrp FROM OITM T0 WHERE ItemCode = '{itemCode}')";
            return GetValueByQuery(query, "CostCenter");
    }
    private static string GetRegion(string cardCode)
    {
        var query = $"SELECT SUBSTRING(AddID,1,1) As Region FROM OCRD WHERE CardCode = '{cardCode}'";
        //var query = $"SELECT AddID As Region FROM OCRD WHERE CardCode = '{cardCode}')";
        return GetValueByQuery(query);
    }

    private static string GetCity(string cardCode)
    {
        var query = $"SELECT SUBSTRING(AddID,1,3) As City FROM OCRD WHERE CardCode = '{cardCode}'";
        //var query = $"SELECT AddID As City FROM OCRD WHERE CardCode = '{cardCode}')";
        return GetValueByQuery(query);
    }
    private static string GetValueByQuery(string query)
    {
        var output = "";
        try
        {
            if (!ClientHandler.Company.Connected)
                ClientHandler.InitializeClientObjects(_client, out _, out _);

            var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(query);

            output = oRecordSet.Fields.Item(0).Value.ToString();
        }
        catch (Exception e) { }

        return output;
    }

    private static string GetValueByQuery(string query, string field)
    {
        var output = "";
        try
        {
            if (!ClientHandler.Company.Connected)
                ClientHandler.InitializeClientObjects(_client, out _, out _);

            var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(query);

            output = oRecordSet.Fields.Item(field).Value.ToString();
        }
        catch (Exception e) { }

        return output;
    }
}