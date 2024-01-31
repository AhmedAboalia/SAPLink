namespace SAPLink.Domain.System;

public class Credentials
{
    [Key]
    public int Id { get; set; }
    public int EnvironmentCode { get; set; }
    public string EnvironmentName { get; set; }
    public bool Active { get; set; }
    public int ClientId { get; set; }
    public Clients Client { get; set; }

    #region Prsim Credentials
    public string PrismUserName { get; set; }
    public string PrismPassword { get; set; }
    public string BaseUri { get; set; }
    public string BackOfficeUri { get; set; }
    public string CommonUri { get; set; }
    public string RestUri { get; set; }
    public string AuthSession { get; set; }
    public string Origin { get; set; }
    public string Referer { get; set; }

    #endregion

    #region SAP Credentials

    public string ServiceLayerUri { get; set; }
    public string Server { get; set; }
    public BoDataServerTypes ServerTypes { get; set; }
    public string CompanyDb { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string DbUserName { get; set; }
    public string DbPassword { get; set; }

    public string AuthUserName { get; set; }
    public string AuthPassword { get; set; }
    public string Authorization { get; set; }
    public string Cookie { get; set; }

    #endregion

    public ICollection<Subsidiaries?> Subsidiaries { get; set; }

    public string IntegrationUrl { get; set; }
    public bool ActiveLog { get; set; }

}