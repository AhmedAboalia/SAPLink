using SAPbobsCOM;
using SAPLink.Application.SAP.Application;
using SAPLink.Domain.Models.System;
using PrismInvoice = SAPLink.Domain.Models.Prism.Sales.Invoice;

namespace SAPLink.Application.Prism.Handlers.OutboundData.PointOfSale;

public partial class Handler
{
    private static Clients _client;
    
    public Handler(Clients client)
    {
        _client = client;
    }

    public static bool IsBusinessPartnerExists(string cardCode)
    {
        var businessPartner = (BusinessPartners)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
        return businessPartner.GetByKey(cardCode);

    }

    public static string GetCustomerCodeByStoreCode(string storeCode, out string message)
    {
        var cardCode = "";
        message = "";

        try
        {
            var query = @$"SELECT 
                            T0.[CardCode], T0.[CardName] 
                                   FROM OCRD T0 WHERE T0.[U_PrismStoreCode] = '{storeCode}'";

            CheckCompanyConnection(ref message);

            var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(query);

            //var customer = new BusinessPartner();
            //customer.CardCode = 
                cardCode = oRecordSet.Fields.Item("CardCode").Value.ToString();
            //customer.CardName = oRecordSet.Fields.Item("CardName").Value.ToString();
        }
        catch (Exception e)
        {
            message = e.Message;
        }

        return cardCode;
    }
    public static string GetSeriesCode(string storeCode, out string message)
    {
        var cardCode = "";
        message = "";

        try
        {
            var query = @$"SELECT T0.[Series]
                            FROM NNM1 T0 
	                            INNER JOIN OWHS T1 ON T0.[SeriesName] = T1.[Street]
                                     WHERE T0.[ObjectCode] = '13' AND T1.WhsCode = '{storeCode}'";

            CheckCompanyConnection(ref message);

            var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(query);

            cardCode = oRecordSet.Fields.Item("Series").Value.ToString();
        }
        catch (Exception e)
        {
            message = e.Message;
        }

        return cardCode;
    }

    private static void CheckCompanyConnection(ref string message)
    {
        if (!ClientHandler.Company.Connected)
        {
            ClientHandler.InitializeClientObjects(_client, out var code, out var errorMessage);

            message = code != 0
                ? $"Error :{code}  : {errorMessage}"
                : "Not Connected to Database.";
        }
    }

    public static bool AddBusinessPartner(PrismInvoice invoice)
    {
        var businessPartner = (BusinessPartners)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oBusinessPartners);

        businessPartner.CardCode = invoice.CustomerID;
        businessPartner.CardName = invoice.CustomerName;
        businessPartner.EmailAddress = invoice.CustomerEmail;

        if (businessPartner.Add() != 0)
        {
            var erorr = ClientHandler.Company.GetLastErrorDescription();
            return false;
        }
        return true;
    }

    private static void AddItemMaster(PrismInvoice invoice)
    {
        var item = (Items)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oItems);

        foreach (var items in invoice.Items)
        {
            try
            {
                item.ItemCode = items.Alu;
                item.ItemName = $"{items.Alu} {items.ItemDescription1} {items.ItemDescription2}";
                item.Add();
            }
            catch (Exception e)
            {
                //
            }
        }
    }
}