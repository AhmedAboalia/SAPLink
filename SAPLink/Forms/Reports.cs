using SAPLink.Handler.Prism.Connection.Auth;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;
using Documents = SAPLink.Core.InboundEnums.Documents;
using SAPLink.Handler.Prism.Handlers.InboundData.Receiving;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Vendors;
using SAPLink.Utilities.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using SAPLink.Handler.SAP.Application;
using System.ComponentModel.DataAnnotations;
using SAPLink.Handler.Prism.Handlers.InboundData.Receiving.GoodsIssue;
using SAPLink.Handler.Prism.Handlers.InboundData.Receiving.GoodsReceipt;
using SAPLink.Handler.Prism.Handlers.InboundData.Receiving.GoodsReceiptPo;

namespace SAPLink.Forms;

public partial class Reports : Form
{
    #region Initial variables

    private readonly UnitOfWork _unitOfWork;
    private readonly ServiceLayerHandler _serviceLayer;
    private static  Clients _client;
    private readonly ItemsService _itemsService;
    private readonly ItemsHandler _itemsHandler;
    private readonly DepartmentService _departmentServices;
    private readonly DepartmentsHandler _departmentsHandler;
    private readonly VendorsHandler _vendorsHandler;
    private readonly GoodsReceiptPoHandler _goodsReceiptPoHandler;
    private readonly GoodsReceiptHandler _goodsReceiptHandler;
    private readonly GoodsIssueHandler _goodsIssueHandler;
    private readonly Credentials _credentials;

    public Reports(UnitOfWork unitOfWork, ServiceLayerHandler serviceLayer, DepartmentService departmentServices,
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
    }

    #endregion

    private async void buttonRunReport_Click(object sender, EventArgs e)
    {
        if (togglePrismActiveItems.Checked)
        {
            PlaySound.Click();
            var items = GetItems("");
            dataGridView.DataSource = items;
            SetDataGridViewHeaders(dataGridView, typeof(ItemMasterDataReport));
        }

        if (toggleSyncedItems.Checked)
        {
            PlaySound.Click();
            var items = GetItems("AND T0.[U_SyncToPrism] = 'Y'");
            dataGridView.DataSource = items;
            SetDataGridViewHeaders(dataGridView, typeof(ItemMasterDataReport));
        }

        if (toggleNotSyncedItems.Checked)
        {
            PlaySound.Click();
            var items = GetItems("AND (T0.[U_SyncToPrism] IS NULL OR  T0.[U_SyncToPrism] = '' OR  T0.[U_SyncToPrism] = 'N')");
            dataGridView.DataSource = items;
            SetDataGridViewHeaders(dataGridView, typeof(ItemMasterDataReport));
        }

        if (toggleGoodsReceiptPO.Checked)
        {
            PlaySound.Click();

        }
        if (toggleGoodsReceipt.Checked)
        {
            PlaySound.Click();


        }
        if (toggleGoodsIssue.Checked)
        {
            PlaySound.Click();

        }
    }

    public void OpenFormWithSettings(int tabControlInventoryIndex, int report)
    {
        tabControlInventory.SelectedIndex = tabControlInventoryIndex;

        if (report == (int)Enums.Reports.PrismActiveItems)
            togglePrismActiveItems.Checked = true;

        else if (report == (int)Enums.Reports.SyncedItems)
            toggleSyncedItems.Checked = true;

        else if (report == (int)Enums.Reports.NotSynced)
            toggleNotSyncedItems.Checked = true;

    }
    private void Log(UpdateType updateType, string message, string status)
    {
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

    private void guna2Button4_Click(object sender, EventArgs e)
    {
        Close();
        PlaySound.Click();
        var administration = new Settings(_unitOfWork, _serviceLayer, _departmentServices, _itemsService, _client);
        administration.Show();
    }

    private void MinimizeButton_Click(object sender, EventArgs e)
        => WindowState = FormWindowState.Minimized;

    private void MainPanel_DoubleClick(object sender, EventArgs e)
        => ResizeForm();

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
        if (togglePrismActiveItems.Checked)
        {
            PlaySound.Click();
            if (toggleSyncedItems.Checked)
                toggleSyncedItems.InvokeUnCheck();

            if (toggleNotSyncedItems.Checked)
                toggleNotSyncedItems.InvokeUnCheck();

            if (toggleGoodsReceiptPO.Checked)
                toggleGoodsReceiptPO.InvokeUnCheck();

            if (toggleGoodsReceipt.Checked)
                toggleGoodsReceipt.InvokeUnCheck();

            if (toggleGoodsIssue.Checked)
                toggleGoodsIssue.InvokeUnCheck();
        }
    }

    private void toggleVendors_CheckedChanged(object sender, EventArgs e)
    {
        if (toggleSyncedItems.Checked)
        {
            PlaySound.Click();
            if (togglePrismActiveItems.Checked)
                togglePrismActiveItems.InvokeUnCheck();

            //if (toggleVendors.Checked)
            //    toggleVendors.InvokeUnCheck();

            if (toggleNotSyncedItems.Checked)
                toggleNotSyncedItems.InvokeUnCheck();

            if (toggleGoodsReceiptPO.Checked)
                toggleGoodsReceiptPO.InvokeUnCheck();

            if (toggleGoodsReceipt.Checked)
                toggleGoodsReceipt.InvokeUnCheck();

            if (toggleGoodsIssue.Checked)
                toggleGoodsIssue.InvokeUnCheck();
        }
    }
    private void ToggleItemsCheckedChanged(object sender, EventArgs e)
    {
        if (toggleNotSyncedItems.Checked)
        {
            PlaySound.Click();
            if (togglePrismActiveItems.Checked)
                togglePrismActiveItems.InvokeUnCheck();

            if (toggleSyncedItems.Checked)
                toggleSyncedItems.InvokeUnCheck();

            //if (toggleItems.Checked)
            //    toggleItems.InvokeUnCheck();

            if (toggleGoodsReceiptPO.Checked)
                toggleGoodsReceiptPO.InvokeUnCheck();

            if (toggleGoodsReceipt.Checked)
                toggleGoodsReceipt.InvokeUnCheck();

            if (toggleGoodsIssue.Checked)
                toggleGoodsIssue.InvokeUnCheck();
        }
    }

    private void toggleGoodsReceiptPO_CheckedChanged(object sender, EventArgs e)
    {
        if (toggleGoodsReceiptPO.Checked)
        {
            PlaySound.Click();
            if (togglePrismActiveItems.Checked)
                togglePrismActiveItems.InvokeUnCheck();

            if (toggleSyncedItems.Checked)
                toggleSyncedItems.InvokeUnCheck();

            if (toggleNotSyncedItems.Checked)
                toggleNotSyncedItems.InvokeUnCheck();

            //if (toggleGoodsReceiptPO.Checked)
            //    toggleGoodsReceiptPO.InvokeUnCheck();

            if (toggleGoodsReceipt.Checked)
                toggleGoodsReceipt.InvokeUnCheck();

            if (toggleGoodsIssue.Checked)
                toggleGoodsIssue.InvokeUnCheck();
        }
    }

    private void toggleGoodsReceipt_CheckedChanged(object sender, EventArgs e)
    {
        if (toggleGoodsReceipt.Checked)
        {
            PlaySound.Click();
            if (togglePrismActiveItems.Checked)
                togglePrismActiveItems.InvokeUnCheck();

            if (toggleSyncedItems.Checked)
                toggleSyncedItems.InvokeUnCheck();

            if (toggleNotSyncedItems.Checked)
                toggleNotSyncedItems.InvokeUnCheck();

            if (toggleGoodsReceiptPO.Checked)
                toggleGoodsReceiptPO.InvokeUnCheck();

            //if (toggleGoodsReceipt.Checked)
            //    toggleGoodsReceipt.InvokeUnCheck();

            if (toggleGoodsIssue.Checked)
                toggleGoodsIssue.InvokeUnCheck();
        }
    }

    private void toggleGoodsIssue_CheckedChanged(object sender, EventArgs e)
    {
        if (toggleGoodsIssue.Checked)
        {
            PlaySound.Click();
            if (togglePrismActiveItems.Checked)
                togglePrismActiveItems.InvokeUnCheck();

            if (toggleSyncedItems.Checked)
                toggleSyncedItems.InvokeUnCheck();

            if (toggleNotSyncedItems.Checked)
                toggleNotSyncedItems.InvokeUnCheck();

            if (toggleGoodsReceiptPO.Checked)
                toggleGoodsReceiptPO.InvokeUnCheck();

            if (toggleGoodsReceipt.Checked)
                toggleGoodsReceipt.InvokeUnCheck();

            //if (toggleGoodsIssue.Checked)
            //    toggleGoodsIssue.InvokeUnCheck();
        }
    }

    private void PlayClickSound(object sender, EventArgs e)
        => PlaySound.KeyPress();


    private void ClearInitialDataGridView()
    {
        dataGridView.DataSource = null;
        dataGridView.Rows.Clear();
    }

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


    private async void RefreshAuthSession()
    {
        var newAuth = await LoginManager.GetAuthSessionAsync(_credentials.BaseUri, _credentials.PrismUserName, _credentials.PrismPassword);
        if (newAuth.IsHasValue())
            labelStatus.Log("Status: Auth Session refreshed.", Logger.MessageTypes.Warning, Logger.MessageTime.Long);
        else
            labelStatus.Log("Status: Cant refresh Auth-Session, Wait a few seconds before you try again.", Logger.MessageTypes.Error, Logger.MessageTime.Long);
    }


    private void toggleGoodsReceiptPO_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
            buttonInitialzeNow.PerformClick();
    }

    private void guna2Button2_Click(object sender, EventArgs e)
    {
        Close();
        PlaySound.Click();
        var inboundData = new InboundData(_unitOfWork, _serviceLayer, _departmentServices, _itemsService, _client);
        inboundData.Show();
    }

    private void guna2Button5_Click(object sender, EventArgs e)
    {
        Refresh();
    }

    private static List<ItemMasterDataReport> GetItems(string filter)
    {

        var query = Query() + filter;

        if (!ClientHandler.Company.Connected)
        {
            ClientHandler.InitializeClientObjects(_client, out _, out _);
        }

        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        var items = new List<ItemMasterDataReport>();

        oRecordSet.DoQuery(query);

        for (var i = 0; i < oRecordSet.RecordCount; i++)
        {
            var item = CreateItemMasterData(oRecordSet);
            item.RowNumber = i + 1; 
            items.Add(item);
            oRecordSet.MoveNext();
        }

        return items;
    }
    private static string Query()
    {
        var query = "SELECT DISTINCT" +
                    " T0.ItemCode 'ItemCode'" +
                    ",T0.ItemName 'ItemName'" +
                    ",T0.FrgnName 'ForeignName'" +
                    "FROM " +
                    "   OITM T0 " +
                    "           WHERE " +
                    "               T0.[U_Active] = 'Y' ";
        return query;
    }
    private static ItemMasterDataReport CreateItemMasterData(Recordset recordSet)
    {
        var item = new ItemMasterDataReport();
        var field = recordSet.Fields;

        item.ItemCode = field.GetValue("ItemCode");
        item.ItemName = field.GetValue("ItemName");
        item.ForeignName = field.GetValue("ForeignName");

        return item;
    }
    private void SetDataGridViewHeaders(DataGridView dgv, Type type)
    {
        foreach (DataGridViewColumn column in dgv.Columns)
        {
            var prop = type.GetProperty(column.Name);
            if (prop != null)
            {
                var displayNameAttribute = prop.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
                if (displayNameAttribute != null)
                {
                    column.HeaderText = displayNameAttribute.Name;
                }
            }
        }
    }
}