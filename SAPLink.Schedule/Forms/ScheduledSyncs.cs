using Every;
using SAPLink.Handler.Prism.Connection.Auth;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Vendors;
using SAPLink.Handler.Prism.Handlers.InboundData.Receiving;
using SAPLink.Handler.Prism.Handlers.OutboundData.StockManagement.InventoryPosting;
using static SAPLink.Core.Models.Schedules;
using InventoryTransferService = SAPLink.Handler.Prism.Handlers.OutboundData.StockManagement.InventoryTransfer.InventoryTransferService;
using InventoryTransferHandler = SAPLink.Handler.Prism.Handlers.OutboundData.StockManagement.InventoryTransfer.InventoryTransferHandler;
using Documents = SAPLink.Core.InboundEnums.Documents;
using SAPLink.Handler.SAP.Application;
using SAPLink.Core.Models.Prism.StockManagement;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using SAPLink.Schedule.Utilities;
using System.ComponentModel.DataAnnotations;
using static SAPLink.Schedule.Forms.ScheduledSyncs;
using static Guna.UI2.Native.WinApi;
using SAPLink.Core.Models;
using SAPLink.EF;
using SAPLink.Utilities;
using SAPLink.Handler.Prism.Handlers.InboundData.Receiving.GRPO;

namespace SAPLink.Schedule.Forms;

public partial class ScheduledSyncs : Form
{
    #region Initial variables

    private readonly UnitOfWork _unitOfWork;
    private readonly ServiceLayerHandler _serviceLayer;
    private readonly Clients _client;
    private static List<Schedules> Schedules;
    private readonly ItemsService _itemsService;
    private readonly ItemsHandler _itemsHandler;
    private readonly DepartmentService _departmentServices;
    private readonly DepartmentsHandler _departmentsHandler;
    private readonly VendorsHandler _vendorsHandler;
    private readonly GoodsReceiptPOHandler _goodsReceiptPoHandler;
    private readonly GoodsReceiptHandler _goodsReceiptHandler;
    private readonly GoodsIssueHandler _goodsIssueHandler;
    private readonly Credentials _credentials;

    private readonly InvoiceHandler _invoiceHandler;
    private readonly Handler.Prism.Handlers.OutboundData.PointOfSale.Handler _handler;
    private readonly OrdersHandler _ordersHandler;
    private readonly DownPaymentHandler _downPaymentHandler;
    private readonly CreditMemoHandler _creditMemoHandler;
    private readonly ReturnsHandler _returnsHandler;
    private readonly InvoiceService _invoiceService;
    private readonly DepartmentService _departmentService;

    private readonly InventoryTransferService _verifiedVoucherService;
    private readonly InventoryTransferHandler _verifiedVoucherHandler;

    private readonly InventoryPostingService _inventoryPostingService;
    private readonly InventoryPostingHandler _inventoryPostingHandler;

    public ScheduledSyncs(UnitOfWork unitOfWork, ServiceLayerHandler serviceLayer, DepartmentService departmentService,
        ItemsService itemsService, Clients client)
    {
        InitializeComponent();
        _unitOfWork = unitOfWork;
        _serviceLayer = serviceLayer;
        _departmentServices = departmentService;
        _itemsService = itemsService;
        _client = client;
        _credentials = _client.Credentials.FirstOrDefault();

        _goodsReceiptPoHandler = new GoodsReceiptPOHandler(unitOfWork, client);
        _goodsReceiptHandler = new GoodsReceiptHandler(unitOfWork, client);
        _goodsIssueHandler = new GoodsIssueHandler(unitOfWork, client);

        _departmentsHandler = new DepartmentsHandler(_unitOfWork, _departmentServices, client);
        _itemsHandler = new ItemsHandler(_unitOfWork, _itemsService, client);
        _vendorsHandler = new VendorsHandler(_unitOfWork, client);



        _invoiceService = new InvoiceService(_client);
        _handler = new Handler.Prism.Handlers.OutboundData.PointOfSale.Handler(_client);
        _invoiceHandler = new InvoiceHandler(_serviceLayer);
        _returnsHandler = new ReturnsHandler(_client, _serviceLayer);
        _ordersHandler = new OrdersHandler(_serviceLayer);
        _downPaymentHandler = new DownPaymentHandler(_client, _serviceLayer);
        _creditMemoHandler = new CreditMemoHandler(_client);

        _verifiedVoucherService = new InventoryTransferService(_client);
        _verifiedVoucherHandler = new InventoryTransferHandler(_client);

        _inventoryPostingService = new InventoryPostingService(_client);
        _inventoryPostingHandler = new InventoryPostingHandler();


        //try
        //{
        //    string Url = _client.Credentials.FirstOrDefault().IntegrationUrl;
        //    webViewSchedule.Source = new Uri($"{Url}/dashboard/recurring");
        //}
        //catch (Exception e)
        //{
        //}
        comboBoxDocTypeSchedule.SelectedIndex = (int)Documents.Items;

        Schedules = GetSchedules();

        InitializeDataGridView();
    }

    private List<Schedules> GetSchedules()
    {
        string[] includes = { "Recurring" };
        return _unitOfWork.Schedule.GetAll(includes).ToList();
    }

    #endregion
    private bool hidden = false;


    private void buttonScheduleIt_Click(object sender, EventArgs e)
    {
        if (buttonScheduleIt.Enabled)
        {

            var schedules = GetSchedules();

            foreach (var schedule in schedules)
            {
                foreach (var recurring in schedule.Times)
                {
                    var recurringTime = recurring.Time.ToString().To24HourMinutesFormat();

                    if (recurring.Active && schedule.Document == SyncDocuments.Items)
                    {
                        Ever.y().Day.At(recurringTime.Hours).Do(HandleItems);
                        //Ever.y().Minute.From(23, 35).To(23, 37).Do((() => HandleItems("")));
                    }
                    if (recurring.Active && schedule.Document == SyncDocuments.GoodsReceiptPos)
                    {
                        Ever.y().Day.At(recurringTime.Hours).Do(HandleGoodsReceiptPo);
                        //Ever.y().Minute.From(23, 35).To(23, 37).Do((() => HandleItems("")));
                    }
                    if (recurring.Active && schedule.Document == SyncDocuments.SalesInvoices)
                    {
                        Ever.y().Day.At(recurringTime.Hours).Do(HandleSalesInvoices);
                        //Ever.y().Minute.From(23, 35).To(23, 37).Do((() => HandleItems("")));
                    }

                    if (recurring.Active && schedule.Document == SyncDocuments.ReturnInvoices)
                    {
                        Ever.y().Day.At(recurringTime.Hours).Do(HandleReturnInvoices);
                        //Ever.y().Minute.From(23, 35).To(23, 37).Do((() => HandleItems("")));
                    }

                    if (recurring.Active && schedule.Document == SyncDocuments.StockTransfers)
                    {
                        Ever.y().Day.At(recurringTime.Hours).Do(HandleStockTransfer);
                        //Ever.y().Minute.From(23, 35).To(23, 37).Do((() => HandleItems("")));
                    }
                }
            }
        }
    }
    private int FindComboBoxIndexByTime(ComboBox comboBox, string targetTime)
    {
        for (int i = 0; i < comboBox.Items.Count; i++)
        {
            string comboBoxItem = comboBox.Items[i].ToString();
            if (comboBoxItem == targetTime)
            {
                return i; // Return the index of the matching item
            }
        }

        return -1; // Return -1 if the target time is not found
    }
    private async Task HandleItems()
    {
        var filter = GetSyncQueryByRangOfDate();

        var LogMessage = "";
        await foreach (var syncResult in _itemsHandler.SyncAsync(filter))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var itemMaster in syncResult.EntityList)
                {
                    LogMessage += syncResult.Message;
                }
            }
            Log(LogMessage);
        }
    }

    private async Task HandleGoodsReceiptPo()
    {
        var filter = GetSyncQueryByRangOfDate();

        var LogMessage = "";
        await foreach (var syncResult in _goodsReceiptPoHandler.SyncAsync(filter))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var itemMaster in syncResult.EntityList)
                {
                    LogMessage += syncResult.Message;
                }
            }
            Log(LogMessage);
        }
    }
    private async Task HandleSalesInvoices()
    {
        var salesInvoices = new RequestResult<PrismInvoice>();


        string filterByDateRang = GetSyncQuery();
        salesInvoices = await _invoiceService.GetInvoicesAsync(OutboundDocuments.SalesInvoice, filterByDateRang, -1);

        if (salesInvoices.EntityList.Any())
        {
            foreach (var sInvoice in salesInvoices.EntityList)
            {
                var invoiceResult = await _invoiceService.GetInvoiceDetailsBySidAsync(sInvoice.Sid);
                var invoice = invoiceResult.EntityList.FirstOrDefault();

                Log($"Invoice/s." +
                            $"\r\nFirst Request Message {salesInvoices.Message}" +
                            $"\r\n\r\nSecond Request Message: {invoiceResult.Message}");

                if (invoice == null) continue;

                var isARDownPayment = invoice.Items.Any(p => p.Alu == "SP0012");


                if (isARDownPayment && CheckInvoiceExist(sInvoice.Sid, "ODPI"))
                {
                    var docNum = GetInvoiceDocNum(sInvoice.Sid, "ODPI");

                    Log($"\r\nPrism Invoice No. ({sInvoice.DocumentNumber}) is Already Exist with SAP A/r Down Payment No. ({docNum}).");
                }

                else if (!isARDownPayment && CheckInvoiceExist(sInvoice.Sid, "OINV"))
                {
                    var docNum = GetInvoiceDocNum(sInvoice.Sid, "OINV");

                    Log($"\r\nPrism Invoice No. ({sInvoice.DocumentNumber}) is Already Exist with SAP Invoice No. ({docNum}).");
                }

                var isWholesale = invoice.Items.Any(p => p.IsWholesale == "Yes");
                var wholesaleCustomerCode = invoice.Items.FirstOrDefault().WholesaleCustomerCode;


                if (isARDownPayment && !CheckInvoiceExist(sInvoice.Sid, "ODPI"))
                    await HandleDownPayment(invoiceResult.EntityList);

                else if (!isARDownPayment && !CheckInvoiceExist(sInvoice.Sid, "OINV"))
                    await HandleInvoices(invoiceResult.EntityList, UpdateType.SyncInvoice,"");

                else if (!isWholesale && !CheckInvoiceExist(sInvoice.Sid, "OINV"))
                    await HandleInvoices(invoiceResult.EntityList, UpdateType.SyncWholesale, wholesaleCustomerCode);
            }

        }
        else
        {
            Log($"No Available invoice/s." +
                        $"\r\nResponse: {salesInvoices.Response.Content} " +
                        $"\r\nResult Message: {salesInvoices.Message}");
        }
    }

    private async Task HandleReturnInvoices()
    {
        var returnInvoices = new RequestResult<PrismInvoice>();
        string filterByDateRang = GetSyncQuery();

        returnInvoices = await _invoiceService.GetInvoicesAsync(OutboundDocuments.ReturnInvoice, filterByDateRang, -1);

        if (returnInvoices.EntityList.Any())
        {
            foreach (var rInvoice in returnInvoices.EntityList)
            {
                var invoiceResult = await _invoiceService.GetInvoiceDetailsBySidAsync(rInvoice.Sid);
                var returnInvoice = invoiceResult.EntityList.FirstOrDefault();
                Log($"Return invoice/s." +
                            $"\r\nFirst Request Message: {returnInvoices.Message}" +
                            $"\r\n\r\nSecond Request Message: {invoiceResult.Message}");

                if (returnInvoice == null) continue;

                if (CheckInvoiceExist(returnInvoice.Sid, "ORIN"))
                {
                    var docNum = GetReturnInvoiceDocNum(returnInvoice.Sid);

                    Log($"\r\nPrism Invoice No. ({returnInvoice.DocumentNumber}) is Already Exist with SAP A/R Credit Payment No. ({docNum}).");
                }
                var isCreditMemo = returnInvoice.Items.Any(p => p.Alu == "SP0012");

                if (isCreditMemo && !CheckInvoiceExist(returnInvoice.Sid, "ORIN"))
                    await HandleCreditMemo(invoiceResult.EntityList);


                else if (!isCreditMemo && !CheckInvoiceExist(returnInvoice.Sid, "ORIN"))
                    await HandleInvoiceReturn(invoiceResult.EntityList);

            }
        }
        else
        {
            Log($"No Available Return invoice/s." +
                        $"\r\nResponse: {returnInvoices.Response.Content} " +
                        $"\r\nResult Message: {returnInvoices.Message}" +
                        $"\r\nStatus: {returnInvoices.Status}");
        }
    }
    private async Task HandleCreditMemo(List<PrismInvoice> invoicesList)
    {

        await foreach (var syncResult in _creditMemoHandler.AddCreditMemoAsync(invoicesList))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var invoice in syncResult.EntityList)
                {
                    //dt.BringToFront();
                    //dt.Visible = true;
                }
            }

            if (textBoxLogsSchedule.Text.Contains(syncResult.Message))
                return;
            Log(syncResult.Message);

            //syncResult.UpdateResponse;
        }
    }
    private async Task HandleInvoiceReturn(List<PrismInvoice> invoicesList)
    {
        //var bindingList = dt.DataSource as BindingList<SAPInvoice>;

        await foreach (var syncResult in _returnsHandler.AddReturnInvoiceAsync(invoicesList))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var invoice in syncResult.EntityList)
                {
                    //dt.BindInvoices(ref bindingList, invoice);
                    //treeView.BindInvoices(ref bindingList, invoice);

                    //Log(UpdateType, syncResult.Message, syncResult.StatusBarMessage);
                }
            }

            if (textBoxLogsSchedule.Text.Contains(syncResult.Message))
                return;
            Log(syncResult.Message);
        }
    }

    private async Task HandleDownPayment(List<PrismInvoice> invoicesList)
    {

        await foreach (var syncResult in _downPaymentHandler.AddDownPaymentAsync(invoicesList))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var invoice in syncResult.EntityList)
                {
                    //if (textBoxLogsSchedule.Text.Contains(syncResult.Message))
                    //    return;
                    //Log(syncResult.Message);
                }
            }

            if (textBoxLogsSchedule.Text.Contains(syncResult.Message))
                return;
            Log(syncResult.Message);
        }
    }
    private async Task HandleInvoices(List<PrismInvoice> invoicesList, UpdateType updateType, string wholesaleCustomerCode)
    {
        await foreach (var syncResult in _invoiceHandler.AddSalesInvoiceAsync(invoicesList, updateType, wholesaleCustomerCode))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var invoice in syncResult.EntityList)
                {
                    //if (textBoxLogsSchedule.Text.Contains(syncResult.Message))
                    //    return;
                    //Log(syncResult.Message);
                }


                if (textBoxLogsSchedule.Text.Contains(syncResult.Message))
                    return;
                Log(syncResult.Message);
            }
        }
    }

    private async Task HandleStockTransfer()
    {
        var verifiedVouchers = new RequestResult<VerifiedVoucher>();

        var dateFrom = DateTime.Now.AddYears(-5);
        var dateTo = DateTime.Now;

        verifiedVouchers = await _verifiedVoucherService.GetVerifiedVoucher(dateFrom, dateTo, -1);


        if (verifiedVouchers.EntityList.Any())
        {
            foreach (var verifiedVoucher in verifiedVouchers.EntityList)
            {
                Log($"\r\nStock Transfer/s." + $"\r\nRequest Message: {verifiedVouchers.Message}");

                if (verifiedVoucher == null) continue;

                if (!CheckStockTransferExist(verifiedVoucher.Sid))// To-Do Add Check Exist in SAP by Sid in field U_PrismSid and U_SyncToPrism
                    await HandleVerifiedVoucher(verifiedVoucher);
                else
                {
                    var docNum = GetStockTransferDocNum(verifiedVoucher.Sid);

                    Log($"Prism Voucher No. ({verifiedVoucher.Vouno}) Slip No ({verifiedVoucher.Slipno}) is Already Exist with SAP Stock Transfer No. ({docNum}).");
                }
            }
        }
        else
        {
            Log($"No Available Verified Voucher/s." +
                        $"\r\nResponse:\r\n{verifiedVouchers.Response.Content.PrettyJson()} " +
                        $"\r\n\r\nResult Message: {verifiedVouchers.Message}" +
                        $"\r\nStatus: {verifiedVouchers.Status}");
        }
    }
    private async Task HandleVerifiedVoucher(VerifiedVoucher verifiedVoucher)
    {

        await foreach (var syncResult in _verifiedVoucherHandler.SyncVerifiedVoucher(verifiedVoucher))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                //if (textBoxLogsSchedule.Text.Contains(syncResult.Message))
                //    return;
                //Log(syncResult.Message);
            }


            if (textBoxLogsSchedule.Text.Contains(syncResult.Message))
                return;
            Log(syncResult.Message);
        }
    }
    private static bool CheckStockTransferExist(string voucherSid)
    {
        var query = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM OWTR WHERE U_PrismSid = '{voucherSid}') THEN 1 ELSE 0 END";

        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        oRecordSet.DoQuery(query);

        if (oRecordSet.RecordCount > 0)
            return oRecordSet.Fields.Item(0).Value.ToString() == "1";

        return false;
    }
    private static string GetStockTransferDocNum(string voucherSid)
    {
        var query = $"SELECT DocNum FROM OWTR WHERE U_PrismSid = '{voucherSid}'";

        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        oRecordSet.DoQuery(query);

        return oRecordSet.RecordCount > 0
            ? oRecordSet.Fields.Item(0).Value.ToString()
            : "";
    }
    private string GetSyncQueryByRangOfDate()
    {
        var dateFrom = DateTime.Now.AddYears(-5).ToSAPDateFormat();
        var dateTo = DateTime.Now.ToSAPDateFormat();

        return $" WHERE T0.[DocDate] Between '{dateFrom}' AND '{dateTo}' AND (T0.[U_SyncToPrism] IS NULL OR  T0.[U_SyncToPrism] = '' OR  T0.[U_SyncToPrism] = 'N')";
    }
    private string GetSyncQuery()
    {
        var dateFrom = DateTime.Now.AddYears(-5).ToPrismFromDateFormat();
        var dateTo = DateTime.Now.ToPrismToDateFormat();

        return $"AND(invoice_posted_date,ge,{dateFrom})AND(invoice_posted_date,le,{dateTo})";
    }

    private static bool CheckInvoiceExist(string invoiceSid, string table)
    {
        var query = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM {table} WHERE U_PrismSid = '{invoiceSid}') THEN 1 ELSE 0 END";

        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        oRecordSet.DoQuery(query);

        if (oRecordSet.RecordCount > 0)
            return oRecordSet.Fields.Item(0).Value.ToString() == "1";

        return false;
    }
    private static string GetInvoiceDocNum(string invoiceSid, string table)
    {
        var query = $"SELECT DocNum FROM {table} WHERE U_PrismSid = '{invoiceSid}'";

        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        oRecordSet.DoQuery(query);

        return oRecordSet.RecordCount > 0
            ? oRecordSet.Fields.Item(0).Value.ToString()
            : "";
    }

    private static bool CheckReturnInvoiceExist(string invoiceSid)
    {
        var query = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM ORIN WHERE U_PrismSid = '{invoiceSid}') THEN 1 ELSE 0 END";

        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        oRecordSet.DoQuery(query);

        if (oRecordSet.RecordCount > 0)
            return oRecordSet.Fields.Item(0).Value.ToString() == "1";

        return false;
    }
    private static string GetReturnInvoiceDocNum(string invoiceSid)
    {
        var query = $"SELECT DocNum FROM ORIN WHERE U_PrismSid = '{invoiceSid}'";

        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        oRecordSet.DoQuery(query);

        return oRecordSet.RecordCount > 0
            ? oRecordSet.Fields.Item(0).Value.ToString()
            : "";
    }
    private static bool CheckInventoryPostingExist(string invoiceSid)
    {
        var query = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM OIQR WHERE U_PrismSid = '{invoiceSid}') THEN 1 ELSE 0 END";

        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        oRecordSet.DoQuery(query);

        if (oRecordSet.RecordCount > 0)
            return oRecordSet.Fields.Item(0).Value.ToString() == "1";

        return false;
    }
    private static string GetInventoryPostingDocNum(string invoiceSid)
    {
        var query = $"SELECT DocNum FROM OIQR WHERE U_PrismSid = '{invoiceSid}'";

        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        oRecordSet.DoQuery(query);

        return oRecordSet.RecordCount > 0
            ? oRecordSet.Fields.Item(0).Value.ToString()
            : "";
    }
    private void Log(string message)
    {
        //textBoxLogsSchedule.Clear();
        textBoxLogsSchedule.AppendText(message);
    }
    private void ComboBoxDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedIndex = comboBoxDocTypeSchedule.SelectedIndex;
        textBoxLogsSchedule.Clear();

        buttonScheduleIt.Enabled = true;

        var schedule = GetSchedule(selectedIndex);

        if (schedule.Times.Any())
        {
            foreach (var recurring in schedule.Times)
            {
                if (recurring.TimeId == 1)
                {
                    if (recurring.Active)
                    {
                        int indexToSelect = FindComboBoxIndexByTime(comboBoxHour1, recurring.Time.ToString("hh:mm tt"));

                        if (indexToSelect != -1)
                            comboBoxHour1.SelectedIndex = indexToSelect;

                        checkBox1.Checked = recurring.Active;
                    }
                    else
                    {
                        comboBoxHour1.SelectedIndex = 0;
                        checkBox1.Checked = recurring.Active;
                    }
                }


                if (recurring.TimeId == 2)
                {
                    if (recurring.Active)
                    {
                        int indexToSelect = FindComboBoxIndexByTime(comboBoxHour2, recurring.Time.ToString("hh:mm tt"));

                        if (indexToSelect != -1)
                            comboBoxHour2.SelectedIndex = indexToSelect;

                        checkBox2.Checked = recurring.Active;
                    }
                    else
                    {
                        comboBoxHour2.SelectedIndex = 0;
                        checkBox2.Checked = recurring.Active;
                    }
                }

                if (recurring.TimeId == 3)
                {
                    if (recurring.Active)
                    {
                        int indexToSelect = FindComboBoxIndexByTime(comboBoxHour3, recurring.Time.ToString("hh:mm tt"));

                        if (indexToSelect != -1)
                            comboBoxHour3.SelectedIndex = indexToSelect;

                        checkBox3.Checked = recurring.Active;
                    }
                    else
                    {
                        comboBoxHour3.SelectedIndex = 0;
                        checkBox3.Checked = recurring.Active;
                    }
                }
            }
        }
        else
        {
            comboBoxHour1.SelectedIndex = 0;
            checkBox1.Checked = false;

            comboBoxHour2.SelectedIndex = 0;
            checkBox2.Checked = false;

            comboBoxHour3.SelectedIndex = 0;
            checkBox3.Checked = false;
        }
    }

    private static Schedules GetSchedule(int selectedIndex)
    {
        var schedule = new Schedules();

        switch (selectedIndex)
        {
            case (int)SyncDocuments.Departments:
                schedule = Schedules.Find(x => x.Document == SyncDocuments.Departments);
                break;

            case (int)SyncDocuments.Items:
                schedule = Schedules.Find(x => x.Document == SyncDocuments.Items);
                break;

            case (int)SyncDocuments.Vendors:
                schedule = Schedules.Find(x => x.Document == SyncDocuments.Vendors);
                break;

            case (int)SyncDocuments.GoodsReceiptPos:
                schedule = Schedules.Find(x => x.Document == SyncDocuments.GoodsReceiptPos);
                break;

            case (int)SyncDocuments.GoodsReceipts_Inbound:
                schedule = Schedules.Find(x => x.Document == SyncDocuments.GoodsReceipts_Inbound);
                break;

            case (int)SyncDocuments.SalesInvoices:
                schedule = Schedules.Find(x => x.Document == SyncDocuments.SalesInvoices);
                break;

            case (int)SyncDocuments.ReturnInvoices:
                schedule = Schedules.Find(x => x.Document == SyncDocuments.ReturnInvoices);
                break;

            case (int)SyncDocuments.CustomerOrders:
                schedule = Schedules.Find(x => x.Document == SyncDocuments.CustomerOrders);
                break;

            case (int)SyncDocuments.StockTransfers:
                schedule = Schedules.Find(x => x.Document == SyncDocuments.StockTransfers);
                break;

            case (int)SyncDocuments.InventoryPosting:
                schedule = Schedules.Find(x => x.Document == SyncDocuments.InventoryPosting);
                break;

            case (int)SyncDocuments.GoodsReceipts_Outbound:
                schedule = Schedules.Find(x => x.Document == SyncDocuments.GoodsReceipts_Outbound);
                break;
            case (int)SyncDocuments.GoodsIssues_Outbound:
                schedule = Schedules.Find(x => x.Document == SyncDocuments.GoodsIssues_Outbound);
                break;

        }

        return schedule;
    }
    private void guna2ControlBox1_Click(object sender, EventArgs e)
    {
        Helper.TryKillProcess();
    }

    private void MinimizeButton_Click(object sender, EventArgs e)
    {
        WindowState = FormWindowState.Minimized;
        notifyIcon1.ShowBalloonTip(1);
        notifyIcon1.Visible = true;
        ShowInTaskbar = false;
        Hide();
        hidden = true;
    }

    private void MainPanel_DoubleClick(object sender, EventArgs e) => ResizeForm();
    void ResizeForm()
    {
        if (WindowState == FormWindowState.Normal)
        {
            WindowState = FormWindowState.Maximized;
            guna2Elipse1.BorderRadius = 0;
        }
        else
        {
            WindowState = FormWindowState.Normal;
            guna2Elipse1.BorderRadius = 30;
        }
    }

    private void guna2ControlBox3_Click(object sender, EventArgs e)
    {
        guna2Elipse1.BorderRadius = WindowState == FormWindowState.Normal ? 30 : 0;
    }
    private void MenuRefreshAuth_Click(object sender, EventArgs e)
        => RefreshAuthSession();

    private async void RefreshAuthSession()
    {
        var newAuth = await LoginManager.GetAuthSessionAsync(_credentials.BaseUri, _credentials.PrismUserName, _credentials.PrismPassword);
        //if (newAuth.IsHasValue())
        //labelStatus.Log("Status: Auth Session refreshed.", Logger.MessageTypes.Warning, Logger.MessageTime.Long);
        //else
        ///labelStatus.Log("Status: Cant refresh Auth-Session, Wait a few seconds before you try again.", Logger.MessageTypes.Error, Logger.MessageTime.Long);
    }

    private void notifyIcon1_Click(object sender, EventArgs e)
    {
        if (hidden)
        {
            Show();
            WindowState = FormWindowState.Maximized;
            hidden = false;
            ShowInTaskbar = true;
        }
        else
        {
            Hide();
            hidden = true;
        }
    }

    private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
    {
        if (hidden)
        {
            Show();
            WindowState = FormWindowState.Maximized;
            hidden = false;
            ShowInTaskbar = true;
        }
        else
        {
            Hide();
            hidden = true;
        }
    }

    private void buttonSaveToDatabase_Click(object sender, EventArgs e)
    {
        if (buttonScheduleIt.Enabled)
        {
            var document = (SyncDocuments)comboBoxDocTypeSchedule.SelectedIndex;

            try
            {
                var schedule = Schedules.Find(x => x.Document == document);
                if (schedule != null 
                    && checkBox1.Checked && comboBoxHour1.Text.IsHasValue()
                    && checkBox2.Checked && comboBoxHour2.Text.IsHasValue()
                    && checkBox3.Checked && comboBoxHour3.Text.IsHasValue()
                    && checkBox4.Checked && comboBoxHour4.Text.IsHasValue()
                    )
                {
                    schedule.Document = document;

                    var rec1 = schedule.Times.Find(t => t.TimeId == 1);

                    var recurring1 = comboBoxHour1.Text.To24HourMinutesFormat();

                    rec1.Time = new TimeOnly(recurring1.Hours, recurring1.Minutes);
                    rec1.Active = checkBox1.Checked;

                    var recurring2 = comboBoxHour1.Text.To24HourMinutesFormat();

                    var rec2 = schedule.Times.Find(t => t.TimeId == 2);
                    rec2.Time = new TimeOnly(recurring2.Hours, recurring2.Minutes);
                    rec2.Active = checkBox1.Checked;

                    var recurring3 = comboBoxHour1.Text.To24HourMinutesFormat();

                    var rec3 = schedule.Times.Find(t => t.TimeId == 3);
                    rec3.Time = new TimeOnly(recurring3.Hours, recurring3.Minutes);
                    rec3.Active = checkBox1.Checked;

                    schedule.Times.Clear();
                    schedule.Times.Add(rec1);
                    schedule.Times.Add(rec2);
                    schedule.Times.Add(rec3);

                    _unitOfWork.Schedule.Update(schedule);
                    _unitOfWork.SaveChanges();

                    dataGridView1.DataSource = GetSchedulesViewModel();

                    labelStatus.Log("Selected Schedule is updated to database.", Logger.MessageTypes.Warning, Logger.MessageTime.Short);
                }
                else
                    labelStatus.Log("Kindly Check at least 1 and select a valid time.", Logger.MessageTypes.Error, Logger.MessageTime.Short);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }

    private void ScheduledSyncs_Load(object sender, EventArgs e)
    {
       dataGridView1.DataSource = GetSchedulesViewModel();
    }

    private List<ScheduleViewModel> GetSchedulesViewModel()
    {
        string[] includes = { "Recurring" };
        return _unitOfWork.Schedule.GetAll(includes).Select(schedule =>
        {
            var model = new ScheduleViewModel
            {
                Id = schedule.Id,
                DocumentName = schedule.DocumentName
            };

            if (schedule.Times.Count > 0)
                model.Times = string.Join(", ", schedule.Times.Select(t => t.Time.ToString()));
            else
                model.Times = "-- Not Defined --";

            return model;
        }).ToList();
    }

    private void InitializeDataGridView()
    {
        dataGridView1.AutoGenerateColumns = false;

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Id",
            HeaderText = "Schedule ID"
        });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "DocumentName",
            HeaderText = "Document Name"
        });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Times",
            HeaderText = "Recurring Times"
        });
    }

    public class ScheduleViewModel
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public string Times { get; set; }
    }

 

    private void dataGridView1_SelectionChanged(object sender, EventArgs e)
    {
        if (dataGridView1.SelectedRows.Count > 0)
        {
            // Get the selected ScheduleViewModel
            ScheduleViewModel selectedSchedule = dataGridView1.SelectedRows[0].DataBoundItem as ScheduleViewModel;

            // Now, you can access the Id of the selected item
            int selectedId = selectedSchedule.Id;

            // Set the ComboBox selection based on the selectedId
            comboBoxDocTypeSchedule.SelectedIndex = selectedId;
        }
    }
}