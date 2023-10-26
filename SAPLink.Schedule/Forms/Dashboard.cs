using SAPLink.Core.Models.System;
using SAPLink.Core.Utilities;
using SAPLink.EF;
using SAPLink.Handler.Prism.Connection.Auth;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Departments;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Inventory;
using SAPLink.Handler.SAP.Handlers;
using SAPLink.Utilities;

namespace SAPLink.Schedule.Forms
{
    public partial class Dashboard : Form
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ServiceLayerHandler _serviceLayer;
        private readonly Clients _client;
        private readonly ItemsService _itemsService;
        private readonly DepartmentService _departmentService;
        private readonly Credentials _credentials;



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
            WindowState = FormWindowState.Maximized;
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

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            //var settings = new Settings(_unitOfWork, _serviceLayer, _departmentService, _itemsService, _client);
            //settings.Show();
            //Hide();
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
                //CheckConnectivity(true);
                Program.IsPrismConnected = true;
            }
            else
                RefreshAuthSession();
        }


        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
            => Application.Restart();

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
            => Helper.TryKillProcess();
    }
}
