using SAPLink.Core;
using SAPLink.Core.Models;
using SAPLink.Core.Models.Prism.Sales;
using SAPLink.Core.Models.System;
using SAPLink.Core.Utilities;
using SAPLink.Handler.Prism.Settings;
using Serilog;
using HttpClientFactory = SAPLink.Handler.Connection.HttpClientFactory;
using PrismInvoices = SAPLink.Core.Models.Prism.Sales.Invoices;
using PrismInvoice = SAPLink.Core.Models.Prism.Sales.Invoice;
using SAPLink.Core.Models.Prism.Receiving;
using SAPLink.Handler.Connected_Services;
using SAPLink.Core.Models.SAP.Sales;
using Serilog.Events;
using TaxCodes = SAPLink.Core.Models.Prism.Settings.TaxCodes;

namespace SAPLink.Handler.Prism.Handlers.OutboundData.PointOfSale;

public partial class InvoiceService
{
    private readonly Credentials? _credentials;
    private readonly Subsidiaries? _subsidiary;
    public readonly TaxCodesService? TaxCodesService;

    public StoresService? StoresService;
    public List<Store>? Stores;
    public List<TaxCodes> TaxCodes { get; set; }
    private static ILogger _loger;

    static InvoiceService()
    {
        _loger = Helper.CreateLoggerConfiguration("Sale - (AR Invoice)", "Service", LogsTypes.OutboundData);
    }

    public InvoiceService(Clients client)
    {
        _credentials = client.Credentials.FirstOrDefault();
        _subsidiary = _credentials?.Subsidiaries.FirstOrDefault();
        TaxCodesService = new TaxCodesService(client);
        StoresService = new StoresService(client);
    }

    public async Task<RequestResult<PrismInvoice>> GetInvoicesAsync(OutboundEnums.Documents document, string dateRange, int storeNumber, string code = "nn")
    {
        RequestResult<PrismInvoice> result = new();
        try
        {
            string query = _credentials.BaseUri;

            string storeFilter = "";
            if (storeNumber >= 0)
            {
                Stores = await StoresService.GetAll();
                var storeSid = Stores.FirstOrDefault(s => s.StoreNumber == storeNumber).Sid;
                storeFilter = $"AND((store_uid,eq,{storeSid})LOR(item.fulfill_store_sid,eq,{storeSid}))";
            }

            string docFilter = "";
            if (document == OutboundEnums.Documents.SalesInvoice)
                docFilter = "AND(has_sale,eq,true)AND(document_number,nn)";

            if (document == OutboundEnums.Documents.ReturnInvoice)
                docFilter = "AND(has_return,eq,true)AND(document_number,nn)";

            string order = "";
            if (document == OutboundEnums.Documents.CustomerOrder)
                order = "AND((document_number,nn)OR(order_document_number,nn))"; //(receipt_type,eq,2)

            string codeFilter = "";
            if (code != "nn")
                codeFilter = $"AND((document_number,eq,{code}))";//OR(order_document_number,eq,{code})


            var resource = $"/v1/rest/document" +
                           $"?filter=(subsidiary_uid,eq,{_subsidiary.SID}){dateRange}{storeFilter}{codeFilter}{docFilter}" + //AND(pos_flag3,nq,Yes)
                           $"&cols=employee1_login_name,subsidiary_uid,store_uid,bt_first_name,bt_last_name,bt_company_name,bt_title,invoice_posted_date,document_number,order_qty,order_quantity_filled,cashier_login_name,order_qty,return_qty,sid,sold_qty,tender_name,transaction_total_amt,row_version,order_document_number,send_sale_status,order_status,send_sale_fulfillment,item.order_quantity_filled,item.order_type,receipt_type,order_type,store_number,eft_invoice_number,so_deposit_amt_paid,transaction_total_tax_amt,store_code,comment1,comment2,doc_tender_type,has_sale,has_return,pos_flag3";

            result.Message = $"Resource: \r\n" +
                             $"{query}{resource}\r\n" +
                             $"Auth Session: {_credentials.AuthSession}";
            
            _loger.Information("-----------------------------------------------------------------");

            _loger.Information(result.Message);

            result.Response = await HttpClientFactory.InitializeAsync(query, resource, Method.GET);
            //_loger.Information(result.Response.Content);

            if (result.Response.StatusCode == HttpStatusCode.OK)
            {
                var invoices = PrismInvoice.FromJson(result.Response.Content);
                result.EntityList.AddRange(invoices);
                _loger.Information("Successfully fetched the invoices.");

                //result.Message += $"Successfully fetched the invoices.";
            }
            else
            {
                _loger.Warning($"Failed to fetch invoices. Status Code: {result.Response.StatusCode}. Content: {result.Response.Content.PrettyJson()}");
                //result.Message += $"Failed to fetch invoices. Status Code: {result.Response.StatusCode}. Content: {result.Response.Content.PrettyJson()}";
            }

            return result;

            //return LoadMockData("Order.json");
        }
        catch (Exception ex)
        {
            _loger.Error(ex, "Error occurred while fetching the invoices.");
            result.Message += $"Error occurred while fetching the invoice details for SID: {code}. \r\n" +
                              $"Exception: \r\n" +
                              $"{ex.Message}";
        }
        finally
        {
          //
        }

        return result;
    }
    private RequestResult<PrismInvoice> LoadMockData(string fileName)
    {
        var file = File.ReadAllText($"Resources\\{fileName}");
        var invoices = PrismInvoice.FromJson(file);

        RequestResult<PrismInvoice> result = new RequestResult<PrismInvoice>();

        result.EntityList.AddRange(invoices);
        return result;
    }

    public async Task<RequestResult<PrismInvoice>> GetInvoiceDetailsBySidAsync(string code)
    {
        RequestResult<PrismInvoice> result = new();

        try
        {
            string query = _credentials.BaseUri;
            //var resource = $"v1/rest/document/{code}?filter=(subsidiary_uid,eq,{_subsidiary.SID})AND(document_number,nn)/\"&cols=receipt_type,original_store_code,original_store_number,store_name,store_code,store_number,subsidiary_uid,store_uid,original_store_uid,subsidiary_number,subsidiary_name,sid,row_version,created_by,created_datetime,document_number,invoice_posted_date,transaction_total_tax_amt,transaction_total_amt,transaction_subtotal_with_tax,transaction_total_shipping_tax,transaction_total_shipping_amt_no_tax,transaction_total_shipping_amt_with_tax,item.alu,item.scan_upc,item.item_description1,item.item_description2,item.item_description3,item.original_price,item.original_tax_amount,ref_sale_doc_no,item.quantity,item.price,item.tax_percent,item.tax_amount,item.total_discount_percent,item.total_discount_amount,item.discount_amt,item.discount_perc,item.discount_reason,tender.sid,tender.created_by,tender.created_datetime,tender.post_date,tender.row_version,tender.tender_type,tender.tender_pos,tender.amount,tender.taken,tender.given,tender.currency_name,tender.tender_name,tender.alphabetic_code\"+/&sort=invoice_posted_date,asc";
            //var resource = $"v1/rest/document/{code}?filter=(subsidiary_uid,eq,{_subsidiary.SID})&cols=receipt_type,original_store_code,original_store_number,store_name,store_code,store_number,subsidiary_uid,store_uid,original_store_uid,subsidiary_number,subsidiary_name,sid,row_version,created_by,created_datetime,document_number,invoice_posted_date,transaction_total_tax_amt,transaction_total_amt,transaction_subtotal_with_tax,transaction_total_shipping_tax,transaction_total_shipping_amt_no_tax,transaction_total_shipping_amt_with_tax,item.alu,item.scan_upc,item.item_description1,item.item_description2,item.item_description3,item.original_price,item.original_tax_amount,ref_sale_doc_no,item.quantity,item.price,item.tax_percent,item.tax_amount,item.total_discount_percent,item.total_discount_amount,item.discount_amt,item.discount_perc,item.discount_reason,tender.sid,tender.created_by,tender.created_datetime,tender.post_date,tender.row_version,tender.tender_type,tender.tender_pos,tender.amount,tender.taken,tender.given,tender.currency_name,tender.tender_name,tender.alphabetic_code\"+/&sort=invoice_posted_date,asc";
            var resource = $"/v1/rest/document/{code}" +
                           $"?cols=employee1_login_name,bt_info1,bt_cuid,bt_first_name,bt_email,bt_company_name,bt_title,receipt_type,original_store_code,original_store_number,store_name,store_code,store_number,subsidiary_uid,store_uid,original_store_uid,subsidiary_number,subsidiary_name,sid,row_version,created_by,created_datetime,document_number,invoice_posted_date,transaction_total_tax_amt,transaction_total_amt,transaction_subtotal_with_tax,transaction_total_shipping_tax,transaction_total_shipping_amt_no_tax,transaction_total_shipping_amt_with_tax,shipping_tax_percentage,total_fee_amt,fee_amt1,fee_tax_amt1,fee_tax_perc1,item.alu,item.scan_upc,item.item_description1,item.item_description2,item.item_description3,item.original_price,item.original_tax_amount,ref_sale_doc_no,ref_sale_sid,item.quantity,item.price,item.tax_percent,item.tax_amount,item.total_discount_percent,item.total_discount_amount,item.discount_amt,item.discount_perc,item.discount_reason,tender.sid,tender.created_by,tender.created_datetime,tender.post_date,tender.row_version,tender.tender_type,tender.tender_pos,tender.amount,tender.taken,tender.given,tender.currency_name,tender.tender_name,tender.alphabetic_code&sort=invoice_posted_date,asc";

            result.Message = $"Resource: \r\n\r\n" +
                             $"{query}{resource}\r\n\r\n" +
                             $"Auth Session: {_credentials.AuthSession}\r\n";

            _loger.Information(result.Message);

            result.Response = await HttpClientFactory.InitializeAsync(query, resource, Method.GET);

            _loger.Information(result.Response.Content);

            if (result.Response.StatusCode == HttpStatusCode.OK)
            {
                var invoices = PrismInvoice.FromJson(result.Response.Content);
                result.Status = Enums.StatusType.Success;

                result.EntityList.AddRange(invoices);
                _loger.Information($"Successfully fetched the invoice details for SID: {code}.");
                //result.Message += $"Successfully fetched the invoice details for SID: {code}.";
            }
            else
            {
                _loger.Warning($"Failed to fetch invoice details for SID: {code}. Status Code: {result.Response.StatusCode}. Content: {result.Response.Content}");
                //result.Message += $"Failed to fetch invoice details for SID: {code}. Status Code: {result.Response.StatusCode}. Content: {result.Response.Content}";
            }
        }
        catch (Exception ex)
        {
            _loger.Error(ex, $"Error occurred while fetching the invoice details for SID: {code}.");
            result.Message += $"Error occurred while fetching the invoice details for SID: {code}. \r\n" +
                              $"Exception: \r\n" +
                              $"{ex.Message}";
        }
        finally
        {
          //
        }

        return result;

        //return LoadMockData("OrderBySid.json");
    }

    //public async Task<RequestResult<PrismInvoice>> UpdateIsSynced(string invoiceSid, string rowVersion, string InvoiceNo = "")
    //{
    //    string query = _credentials.BaseUri;
    //    var resource = $"/v1/rest/document/{invoiceSid}/?filter=row_version,eq,{rowVersion}";
    //    RequestResult<PrismInvoice> result = new();

    //    string body = @"[
    //                          {
    //                              ""pos_flag3"": ""Yes"",
    //                              ""comment2"": """ + InvoiceNo + @"""
    //                          }
    //                    ]";

    //    result.Response = await HttpClientFactory.InitializeAsync(query, resource, Method.PUT, body);


    //    if (result.Response.StatusCode == HttpStatusCode.OK)
    //    {
    //        result.Status = Enums.StatusType.Success;
    //        result.Message = $"Successfully Update Sync Flag for the Prism invoice SID: {invoiceSid} - SAP invoice No: {InvoiceNo}.";
           
    //        _loger.Information(result.Message);
    //    }
    //    else
    //    {
    //        result.Status = Enums.StatusType.Failed;
    //        result.Message = $"Failed to Update Sync Flag for the Prism invoice SID: {invoiceSid}" +
    //                         $"\r\nRequest EndPoint: {query}{resource}" +
    //                         $"\r\nBody:\r\n" +
    //                         $"{body.PrettyJson()}\r\n" +
    //                         $"\r\nStatus Code: {result.Response.StatusCode}" +
    //                         $"\r\nContent: {result.Response.Content.PrettyJson()}\r\n";

    //        _loger.Error(result.Message);
    //    }

    //    return result;
    //}
}