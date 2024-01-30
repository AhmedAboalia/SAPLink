using SAPLink.Domain.Models.Prism.Settings;

namespace SAPLink.Application.Prism.Connection;

public class Subsidiaries
{
    public long SID { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public string ActivePriceLevelid { get; set; }
    public string ActiveSeasonSid { get; set; }
    public string ActiveStoreSid { get; set; }
    public string ActiveTaxCode { get; set; }
    public string Clerksid { get; set; }

    private List<PriceLevel> PriceLevels;
    private List<Season> Seasons;
    public List<TaxCodes> TaxCodes;
    public List<Store> Stores;


    private static Subsidiaries Create(long sId, string name, int number, string priceLevel, string season, string store, string activeTaxCode, string clerksid,
        List<PriceLevel> priceLevels, List<Season> seasons, List<Store> stores, List<TaxCodes> taxCodes)
    {
        var subsidiary = new Subsidiaries
        {
            SID = sId,
            Name = name,
            Number = number,
            PriceLevels = priceLevels,
            Seasons = seasons,
            Stores = stores,
            ActivePriceLevelid = priceLevel,
            ActiveSeasonSid = season,
            ActiveStoreSid = store,
            ActiveTaxCode = activeTaxCode,
            Clerksid = clerksid,
            TaxCodes = taxCodes
        };
        return subsidiary;
    }
    public static Subsidiaries PublicAPI_RTC()
    {
        var sbsSid = 665151872000149257;
        var Clerksid = "665151872000152260";

        var priceLevels = new List<PriceLevel>
        {
            new("665151925000109722", sbsSid, 1, "PrLvl1", true),
            new("665151925000110723", sbsSid, 2, "PrLvl2", false)
        };

        var seasons = new List<Season>()
        {
            new("665151925000152735", "None", "None", 0, true),
            new("665151925000152736", "SPR", "Spring", 1, false),
            new("665151925000152737", "SUM", "Summer", 2, false),
            new("665151925000154738", "FAL", "Fall", 3, false),
            new("665151925000154739", "WNT", "Winter", 4, false),
            new("665151925000154740", "HOL", "Holiday", 5, false)
        };

        var taxCodes = new List<TaxCodes>()
        {
            new("665151925000173747", sbsSid, 0, "Taxable",true),
            new("665151925000174748", sbsSid, 1, "Exempt",false),
            new("665151925000176749", sbsSid, 2, "Luxury",false),
        };

        var priceLevel = priceLevels.FirstOrDefault(x => x.Active).Sid;

        var stores = new List<Store>()
        {
            new("665151872000156262",sbsSid,0,"WH","000", priceLevel,false),
            new("665151872000154261",sbsSid,1,"STORE 1","001", priceLevel,true),
        };

        var season = seasons.FirstOrDefault(x => x.Active).Sid;
        var taxCode = taxCodes.FirstOrDefault(x => x.IsDefault).Sid;
        var store = stores.FirstOrDefault(x => x.Active).Sid;

        return Create(sbsSid, "RTC", 1, priceLevel, season, store, taxCode, Clerksid, priceLevels, seasons, stores, taxCodes);
    }


    public static Subsidiaries KaffarySBS()
    {
        var sbsSid = 663852103000153257;
        var Clerksid = "";

        var priceLevels = new List<PriceLevel>()
        {
            new("663852140000113721", sbsSid, 1, "RETAIL", true),
            new("663852140000113722", sbsSid, 2, "WHOLE", false),
            new("669905830000106367", sbsSid, 3, "PROJCT", false)
        };

        var seasons = new List<Season>()
        {
            new("663852140000143734", "None", "None", 0,true),
            new("663852140000144735", "SPR", "Spring", 1, false),
            new("663852140000144736", "SUM", "Summer", 2, false),
            new("663852140000144737", "FAL", "Fall", 3, false),
            new("663852140000144738", "WNT", "Winter", 4, false),
            new("663852140000144739", "HOL", "Holiday", 5, false)
        };

        var taxCodes = new List<TaxCodes>()
        {
            new("663852140000157746", sbsSid, 0, "Taxable",true),
            new("663852140000157747", sbsSid, 1, "Exempt",false),
            new("663852140000158748", sbsSid, 2, "Luxury",false),
        };

        var priceLevel = priceLevels.FirstOrDefault(x => x.Active).Sid;
        var stores = new List<Store>()
        {
            new("663852103000156261",sbsSid,1,"STORE 1","001", priceLevel,true),
            new("669745537000113391",sbsSid,2,"STORE 2","002", priceLevel,false),
        };

        var season = seasons.FirstOrDefault(x => x.Active).Sid;
        var taxCode = taxCodes.FirstOrDefault(x => x.IsDefault).Sid;
        var store = stores.FirstOrDefault(x => x.Active).Sid;

        return Create(sbsSid, "Kaffary SBS", 1, priceLevel, season, store, taxCode, Clerksid, priceLevels, seasons, stores, taxCodes);
    }

    public static Subsidiaries KaffarySAPTest()
    {
        var sbsSid = 672158959000191487;
        var Clerksid = "663852103000155260";

        var priceLevels = new List<PriceLevel>()
        {
            new("672158961000176603", sbsSid, 1, "RETAIL", true),
            new("672158961000181606", sbsSid, 2, "WHOLE", false),
            new("672164335000146853", sbsSid, 3, "PROJCT", false)
        };

        var seasons = new List<Season>()
        {
            new("663852140000143734", "None", "None", 0,true),
            new("663852140000144735", "SPR", "Spring", 1, false),
            new("663852140000144736", "SUM", "Summer", 2, false),
            new("663852140000144737", "FAL", "Fall", 3, false),
            new("663852140000144738", "WNT", "Winter", 4, false),
            new("663852140000144739", "HOL", "Holiday", 5, false)
        };

        var taxCodes = new List<TaxCodes>()
        {
            new("672158960000161536", sbsSid, 1, "TAXABLE",true),
            new("672158960000169539", sbsSid, 2, "EXEMPT", false),
            new("672158960000173542", sbsSid, 3, "LUXURY", false),
            new("673016662000135646", sbsSid, 4, "TEST", false)
        };

        var priceLevel = priceLevels.FirstOrDefault(x => x.Active).Sid;

        var stores = new List<Store>()
        {
            new("672158960000148530",sbsSid,0,"Default0","000", priceLevel,true),
            new("672158960000156533",sbsSid,2,"Default1","001", priceLevel,false),
        };


        var season = seasons.FirstOrDefault(x => x.Active).Sid;

        var taxCode = taxCodes.FirstOrDefault(x => x.IsDefault).Sid;
        var store = stores.FirstOrDefault(x => x.Active).Sid;


        return Create(sbsSid, "SAP Test", 2, priceLevel, season, store, taxCode, Clerksid, priceLevels, seasons, stores, taxCodes);
    }

    public static Subsidiaries KaffaryProduction()
    {
        var sbsSid = 664651285000113257;
        var Clerksid = "674955099100039866";//"664651285000114260";


        var activePriceLevel = "664651377000135721";
        var season = "664651377000169734";
        var store = "664651285000116261";
        var taxCode = "664651377000183746";

        var priceLevels = new List<PriceLevel>()
        {
            new("664651377000135721", sbsSid, 1, "RETAIL", true),
            new("664651377000135722", sbsSid, 2, "WHOLE", false),
            new("674643627000142249", sbsSid, 3, "PROJCT", false)
        };

        var seasons = new List<Season>()
        {
            new("664651377000169734", "None", "None", 0,true),
            new("664651377000169735", "SPR", "Spring", 1, false),
            new("664651377000169736", "SUM", "Summer", 2, false),
            new("664651377000169737", "FAL", "Fall", 3, false),
            new("664651377000171738", "WNT", "Winter", 4, false),
            new("664651377000171739", "HOL", "Holiday", 5, false)
        };

        var taxCodes = new List<TaxCodes>()
        {
            new("664651377000183746", sbsSid, 0, "Taxable",true),
            new("664651377000185747", sbsSid, 2, "Exempt", false),
            new("664651377000185748", sbsSid, 3, "Luxury", false),
        };

        //var priceLevel = priceLevels.FirstOrDefault(x => x.Active).Sid;

        var stores = new List<Store>()
        {
            new("664651285000116262", sbsSid,0,"br0","000", activePriceLevel,false),
            new("664651285000116261",sbsSid,1,"br1","001", activePriceLevel,true),
        };


        //var season = seasons.FirstOrDefault(x => x.Active).Sid;

        //var taxCode = taxCodes.FirstOrDefault(x => x.Isdefault).Sid;
        //var store = stores.FirstOrDefault(x => x.Active).Sid;


        return Create(sbsSid, "AlKaffary", 1, activePriceLevel, season, store, taxCode, Clerksid, priceLevels, seasons, stores, taxCodes);
    }

    public static Subsidiaries KaffaryNewTest()
    {
        var sbsSid = 674650600000126277;
        var Clerksid = "674654182000171601";

        var season = "663852140000143734";
        var activePriceLevel = "674650602000169420";

        var taxCode = "674650601000152353";
        var store = "674650601000132347";

        var priceLevels = new List<PriceLevel>()
        {
            new("674650602000169420", sbsSid, 1, "RETAIL", true),
            new("674650602000173423", sbsSid, 2, "WHOLE", false),
            new("674653901000147593", sbsSid, 3, "PROJCT", false)
        };

        var seasons = new List<Season>()
        {
            new("663852140000143734", "None", "None", 0,true),
            new("663852140000144735", "SPR", "Spring", 1, false),
            new("663852140000144736", "SUM", "Summer", 2, false),
            new("663852140000144737", "FAL", "Fall", 3, false),
            new("663852140000144738", "WNT", "Winter", 4, false),
            new("663852140000144739", "HOL", "Holiday", 5, false)
        };

        var taxCodes = new List<TaxCodes>()
        {
            new("674650601000152353", sbsSid, 1, "TAXABLE",true),
            new("674650601000160356", sbsSid, 2, "EXEMPT", false),
            new("674650601000163359", sbsSid, 3, "LUXURY", false),
        };


        var stores = new List<Store>()
        {
            new("674650601000132347",sbsSid,0,"STORE_0","000", activePriceLevel,true),
            new("674650601000147350",sbsSid,2,"STORE_1","001", activePriceLevel,false),
        };

        return Create(sbsSid, "SAP Test", 3, activePriceLevel, season, store, taxCode, Clerksid, priceLevels, seasons, stores, taxCodes);
    }
}