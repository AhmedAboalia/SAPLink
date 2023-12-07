using Guna.UI2.WinForms;
using Humanizer;
using SAPLink.Handler.Prism.Connection.Auth;
using SAPLink.Handler.SAP.Application;
using System.Security.AccessControl;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Documents = SAPLink.Core.InboundEnums.Documents;

namespace SAPLink.Forms
{
    public partial class Dashboard : Form
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ServiceLayerHandler _serviceLayer;
        private readonly Clients _client;
        private readonly ItemsService _itemsService;
        private readonly DepartmentService _departmentService;
        private readonly Credentials _credentials;
        private Guna2Button buttonGoodsReturn;


        public Dashboard(UnitOfWork unitOfWork, ServiceLayerHandler serviceLayer, DepartmentService departmentService,
            ItemsService itemsService, Clients client)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _serviceLayer = serviceLayer;
            _departmentService = departmentService;
            _itemsService = itemsService;
            _client = client;
            _credentials = _client.Credentials.FirstOrDefault();
        }

        private async void Dashboard_Load(object sender, EventArgs e)
        {
            // ResizeForm();
            WindowState = FormWindowState.Maximized;
            var user = _client.Credentials.FirstOrDefault().PrismUserName;
            if (user.IsHasValue())
            {
                if (user == "sysadmin")
                    label2.Text = "Hi, System Admin.";//Hi, Prism Sync User
                else
                    label2.Text = "Hi, SAP Link.";//Hi, Prism Sync User

                string databaseType = _credentials.CompanyDb switch
                {
                    "TESTDB" => "Test Environment",
                    "SBODemoGB" => "Local Environment",
                    "KaffaryDB" => "Production Environment",
                    _ => "Unknown Environment"
                };

                label4.Text = $"{databaseType} ({_credentials.CompanyDb})";
            }

            timer1.Enabled = true;
            timer1.Start();

            //GetNewPrismAuthSession(_credentials.PrismUserName, _credentials.PrismPassword);


            //var allDepartments = await _departmentService.GetAll();

            //var isPrismConnected = allDepartments.Response.StatusCode == HttpStatusCode.OK;

            //if (isPrismConnected)
            //    CheckConnectivity(true);
            //else
            //    CheckConnectivity(false);

            //var isReachable = await Helper.IsDashboardAvailable();
            //if (isReachable)
            //{
            //    //hangFireDashBoard.Visible = true;
            //    guna2Button6.Visible = true;
            //    guna2GroupBox3.Visible = true;
            //}
        }
        private void UpdateWelcomingMessage()
        {
            if (_credentials.PrismUserName == "sysadmin")
                label2.Text = "Welcome Back, System Admin.";
            else if (_credentials.PrismUserName.ToLower() == "saplink")
                label2.Text = "Welcome Back, SAP Link.";
            else
            {
                var user = _credentials.PrismUserName.Transform(To.TitleCase);
                label2.Text = $"Welcome Back, {user}.";
            }
        }

        private async void GetNewPrismAuthSession(string userName, string password)
        {
            var newAuth = await LoginManager.GetAuthSessionAsync(_credentials.BaseUri, userName, password);
            if (newAuth.IsHasValue())
            {
                Program.IsPrismConnected = true;
                _credentials.AuthSession = newAuth;

                _unitOfWork.Credentials.Update(_credentials);
                _unitOfWork.SaveChanges();
            }
            else
                GetNewPrismAuthSession(userName, password);
        }


        public void CheckConnectivity(bool isGenerateNewAuthChecked)
        {
            if (isGenerateNewAuthChecked)
            {
                ClientHandler.InitializeClientObjects(_client, out var code, out var Message);
                if (ClientHandler.Company != null && ClientHandler.Company.Connected)
                {
                    Program.IsSapConnected = true;
                    guna2GradientPanel1.FillColor = Color.Teal;
                    guna2GradientPanel2.FillColor = Color.MediumSeaGreen;
                    label6.ForeColor = Color.Lime;
                    label7.Text = "Ready for Sync (SAP and Prism are connected).";
                }
                else
                {
                    guna2GradientPanel1.FillColor = Color.OrangeRed;
                    guna2GradientPanel2.FillColor = Color.DarkRed;
                    label6.ForeColor = Color.Gold;
                    label7.Text = $"Not ready for Sync (SAP not connected {Message} - Prism connected).";
                }
            }
            else
            {
                ClientHandler.InitializeClientObjects(_client, out var code, out var Message);
                if (ClientHandler.Company != null && ClientHandler.Company.Connected)
                {
                    Program.IsSapConnected = true;
                    guna2GradientPanel1.FillColor = Color.OrangeRed;
                    guna2GradientPanel2.FillColor = Color.DarkRed;
                    label6.ForeColor = Color.Gold;
                    label7.Text = "Not ready for Sync (SAP Connected - Prism not connected).";
                }
                else
                {
                    Program.IsSapConnected = false;
                    Program.IsPrismConnected = false;
                    guna2GradientPanel1.FillColor = Color.OrangeRed;
                    guna2GradientPanel2.FillColor = Color.DarkRed;
                    label6.ForeColor = Color.Gold;
                    label7.Text = $"Not ready for Sync (SAP not connected {Message}  - Prism not connected).";
                }
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            string Url = _client.Credentials.FirstOrDefault().IntegrationUrl;
            //hangFireDashBoard.Source = new Uri($"{Url}/dashboard/recurring");
            //hangFireDashBoard.Source = new Uri($"{Url}/dashboard");
        }

        private void buttonDepartments_Click(object sender, EventArgs e)
            => OpenInboundData(Documents.Departments);
        private void buttonVendors_Click(object sender, EventArgs e)
            => OpenInboundData(Documents.Vendors);
        private void buttonItems_Click(object sender, EventArgs e)
            => OpenInboundData(Documents.Items);

        private void buttonGoodsReceiptPo_Click(object sender, EventArgs e)
            => OpenInboundData(Documents.GoodsReceiptPo);

        private void buttonGoodsReceipt_Click(object sender, EventArgs e)
            => OpenInboundData(Documents.GoodsReceipt);

        private void buttonGoodsIssue_Click(object sender, EventArgs e)
            => OpenInboundData(Documents.GoodsIssue);


        private void buttonInvoice_Click(object sender, EventArgs e)
            => OpenOutboundData(OutboundDocuments.SalesInvoice);

        private void buttonReturnInvoice_Click(object sender, EventArgs e)
            => OpenOutboundData(OutboundDocuments.ReturnInvoice);
        private void buttonStockTransfer_Click(object sender, EventArgs e)
            => OpenOutboundData(OutboundDocuments.StockTransfer);

        private void buttonStockTaking_Click(object sender, EventArgs e)
            => OpenOutboundData(OutboundDocuments.InventoryPosting);

        private void buttonOutGoodsReceipt_Click(object sender, EventArgs e)
            => OpenOutboundData(OutboundDocuments.GoodsReceipt);

        private void buttonOrders_Click(object sender, EventArgs e)
            => OpenOutboundData(OutboundDocuments.CustomerOrder);

        private void buttonOutGoodsIssue_Click(object sender, EventArgs e)
            => OpenOutboundData(OutboundDocuments.GoodsIssue);

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            PlaySound.Click();
            var settings = new Settings(_unitOfWork, _serviceLayer, _departmentService, _itemsService, _client);
            settings.Show();
            Hide();
        }

        private void MouseHover(object sender, EventArgs e) => PlaySound.Hover();

        private void OpenInboundData(Documents documents)
        {
            var inboundData = new InboundData(_unitOfWork, _serviceLayer, _departmentService, _itemsService, _client);
            inboundData.Show();
            inboundData.OpenFormWithSettings(1, (int)documents);
            Hide();
        }

        private void OpenOutboundData(OutboundDocuments documents)
        {
            var outboundData = new OutboundData(_unitOfWork, _serviceLayer, _departmentService, _itemsService, _client);
            outboundData.Show();
            outboundData.OpenFormWithSettings(0, (int)documents);
            Hide();
        }

        private void controlBoxExit_Click(object sender, EventArgs e)
        {
            Helper.TryKillProcess();
        }

        private void controlBoxMinimized_Click(object sender, EventArgs e) => WindowState = FormWindowState.Minimized;

        private void pictureBoxMain_DoubleClick(object sender, EventArgs e) => ResizeForm();

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

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
            => RefreshAuthSession();

        private async void RefreshAuthSession()
        {
            var newAuth = await LoginManager.GetAuthSessionAsync(_credentials.BaseUri, _credentials.PrismUserName, _credentials.PrismPassword);
            if (newAuth.IsHasValue())
            {
                CheckConnectivity(true);
                Program.IsPrismConnected = true;
            }
            else
                RefreshAuthSession();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            var partTick = 200;
            if (Program.IsSapConnected && Program.IsPrismConnected)
            {
                label6.ForeColor = Color.Lime;
                await Task.Delay(partTick);

                label6.ForeColor = Color.DarkOliveGreen;
                await Task.Delay(partTick);

                label6.ForeColor = Color.Lime;
                await Task.Delay(partTick);

                label6.ForeColor = Color.DarkOliveGreen;
            }
            else
            {
                label6.ForeColor = Color.Red;
                await Task.Delay(partTick);

                label6.ForeColor = Color.Gold;
                await Task.Delay(partTick);

                label6.ForeColor = Color.Red;
                await Task.Delay(partTick);

                label6.ForeColor = Color.Gold;
            }
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
            => Application.Restart();

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
            => Helper.TryKillProcess();

        private void buttonPrismActiveItems_Click(object sender, EventArgs e)
            => OpenReportsWithSettings(Enums.Reports.PrismActiveItems);

        private void buttonSyncedItems_Click(object sender, EventArgs e)
        => OpenReportsWithSettings(Enums.Reports.SyncedItems);    
        
        private void buttonNotSyncedItems_Click(object sender, EventArgs e)
        => OpenReportsWithSettings(Enums.Reports.NotSynced);

        private void OpenReportsWithSettings(Enums.Reports report)
        {
            var inboundData = new InboundData(_unitOfWork, _serviceLayer, _departmentService, _itemsService, _client);
            inboundData.Show();
            inboundData.OpenReportsWithSettings(2, (int)report);
            Hide();
        }

        private void buttonGoodsReturn_Click(object sender, EventArgs e)
            => OpenInboundData(Documents.GoodsReturn);
    }
}
