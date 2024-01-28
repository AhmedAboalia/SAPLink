using SAPLink.Handler.Prism.Connection.Auth;
using SAPLink.Handler.Prism.Settings;
using System.Windows.Forms;
using static SAPLink.Core.Utilities.Extensions;
using static SAPLink.Utilities.Controls;

namespace SAPLink.Forms
{
    public partial class Settings : Form
    {
        private Clients _client;
        private Credentials _credentials;
        private readonly Subsidiaries _subsidiary;

        private readonly UnitOfWork _unitOfWork;
        private readonly ServiceLayerHandler _serviceLayer;
        private readonly ItemsService _itemsService;
        private readonly DepartmentService _departmentService;
        private readonly AdministrationService administrationService;

        //private PriceLevel _priceLevel;
        // private Season _season;

        public Settings(UnitOfWork unitOfWork, ServiceLayerHandler serviceLayer, DepartmentService departmentService,
            ItemsService itemsService, Clients client)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _serviceLayer = serviceLayer;
            _departmentService = departmentService;
            _itemsService = itemsService;
            _client = client;
            _credentials = _client.Credentials.FirstOrDefault();
            _subsidiary = client.Credentials.FirstOrDefault().Subsidiaries.FirstOrDefault();
            //_priceLevel = new PriceLevel();
            //_season = new Season();

            administrationService = new AdministrationService(_client);
            LoadSubsidiary();
            LoadCredentials();
            toggleActiveUILog.Checked = _credentials.ActiveLog;
        }


        private void LoadCredentials()
        {
            var clients = _unitOfWork.Clients.GetAll().OrderBy(x => x.Id);
            var allCredentials = _unitOfWork.Credentials.GetAll().OrderBy(x => x.EnvironmentCode);

            if (clients.Any())
            {
                foreach (var client in clients)
                {
                    var name = client.Name.Split(' ').FirstOrDefault();
                    comboBoxClient.AddItem(client.Id, client.Id + " - " + name + " Subsidiary");
                }
            }

            if (allCredentials.Any())
            {
                foreach (var cred in allCredentials)
                {
                    var name = cred.EnvironmentName.Replace(" Environment", "");
                    comboBoxEnvironment.AddItem(cred.EnvironmentCode, cred.EnvironmentCode + " - " + name);
                }
            }

            comboBoxClient.SelectedIndex = _client.Id;
            textBoxClient.Text = _client.Name;
            toggleActiveClient.Checked = _client.Active;



            comboBoxEnvironment.SelectedIndex = _credentials.EnvironmentCode;

            textBoxEnviromentName.Text = _credentials.EnvironmentName;

            textBoxServiceLayerUri.Text = _credentials.ServiceLayerUri;
            textBoxServer.Text = _credentials.Server;

            textBoxCompany.Text = _credentials.CompanyDb;
            var serverType = _credentials.ServerTypes;

            if (serverType == Enums.BoDataServerTypes.dst_MSSQL2016)
                toggleSQL2016.Checked = true;
            if (serverType == Enums.BoDataServerTypes.dst_MSSQL2019)
                toggleSQL2019.Checked = true;
            if (serverType == Enums.BoDataServerTypes.dst_HANADB)
                toggleHANA.Checked = true;

            textBoxUserName.Text = _credentials.UserName;
            textBoxPassword.Text = _credentials.Password;
            //textBoxAuthUserName.Text = _credentials.AuthUserName;
            //textBoxAuthPassword.Text = _credentials.AuthPassword;
            textBoxAuthorization.Text = _credentials.Authorization;
            //textBoxCookie.Text = _credentials.Cookie;


            textBoxServerBaseUri.Text = _credentials.BaseUri;
            textBoxAuthSession.Text = _credentials.AuthSession;

            textBoxPrismUserName.Text = _credentials.PrismUserName;
            textBoxPrismPassword.Text = _credentials.PrismPassword;

            if (comboBoxEnvironment.SelectedIndex == (int)Environments.None)
            {
                comboBoxEnvironment.SelectedIndex = (int)Environments.None;

                textBoxEnviromentName.Text = "";
                textBoxServiceLayerUri.Text = "";
                textBoxServer.Text = "";
                textBoxCompany.Text = "";
                toggleSQL2016.Checked = false;
                toggleSQL2019.Checked = false;
                toggleHANA.Checked = false;
                textBoxUserName.Text = "";
                textBoxPassword.Text = "";
                //textBoxAuthUserName.Text = "";
                //textBoxAuthPassword.Text = "";
                textBoxAuthorization.Text = "";
                //textBoxCookie.Text = "";
                //textBoxCookie.Text = "";

                textBoxServerBaseUri.Text = "";
                textBoxAuthSession.Text = "";

                textBoxPrismUserName.Text = "";
                textBoxPrismPassword.Text = "";

            }
        }

        private void LoadSubsidiary()
        {
            ComboBoxSubCode.Items.Clear();
            ComboBoxSubCode.AddItem(_subsidiary.Number, $"{_subsidiary.Number} - {_subsidiary.Name}");

            textBoxSubName.Text = _subsidiary.Name;
            textBoxSubSid.Text = _subsidiary.SID.ToString();
            textBoxClerkSid.Text = _subsidiary.Clerksid;
            TextBoxActivePrLvSID.Text = _subsidiary.ActivePriceLevelid;
            TextBoxActiveStoreSid.Text = _subsidiary.ActivePriceLevelid;
            TextBoxAciveSeasonSID.Text = _subsidiary.ActiveSeasonSid;
            TextBoxActiveTaxSid.Text = _subsidiary.ActiveTaxCode;
            ComboBoxSubCode.SelectedIndex = 0;

            //_priceLevel = _unitOfWork.PriceLevel.Find(x => x.Sid == _subsidiary.ActivePriceLevelid);
            //if (_priceLevel != null)
            //{
            //    ComboBoxPriceLvlNo.AddItem((int)_priceLevel.Pricelvl, _priceLevel.Pricelvlname);
            //    TextBoxPriceLvlSID.Text = _priceLevel.Sid;
            //}

            //_season = _unitOfWork.Season.Find(x => x.Sid == _subsidiary.ActiveSeasonSid);
            //if (_season != null)
            //{
            //    ComboBoxSeason.AddItem((int)_season.SeasonId, _season.Seasonname);
            //    TextBoxSeasonSID.Text = _season.Sid;
            //}
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
            => Helper.TryKillProcess();


        private void guna2ControlBox2_Click(object sender, EventArgs e)
            => WindowState = FormWindowState.Minimized;

        private void guna2PictureBox1_DoubleClick(object sender, EventArgs e)
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

        private void guna2Panel1_DoubleClick(object sender, EventArgs e) => ResizeForm();

        private void guna2ControlBox3_Click(object sender, EventArgs e)
        {
            guna2Elipse1.BorderRadius = WindowState == FormWindowState.Normal ? 30 : 0;
        }


        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            Close();
            Dashboard mainScreen = new Dashboard(_unitOfWork, _serviceLayer, _departmentService, _itemsService, _client);

            var allDepartments = await _departmentService.GetAll();

            var result = allDepartments.Response;
            var entityList = allDepartments.EntityList;
            var isPrismConnected = allDepartments.Response.StatusCode == HttpStatusCode.OK;

            if (isPrismConnected)
                mainScreen.CheckConnectivity(true);
            else
                mainScreen.CheckConnectivity(false);

            mainScreen.Show();


        }
        private void ButtonOpenInboundData_Click(object sender, EventArgs e)
        {
            Close();
            InboundData inboundData = new InboundData(_unitOfWork, _serviceLayer, _departmentService, _itemsService, _client);
            inboundData.Show();
        }



        private async void ButtonRefreshAuthSession_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Do you want to refresh 'Auth Session'.", "Confirmation Message", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                var newAuth = await LoginManager.GetAuthSessionAsync(_credentials.BaseUri, _credentials.PrismUserName, _credentials.PrismPassword);
                if (newAuth.IsHasValue())
                {
                    textBoxAuthSession.Text = newAuth;
                    labelStatus.Log("Status: Auth Session Updated.", Logger.MessageTypes.Info, Logger.MessageTime.Long);
                }
                else
                    labelStatus.Log("Status: Cant refresh 'Auth Session', Wait a Few Seconds Before You Try Again.", Logger.MessageTypes.Error, Logger.MessageTime.Long);

            }
        }


        private async void comboBoxEnvironment_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedIndex = (Environments)comboBoxEnvironment.SelectedIndex;

            if (selectedIndex == (int)Environments.None)
            {
                comboBoxEnvironment.SelectedIndex = (int)Environments.None;
                textBoxEnviromentName.Text = "";
                textBoxServiceLayerUri.Text = "";
                textBoxServer.Text = "";
                textBoxCompany.Text = "";
                toggleSQL2016.Checked = false;
                toggleSQL2019.Checked = false;
                toggleHANA.Checked = false;
                textBoxUserName.Text = "";
                textBoxPassword.Text = "";
                textBoxAuthorization.Text = "";

                textBoxServerBaseUri.Text = "";
                textBoxAuthSession.Text = "";

                textBoxPrismUserName.Text = "";
                textBoxPrismPassword.Text = "";
                toggleActiveUILog.Checked = false;
            }
            else
            {
                toggleSQL2016.Checked = false;
                toggleSQL2019.Checked = false;
                toggleHANA.Checked = false;

                _client = _unitOfWork.Clients.Find(x => x.Id == (int)selectedIndex);

                _credentials = _client.Credentials.FirstOrDefault();

                comboBoxEnvironment.SelectedIndex = _credentials.EnvironmentCode;
                textBoxEnviromentName.Text = _credentials.EnvironmentName;
                toggleActiveClient.Checked = _client.Active;

                textBoxServiceLayerUri.Text = _credentials.ServiceLayerUri;
                textBoxServer.Text = _credentials.Server;

                textBoxCompany.Text = _credentials.CompanyDb;
                var serverType = _credentials.ServerTypes;

                if (serverType == Enums.BoDataServerTypes.dst_MSSQL2016)
                    toggleSQL2016.Checked = true;
                if (serverType == Enums.BoDataServerTypes.dst_MSSQL2019)
                    toggleSQL2019.Checked = true;
                if (serverType == Enums.BoDataServerTypes.dst_HANADB)
                    toggleHANA.Checked = true;

                textBoxUserName.Text = _credentials.UserName;
                textBoxPassword.Text = _credentials.Password;
                textBoxAuthorization.Text = _credentials.Authorization;

                textBoxServerBaseUri.Text = _credentials.BaseUri;
                textBoxAuthSession.Text = _credentials.AuthSession;

                textBoxPrismUserName.Text = _credentials.PrismUserName;
                textBoxPrismPassword.Text = _credentials.PrismPassword;
                toggleActiveUILog.Checked = _credentials.ActiveLog;
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            Close();
            var administration = new Settings(_unitOfWork, _serviceLayer, _departmentService, _itemsService, _client);
            administration.Show();
        }

        private void SaveChanges()
        {
            if (comboBoxEnvironment.SelectedIndex != 0)
            {
                var selectedClient = comboBoxClient.SelectedIndex;
                var client = _unitOfWork.Clients.Find(x => x.Id == selectedClient);

                comboBoxClient.SelectedIndex = client.Id;
                client.Name = textBoxClient.Text;
                client.Active = toggleActiveClient.Checked;

                var selectedIndex = comboBoxEnvironment.SelectedIndex;
                var credentials = client.Credentials
                    .OrderBy(x => x.EnvironmentCode)
                    .FirstOrDefault(x => x.EnvironmentCode == selectedIndex);

                credentials.EnvironmentCode = comboBoxEnvironment.SelectedIndex;
                credentials.EnvironmentName = textBoxEnviromentName.Text;
                credentials.ActiveLog = toggleActiveUILog.Checked;
                credentials.ServiceLayerUri = textBoxServiceLayerUri.Text;
                credentials.Server = textBoxServer.Text;
                credentials.CompanyDb = textBoxCompany.Text;

                if (toggleSQL2016.Checked)
                    credentials.ServerTypes = Enums.BoDataServerTypes.dst_MSSQL2016;

                if (toggleSQL2019.Checked)
                    credentials.ServerTypes = Enums.BoDataServerTypes.dst_MSSQL2019;

                if (toggleHANA.Checked)
                    credentials.ServerTypes = Enums.BoDataServerTypes.dst_HANADB;

                credentials.UserName = textBoxUserName.Text;
                credentials.Password = textBoxPassword.Text;
                credentials.AuthUserName =
                    $@"{{""UserName"" : ""{credentials.UserName}"",""CompanyDB"" : ""{credentials.CompanyDb}""}}";
                credentials.AuthPassword = textBoxPassword.Text;
                credentials.Authorization = textBoxAuthorization.Text;


                credentials.BaseUri = textBoxServerBaseUri.Text;
                credentials.BackOfficeUri = $"{credentials.BaseUri}/api/backoffice";
                credentials.CommonUri = $"{credentials.BaseUri}/v1/rest";
                credentials.RestUri = $"{credentials.BaseUri}/api/common";
                credentials.Origin = credentials.BaseUri;
                credentials.Referer = $"{credentials.BaseUri}/prism.shtml";

                credentials.AuthSession = textBoxAuthSession.Text;

                credentials.PrismUserName = textBoxPrismUserName.Text;
                credentials.PrismPassword = textBoxPrismPassword.Text;
                credentials.ActiveLog = toggleActiveUILog.Checked;

                _unitOfWork.Credentials.Update(credentials);
                _unitOfWork.SaveChanges();

                labelStatus.Log("Selected Credential is updated.", Logger.MessageTypes.Warning, Logger.MessageTime.Short);
            }
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            Close();
            OutboundData outboundData = new OutboundData(_unitOfWork, _serviceLayer, _departmentService, _itemsService, _client);
            outboundData.Show();
        }

        private void guna2TabControl1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            PlaySound.Click();
            //if (guna2TabControl1.SelectedIndex == 0)
            //{
            //    LoadSubsidiary();
            //}
        }
        private void PlayClickSound(object sender, EventArgs e)
        {
            PlaySound.Click();
            //if (guna2TabControl1.SelectedIndex == 0)
            //{
            //    LoadSubsidiary();
            //}
        }
        private async void comboBoxClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxClient.SelectedIndex == 0)
            {
                comboBoxEnvironment.SelectedIndex = (int)Environments.None;
                textBoxClient.Text = "";
                toggleActiveClient.Checked = false;
            }
            else
            {
                var selectedClient = comboBoxClient.SelectedIndex;

                string[] includes = { "Credentials", "Credentials.Subsidiaries" };
                var client = _unitOfWork.Clients.Find(x => x.Id == selectedClient, includes);

                comboBoxClient.SelectedIndex = client.Id;
                textBoxClient.Text = client.Name;
                toggleActiveClient.Checked = client.Active;

                comboBoxEnvironment.SelectedIndex = client.Credentials.FirstOrDefault().EnvironmentCode;
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!textBoxServiceLayerUri.Enabled)
            {
                editToolStripMenuItem.Text = "Save Changes";
                EnableControls();
            }
            else
            {
                editToolStripMenuItem.Text = "Change Settings";
                DisableControls();
                SaveChanges();
            }
        }



        private void DisableControls()
        {
            textBoxClient.Enabled = false;
            textBoxEnviromentName.Enabled = false;
            textBoxServiceLayerUri.Enabled = false;
            textBoxServer.Enabled = false;
            toggleSQL2016.Enabled = false;
            toggleSQL2019.Enabled = false;
            toggleHANA.Enabled = false;
            textBoxCompany.Enabled = false;

            textBoxUserName.Enabled = false;
            textBoxPassword.Enabled = false;
            textBoxAuthorization.Enabled = false;

            textBoxServerBaseUri.Enabled = false;
            textBoxAuthSession.Enabled = false;

            textBoxPrismUserName.Enabled = false;
            textBoxPrismPassword.Enabled = false;
            buttonRefreshAuthSession.Enabled = false;
            toggleActiveUILog.Enabled = false;
        }

        private void EnableControls()
        {
            textBoxClient.Enabled = true;
            textBoxEnviromentName.Enabled = true;
            textBoxServiceLayerUri.Enabled = true;
            textBoxServer.Enabled = true;
            toggleSQL2016.Enabled = true;
            toggleSQL2019.Enabled = true;
            toggleHANA.Enabled = true;
            textBoxCompany.Enabled = true;

            textBoxUserName.Enabled = true;
            textBoxPassword.Enabled = true;
            textBoxAuthorization.Enabled = true;

            textBoxServerBaseUri.Enabled = true;
            textBoxAuthSession.Enabled = true;

            textBoxPrismUserName.Enabled = true;
            textBoxPrismPassword.Enabled = true;
            buttonRefreshAuthSession.Enabled = true;
            toggleActiveUILog.Enabled = true;
        }


        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Do you want to restart Application?", "Restart Application!!",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
                Application.Restart();
        }

        private async void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ComboBoxSubCode.Items.Clear();

            var sbss = await administrationService.GetSubsidiaries();

            foreach (var sub in sbss)
            {
                var code = Convert.ToInt32(sub.SubsidiaryNumber);
                var name = sub.SubsidiaryName.ToTitleFormat();
                ComboBoxSubCode.AddItem(code, $"{code} - {name}");
            }

        }

        private async void ComboBoxSubCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBoxItem Sub = (ComboBoxItem)ComboBoxSubCode.SelectedItem;

                var subsidiary = await administrationService.GetSubsidiaryByNumber(Sub.Value.ToString());

                if (subsidiary != null)
                {
                    textBoxSubName.Text = subsidiary.SubsidiaryName;
                    textBoxSubSid.Text = subsidiary.Sid.ToString();
                    TextBoxActivePrLvSID.Text = subsidiary.ActivePriceLevelSid;
                    TextBoxAciveSeasonSID.Text = subsidiary.ActiveSeasonSid;

                }
            }
            catch (Exception exception)
            {
            }
        }

    }
}
