using SAPLink.Handler.Prism.Connection.Auth;
using SAPLink.Handler.SAP.Application;
using Serilog;
using System.Diagnostics;
using System.Net.Cache;
using System.Net;
using System.Text;
using Guna.UI2.WinForms;
using SAPLink.Forms.AutoUpdate;

namespace SAPLink.Forms
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

        private string _tempPath;
        private WebClient _webClient;


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

            //if (AutoUpdater.IsUpdateAvailable)
            //{
            //    panel2.Show();
            //    guna2CirclePictureBox2.Show();
            //}

            //Text = AutoUpdater.DialogTitle;
            //labelUpdate.Text = $"A New Version ({AutoUpdater.CurrentVersion}) is Available. {AutoUpdater.InstalledVersion} installed.";


            //label1.Text = $"Version {AutoUpdater.InstalledVersion}";
            Log.Information($"{code} - {message}");


            ClientHandler.AddPrismFieldsToSAP();
            if (code != 0)
            {
                MessageBox.Show($"Error While Connecting to SAP B1. Details\r\n{message}", "Error!!");
            }
        }
        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }
        private void ButtonUpdateClick(object sender, EventArgs e)
        {
            if (AutoUpdater.OpenDownloadPage)
            {
                Process.Start(new ProcessStartInfo(AutoUpdater.DownloadURL));
            }
            else
            {
                labelInformation.Visible = true;
                progressBar.Visible = true;

                _webClient = new WebClient();
                _tempPath = Path.Combine(Path.GetTempPath(), GetFileName(AutoUpdater.DownloadURL));

                _webClient.DownloadProgressChanged += OnDownloadProgressChanged;
                _webClient.DownloadFileCompleted += OnDownloadComplete;

                _webClient.DownloadFileAsync(new Uri(AutoUpdater.DownloadURL), _tempPath);
            }
        }
        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;

            StringBuilder stringBuilder = new StringBuilder(labelInformation.Text);

            if (stringBuilder.Length != 28)
                stringBuilder.Append(".");
            else
            {
                stringBuilder.Clear();
                stringBuilder.Append("Downloading Update...");
            }

            labelInformation.Text = stringBuilder.ToString();

        }

        private void OnDownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                var processStartInfo = new ProcessStartInfo { FileName = _tempPath, UseShellExecute = true };

                try
                {
                    labelInformation.Visible = false;
                    progressBar.Visible = false;

                    Process.Start(processStartInfo);

                    if (AutoUpdater.IsWinFormsApplication)
                        Application.Exit();
                    else
                    {
                        Environment.Exit(0);
                        Application.Exit();
                    }
                }
                catch (Exception)
                {
                    // Handle the exception if needed
                }
            }
        }

        private static string GetFileName(string url)
        {
            var fileName = string.Empty;
            var uri = new Uri(url);

            if (uri.Scheme.Equals(Uri.UriSchemeHttp) || uri.Scheme.Equals(Uri.UriSchemeHttps))
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                httpWebRequest.Method = "HEAD";
                httpWebRequest.AllowAutoRedirect = false;

                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    if (httpWebResponse.StatusCode.Equals(HttpStatusCode.Redirect) ||
                        httpWebResponse.StatusCode.Equals(HttpStatusCode.Moved) ||
                        httpWebResponse.StatusCode.Equals(HttpStatusCode.MovedPermanently))
                    {
                        var location = httpWebResponse.Headers["Location"];
                        fileName = GetFileName(location);
                        return fileName;
                    }

                    var contentDisposition = httpWebResponse.Headers["content-disposition"];
                    if (!string.IsNullOrEmpty(contentDisposition))
                    {
                        const string lookForFileName = "filename=";
                        var index = contentDisposition.IndexOf(lookForFileName, StringComparison.CurrentCultureIgnoreCase);
                        if (index >= 0)
                            fileName = contentDisposition.Substring(index + lookForFileName.Length);
                        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                            fileName = fileName[1..^1];
                    }
                }
            }

            return string.IsNullOrEmpty(fileName) ? Path.GetFileName(uri.LocalPath) : fileName;
        }

        private void DownloadUpdateDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            _webClient?.CancelAsync();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            var comboBoxClient = comboBoxClients.SelectedIndex;

            if (comboBoxClient == (int)Environments.None)
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
                Dashboard mainScreen = new Dashboard(_unitOfWork, _serviceLayer, _departmentService, _itemsService, _client);
                GetNewPrismAuthSession(userName, password);


                var allDepartments = await _departmentService.GetAll();

                var isPrismConnected = allDepartments.Response.StatusCode == HttpStatusCode.OK;

                if (isPrismConnected)
                {
                    Program.IsPrismConnected = true;
                    mainScreen.CheckConnectivity(true);
                }
                else
                    mainScreen.CheckConnectivity(false);

                PlaySound.Click();
                Hide();
                mainScreen.Show();
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
            PlaySound.Click();
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
            PlaySound.Click();
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

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            textBoxUserName.Focus();
        }

        private void guna2Panel2_Click(object sender, EventArgs e)
        {
            textBoxUserName.Focus();
        }

        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            PlaySound.Click();
        }

        private void PlayKeySound(object sender, EventArgs e)
        {
            PlaySound.KeyPress();
        }

        private void comboBoxClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlaySound.KeyPress();

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
