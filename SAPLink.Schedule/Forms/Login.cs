using SAPLink.Handler.Prism.Connection.Auth;
using SAPLink.Handler.SAP.Application;
using SAPLink.Schedule.Utilities;
using Serilog;
using Helper = SAPLink.Schedule.Utilities.Helper;

namespace SAPLink.Schedule.Forms
{
    public partial class Login : Form
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ServiceLayerHandler _serviceLayer;
        private IOrderedEnumerable<Clients> _clients;
        private Clients _client;
        private Credentials _credentials;
        private ItemsService _itemsService;
        private readonly DepartmentService _departmentService;
        private static readonly ILogger Log = Serilog.Log.ForContext<Login>();

        public Login(UnitOfWork unitOfWork, ServiceLayerHandler serviceLayer, DepartmentService departmentService,
            ItemsService itemsService, Clients client)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _serviceLayer = serviceLayer;
            _departmentService = departmentService;
            _itemsService = itemsService;
            _client = client;
            _credentials = _client.Credentials.FirstOrDefault();

            string[] includes = { "Credentials", "Credentials.Subsidiaries" };
            _clients = _unitOfWork.Clients.GetAll(includes).OrderBy(x => x.Id);


            if (_clients.Any())
            {
                comboBoxClients.AddItem(0, "--Choose Environment--");

                foreach (var cli in _clients)
                {
                    var envName = cli.Credentials.FirstOrDefault().EnvironmentName.Replace(" Environment", " Env. - ");
                    comboBoxClients.AddItem(cli.Id, envName + cli.Name);
                }

                comboBoxClients.SelectedIndex = _client.Id;
            }

            ClientHandler.InitializeClientObjects(_client, out var code, out var message);

            Log.Information($"{code} - {message}");

            ClientHandler.AddPrismFieldsToSAP();
            if (code != 0)
            {
                MessageBox.Show($"Error While Connecting to SAP B1. Details\r\n{message}", "Error!!");
            }
        }


        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            var comboBoxClient = comboBoxClients.SelectedIndex;

            if (comboBoxClient == (int)Enums.Environments.None)
                MessageBox.Show("Please Select a valid Client.");

            //else if (comboBoxClient == (int)Environments.Production)
            //{
            //    var result = MessageBox.Show("You trying to login to 'Prism Production System'.\r\nWould you like to continue?", "Confirmation Message", MessageBoxButtons.YesNo);
            //    if (result == DialogResult.Yes)
            //        SignIn();
            //}
            //else
            //{
                SignIn();
            //}
        }

        async void SignIn()
        {
            var userName = textBoxUserName.Text;
            var password = textBoxPassword.Text;

            if ((userName.IsHasValue() && userName != "User Name")
                                && (password.IsHasValue() && password != "Password"))
            {
                ScheduledSyncs scheduledSyncs = new ScheduledSyncs(_unitOfWork, _serviceLayer, _departmentService, _itemsService, _client);

                //GetNewPrismAuthSession(userName, password);


               // var allDepartments = await _departmentService.GetAll();

                //var isPrismConnected = allDepartments.Response.StatusCode == HttpStatusCode.OK;

                //if (isPrismConnected)
                //{
                //    Program.IsPrismConnected = true;
                //    //mainScreen.CheckConnectivity(true);
                //}
               // else
                    //mainScreen.CheckConnectivity(false);

                Hide();
                scheduledSyncs.Show();
            }
            else
                MessageBox.Show("Wrong User Name Or Password!!");
        }
        private async void GetNewPrismAuthSession(string userName, string password)
        {
            var newAuth = await LoginManager.GetAuthSessionAsync(_credentials.BaseUri, userName, password);
            if (newAuth.IsHasValue())
            {
                _credentials.AuthSession = newAuth;

                _unitOfWork.Credentials.Update(_credentials);
                _unitOfWork.SaveChanges();
            }
            else
                GetNewPrismAuthSession(userName, password);
        }
        private async Task<bool> IsUrlReachable()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(3);
                try
                {
                    var cts = new CancellationTokenSource();
                    var timeoutTask = Task.Delay(httpClient.Timeout, cts.Token);
                    var requestTask = httpClient.GetAsync("https://www.google.com/");

                    var completedTask = await Task.WhenAny(timeoutTask, requestTask);
                    if (completedTask == timeoutTask)
                    {
                        // Timeout occurred
                        cts.Cancel();
                        return false;
                    }

                    var response = await requestTask;
                    return response.IsSuccessStatusCode;
                }
                catch
                {
                    return false;
                }
            }
        }
        private void guna2TextBox1_Enter(object sender, EventArgs e)
        {
            if (textBoxUserName.Text == "User Name")
            {
                textBoxUserName.Clear();
                textBoxUserName.ForeColor = Color.Black;
            }
        }

        private void guna2TextBox1_Leave(object sender, EventArgs e)
        {
            if (textBoxUserName.Text == "")
            {
                textBoxUserName.Text = "User Name";
                textBoxUserName.ForeColor = Color.DarkGray;
            }
        }

        private void guna2TextBox2_Enter(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == "Password")
            {
                textBoxPassword.Clear();
                textBoxPassword.ForeColor = Color.Black;
            }

            textBoxPassword.PasswordChar = '*';
        }

        private void guna2TextBox2_Leave(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == "")
            {
                textBoxPassword.Text = "Password";
                textBoxPassword.ForeColor = Color.DarkGray;
                textBoxPassword.PasswordChar = (char)0;
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Helper.TryKillProcess();
        }

        private void textBoxUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBoxPassword.Focus();
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonLogin.Focus();
        }

        private void buttonLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonLogin.PerformClick();
        }

        private async void LoginScreen_Load(object sender, EventArgs e)
        {
            //var isReachable = await IsUrlReachable();
            //if (!isReachable)
            //{
            //    guna2ToggleSwitch1.Checked = false;
            //}

            textBoxUserName.Text = _credentials.PrismUserName;
            textBoxPassword.Text = _credentials.PrismPassword;
            textBoxPassword.PasswordChar = '*';

            buttonLogin.Focus();
        }

        private void guna2Panel1_Click(object sender, EventArgs e)
        {
            textBoxUserName.Focus();
        }

        

        private void comboBoxClients_SelectedIndexChanged(object sender, EventArgs e)
        {

            var selectedClient = _clients.FirstOrDefault(x => x.Id == comboBoxClients.SelectedIndex);

            if (selectedClient != null)
            {
                // Update all other clients as not activated
                foreach (var client in _clients)
                {
                    if (client.Id != selectedClient.Id)
                    {
                        client.Active = false;
                        var credential = client.Credentials.FirstOrDefault();
                        credential.Active = false;

                        _unitOfWork.Clients.Update(client);
                        _unitOfWork.Credentials.Update(credential);

                    }
                    else
                    {
                        client.Active = true;
                        _client = client;

                        _credentials = _client.Credentials.FirstOrDefault();
                        _credentials.Active = true;

                        _unitOfWork.Clients.Update(_client);
                        _unitOfWork.Credentials.Update(_credentials);
                        textBoxUserName.Text = _credentials.PrismUserName;
                        textBoxPassword.Text = _credentials.PrismPassword;
                    }
                }

                _unitOfWork.SaveChanges();
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
