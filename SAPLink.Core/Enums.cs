﻿using System.ComponentModel;
using InboundDocuments = SAPLink.Core.InboundEnums.Documents;
using OutboundDocuments = SAPLink.Core.OutboundEnums.Documents;

namespace SAPLink.Core;


public static class InboundEnums
{
    public enum Documents
    {
        Departments = 0,
        Vendors = 1,
        Items = 2,
        GoodsReceiptPo = 3,
        GoodsReceipt = 4,
        GoodsIssue = 5,
        GoodsReturn = 6,
    }

}
public static class OutboundEnums
{
    public enum Documents
    {
        SalesInvoice = 0,
        ReturnInvoice = 1,
        CustomerOrder = 2,
        StockTransfer = 3,
        InventoryPosting = 4,
        GoodsReceipt = 5,
        GoodsIssue = 6,
    }

}

public static class Enums
{
    public enum Reports
    {
        PrismActiveItems = 0,
        SyncedItems = 1,
        NotSynced = 2,
        //GoodsReceiptPo = 3,
        //GoodsReceipt = 4,
        //GoodsIssue = 5,
    }

  

    public enum Repeats
    {
        None = 0,
        Hourly,
        Daily
    }

    public class SyncType
    {
        public UpdateType Initial { get; set; }
        public UpdateType Sync { get; set; }

        public static SyncType? GetSyncType(InboundDocuments document)
        {
            return document switch
            {
                InboundDocuments.Departments => new SyncType
                {
                    Initial = UpdateType.InitialDepartment,
                    Sync = UpdateType.SyncDepartment
                },
                InboundDocuments.Vendors => new SyncType
                {
                    Initial = UpdateType.InitialVendors,
                    Sync = UpdateType.SyncVendors
                },
                InboundDocuments.Items => new SyncType
                {
                    Initial = UpdateType.InitialItems,
                    Sync = UpdateType.SyncItems
                },
                InboundDocuments.GoodsReceiptPo => new SyncType
                {
                    Initial = UpdateType.InitialGoodsReceiptPO,
                    Sync = UpdateType.SyncGoodsReceiptPO
                },
                InboundDocuments.GoodsReceipt => new SyncType
                {
                    Initial = UpdateType.InitialInGoodsReceipt,
                    Sync = UpdateType.SyncInGoodsReceipt
                },
                InboundDocuments.GoodsIssue => new SyncType
                {
                    Initial = UpdateType.InitialInGoodsIssue,
                    Sync = UpdateType.SyncInGoodsIssue
                },
                InboundDocuments.GoodsReturn => new SyncType
                {
                    Initial = UpdateType.InitialInGoodsReturn,
                    Sync = UpdateType.SyncInGoodsReturn
                },
                _ => null
            };
        }
        public static SyncType? GetSyncType(OutboundDocuments document)
        {
            return document switch
            {
                OutboundDocuments.SalesInvoice => new SyncType
                {
                    Initial = UpdateType.InitialInvoice,
                    Sync = UpdateType.SyncInvoice
                },
                OutboundDocuments.ReturnInvoice => new SyncType
                {
                    Initial = UpdateType.InitialCreditMemo,
                    Sync = UpdateType.SyncCreditMemo
                },
                OutboundDocuments.StockTransfer => new SyncType
                {
                    Initial = UpdateType.InitialInventoryTransfer,
                    Sync = UpdateType.SyncInventoryTransfer
                },
                OutboundDocuments.InventoryPosting => new SyncType
                {
                    Initial = UpdateType.InitialInventoryPosting,
                    Sync = UpdateType.SyncInventoryPosting
                },
                OutboundDocuments.GoodsReceipt => new SyncType
                {
                    Initial = UpdateType.InitialOutGoodsReceipt,
                    Sync = UpdateType.SyncOutGoodsReceipt
                },
                OutboundDocuments.GoodsIssue => new SyncType
                {
                    Initial = UpdateType.InitialOutGoodsIssue,
                    Sync = UpdateType.SyncOutGoodsIssue
                },
                _ => null
            };
        }
    }

    public enum UpdateType
    {
        InitialDepartment,
        SyncDepartment,

        InitialVendors,
        SyncVendors,

        InitialItems,
        SyncItems,

        InitialGoodsReceiptPO,
        SyncGoodsReceiptPO,

        InitialInGoodsReceipt,
        SyncInGoodsReceipt,

        InitialInGoodsIssue,
        SyncInGoodsIssue,


        InitialInvoice,
        SyncInvoice,

        InitialCreditMemo,
        SyncCreditMemo,

        InitialInventoryTransfer,
        SyncInventoryTransfer,

        InitialInventoryPosting,
        SyncInventoryPosting,

        InitialOutGoodsReceipt,
        SyncOutGoodsReceipt,

        InitialOutGoodsIssue,
        SyncOutGoodsIssue,

        InitialOrders,
        SyncOrders,

        InitialWholesale,
        SyncWholesale,

        InitialWholesaleRetail,
        SyncWholesaleRetail,

        InitialInGoodsReturn,
        SyncInGoodsReturn,
    }
    public enum BoDataServerTypes
    {
        dst_MSSQL = 1,
        dst_DB_2 = 2,
        dst_SYBASE = 3,
        dst_MSSQL2005 = 4,
        dst_MAXDB = 5,
        dst_MSSQL2008 = 6,
        dst_MSSQL2012 = 7,
        dst_MSSQL2014 = 8,
        dst_HANADB = 9,
        dst_MSSQL2016 = 10,
        dst_MSSQL2017 = 11,
        dst_MSSQL2019 = 15,
    }
    public enum Environments
    {
        None = 0,

        [Description("Production Environment")]
        Production,

        [Description("Test Environment")]
        Test,

        [Description("Local Environment")]
        Local,
    }
    public enum StatusType
    {
        Success,
        Failed,
        NotFound
    }
}