namespace SAPLink.Domain.System;

public class Subsidiaries
{
    [Key]
    public int Id { get; set; }
    public long SID { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public string ActivePriceLevelid { get; set; }
    public string ActiveSeasonSid { get; set; }
    public string ActiveStoreSid { get; set; }
    public string ActiveTaxCode { get; set; }
    public string Clerksid { get; set; }

    public int CredentialId { get; set; }
    public Credentials Credential { get; set; }

    //public ICollection<TaxCodes?> TaxCodes { get; set; }


    //public List<PriceLevelDto> PriceLevels;
    //public List<Season> Seasons;
    //public List<TaxCode> TaxCodes;
    //public List<Store> Stores;
}