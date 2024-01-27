using SAPLink.Core.Models.Prism.StockManagement;
using SAPLink.Handler.Prism.Handlers.OutboundData.StockManagement.InventoryPosting;
using InventoryTransferService = SAPLink.Handler.Prism.Handlers.OutboundData.StockManagement.InventoryTransfer.InventoryTransferService;
using InventoryTransferHandler = SAPLink.Handler.Prism.Handlers.OutboundData.StockManagement.InventoryTransfer.InventoryTransferHandler;
using SAPLink.Utilities.Forms;
using InventoryPosting = SAPLink.Core.Models.Prism.StockManagement.InventoryPosting;
using SAPLink.Handler.SAP.Application;
using SAPLink.Handler.Prism.Connection.Auth;
using SAPLink.Core.Connection;
using SAPLink.EF.Data;

namespace SAPLink.Forms;

public partial class OutboundData : Form
{
    #region Initial Variables

    private readonly UnitOfWork _unitOfWork;
    private readonly ServiceLayerHandler _serviceLayer;
    private readonly Clients _client;
    private readonly Credentials _credentials;
    private readonly ItemsService _itemsService;
    private readonly InvoiceHandler _invoiceHandler;
    private readonly Handler.Prism.Handlers.OutboundData.PointOfSale.Handler _handler;
    private readonly OrdersHandler _ordersHandler;
    private readonly DownPaymentHandler _downPaymentHandler;
    private readonly CreditMemoHandler _creditMemoHandler;
    private readonly ReturnsHandler _returnsHandler;
    private readonly InvoiceService _invoiceService;
    private readonly DepartmentService _departmentService;

    private readonly InventoryTransferService _inventoryTransferService;
    private readonly InventoryTransferHandler _verifiedVoucherHandler;

    private readonly InventoryPostingService _inventoryPostingService;
    private readonly InventoryPostingHandler _inventoryPostingHandler;

    public OutboundData(UnitOfWork unitOfWork, ServiceLayerHandler serviceLayer, DepartmentService departmentService,
        ItemsService itemsService, Clients client)
    {
        InitializeComponent();
        _unitOfWork = unitOfWork;
        _serviceLayer = serviceLayer;
        _client = client;
        _credentials = _client.Credentials.FirstOrDefault();

        _invoiceService = new InvoiceService(_client);
        _handler = new Handler.Prism.Handlers.OutboundData.PointOfSale.Handler(_client);
        _invoiceHandler = new InvoiceHandler(_serviceLayer);
        _returnsHandler = new ReturnsHandler(_client, _serviceLayer);
        _ordersHandler = new OrdersHandler(_serviceLayer);
        _downPaymentHandler = new DownPaymentHandler(_client, _serviceLayer);
        _creditMemoHandler = new CreditMemoHandler(_client);

        _inventoryTransferService = new InventoryTransferService(_client);
        _verifiedVoucherHandler = new InventoryTransferHandler(_client);

        _inventoryPostingService = new InventoryPostingService(_client);
        _inventoryPostingHandler = new InventoryPostingHandler();

        _itemsService = itemsService;
        _departmentService = departmentService;
    }


    #endregion

    private async void buttonSyncNow_Click(object sender, EventArgs e)
    {
        var documentType = (OutboundDocuments)comboBoxDocTypeSync.SelectedIndex;

        var storeNum = -1;
        if (comboBoxBranch.SelectedIndex != 0)
            storeNum = (int)comboBoxBranch.SelectedValue;


        var filterByDateRang = GetSyncQuery();

        var docCode = "";
        if (checkBoxDocCode.Checked)
            docCode = textBoxDocCode.Text;

        textBoxLogsSync.Clear();
        dataGridView.DataSource = null;

        switch (documentType)
        {
            case OutboundDocuments.SalesInvoice:
                {
                    var salesInvoices = new RequestResult<PrismInvoice>();

                    if (docCode.IsHasValue())
                        salesInvoices = await _invoiceService.GetInvoicesAsync(OutboundDocuments.SalesInvoice, filterByDateRang, storeNum, docCode);
                    else
                        salesInvoices = await _invoiceService.GetInvoicesAsync(OutboundDocuments.SalesInvoice, filterByDateRang, storeNum);

                    if (salesInvoices.EntityList.Any())
                    {
                        foreach (var sInvoice in salesInvoices.EntityList)
                        {
                            var invoiceResult = await _invoiceService.GetInvoiceDetailsBySidAsync(sInvoice.Sid);
                            var invoice = invoiceResult.EntityList.FirstOrDefault();

                            Log(UpdateType.SyncInvoice, $"Invoice/s." +
                                        $"\r\nFirst Request Message {salesInvoices.Message}" +
                                        $"\r\n\r\nSecond Request Message: {invoiceResult.Message}",
                                "");

                            if (invoice == null) continue;

                            bool isInvoiceHasReturnItem = invoice.Items.Any(p => p.Alu == "SP0012");


                            if (isInvoiceHasReturnItem && CheckInvoiceExist(sInvoice.Sid, "ODPI"))
                            {
                                var docNum = GetInvoiceDocNum(sInvoice.Sid, "ODPI");

                                Log(UpdateType.SyncInvoice,
                                    $"\r\nPrism Invoice No. ({sInvoice.DocumentNumber}) is Already Exist with SAP A/r Down Payment No. ({docNum}).",
                                     "");
                            }

                            else if (!isInvoiceHasReturnItem && CheckInvoiceExist(sInvoice.Sid, "OINV"))
                            {
                                var docNum = GetInvoiceDocNum(sInvoice.Sid, "OINV");

                                Log(UpdateType.SyncInvoice,
                                    $"\r\nPrism Invoice No. ({sInvoice.DocumentNumber}) is Already Exist with SAP Invoice No. ({docNum}).",
                                     "");
                            }

                            var isWholesale = invoice.IsWholesale == "B2P";
                            var wholesaleCustomerCode = invoice.WholesaleCustomerCode;


                            if (IsDownPayment(sInvoice, isInvoiceHasReturnItem))
                                await HandleDownPayment(invoiceResult.EntityList, UpdateType.SyncInvoice);

                            else if (IsRetailSale(sInvoice, isInvoiceHasReturnItem, isWholesale, wholesaleCustomerCode))
                                await HandleInvoices(invoiceResult.EntityList, UpdateType.SyncInvoice);

                            else if (IsWholesaleRetail(sInvoice, isInvoiceHasReturnItem, isWholesale, wholesaleCustomerCode))
                                await HandleInvoices(invoiceResult.EntityList, UpdateType.SyncWholesaleRetail);

                            else if (IsWholesale(sInvoice, isWholesale))
                                await HandleInvoices(invoiceResult.EntityList, UpdateType.SyncWholesale, wholesaleCustomerCode);
                        }

                    }
                    else
                    {
                        dataGridView.DataSource = null;
                        LogMessages($"No Available invoice/s." +
                                    $"\r\nResponse: {salesInvoices.Response.Content} " +
                                    $"\r\nResult Message: {salesInvoices.Message}" +
                                    $"\r\nStatus: {salesInvoices.Status}",
                            $"No Available invoice/s.");
                    }

                    break;
                }
            case OutboundDocuments.ReturnInvoice:
                {
                    var returnInvoices = new RequestResult<PrismInvoice>();


                    if (docCode.IsHasValue())
                        returnInvoices = await _invoiceService.GetInvoicesAsync(OutboundDocuments.ReturnInvoice, filterByDateRang, storeNum, docCode);
                    else
                        returnInvoices = await _invoiceService.GetInvoicesAsync(OutboundDocuments.ReturnInvoice, filterByDateRang, storeNum);

                    if (returnInvoices.EntityList.Any())
                    {
                        foreach (var rInvoice in returnInvoices.EntityList)
                        {
                            var invoiceResult = await _invoiceService.GetInvoiceDetailsBySidAsync(rInvoice.Sid);
                            var returnInvoice = invoiceResult.EntityList.FirstOrDefault();
                            Log(UpdateType.SyncCreditMemo, $"Return invoice/s." +
                                        $"\r\nFirst Request Message: {returnInvoices.Message}" +
                                        $"\r\n\r\nSecond Request Message: {invoiceResult.Message}",
                                "");

                            if (returnInvoice == null) continue;

                            if (CheckInvoiceExist(returnInvoice.Sid, "ORIN"))
                            {
                                var docNum = GetReturnInvoiceDocNum(returnInvoice.Sid);

                                Log(UpdateType.SyncCreditMemo, $"\r\nPrism Invoice No. ({returnInvoice.DocumentNumber}) is Already Exist with SAP A/R Credit Memo No. ({docNum}).",
                                     "");
                            }

                            var isWholesale = returnInvoice.IsWholesale == "B2P";
                            var wholesaleCustomerCode = returnInvoice.WholesaleCustomerCode;

                            var isHasReturnItem = returnInvoice.Items.Any(p => p.Alu == "SP0012");

                            if (isHasReturnItem && !CheckInvoiceExist(returnInvoice.Sid, "ORIN"))
                                await HandleCreditMemoWithoutPayment(invoiceResult.EntityList, UpdateType.SyncInvoice);

                            else if (!isHasReturnItem && isWholesale && wholesaleCustomerCode.IsHasValue() && !CheckInvoiceExist(returnInvoice.Sid, "ORIN"))
                                await HandleCreditMemoWithPayment(invoiceResult.EntityList, UpdateType.SyncWholesale, wholesaleCustomerCode);

                            else if (!isHasReturnItem && isWholesale && wholesaleCustomerCode.IsNullOrEmpty() && !CheckInvoiceExist(returnInvoice.Sid, "ORIN"))
                                await HandleCreditMemoWithPayment(invoiceResult.EntityList, UpdateType.SyncWholesaleRetail);

                            else if (!isHasReturnItem && !CheckInvoiceExist(returnInvoice.Sid, "ORIN"))
                                await HandleCreditMemoWithPayment(invoiceResult.EntityList, UpdateType.SyncCreditMemo);

                        }
                    }
                    else
                    {
                        dataGridView.DataSource = null;
                        LogMessages($"No Available Return invoice/s." +
                                    $"\r\nResponse: {returnInvoices.Response.Content} " +
                                    $"\r\nResult Message: {returnInvoices.Message}" +
                                    $"\r\nStatus: {returnInvoices.Status}",
                            $"No Available Return invoice/s.");
                    }
                }
                break;
            case OutboundDocuments.CustomerOrder:
                {
                    var customerOrders = new RequestResult<PrismInvoice>();


                    if (docCode.IsHasValue())
                        customerOrders = await _invoiceService.GetInvoicesAsync(OutboundDocuments.CustomerOrder, filterByDateRang, storeNum, docCode);
                    else
                        customerOrders = await _invoiceService.GetInvoicesAsync(OutboundDocuments.CustomerOrder, filterByDateRang, storeNum);


                    if (customerOrders.EntityList.Any())
                    {
                        foreach (var sInvoice in customerOrders.EntityList)
                        {
                            var order = await _invoiceService.GetInvoiceDetailsBySidAsync(sInvoice.Sid);

                            LogMessages($"Order/s." +
                                        $"\r\nFirst Request Message: {customerOrders.Message}" +
                                        $"\r\n\r\nSecond Request Message: {order.Message}",
                                "");

                            await HandleOrders(order.EntityList, UpdateType.SyncInvoice);
                        }

                    }
                    else
                    {
                        dataGridView.DataSource = null;
                        LogMessages($"No Available Order/s." +
                                    $"\r\nResponse: {customerOrders.Response.Content} " +
                                    $"\r\nResult Message: {customerOrders.Message}" +
                                    $"\r\nStatus: {customerOrders.Status}",
                            $"No Available invoice/s.");
                    }
                }
                break;

            case OutboundDocuments.StockTransfer:
                {
                    var verifiedVouchers = new RequestResult<VerifiedVoucher>();
                    int pageNumber = 1; // Set the initial page number
                    int pageSize = 30; // Set the page size

                    do
                    {
                        if (docCode.IsHasValue())
                            verifiedVouchers = await _inventoryTransferService.GetVerifiedVoucher(dateTimePickerFrom.Value, dateTimePickerTo.Value, storeNum, pageNumber, docCode);
                        else
                            verifiedVouchers = await _inventoryTransferService.GetVerifiedVoucher(dateTimePickerFrom.Value, dateTimePickerTo.Value, pageNumber, storeNum);
                        if (verifiedVouchers.EntityList.Any())
                        {

                            foreach (var verifiedVoucher in verifiedVouchers.EntityList)
                            {
                                Log(UpdateType.SyncInventoryTransfer,
                                    $"\r\nStock Transfer/s." + $"\r\nRequest Message: {verifiedVouchers.Message}", "");

                                if (verifiedVoucher == null) continue;

                                if (!CheckStockTransferExist(verifiedVoucher.Sid)) // To-Do Add Check Exist in SAP by Sid in field U_PrismSid and U_SyncToPrism
                                    await HandleVerifiedVoucher(verifiedVoucher, UpdateType.SyncInvoice);
                                else
                                {
                                    var docNum = GetStockTransferDocNum(verifiedVoucher.Sid);

                                    Log(UpdateType.SyncInventoryTransfer,
                                        $"Prism Voucher No. ({verifiedVoucher.Vouno}) Slip No ({verifiedVoucher.Slipno}) is Already Exist with SAP Stock Transfer No. ({docNum}).",
                                        "");
                                }
                            }

                        }
                        else
                            break;

                    }
                    while (verifiedVouchers.EntityList.Count == pageSize); // Continue until there are no more results

                    if (!verifiedVouchers.EntityList.Any())
                    {
                        dataGridView.DataSource = null;
                        LogMessages($"No Available Verified Voucher/s." +
                                    $"\r\nResponse:\r\n{verifiedVouchers.Response.Content.PrettyJson()} " +
                                    $"\r\n\r\nResult Message: {verifiedVouchers.Message}" +
                                    $"\r\nStatus: {verifiedVouchers.Status}",
                            $"No Available Verified Voucher/s.");
                    }
                }
                break;

            case OutboundDocuments.InventoryPosting:
                await ProcessOutboundDocument(OutboundDocuments.InventoryPosting, storeNum, "AND(creatingdoctype,eq,1)AND(status,eq,4)AND(adjtype,eq,0)", docCode);
                break;

            case OutboundDocuments.GoodsReceipt:
                await ProcessOutboundDocument(OutboundDocuments.GoodsReceipt, storeNum, "AND(creatingdoctype,eq,8)AND(reasonname,eq,GR)", docCode);
                break;

            case OutboundDocuments.GoodsIssue:
                await ProcessOutboundDocument(OutboundDocuments.GoodsIssue, storeNum, "AND(creatingdoctype,eq,8)AND(reasonname,eq,GI)", docCode);
                break;
        }
    }

    private static bool IsWholesale(PrismInvoice sInvoice, bool isWholesale)
    {
        return isWholesale && !CheckInvoiceExist(sInvoice.Sid, "OINV");
    }

    private static bool IsWholesaleRetail(PrismInvoice sInvoice, bool isInvoiceHasReturnItem, bool isWholesale, string wholesaleCustomerCode)
    {
        return !isInvoiceHasReturnItem && isWholesale && wholesaleCustomerCode.IsNullOrEmpty() && !CheckInvoiceExist(sInvoice.Sid, "OINV");
    }

    private static bool IsRetailSale(PrismInvoice sInvoice, bool isInvoiceHasReturnItem, bool isWholesale, string wholesaleCustomerCode)
    {
        return !isInvoiceHasReturnItem && !isWholesale && wholesaleCustomerCode.IsNullOrEmpty() && !CheckInvoiceExist(sInvoice.Sid, "OINV");
    }

    private static bool IsDownPayment(PrismInvoice sInvoice, bool isARDownPayment)
    {
        return isARDownPayment && !CheckInvoiceExist(sInvoice.Sid, "ODPI");
    }

    private async Task HandleInvoices(List<PrismInvoice> invoicesList, UpdateType updateType, string wholesaleCustomerCode = "")
    {
        PlaySound.Click();
        var bindingList = dataGridView.DataSource as BindingList<SAPInvoice>;

        await foreach (var syncResult in _invoiceHandler.AddSalesInvoiceAsync(invoicesList, updateType, wholesaleCustomerCode))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var invoice in syncResult.EntityList)
                {
                    dataGridView.BindInvoices(ref bindingList, invoice);
                    treeView1.BindInvoices(ref bindingList, invoice);

                    //Log(UpdateType, syncResult.Message, syncResult.StatusBarMessage);
                }
            }
            else
            {
                dataGridView.DataSource = null;
                treeView1.Nodes.Clear();
            }


            if (textBoxLogsSync.Text.Contains(syncResult.Message))
                return;

            if (_credentials.ActiveLog)
                Log(updateType, syncResult.Message, syncResult.StatusBarMessage);

            //syncResult.UpdateResponse;
        }
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
    private async Task HandleCreditMemoWithPayment(List<PrismInvoice> invoicesList, UpdateType updateType, string wholesaleCustomerCode = null)
    {
        PlaySound.Click();
        var bindingList = dataGridView.DataSource as BindingList<SAPInvoice>;

        await foreach (var syncResult in _returnsHandler.AddReturnInvoiceAsync(invoicesList, updateType, wholesaleCustomerCode))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var invoice in syncResult.EntityList)
                {
                    dataGridView.BindInvoices(ref bindingList, invoice);
                    treeView1.BindInvoices(ref bindingList, invoice);

                    //Log(UpdateType, syncResult.Message, syncResult.StatusBarMessage);
                }
            }
            else
            {
                dataGridView.DataSource = null;
                treeView1.Nodes.Clear();
            }

            if (textBoxLogsSync.Text.Contains(syncResult.Message))
                return;

            if (_credentials.ActiveLog)
                Log(updateType, syncResult.Message, syncResult.StatusBarMessage);

            //syncResult.UpdateResponse;
        }
    }
    private async Task HandleOrders(List<PrismInvoice> invoicesList, UpdateType UpdateType)
    {
        PlaySound.Click();
        var bindingList = dataGridView.DataSource as BindingList<SAPInvoice>;

        await foreach (var syncResult in _ordersHandler.AddOrdersAsync(invoicesList))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var invoice in syncResult.EntityList)
                {
                    dataGridView.BindInvoices(ref bindingList, invoice);
                    treeView1.BindInvoices(ref bindingList, invoice);

                    //Log(UpdateType, syncResult.Message, syncResult.StatusBarMessage);
                }
            }
            else
            {
                dataGridView.DataSource = null;
                treeView1.Nodes.Clear();
            }

            if (textBoxLogsSync.Text.Contains(syncResult.Message))
                return;

            if (_credentials.ActiveLog)
                Log(UpdateType, syncResult.Message, syncResult.StatusBarMessage);

            //syncResult.UpdateResponse;
        }
    }

    private async Task HandleVerifiedVoucher(VerifiedVoucher verifiedVoucher, UpdateType updateType)
    {
        PlaySound.Click();
        var bindingList = dataGridView.DataSource as BindingList<VerifiedVoucher>;

        await foreach (var syncResult in _verifiedVoucherHandler.SyncVerifiedVoucher(verifiedVoucher))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                dataGridView.BringToFront();
                dataGridView.Visible = true;
                dataGridView.BindVerifiedVouchers(ref bindingList, verifiedVoucher);
                dataGridView.SelectLastRow();

                //treeView.BindVerifiedVouchers(ref bindingList, verifiedVoucher);

                if (textBoxLogsSync.Text.Contains(syncResult.Message))
                    return;

                if (_credentials.ActiveLog)
                    Log(updateType, syncResult.Message, syncResult.StatusBarMessage);
            }
            else
            {
                dataGridView.DataSource = null;
                treeView1.Nodes.Clear();
            }

            if (textBoxLogsSync.Text.Contains(syncResult.Message))
                return;

            if (_credentials.ActiveLog)
                Log(updateType, syncResult.Message, syncResult.StatusBarMessage);

            //syncResult.UpdateResponse;
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
    private async Task HandleInventoryPosting(List<InventoryPosting> inventoryPostings, UpdateType updateType)
    {
        PlaySound.Click();
        var bindingList = dataGridView.DataSource as BindingList<InventoryPosting>;

        await foreach (var syncResult in _inventoryPostingHandler.SyncInventoryPosting(inventoryPostings))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var count in syncResult.EntityList)
                {
                    //dataGridView.BringToFront();
                    //dataGridView.Visible = true;
                    //dt.BindInventoryPosting(ref bindingList, count);
                    //dt.SelectLastRow();


                    treeView1.BringToFront();
                    treeView1.Visible = true;
                    treeView1.BindInventoryCounting(ref bindingList, count);

                    //Log(UpdateType, syncResult.Message, syncResult.StatusBarMessage);
                }
            }
            else
            {
                dataGridView.DataSource = null;
                treeView1.Nodes.Clear();
            }

            if (textBoxLogsSync.Text.Contains(syncResult.Message))
                return;

            if (_credentials.ActiveLog)
                Log(updateType, syncResult.Message, syncResult.StatusBarMessage);

            //syncResult.UpdateResponse;
        }
    }
    private async Task ProcessOutboundDocument(OutboundDocuments documentType, int storeNum, string filter, string docCode)
    {
        var result = await _inventoryPostingService.GetInventoryPosting(storeNum, filter, docCode);

        if (result.EntityList.Any())
        {
            if (_credentials.ActiveLog)
                LogMessages($"Inventory Posting/s.\r\n\r\nRequest Message: {result.Message}", "");

            foreach (var inventoryPosting in result.EntityList)
            {
                if (inventoryPosting == null) continue;

                if (!CheckInventoryPostingExist(inventoryPosting.Sid)) // To-Do Add Check Exist in SAP by Sid in field U_PrismSid and U_SyncToPrism
                {
                    await HandleInventoryPosting(result.EntityList, UpdateType.SyncInvoice);
                }
                else
                {
                    var docNum = GetInventoryPostingDocNum(inventoryPosting.Sid);
                    var message = $"Prism Adjustment No. ({inventoryPosting.Adjno}) is Already Exist with SAP Inventory Posting No. ({docNum}).";

                    if (_credentials.ActiveLog)
                        LogMessages(message, message);
                }
            }
        }
        else
        {
            dataGridView.DataSource = null;
            LogMessages($"No Available Inventory Posting/s.\r\nResponse: {result.Response.Content} \r\n\r\nResult Message: {result.Message}\r\nStatus: {result.Status}",
                $"No Available Inventory Posting/s.");
        }
    }
    private async Task HandleDownPayment(List<PrismInvoice> invoicesList, UpdateType updateType)
    {
        PlaySound.Click();
        var bindingList = dataGridView.DataSource as BindingList<SAPInvoice>;

        await foreach (var syncResult in _downPaymentHandler.AddDownPaymentAsync(invoicesList))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var invoice in syncResult.EntityList)
                {
                    dataGridView.BringToFront();
                    dataGridView.Visible = true;
                    dataGridView.BindInvoices(ref bindingList, invoice);
                    dataGridView.SelectLastRow();

                    //treeView.BindInvoices(ref bindingList, invoice);

                    //Log(UpdateType, syncResult.Message, syncResult.StatusBarMessage);
                }
            }
            else
            {
                dataGridView.DataSource = null;
                treeView1.Nodes.Clear();
            }

            if (textBoxLogsSync.Text.Contains(syncResult.Message))
                return;

            if (_credentials.ActiveLog)
                Log(updateType, syncResult.Message, syncResult.StatusBarMessage);

            //syncResult.UpdateResponse;
        }
    }
    private async Task HandleCreditMemoWithoutPayment(List<PrismInvoice> invoicesList, UpdateType UpdateType)
    {
        PlaySound.Click();
        //var BpList = new List<BusinessPartner>();
        var bindingList = dataGridView.DataSource as BindingList<SAPInvoice>;

        await foreach (var syncResult in _creditMemoHandler.AddCreditMemoAsync(invoicesList))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var invoice in syncResult.EntityList)
                {
                    dataGridView.BringToFront();
                    dataGridView.Visible = true;
                    dataGridView.BindInvoices(ref bindingList, invoice);
                    dataGridView.SelectLastRow();

                    //treeView.BindInvoices(ref bindingList, invoice);

                    //Log(UpdateType, syncResult.Message, syncResult.StatusBarMessage);
                }
            }
            else
            {
                dataGridView.DataSource = null;
                treeView1.Nodes.Clear();
            }

            if (textBoxLogsSync.Text.Contains(syncResult.Message))
                return;

            if (_credentials.ActiveLog)
                Log(UpdateType, syncResult.Message, syncResult.StatusBarMessage);

            //syncResult.UpdateResponse;
        }
    }

    public void OpenFormWithSettings(int tabControlInventoryIndex, int comboBoxDocTypeSyncIndex)
    {
        tabControl.SelectedIndex = tabControlInventoryIndex;
        comboBoxDocTypeSync.SelectedIndex = comboBoxDocTypeSyncIndex;
        //comboBoxDocTypeSchedule.SelectedIndex = comboBoxDocTypeSyncIndex;

        //if (comboBoxDocTypeSyncIndex == (int)Documents.Invoice)
        //    toggleARInvoice.Checked = true;

        //else if (comboBoxDocTypeSyncIndex == (int)Documents.CreditMemo)
        //    toggleCreditMemo.Checked = true;

        //else if (comboBoxDocTypeSyncIndex == (int)Documents.StockTransfer)
        //    toggleStockTransfer.Checked = true;

        //else if (comboBoxDocTypeSyncIndex == (int)Documents.StockTaking)
        //    toggleStockTaking.Checked = true;

        //else if (comboBoxDocTypeSyncIndex == (int)Documents.GoodsReceipt)
        //    toggleGoodsReceipt.Checked = true;

        //else if (comboBoxDocTypeSyncIndex == (int)Documents.GoodsIssue)
        //    toggleGoodsIssue.Checked = true;
    }

    private async void OutboundData_Load(object sender, EventArgs e)
    {
        comboBoxDocTypeSync.SelectedIndex = (int)OutboundDocuments.SalesInvoice;
        comboBoxBranch.SelectedIndex = 0;
        dateTimePickerFrom.Value = DateTime.Now.AddMonths(-1);

        dateTimePickerTo.Value = DateTime.Now;

        _invoiceService.Stores = await _invoiceService.StoresService.GetAll();
        _invoiceService.TaxCodes = await _invoiceService.TaxCodesService.GetAll();

        //var branches = _invoiceService.Stores
        //    .Select(i => new { i.StoreNumber, i.StoreName })
        //    .OrderBy(branch => branch.StoreNumber); // To sort by StoreNumber in ascending order

        ////comboBoxBranch.DataSource = branches.ToList();
        ////comboBoxBranch.DisplayMember = "StoreNumber - StoreName"; // The property to display
        ////comboBoxBranch.ValueMember = "Sid"; // The property to use as the value

        //foreach (var branch in branches)
        //    comboBoxBranch.AddItem(branch.StoreNumber, $"{branch.StoreNumber} - {branch.StoreName}");


        // Create a list of Store objects from _invoiceService.Stores
        var branches = _invoiceService.Stores
            .Select(i => new Store { StoreNumber = i.StoreNumber, StoreCode = i.StoreCode, StoreName = i.StoreName })
            .ToList()
            .OrderBy(branch => branch.StoreCode);

        // Create a new list
        List<Store> allBranches = new List<Store>();

        // Add the "All" item
        allBranches.Add(new Store { StoreNumber = -1, DisplayMember = "All" });

        // Add the rest of the items
        allBranches.AddRange(branches);

        comboBoxBranch.DataSource = allBranches; // You need to set DataSource to allBranches instead of branches

        comboBoxBranch.ValueMember = "StoreNumber"; // The property to use as the value
        comboBoxBranch.DisplayMember = "DisplayMember"; // The property to display


        //var isReachable = await Helper.IsDashboardAvailable();
        //if (isReachable)
        //    webViewSchedule.Visible = true;
        //else
        //    webViewSchedule.Visible = false;

        textBoxDocCode.Focus();

        //tabControl.Controls[2].Enabled = false;
    }



    private string GetSyncQuery()
    {
        var dateFrom = dateTimePickerFrom.Value.ToPrismFromDateFormat();

        var dateTo = dateTimePickerTo.Value.ToPrismToDateFormat();


        //dateFrom    2023-06-26T21:00:00.736Z   -  dateTo     2023-07-27T20:59:59.736Z
        return $"AND(invoice_posted_date,ge,{dateFrom})AND(invoice_posted_date,le,{dateTo})";

        //AND(invoice_posted_date,ge,2023-07-24T21:00:00.877Z)AND(invoice_posted_date,le,2023-07-25T20:59:59.877Z)
    }

    private bool ShouldSync(string documentName, OutboundDocuments document, out string filter)
    {
        var updateType = SyncType.GetSyncType(document);
        var initialDate = SyncEntityService.CompareSyncDateWithDateNow(_unitOfWork, updateType.Initial,
            out var needToSyncBasedOnInitialDate);
        var syncDate =
            SyncEntityService.CompareSyncDateWithDateNow(_unitOfWork, updateType.Sync,
                out var needToSyncBasedOnSyncDate);

        if (needToSyncBasedOnInitialDate || needToSyncBasedOnSyncDate)
        {
            var lastInitialDate = initialDate.ToSAPDateFormat();
            var lastSyncDate = syncDate.ToSAPDateFormat();

            filter =
                $" WHERE (T0.[CreateDate] >= '{lastInitialDate}' OR T0.[UpdateDate] >= '{lastSyncDate}') AND (T0.[U_SyncToPrism] IS NULL OR  T0.[U_SyncToPrism] = '' OR  T0.[U_SyncToPrism] = 'N')";

            return true;
        }

        var message =
            $"There is no {documentName} available to be synced, or may it flagged as synced to prism or not active to be synced.";
        var status =
            $"Status: there is no {documentName} available to be synced, or may it flagged as synced to prism or not active to be synced.";
        LogMessages(message, status);

        filter = "";
        return false;
    }

    private void LogMessages(string message, string status)
    {
        textBoxLogsSync.Log(new[] { message + "\r\n" }, Logger.MessageTime.Long);
        labelStatus.Log(status, Logger.MessageTypes.Warning,
            Logger.MessageTime.Long);
    }

    private void Log(UpdateType updateType, string message, string status)
    {
        if (updateType
            is UpdateType.SyncInvoice
            or UpdateType.SyncCreditMemo
            or UpdateType.SyncOrders
            or UpdateType.SyncInventoryTransfer
            or UpdateType.SyncInventoryPosting
            or UpdateType.SyncOutGoodsReceipt
            or UpdateType.SyncOutGoodsIssue
            or UpdateType.SyncWholesale
            )
        {
            //textBoxLogsSync.Clear();
            textBoxLogsSync.Text += message;
            // UpdateTextBox(textBoxLogsInitialize, message);
        }

        //else
        //{
        //    textBoxLogsInitialize.Clear();
        //    textBoxLogsInitialize.AppendText(message);
        //}
        labelStatus.Log(status, Logger.MessageTypes.Warning,
            Logger.MessageTime.Long);
    }


    private void guna2ControlBox1_Click(object sender, EventArgs e)
    {
        Helper.TryKillProcess();
    }


    private void guna2ControlBox2_Click(object sender, EventArgs e) => WindowState = FormWindowState.Minimized;

    private void guna2PictureBox1_DoubleClick(object sender, EventArgs e) => ResizeForm();

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

    private void guna2Panel1_DoubleClick(object sender, EventArgs e) => ResizeForm();

    private void guna2ControlBox3_Click(object sender, EventArgs e)
    {
        guna2Elipse1.BorderRadius = WindowState == FormWindowState.Normal ? 30 : 0;
    }

    private void guna2Button1_Click(object sender, EventArgs e)
    {
        Close();
        Dashboard mainScreen = new Dashboard(_unitOfWork, _serviceLayer, _departmentService, _itemsService, _client);
        mainScreen.Show();

        if (Program.IsPrismConnected)
        {
            mainScreen.CheckConnectivity(true);
        }
        else
            mainScreen.CheckConnectivity(false);
    }

    private void guna2Button2_Click(object sender, EventArgs e)
    {
        Close();
        InboundData inboundData =
            new InboundData(_unitOfWork, _serviceLayer, _departmentService, _itemsService, _client);
        inboundData.Show();
    }

    private void guna2Button6_Click(object sender, EventArgs e)
    {
        Close();
        var administration = new Settings(_unitOfWork, _serviceLayer, _departmentService, _itemsService, _client);
        administration.Show();
    }

    private void comboBoxDocTypeSync_SelectedIndexChanged(object sender, EventArgs e)
    {
        var documentType = (OutboundDocuments)comboBoxDocTypeSync.SelectedIndex;

        if (documentType
            is OutboundDocuments.SalesInvoice
            or OutboundDocuments.ReturnInvoice
            or OutboundDocuments.CustomerOrder
            or OutboundDocuments.StockTransfer
            or OutboundDocuments.InventoryPosting
            or OutboundDocuments.GoodsReceipt
            or OutboundDocuments.GoodsIssue
            )
        {
            labelBranchs.Visible = true;
            comboBoxBranch.Visible = true;

            checkBoxDate.Visible = true;
            checkBoxDocCode.Visible = true;
        }
        else
        {
            labelBranchs.Visible = false;
            comboBoxBranch.Visible = false;

            checkBoxDate.Visible = false;
            checkBoxDocCode.Visible = false;
        }
    }

    private void textBoxDocCode_Enter(object sender, EventArgs e)
        => checkBoxDocCode.Checked = true;

    private void textBoxDocCode_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
            buttonSync.PerformClick();
    }

    private void ClearSyncDataGridView()
    {
        dataGridView.DataSource = null;
        dataGridView.Rows.Clear();

        treeView1.Nodes.Clear();
    }

    private void SyncClearLogs_Click(object sender, EventArgs e)
        => textBoxLogsSync.Clear();


    private void MenuISyncCopy_Click(object sender, EventArgs e)
        => CopyText(textBoxLogsSync);


    private void MenuISyncClearMonitoringLogs_Click(object sender, EventArgs e)
        => ClearSyncDataGridView();

    private void CopyText(RichTextBox textBoxLogs)
    {
        if (!string.IsNullOrEmpty(textBoxLogs.SelectedText))
        {
            Clipboard.SetText(textBoxLogs.SelectedText);
            labelStatus.Log("Status: Selected Text Copied", Logger.MessageTypes.Warning, Logger.MessageTime.Short);
        }
    }

    private void MenuRefreshAuth_Click(object sender, EventArgs e)
        => RefreshAuthSession();

    private async void RefreshAuthSession()
    {
        var newAuth = await LoginManager.GetAuthSessionAsync(_credentials.BaseUri, _credentials.PrismUserName, _credentials.PrismPassword);
        if (newAuth.IsHasValue())
            labelStatus.Log("Status: Auth Session refreshed.", Logger.MessageTypes.Warning, Logger.MessageTime.Long);
        else
            labelStatus.Log("Status: Cant refresh Auth-Session, Wait a few seconds before you try again.", Logger.MessageTypes.Error, Logger.MessageTime.Long);
    }

}

