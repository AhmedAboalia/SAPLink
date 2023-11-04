using System.ComponentModel;

namespace SAPLink.Core.Models
{
    public class Schedule
    {
        [Key] 
        public int Id { get; set; }
        private SyncDocuments _document; // backing field for Document

        public SyncDocuments Document
        {
            get => _document;
            set
            {
                _document = value;
                DocumentName = value.ToString();
            }
        }
        public string DocumentName { get; private set; }

        public List<Recurring> Times { get; set; } = new List<Recurring>();

        public enum SyncDocuments
        {
            // Inbound
            Departments = 1,
            Vendors,
            Items,
            GoodsReceiptPos,
            GoodsReceipts_Inbound, // renamed to avoid conflict with Outbound
            GoodsIssues_Inbound,  // renamed to avoid conflict with Outbound

            // Outbound
            SalesInvoices,
            ReturnInvoices,
            CustomerOrders,
            StockTransfers,
            InventoryPosting,
            GoodsReceipts_Outbound, // renamed to clarify this is different from Inbound
            GoodsIssues_Outbound,   // renamed to clarify this is different from Inbound
        }
    }

    public class Recurring
    {
        [Key]
        public int Id { get; set; }
        public bool Active { get; set; }
        public TimeOnly Time { get; set; }
        public int ScheduleId { get; set; } // Foreign key property
        public Schedule Schedule { get; set; } // Navigation property

        public int TimeId { get; set; }

    }
}
