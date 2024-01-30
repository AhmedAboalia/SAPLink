namespace SAPLink.Domain.Models.Prism.Merchandise.Inventory;

public class ProductResponseModel
{
    [JsonProperty("sid")]
    public string Sid { get; set; }

    [JsonProperty("rowversion")]
    public string Rowversion { get; set; }

    [JsonProperty("sbssid")]
    public string Sbssid { get; set; }

    [JsonProperty("alu")]
    public string Alu { get; set; }

    [JsonProperty("upc")]
    public string Upc { get; set; }

}

//public class ProductGetDto
//{
//    [JsonProperty("sid")]
//    public string Sid { get; set; }

//    [JsonProperty("createdby")]
//    public string Createdby { get; set; }

//    [JsonProperty("createddatetime")]
//    public DateTimeOffset Createddatetime { get; set; }

//    [JsonProperty("modifiedby")]
//    public string Modifiedby { get; set; }

//    [JsonProperty("modifieddatetime")]
//    public DateTimeOffset Modifieddatetime { get; set; }

//    [JsonProperty("controllersid")]
//    public string Controllersid { get; set; }

//    [JsonProperty("originapplication")]
//    public string Originapplication { get; set; }

//    [JsonProperty("postdate")]
//    public DateTimeOffset Postdate { get; set; }

//    [JsonProperty("rowversion")]
//    public long Rowversion { get; set; }

//    [JsonProperty("tenantsid")]
//    public string Tenantsid { get; set; }

//    [JsonProperty("invnitemuid")]
//    public string Invnitemuid { get; set; }

//    [JsonProperty("sbssid")]
//    public string Sbssid { get; set; }

//    [JsonProperty("alu")]
//    public string Alu { get; set; }

//    [JsonProperty("stylesid")]
//    public string Stylesid { get; set; }

//    [JsonProperty("dcssid")]
//    public string Dcssid { get; set; }

//    [JsonProperty("vendsid")]
//    public string Vendsid { get; set; }

//    [JsonProperty("description1")]
//    public string Description1 { get; set; }

//    [JsonProperty("description2")]
//    public string Description2 { get; set; }

//    [JsonProperty("description3")]
//    public string Description3 { get; set; }

//    [JsonProperty("description4")]
//    public string Description4 { get; set; }

//    [JsonProperty("longdescription")]
//    public string Longdescription { get; set; }

//    [JsonProperty("text1")]
//    public object Text1 { get; set; }

//    [JsonProperty("text2")]
//    public object Text2 { get; set; }

//    [JsonProperty("text3")]
//    public object Text3 { get; set; }

//    [JsonProperty("text4")]
//    public object Text4 { get; set; }

//    [JsonProperty("text5")]
//    public object Text5 { get; set; }

//    [JsonProperty("text6")]
//    public object Text6 { get; set; }

//    [JsonProperty("text7")]
//    public object Text7 { get; set; }

//    [JsonProperty("text8")]
//    public object Text8 { get; set; }

//    [JsonProperty("text9")]
//    public object Text9 { get; set; }

//    [JsonProperty("text10")]
//    public object Text10 { get; set; }

//    [JsonProperty("attribute")]
//    public object Attribute { get; set; }

//    [JsonProperty("cost")]
//    public long Cost { get; set; }

//    [JsonProperty("spif")]
//    public long Spif { get; set; }

//    [JsonProperty("currencysid")]
//    public string Currencysid { get; set; }

//    [JsonProperty("lastsolddate")]
//    public DateTimeOffset Lastsolddate { get; set; }

//    [JsonProperty("markdowndate")]
//    public object Markdowndate { get; set; }

//    [JsonProperty("discontinueddate")]
//    public object Discontinueddate { get; set; }

//    [JsonProperty("taxcodesid")]
//    public string Taxcodesid { get; set; }

//    [JsonProperty("udf1float")]
//    public object Udf1Float { get; set; }

//    [JsonProperty("udf2float")]
//    public object Udf2Float { get; set; }

//    [JsonProperty("udf3float")]
//    public object Udf3Float { get; set; }

//    [JsonProperty("udf1date")]
//    public object Udf1Date { get; set; }

//    [JsonProperty("udf2date")]
//    public object Udf2Date { get; set; }

//    [JsonProperty("udf3date")]
//    public object Udf3Date { get; set; }

//    [JsonProperty("itemsize")]
//    public object Itemsize { get; set; }

//    [JsonProperty("fccost")]
//    public object Fccost { get; set; }

//    [JsonProperty("fstprice")]
//    public object Fstprice { get; set; }

//    [JsonProperty("firstrcvddate")]
//    public DateTimeOffset Firstrcvddate { get; set; }

//    [JsonProperty("lastrcvddate")]
//    public DateTimeOffset Lastrcvddate { get; set; }

//    [JsonProperty("lastrcvdcost")]
//    public long Lastrcvdcost { get; set; }

//    [JsonProperty("commsid")]
//    public object Commsid { get; set; }

//    [JsonProperty("discschedulesid")]
//    public object Discschedulesid { get; set; }

//    [JsonProperty("udf1string")]
//    public object Udf1String { get; set; }

//    [JsonProperty("udf2string")]
//    public object Udf2String { get; set; }

//    [JsonProperty("udf3string")]
//    public object Udf3String { get; set; }

//    [JsonProperty("udf4string")]
//    public string Udf4String { get; set; }

//    [JsonProperty("udf5string")]
//    public object Udf5String { get; set; }

//    [JsonProperty("sellabledate")]
//    public object Sellabledate { get; set; }

//    [JsonProperty("orderabledate")]
//    public object Orderabledate { get; set; }

//    [JsonProperty("orderable")]
//    public bool Orderable { get; set; }

//    [JsonProperty("useqtydecimals")]
//    public long Useqtydecimals { get; set; }

//    [JsonProperty("description")]
//    public object Description { get; set; }

//    [JsonProperty("regional")]
//    public bool Regional { get; set; }

//    [JsonProperty("active")]
//    public bool Active { get; set; }

//    [JsonProperty("qtypercase")]
//    public object Qtypercase { get; set; }

//    [JsonProperty("upc")]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long Upc { get; set; }

//    [JsonProperty("maxdiscperc1")]
//    public long Maxdiscperc1 { get; set; }

//    [JsonProperty("maxdiscperc2")]
//    public long Maxdiscperc2 { get; set; }

//    [JsonProperty("itemno")]
//    public object Itemno { get; set; }

//    [JsonProperty("serialtype")]
//    public long Serialtype { get; set; }

//    [JsonProperty("lottype")]
//    public long Lottype { get; set; }

//    [JsonProperty("kittype")]
//    public long Kittype { get; set; }

//    [JsonProperty("scalesid")]
//    public object Scalesid { get; set; }

//    [JsonProperty("promoqtydiscweight")]
//    public long Promoqtydiscweight { get; set; }

//    [JsonProperty("promoinvenexclude")]
//    public bool Promoinvenexclude { get; set; }

//    [JsonProperty("noninventory")]
//    public bool Noninventory { get; set; }

//    [JsonProperty("noncommitted")]
//    public bool Noncommitted { get; set; }

//    [JsonProperty("itemstate")]
//    public long Itemstate { get; set; }

//    [JsonProperty("publishstatus")]
//    public long Publishstatus { get; set; }

//    [JsonProperty("ltypriceinpoints")]
//    public long Ltypriceinpoints { get; set; }

//    [JsonProperty("ltypointsearned")]
//    public long Ltypointsearned { get; set; }

//    [JsonProperty("minordqty")]
//    public object Minordqty { get; set; }

//    [JsonProperty("vendorlistcost")]
//    public object Vendorlistcost { get; set; }

//    [JsonProperty("tradediscpercent")]
//    public object Tradediscpercent { get; set; }

//    [JsonProperty("forceorigtax")]
//    public bool Forceorigtax { get; set; }

//    [JsonProperty("taxcode")]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long Taxcode { get; set; }

//    [JsonProperty("activestoresid")]
//    public string Activestoresid { get; set; }

//    [JsonProperty("activepricelevelsid")]
//    public string Activepricelevelsid { get; set; }

//    [JsonProperty("activeseasonsid")]
//    public string Activeseasonsid { get; set; }

//    [JsonProperty("actstrdbprice")]
//    public long Actstrdbprice { get; set; }

//    [JsonProperty("actstrprice")]
//    public double Actstrprice { get; set; }

//    [JsonProperty("actstrpricewt")]
//    public long Actstrpricewt { get; set; }

//    [JsonProperty("actstrohqty")]
//    public long Actstrohqty { get; set; }

//    [JsonProperty("actstrcaseqty")]
//    public long Actstrcaseqty { get; set; }

//    [JsonProperty("actstrsoldqty")]
//    public long Actstrsoldqty { get; set; }

//    [JsonProperty("actstrrcvdqty")]
//    public long Actstrrcvdqty { get; set; }

//    [JsonProperty("actstronorderedqty")]
//    public long Actstronorderedqty { get; set; }

//    [JsonProperty("actstravailqty")]
//    public long Actstravailqty { get; set; }

//    [JsonProperty("actstrextcost")]
//    public long Actstrextcost { get; set; }

//    [JsonProperty("actstrextprice")]
//    public double Actstrextprice { get; set; }

//    [JsonProperty("actstrextpricewt")]
//    public long Actstrextpricewt { get; set; }

//    [JsonProperty("actstrtaxpctg")]
//    public long Actstrtaxpctg { get; set; }

//    [JsonProperty("actstrtaxamt")]
//    public double Actstrtaxamt { get; set; }

//    [JsonProperty("actstrtaxpctg2")]
//    public long Actstrtaxpctg2 { get; set; }

//    [JsonProperty("actstrtaxamt2")]
//    public long Actstrtaxamt2 { get; set; }

//    [JsonProperty("actstrexttaxamt")]
//    public double Actstrexttaxamt { get; set; }

//    [JsonProperty("actstrexttaxamt1")]
//    public double Actstrexttaxamt1 { get; set; }

//    [JsonProperty("actstrexttaxamt2")]
//    public long Actstrexttaxamt2 { get; set; }

//    [JsonProperty("actstrmarginpctg")]
//    public long Actstrmarginpctg { get; set; }

//    [JsonProperty("actstrmarginamt")]
//    public double Actstrmarginamt { get; set; }

//    [JsonProperty("actstrextmarginamt")]
//    public double Actstrextmarginamt { get; set; }

//    [JsonProperty("actstrmarginamtwt")]
//    public long Actstrmarginamtwt { get; set; }

//    [JsonProperty("actstrextmarginamtwt")]
//    public long Actstrextmarginamtwt { get; set; }

//    [JsonProperty("actstrmarkuppctg")]
//    public long Actstrmarkuppctg { get; set; }

//    [JsonProperty("actstrcoefficient")]
//    public long Actstrcoefficient { get; set; }

//    [JsonProperty("actstrminqty")]
//    public long Actstrminqty { get; set; }

//    [JsonProperty("actstrmaxqty")]
//    public long Actstrmaxqty { get; set; }

//    [JsonProperty("actstrminextcost")]
//    public long Actstrminextcost { get; set; }

//    [JsonProperty("actstrminextprice")]
//    public long Actstrminextprice { get; set; }

//    [JsonProperty("actstrminextpricewt")]
//    public long Actstrminextpricewt { get; set; }

//    [JsonProperty("actstrmaxextcost")]
//    public long Actstrmaxextcost { get; set; }

//    [JsonProperty("actstrmaxextprice")]
//    public long Actstrmaxextprice { get; set; }

//    [JsonProperty("actstrmaxextpricewt")]
//    public long Actstrmaxextpricewt { get; set; }

//    [JsonProperty("cmpstrohqty")]
//    public long Cmpstrohqty { get; set; }

//    [JsonProperty("cmpstrextcost")]
//    public long Cmpstrextcost { get; set; }

//    [JsonProperty("cmpstrextprice")]
//    public long Cmpstrextprice { get; set; }

//    [JsonProperty("cmpstrminqty")]
//    public long Cmpstrminqty { get; set; }

//    [JsonProperty("cmpstrminextcost")]
//    public long Cmpstrminextcost { get; set; }

//    [JsonProperty("cmpstrminextprice")]
//    public long Cmpstrminextprice { get; set; }

//    [JsonProperty("cmpstrminextpricewt")]
//    public long Cmpstrminextpricewt { get; set; }

//    [JsonProperty("cmpstrmaxqty")]
//    public long Cmpstrmaxqty { get; set; }

//    [JsonProperty("cmpstrmaxextcost")]
//    public long Cmpstrmaxextcost { get; set; }

//    [JsonProperty("cmpstrmaxextprice")]
//    public long Cmpstrmaxextprice { get; set; }

//    [JsonProperty("cmpstrmaxextpricewt")]
//    public long Cmpstrmaxextpricewt { get; set; }

//    [JsonProperty("udf12string")]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long Udf12String { get; set; }

//    [JsonProperty("udf8string")]
//    public object Udf8String { get; set; }

//    [JsonProperty("udf13string")]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long Udf13String { get; set; }

//    [JsonProperty("udf9string")]
//    public object Udf9String { get; set; }

//    [JsonProperty("udf10string")]
//    public object Udf10String { get; set; }

//    [JsonProperty("udf11string")]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long Udf11String { get; set; }

//    [JsonProperty("udf2largestring")]
//    public object Udf2Largestring { get; set; }

//    [JsonProperty("udf14string")]
//    public object Udf14String { get; set; }

//    [JsonProperty("udf15string")]
//    public object Udf15String { get; set; }

//    [JsonProperty("udf1largestring")]
//    public object Udf1Largestring { get; set; }

//    [JsonProperty("udf6string")]
//    public object Udf6String { get; set; }

//    [JsonProperty("udf7string")]
//    public object Udf7String { get; set; }

//    [JsonProperty("dcscode")]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long Dcscode { get; set; }

//    [JsonProperty("sbsno")]
//    public long Sbsno { get; set; }

//    [JsonProperty("sbsname")]
//    public string Sbsname { get; set; }

//    [JsonProperty("scaleno")]
//    public object Scaleno { get; set; }

//    [JsonProperty("scalename")]
//    public object Scalename { get; set; }

//    [JsonProperty("vendorcode")]
//    public string Vendorcode { get; set; }

//    [JsonProperty("vendorname")]
//    public string Vendorname { get; set; }

//    [JsonProperty("vendorid")]
//    public long Vendorid { get; set; }

//    [JsonProperty("taxname")]
//    public string Taxname { get; set; }

//    [JsonProperty("currencyalphacode")]
//    public string Currencyalphacode { get; set; }

//    [JsonProperty("imagepath")]
//    public string Imagepath { get; set; }

//    [JsonProperty("cname")]
//    public object Cname { get; set; }

//    [JsonProperty("dname")]
//    public string Dname { get; set; }

//    [JsonProperty("sname")]
//    public object Sname { get; set; }

//    [JsonProperty("docitemsid")]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long Docitemsid { get; set; }

//    [JsonProperty("docitemrowversion")]
//    public long Docitemrowversion { get; set; }

//    [JsonProperty("height")]
//    public object Height { get; set; }

//    [JsonProperty("length")]
//    public object Length { get; set; }

//    [JsonProperty("width")]
//    public object Width { get; set; }

//    [JsonProperty("specialorder")]
//    public bool Specialorder { get; set; }

//    [JsonProperty("measureunit")]
//    public object Measureunit { get; set; }

//    [JsonProperty("shipmeasurement1")]
//    public object Shipmeasurement1 { get; set; }

//    [JsonProperty("shipmeasurement2")]
//    public object Shipmeasurement2 { get; set; }

//    [JsonProperty("docrowcount")]
//    public long Docrowcount { get; set; }

//    [JsonProperty("docqty")]
//    public long Docqty { get; set; }

//    [JsonProperty("doccaseqty")]
//    public long Doccaseqty { get; set; }

//    [JsonProperty("docprice")]
//    public long Docprice { get; set; }

//    [JsonProperty("doccost")]
//    public long Doccost { get; set; }

//    [JsonProperty("defaultprice")]
//    public double Defaultprice { get; set; }

//    [JsonProperty("defaultpricewt")]
//    public long Defaultpricewt { get; set; }

//    [JsonProperty("itemimage")]
//    public object Itemimage { get; set; }

//    [JsonProperty("itemimageindex")]
//    public object Itemimageindex { get; set; }

//    [JsonProperty("actstrqtysid")]
//    public string Actstrqtysid { get; set; }

//    [JsonProperty("actstrqtyrowver")]
//    public long Actstrqtyrowver { get; set; }

//    [JsonProperty("mmsid")]
//    public string Mmsid { get; set; }

//    [JsonProperty("actstrmmqtysid")]
//    public string Actstrmmqtysid { get; set; }

//    [JsonProperty("actstrmmqtyrowver")]
//    public long Actstrmmqtyrowver { get; set; }

//    [JsonProperty("actstrnewminqty")]
//    public long Actstrnewminqty { get; set; }

//    [JsonProperty("actstrnewmaxqty")]
//    public long Actstrnewmaxqty { get; set; }

//    [JsonProperty("actstrmmbegindate")]
//    public object Actstrmmbegindate { get; set; }

//    [JsonProperty("actstrmmenddate")]
//    public object Actstrmmenddate { get; set; }

//    [JsonProperty("serialno")]
//    public object Serialno { get; set; }

//    [JsonProperty("lotnumber")]
//    public object Lotnumber { get; set; }

//    [JsonProperty("lotexpirydate")]
//    public object Lotexpirydate { get; set; }
//}
