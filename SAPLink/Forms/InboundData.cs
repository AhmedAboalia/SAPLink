using SAPLink.Handler.Prism.Connection.Auth;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;
using Documents = SAPLink.Core.InboundEnums.Documents;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Vendors;
using SAPLink.Handler.Prism.Handlers.InboundData.Receiving.GoodsReceiptPo;
using SAPLink.Utilities.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using SAPLink.Handler.Prism.Handlers.InboundData.Receiving.GRPO;
using SAPLink.Handler.Prism.Handlers.InboundData.Receiving.GoodsIssue;
using SAPLink.Handler.Prism.Handlers.InboundData.Receiving.GoodsReceipt;

namespace SAPLink.Forms;

public partial class InboundData : Form
{
    #region Initial variables

    private readonly UnitOfWork _unitOfWork;
    private readonly ServiceLayerHandler _serviceLayer;
    private readonly Clients _client;
    private readonly ItemsService _itemsService;
    private readonly ItemsHandler _itemsHandler;
    private readonly DepartmentService _departmentServices;
    private readonly DepartmentsHandler _departmentsHandler;
    private readonly VendorsHandler _vendorsHandler;
    private readonly GoodsReceiptPoHandler _goodsReceiptPoHandler;
    private readonly GoodsReceiptHandler _goodsReceiptHandler;
    private readonly GoodsIssueHandler _goodsIssueHandler;
    private readonly Credentials _credentials;

    public InboundData(UnitOfWork unitOfWork, ServiceLayerHandler serviceLayer, DepartmentService departmentServices,
        ItemsService itemsService, Clients client)
    {
        InitializeComponent();
        _unitOfWork = unitOfWork;
        _serviceLayer = serviceLayer;
        _departmentServices = departmentServices;
        _itemsService = itemsService;
        _client = client;
        _credentials = _client.Credentials.FirstOrDefault();

        _goodsReceiptPoHandler = new GoodsReceiptPoHandler(unitOfWork, client);
        _goodsReceiptHandler = new GoodsReceiptHandler(unitOfWork, client);
        _goodsIssueHandler = new GoodsIssueHandler(unitOfWork, client);

        _departmentsHandler = new DepartmentsHandler(_unitOfWork, _departmentServices, client);
        _itemsHandler = new ItemsHandler(_unitOfWork, _itemsService, client);
        _vendorsHandler = new VendorsHandler(_unitOfWork, client);

        //try
        //{
        //    string Url = _client.Credentials.FirstOrDefault().IntegrationUrl;
        //    webViewSchedule.Source = new Uri($"{Url}/dashboard/recurring");
        //}
        //catch (Exception e)
        //{
        //}

        comboBoxDocTypeSchedule.SelectedIndex = (int)Documents.Departments;
        comboBoxRecurrenceSchedule.SelectedIndex = (int)Repeats.None;
        comboBoxDocTypeSync.SelectedIndex = (int)Repeats.None;
    }

    #endregion

    private async void buttonInitial_Click(object sender, EventArgs e)
    {
        textBoxLogsInitialize.Clear();

        if (toggleDepartments.Checked)
        {
            PlaySound.Click();
            var result = MessageBox.Show("The initial Departments may take some time.\r\nWould you like to continue?", "Confirmation Message", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                await HandleDepartments(dataGridViewInitial, "");
        }

        if (toggleVendors.Checked)
        {
            PlaySound.Click();
            var result = MessageBox.Show("The initial Vendors may take some time.\r\nWould you like to continue?", "Confirmation Message", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                await HandleVendors(dataGridViewInitial, "");
        }

        if (toggleItems.Checked)
        {
            PlaySound.Click();
            var result = MessageBox.Show("The initial Items may take some time.\r\nWould you like to continue?", "Confirmation Message", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                await HandleItems(dataGridViewInitial, "");
        }

        if (toggleGoodsRecepitPO.Checked)
        {
            PlaySound.Click();
            var result = MessageBox.Show("The initial Goods Recepit PO (GRPO) may take some time. Would you like to continue?", "Confirmation", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
                await HandleGoodsReceiptPo(dataGridViewInitial, "");
        }
        if (toggleGoodsRecepit.Checked)
        {
            PlaySound.Click();
            var result = MessageBox.Show("The initial Goods Recepit (GR) may take some time. Would you like to continue?", "Confirmation", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
                await HandleGoodsReceipt(dataGridViewInitial, "");
        }
        if (toggleGoodsIssue.Checked)
        {
            PlaySound.Click();
            var result = MessageBox.Show("The initial Goods Issue (GI) may take some time. Would you like to continue?", "Confirmation", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
                await HandleGoodsIssue(dataGridViewInitial, "");
        }
    }

    private async void buttonSyncNow_Click(object sender, EventArgs e)
    {
        var documentType = (Documents)comboBoxDocTypeSync.SelectedIndex;

        textBoxLogsSync.Clear();

        if (radioButtonNewDoc.Checked)
        {
            string filter;
            switch (documentType)
            {
                case Documents.Departments:
                    var shouldSyncDepartments = ShouldSync("Departments", Documents.Departments, " WHERE", out filter);
                    if (shouldSyncDepartments)
                        await HandleDepartments(dataGridViewSync, filter);
                    break;

                case Documents.Vendors:
                    var shouldSyncVendors = ShouldSync("Vendors", Documents.Vendors, " AND", out filter);
                    if (shouldSyncVendors)
                        await HandleVendors(dataGridViewSync, filter);
                    break;

                case Documents.Items:
                    var shouldSyncItems = ShouldSync("Items", Documents.Items, " AND", out filter);
                    if (shouldSyncItems)
                        await HandleItems(dataGridViewSync, filter);
                    break;

                case Documents.GoodsReceiptPo:
                    var shouldSyncGrPo = ShouldSync("Goods Receipt POs", Documents.GoodsReceiptPo, " WHERE", out filter);
                    if (shouldSyncGrPo)
                        await HandleGoodsReceiptPo(dataGridViewSync, filter);
                    break;

                case Documents.GoodsReceipt:
                    var shouldSyncGr = ShouldSync("Goods Receipts", Documents.GoodsReceipt, " WHERE", out filter);
                    if (shouldSyncGr)
                        await HandleGoodsReceipt(dataGridViewSync, filter);
                    break;

                case Documents.GoodsIssue:
                    var shouldSyncGi = ShouldSync("Goods Issues", Documents.GoodsIssue, " WHERE", out filter);
                    if (shouldSyncGi)
                        await HandleGoodsIssue(dataGridViewSync, filter);
                    break;
            }
        }
        else if (radioButtonDate.Checked)
        {
            var filter = GetSyncQueryByRangOfDate();
            switch (documentType)
            {
                case Documents.Departments:
                    await HandleDepartments(dataGridViewSync, filter);
                    break;
                case Documents.Vendors:
                    await HandleVendors(dataGridViewSync, filter);
                    break;
                case Documents.Items:
                    await HandleItems(dataGridViewSync, filter);
                    break;
                case Documents.GoodsReceiptPo:
                    await HandleGoodsReceiptPo(dataGridViewSync, filter);
                    break;
                case Documents.GoodsReceipt:
                    await HandleGoodsReceipt(dataGridViewSync, filter);
                    break;
                case Documents.GoodsIssue:
                    await HandleGoodsIssue(dataGridViewSync, filter);
                    break;
            }
        }
        else if (radioButtonDocId.Checked)
        {
            var docCode = textBoxDocCode.Text;
            if (docCode.IsHasValue())
            {
                string filter;
                switch (documentType)
                {
                    case Documents.Departments:
                        filter = $" WHERE T0.ItmsGrpCod = '{docCode}' AND (T0.[U_SyncToPrism] IS NULL OR  T0.[U_SyncToPrism] = '' OR  T0.[U_SyncToPrism] = 'N')";
                        await HandleDepartments(dataGridViewSync, filter);
                        break;
                    case Documents.Vendors:
                        filter = $" AND T0.CardCode = '{docCode}' AND (T0.[U_SyncToPrism] IS NULL OR  T0.[U_SyncToPrism] = '' OR  T0.[U_SyncToPrism] = 'N')";
                        await HandleVendors(dataGridViewSync, filter);
                        break;
                    case Documents.Items:
                        filter = $" AND T0.ItemCode = '{docCode}' AND (T0.[U_SyncToPrism] IS NULL OR  T0.[U_SyncToPrism] = '' OR  T0.[U_SyncToPrism] = 'N')";
                        await HandleItems(dataGridViewSync, filter);
                        break;
                    case Documents.GoodsReceiptPo:
                        filter = $" WHERE T0.DocNum = '{docCode}' AND (T0.[U_SyncToPrism] IS NULL OR  T0.[U_SyncToPrism] = '' OR  T0.[U_SyncToPrism] = 'N')";
                        await HandleGoodsReceiptPo(dataGridViewSync, filter);
                        break;
                    case Documents.GoodsReceipt:
                        filter = $" WHERE T0.DocNum = '{docCode}' AND (T0.[U_SyncToPrism] IS NULL OR  T0.[U_SyncToPrism] = '' OR  T0.[U_SyncToPrism] = 'N')";
                        await HandleGoodsReceipt(dataGridViewSync, filter);
                        break;
                    case Documents.GoodsIssue:
                        filter = $" WHERE T0.DocNum = '{docCode}' AND (T0.[U_SyncToPrism] IS NULL OR  T0.[U_SyncToPrism] = '' OR  T0.[U_SyncToPrism] = 'N')";
                        await HandleGoodsIssue(dataGridViewSync, filter);
                        break;
                }
            }
            else
                labelStatus.Log("Please Enter a valid Document No.", Logger.MessageTypes.Warning, Logger.MessageTime.Long);
        }
    }

    private bool ShouldSync(string documentName, Documents document, string condition, out string filter)
    {
        var updateType = SyncType.GetSyncType(document);
        var initialDate = SyncEntityService.CompareSyncDateWithDateNow(_unitOfWork, updateType.Initial, out var needToSyncBasedOnInitialDate);
        var syncDate = SyncEntityService.CompareSyncDateWithDateNow(_unitOfWork, updateType.Sync, out var needToSyncBasedOnSyncDate);

        if (needToSyncBasedOnInitialDate || needToSyncBasedOnSyncDate)
        {
            var lastInitialDate = initialDate.ToSAPDateFormat();
            var lastSyncDate = syncDate.ToSAPDateFormat();

            filter = $" {condition} (T0.[CreateDate] >= '{lastInitialDate}' OR T0.[UpdateDate] >= '{lastSyncDate}')";

            return true;
        }

        var message = $"There is no {documentName} available to be synced, or may it flagged as synced to prism or not active to be synced.";
        var status = $"Status: there is no {documentName} available to be synced, or may it flagged as synced to prism or not active to be synced.";
        LogMessages(message, status);

        filter = "";
        return false;
    }

    private async Task HandleDepartments(Guna2DataGridView dt, string filter)
    {
        PlaySound.Click();
        //var BpList = new List<BusinessPartner>();
        var bindingList = dt.DataSource as BindingList<ItemGroups>;


        await foreach (var syncResult in _departmentsHandler.SyncAsync(filter))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var department in syncResult.EntityList)
                {
                    dt.BindDepartments(ref bindingList, department);
                    dt.SelectLastRow();
                }
            }
            else
            {
                dt.DataSource = null;
            }

            Log(filter.IsNullOrEmpty()
                ? UpdateType.InitialDepartment
                : UpdateType.SyncDepartment, syncResult.Message, syncResult.StatusBarMessage);
        }
    }
    private async Task HandleVendors(Guna2DataGridView dt, string filter)
    {
        PlaySound.Click();
        //var BpList = new List<BusinessPartner>();
        var bindingList = dt.DataSource as BindingList<BusinessPartner>;

        var result = _vendorsHandler.SyncAsync(filter);
        await foreach (var syncResult in result)
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var businessPartner in syncResult.EntityList)
                {
                    dt.BindVendors(ref bindingList, businessPartner);
                    dt.SelectLastRow();
                }
            }
            else
            {
                dt.DataSource = null;
            }

            Log(filter.IsNullOrEmpty()
                ? UpdateType.InitialVendors
                : UpdateType.SyncVendors, syncResult.Message, syncResult.StatusBarMessage);
        }
    }
    private async Task HandleItems(Guna2DataGridView dt, string filter)
    {
        PlaySound.Click();

        var bindingList = dt.DataSource as BindingList<ItemMasterData>;

        var LogMessage = "";
        await foreach (var syncResult in _itemsHandler.SyncAsync(filter))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var itemMaster in syncResult.EntityList)
                {
                    dt.BindItems(ref bindingList, itemMaster);
                    dt.SelectLastRow();
                }
            }
            else
                dt.DataSource = null;

            LogMessage += syncResult.Message;

            Log(filter.IsNullOrEmpty()
                ? UpdateType.InitialItems
                : UpdateType.SyncItems, LogMessage, syncResult.StatusBarMessage);

        }
        Log(filter.IsNullOrEmpty()
            ? UpdateType.InitialItems
            : UpdateType.SyncItems, LogMessage, "");
    }


    private async Task HandleGoodsReceiptPo(Guna2DataGridView dt, string filter)
    {
        PlaySound.Click();
        //var BpList = new List<BusinessPartner>();

        var bindingList = dt.DataSource as BindingList<Goods>;

        var LogMessage = "";
        await foreach (var syncResult in _goodsReceiptPoHandler.SyncAsync(filter))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var itemMaster in syncResult.EntityList)
                {
                    dt.BindGoodsReceiptPo(ref bindingList, itemMaster);
                    dt.SelectLastRow();
                }
            }
            else
            {
                dt.DataSource = null;

            }

            Log(filter.IsNullOrEmpty()
                ? UpdateType.InitialItems
                : UpdateType.SyncItems, syncResult.Message, syncResult.StatusBarMessage);

        }
    }
    private async Task HandleGoodsReceipt(Guna2DataGridView dt, string filter)
    {
        PlaySound.Click();
        var bindingList = dt.DataSource as BindingList<Goods>;

        var LogMessage = "";
        await foreach (var syncResult in _goodsReceiptHandler.SyncAsync(filter))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var itemMaster in syncResult.EntityList)
                {
                    dt.BindGoodsReceiptPo(ref bindingList, itemMaster);
                    dt.SelectLastRow();
                }
            }
            else
            {
                dt.DataSource = null;

            }

            Log(filter.IsNullOrEmpty()
                ? UpdateType.InitialItems
                : UpdateType.SyncItems, syncResult.Message, syncResult.StatusBarMessage);
        }
    }
    private async Task HandleGoodsIssue(Guna2DataGridView dt, string filter)
    {
        PlaySound.Click();
        var bindingList = dt.DataSource as BindingList<Goods>;

        var LogMessage = "";
        await foreach (var syncResult in _goodsIssueHandler.SyncAsync(filter))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var itemMaster in syncResult.EntityList)
                {
                    dt.BindGoodsReceiptPo(ref bindingList, itemMaster);
                    dt.SelectLastRow();
                }
            }
            else
            {
                dt.DataSource = null;

            }

            LogMessage += syncResult.Message;

            Log(filter.IsNullOrEmpty()
                ? UpdateType.InitialItems
                : UpdateType.SyncItems, syncResult.Message, syncResult.StatusBarMessage);
        }
        Log(filter.IsNullOrEmpty()
            ? UpdateType.InitialItems
            : UpdateType.SyncItems, LogMessage, "");
    }

    private string GetSyncQueryByRangOfDate()
    {
        var dateFrom = dateTimePickerFrom.Value.ToSAPDateFormat();
        var dateTo = dateTimePickerTo.Value.ToSAPDateFormat();

        return $" WHERE T0.[DocDate] Between '{dateFrom}' AND '{dateTo}' AND (T0.[U_SyncToPrism] IS NULL OR  T0.[U_SyncToPrism] = '' OR  T0.[U_SyncToPrism] = 'N')";
    }


    private void buttonScheduleIt_Click(object sender, EventArgs e)
    {
        if (buttonScheduleIt.Enabled)
        {
            PlaySound.Click();
            var document = (Documents)comboBoxDocTypeSchedule.SelectedIndex;
            try
            {
                //var recurrence = _unitOfWork.Recurrences.Find(x => x.Document == document);
                //if (recurrence != null)
                //{
                //    recurrence.Document = document;
                //    recurrence.Recurring = (Enums.Repeats)comboBoxRecurrenceSchedule.SelectedIndex;

                //    if (recurrence.Recurring == Enums.Repeats.Hourly)
                //        recurrence.Interval = int.Parse(TextBoxIntervalSchedule.Text);
                //    else
                //        recurrence.DayOfWeek = (DayOfWeek)ComboBoxDaysOfWeek.SelectedIndex;

                //    _unitOfWork.Recurrences.Update(recurrence);
                //    _unitOfWork.SaveChanges();
                //    ScheduleService.Run(recurrence, out var message);
                //    textBoxLogsSchedule.Text = message;
                //}
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }

            //webView.Source = new Uri("http://localhost:7208/dashboard");
            //webView.Source = new Uri("http://localhost:7208/dashboard/recurring");
        }
    }



    private void comboBoxRecurrence_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedIndex = comboBoxRecurrenceSchedule.SelectedIndex;
        textBoxLogsSchedule.Clear();

        labelRepeatEvery.Visible = true;
        //buttonSchdeuleIt.Enabled = true;

        PlaySound.Click();

        if (selectedIndex == (int)Enums.Repeats.Hourly)
        {
            TextBoxIntervalSchedule.Visible = true;
            ComboBoxDaysOfWeek.Visible = false;
        }

        else if (selectedIndex == (int)Enums.Repeats.Daily)
        {
            TextBoxIntervalSchedule.Visible = false;
            ComboBoxDaysOfWeek.Visible = true;
            labelRepeatEvery.Text = "Every";
        }

        else if (selectedIndex == (int)Enums.Repeats.None)
        {
            labelRepeatEvery.Visible = false;
            TextBoxIntervalSchedule.Visible = false;
            //buttonRunIt.Enabled = false;
        }
    }


    private void ComboBoxDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedIndex = comboBoxDocTypeSchedule.SelectedIndex;
        textBoxLogsSchedule.Clear();

        buttonScheduleIt.Enabled = true;

        PlaySound.Click();

        var recurrence = new Recurrence();
        //recurrence = ScheduleService.GetRecurrence(_unitOfWork, selectedIndex, recurrence);

        comboBoxRecurrenceSchedule.SelectedIndex = (int)recurrence.Recurring;
        TextBoxIntervalSchedule.Text = recurrence.Interval.ToString();
        ComboBoxDaysOfWeek.SelectedIndex = (int)recurrence.DayOfWeek;

        label3.Visible = true;
        comboBoxRecurrenceSchedule.Visible = true;

        labelRepeatEvery.Visible = true;
        TextBoxIntervalSchedule.Visible = true;

        //buttonSchdeuleIt.Enabled = false; // disabled for Delivery

    }
    public void OpenFormWithSettings(int tabControlInventoryIndex, int comboBoxDocTypeSyncIndex)
    {
        tabControlInventory.SelectedIndex = tabControlInventoryIndex;
        comboBoxDocTypeSync.SelectedIndex = comboBoxDocTypeSyncIndex;
        //comboBoxDocTypeSchedule.SelectedIndex = comboBoxDocTypeSyncIndex;

        if (comboBoxDocTypeSyncIndex == (int)Documents.Departments)
            toggleDepartments.Checked = true;


        else if (comboBoxDocTypeSyncIndex == (int)Documents.Vendors)
            toggleVendors.Checked = true;

        else if (comboBoxDocTypeSyncIndex == (int)Documents.Items)
            toggleItems.Checked = true;

    }
    private void Log(UpdateType updateType, string message, string status)
    {
        if (updateType == UpdateType.SyncVendors ||
            updateType == UpdateType.SyncDepartment ||
            updateType == UpdateType.SyncItems ||
            updateType == UpdateType.SyncGoodsReceiptPO)
        {
            textBoxLogsSync.Clear();
            textBoxLogsSync.AppendText(message);
            // UpdateTextBox(textBoxLogsInitialize, message);
        }
        else
        {
            textBoxLogsInitialize.Clear();
            textBoxLogsInitialize.AppendText(message);
        }
        labelStatus.Log(status, Logger.MessageTypes.Warning, Logger.MessageTime.Long);
    }
    private void guna2ControlBox1_Click(object sender, EventArgs e)
    {
        Helper.TryKillProcess();
    }

    private async void ButtonDashboard_Click(object sender, EventArgs e)
    {
        Close();
        PlaySound.Click();
        var mainScreen = new Dashboard(_unitOfWork, _serviceLayer, _departmentServices, _itemsService, _client);
        mainScreen.Show();

        if (Program.IsPrismConnected)
        {
            mainScreen.CheckConnectivity(true);
        }
        else
            mainScreen.CheckConnectivity(false);
    }

    private void ButtonOutboundData_Click(object sender, EventArgs e)
    {
        Close();
        PlaySound.Click();
        var outboundData = new OutboundData(_unitOfWork, _serviceLayer, _departmentServices, _itemsService, _client);
        outboundData.Show();
    }
    private void InboundData_Resize(object sender, EventArgs e)
    {
        //webViewSchedule.Size = ClientSize - new Size(webViewSchedule.Location);
    }


    private void TextBoxInterval_Validated(object sender, EventArgs e)
    {
        if (TextBoxIntervalSchedule.IsWithinRange(1, 60))
            return;

        TextBoxIntervalSchedule.Clear();
        TextBoxIntervalSchedule.Text = "00";
    }


    private async void InboundData_LoadAsync(object sender, EventArgs e)
    {
        comboBoxDocTypeSync.SelectedIndex = (int)Documents.Departments;
        dateTimePickerFrom.Value = DateTime.Now.AddDays(-1);

        dateTimePickerTo.Value = DateTime.Now;

        //var isReachable = await Helper.IsDashboardAvailable();
        //if (isReachable)
        //    webViewSchedule.Visible = true;
        //else
        //    webViewSchedule.Visible = false;

        textBoxDocCode.Focus();
    }

    private void guna2Button4_Click(object sender, EventArgs e)
    {
        Close();
        PlaySound.Click();
        var administration = new Settings(_unitOfWork, _serviceLayer, _departmentServices, _itemsService, _client);
        administration.Show();
    }


    private void contextMenuInitial_Click(object sender, EventArgs e)
    {
        dataGridViewInitial.DataSource = null;
        dataGridViewInitial.Rows.Clear();
        textBoxLogsInitialize.Clear();
    }


    private void contextMenuSchedule_Click(object sender, EventArgs e)
    {
        textBoxLogsSchedule.Clear();
    }
    private void LogMessages(string message, string status)
    {
        textBoxLogsSync.Log(new[] { message + "\r\n" }, Logger.MessageTime.Long);
        labelStatus.Log(status, Logger.MessageTypes.Warning, Logger.MessageTime.Long);
    }

    private void MinimizeButton_Click(object sender, EventArgs e) => WindowState = FormWindowState.Minimized;
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

    private void toggleDepartments_CheckedChanged(object sender, EventArgs e)
    {
        if (toggleDepartments.Checked)
        {
            PlaySound.Click();
            if (toggleVendors.Checked)
                toggleVendors.InvokeUnCheck();

            if (toggleItems.Checked)
                toggleItems.InvokeUnCheck();

            if (toggleGoodsRecepitPO.Checked)
                toggleGoodsRecepitPO.InvokeUnCheck();

            if (toggleGoodsRecepit.Checked)
                toggleGoodsRecepit.InvokeUnCheck();

            if (toggleGoodsIssue.Checked)
                toggleGoodsIssue.InvokeUnCheck();
        }
    }

    private void toggleVendors_CheckedChanged(object sender, EventArgs e)
    {
        if (toggleVendors.Checked)
        {
            PlaySound.Click();
            if (toggleDepartments.Checked)
                toggleDepartments.InvokeUnCheck();

            //if (toggleVendors.Checked)
            //    toggleVendors.InvokeUnCheck();

            if (toggleItems.Checked)
                toggleItems.InvokeUnCheck();

            if (toggleGoodsRecepitPO.Checked)
                toggleGoodsRecepitPO.InvokeUnCheck();

            if (toggleGoodsRecepit.Checked)
                toggleGoodsRecepit.InvokeUnCheck();

            if (toggleGoodsIssue.Checked)
                toggleGoodsIssue.InvokeUnCheck();
        }
    }
    private void ToggleItemsCheckedChanged(object sender, EventArgs e)
    {
        if (toggleItems.Checked)
        {
            PlaySound.Click();
            if (toggleDepartments.Checked)
                toggleDepartments.InvokeUnCheck();

            if (toggleVendors.Checked)
                toggleVendors.InvokeUnCheck();

            //if (toggleItems.Checked)
            //    toggleItems.InvokeUnCheck();

            if (toggleGoodsRecepitPO.Checked)
                toggleGoodsRecepitPO.InvokeUnCheck();

            if (toggleGoodsRecepit.Checked)
                toggleGoodsRecepit.InvokeUnCheck();

            if (toggleGoodsIssue.Checked)
                toggleGoodsIssue.InvokeUnCheck();
        }
    }

    private void toggleGoodsRecepitPO_CheckedChanged(object sender, EventArgs e)
    {
        if (toggleGoodsRecepitPO.Checked)
        {
            PlaySound.Click();
            if (toggleDepartments.Checked)
                toggleDepartments.InvokeUnCheck();

            if (toggleVendors.Checked)
                toggleVendors.InvokeUnCheck();

            if (toggleItems.Checked)
                toggleItems.InvokeUnCheck();

            //if (toggleGoodsRecepitPO.Checked)
            //    toggleGoodsRecepitPO.InvokeUnCheck();

            if (toggleGoodsRecepit.Checked)
                toggleGoodsRecepit.InvokeUnCheck();

            if (toggleGoodsIssue.Checked)
                toggleGoodsIssue.InvokeUnCheck();
        }
    }

    private void toggleGoodsRecepit_CheckedChanged(object sender, EventArgs e)
    {
        if (toggleGoodsRecepit.Checked)
        {
            PlaySound.Click();
            if (toggleDepartments.Checked)
                toggleDepartments.InvokeUnCheck();

            if (toggleVendors.Checked)
                toggleVendors.InvokeUnCheck();

            if (toggleItems.Checked)
                toggleItems.InvokeUnCheck();

            if (toggleGoodsRecepitPO.Checked)
                toggleGoodsRecepitPO.InvokeUnCheck();

            //if (toggleGoodsRecepit.Checked)
            //    toggleGoodsRecepit.InvokeUnCheck();

            if (toggleGoodsIssue.Checked)
                toggleGoodsIssue.InvokeUnCheck();
        }
    }

    private void toggleGoodsIssue_CheckedChanged(object sender, EventArgs e)
    {
        if (toggleGoodsIssue.Checked)
        {
            PlaySound.Click();
            if (toggleDepartments.Checked)
                toggleDepartments.InvokeUnCheck();

            if (toggleVendors.Checked)
                toggleVendors.InvokeUnCheck();

            if (toggleItems.Checked)
                toggleItems.InvokeUnCheck();

            if (toggleGoodsRecepitPO.Checked)
                toggleGoodsRecepitPO.InvokeUnCheck();

            if (toggleGoodsRecepit.Checked)
                toggleGoodsRecepit.InvokeUnCheck();

            //if (toggleGoodsIssue.Checked)
            //    toggleGoodsIssue.InvokeUnCheck();
        }
    }

    private void PlayClickSound(object sender, EventArgs e)
        => PlaySound.KeyPress();

    private void textBoxDocCode_TextChanged(object sender, EventArgs e)
        => PlaySound.KeyPress();

    private void TextBoxIntervalSchedule_TextChanged(object sender, EventArgs e)
        => PlaySound.KeyPress();

    private void ClearSyncDataGridView()
    {
        dataGridViewSync.DataSource = null;
        dataGridViewSync.Rows.Clear();
    }

    private void SyncClearLogs_Click(object sender, EventArgs e)
        => textBoxLogsSync.Clear();

    private void MenuISyncClearMonitoringLogs_Click(object sender, EventArgs e)
        => ClearSyncDataGridView();

    private void toolStripMenuItem6_Click(object sender, EventArgs e)
        => RefreshAuthSession();

    private void MenuISyncCopy_Click(object sender, EventArgs e)
        => CopyText(textBoxLogsSync);


    private void ClearInitialDataGridView()
    {
        dataGridViewInitial.DataSource = null;
        dataGridViewInitial.Rows.Clear();
    }

    private void InitialClearLogs_Click(object sender, EventArgs e)
        => textBoxLogsInitialize.Clear();

    private void MenuInitialCopyText_Click(object sender, EventArgs e)
        => CopyText(textBoxLogsInitialize);

    private void CopyText(RichTextBox textBoxLogs)
    {
        if (!string.IsNullOrEmpty(textBoxLogs.SelectedText))
        {
            Clipboard.SetText(textBoxLogs.SelectedText);
            labelStatus.Log("Status: Selected Text Copied", Logger.MessageTypes.Warning, Logger.MessageTime.Short);
        }
    }
    private void MenuInitialClearMoniteringLogs_Click(object sender, EventArgs e)
        => ClearInitialDataGridView();
    private void MenuInitialRefreshAuth_Click(object sender, EventArgs e)
        => RefreshAuthSession();

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

    private void textBoxDocCode_Enter(object sender, EventArgs e)
    {
        radioButtonDocId.Checked = true;
        PlayClickSound(sender, e);
    }

    private void dateTimePickerFrom_Enter(object sender, EventArgs e)
        => radioButtonDate.Checked = true;

    private void dateTimePickerTo_Enter(object sender, EventArgs e)
        => radioButtonDate.Checked = true;

    private void textBoxDocCode_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
            buttonSyncNow.PerformClick();
    }

    private void toggleGoodsRecepitPO_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
            buttonInitialzeNow.PerformClick();
    }

}