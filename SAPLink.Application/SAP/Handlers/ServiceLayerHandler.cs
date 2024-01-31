using SAPLink.Application.Connected_Services;
using SAPLink.Application.Connection;
using SAPLink.Application.SAP.Connection;
using SAPLink.Application.SAP.Interfaces;
using SAPLink.Domain.System;
using PriceList = SAPLink.Domain.SAP.MasterData.Items.PriceList;

namespace SAPLink.Application.SAP.Handlers;

public partial class ServiceLayerHandler : IServiceLayerHandler
{
    #region Decleration & Login

    private Session _session = new Session();
    private static Clients _client;

    private static bool IsConnected { get; set; }
    public bool Connected() => IsConnected;
    public void ReConnect() => Connect();
    public string SessionId() => _session.Id;
    public string SessionVersion() => _session.Version;
    public int SessionTimeout() => _session.Timeout;

    /// <summary>
    /// Constructor of Service Layer
    /// </summary>
    public ServiceLayerHandler(Clients client)
    {
        _client = client;
        Connect();
    }
    public void Connect()
    {
        var response = HttpClientFactory.Initialize("Login", Method.POST, _client);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            _session = JsonConvert.DeserializeObject<Session>(response.Content);
            IsConnected = true;
        }
        //else
        //MessageBox.Show($@"Error While Connecting to Service Layer - Error : {ApiHelper.LastErrorMessage}");
    }
    public void Connect(LoginModel.LoginTypes loginTypes, LoginModel loginData)
    {
        var response = HttpClientFactory.Initialize("Login", Method.POST, _client, loginTypes, loginData);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            _session = JsonConvert.DeserializeObject<Session>(response.Content);
            IsConnected = true;
        }
        //else
        //MessageBox.Show($@"Error While Connecting to Service Layer - Error : {ApiHelper.LastErrorMessage}");
    }
    public void Disconnect()
    {
        var response = HttpClientFactory.Initialize("Logout", Method.POST, _client);

        if (response.StatusCode == HttpStatusCode.NoContent)
            IsConnected = false;
        //else
        //MessageBox.Show($"Can Not Disconnect From Service Layer - Error : {response.ErrorMessage}");
    }

    #endregion

    public List<PriceList> GetAllPriceLists()
    {
        var priceLists = new List<PriceList>();

        IRestResponse response = null;
        try
        {
            var query = "PriceLists?$select=PriceListNo,PriceListName";
            var body = "";

            response = HttpClientFactory.Initialize(query, Method.GET, _client, LoginModel.LoginTypes.Basic, null, body);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var odata = JsonConvert.DeserializeObject<OdataSAP<PriceList>>(response.Content);

                if (odata != null && odata.Value != null)
                    priceLists = odata.Value.ToList();
            }
        }
        catch (Exception e)
        {
            //
        }

        return priceLists;
    }
}