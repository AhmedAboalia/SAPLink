using System.ComponentModel;
using Guna.UI2.WinForms;
using SAPLink.Core;
using SAPLink.Core.Models.SAP.Documents;
using SAPLink.Core.Models.SAP.MasterData.BusinessPartners;
using SAPLink.Core.Models.SAP.MasterData.Items;
using SAPLink.Core.Models.System;
using SAPLink.Core.Utilities;
using SAPLink.EF;
using SAPLink.Handler.Integration;
using SAPLink.Handler.Prism.Connection.Auth;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Departments;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Inventory;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Vendors;
using SAPLink.Handler.Prism.Handlers.InboundData.Receiving;
using SAPLink.Handler.SAP.Handlers;
using SAPLink.Utilities;
using Documents = SAPLink.Core.InboundEnums.Documents;

namespace SAPLink.Schedule.Forms;

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
    private readonly GoodsReceiptPOHandler _goodsReceiptPoHandler;
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

        _goodsReceiptPoHandler = new GoodsReceiptPOHandler(unitOfWork, client);
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
        comboBoxRecurrenceSchedule.SelectedIndex = (int)Enums.Repeats.None;
    }

    #endregion

    private void buttonScheduleIt_Click(object sender, EventArgs e)
    {
        if (buttonScheduleIt.Enabled)
        {
            var document = (Documents)comboBoxDocTypeSchedule.SelectedIndex;

            try
            {
                var recurrence = _unitOfWork.Recurrences.Find(x => x.Document == document);
                if (recurrence != null)
                {
                    recurrence.Document = document;
                    recurrence.Recurring = (Enums.Repeats)comboBoxRecurrenceSchedule.SelectedIndex;

                    if (recurrence.Recurring == Enums.Repeats.Hourly)
                        recurrence.Interval = int.Parse(TextBoxIntervalSchedule.Text);
                    else
                        recurrence.DayOfWeek = (DayOfWeek)ComboBoxDaysOfWeek.SelectedIndex;

                    _unitOfWork.Recurrences.Update(recurrence);
                    _unitOfWork.SaveChanges();
                    ScheduleService.Run(recurrence, out var message);
                    textBoxLogsSchedule.Text = message;
                }
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

        var recurrence = new Recurrence();
        recurrence = ScheduleService.GetRecurrence(_unitOfWork, selectedIndex, recurrence);

        comboBoxRecurrenceSchedule.SelectedIndex = (int)recurrence.Recurring;
        TextBoxIntervalSchedule.Text = recurrence.Interval.ToString();
        ComboBoxDaysOfWeek.SelectedIndex = (int)recurrence.DayOfWeek;

        label3.Visible = true;
        comboBoxRecurrenceSchedule.Visible = true;

        labelRepeatEvery.Visible = true;
        TextBoxIntervalSchedule.Visible = true;

        //buttonSchdeuleIt.Enabled = false; // disabled for Delivery

    }
 
    
    private void guna2ControlBox1_Click(object sender, EventArgs e)
    {
        Helper.TryKillProcess();
    }

    private async void ButtonDashboard_Click(object sender, EventArgs e)
    {
        Close();
        var mainScreen = new Dashboard(_unitOfWork, _serviceLayer, _departmentServices, _itemsService, _client);
        mainScreen.Show();

        if (Program.IsPrismConnected)
        {
            //mainScreen.CheckConnectivity(true);
        }
        //else
           // mainScreen.CheckConnectivity(false);
    }

    private void ButtonOutboundData_Click(object sender, EventArgs e)
    {
        Close();
        //var outboundData = new OutboundData(_unitOfWork, _serviceLayer, _departmentServices, _itemsService, _client);
        //outboundData.Show();
    }
    private void InboundData_Resize(object sender, EventArgs e)
    {
        //webViewSchedule.Size = ClientSize - new Size(webViewSchedule.Location);
    }





    private void guna2Button4_Click(object sender, EventArgs e)
    {
        Close();
        //var administration = new Settings(_unitOfWork, _serviceLayer, _departmentServices, _itemsService, _client);
        //administration.Show();
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

    
    private void toolStripMenuItem6_Click(object sender, EventArgs e)
        => RefreshAuthSession();

    private void MenuInitialRefreshAuth_Click(object sender, EventArgs e)
        => RefreshAuthSession();

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

}