using SAPLink.Core;
using SAPLink.Core.Models;
using SAPLink.Core.Models.Prism.Inventory.Products;
using SAPLink.Core.Models.SAP.MasterData.Items;
using SAPLink.Core.Models.System;
using SAPLink.Core.Utilities;
using SAPLink.Handler.Connected_Services;
using SAPLink.Handler.Connection;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Departments;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Vendors;
using SAPLink.Handler.Prism.Interfaces;
using SAPLink.Handler.SAP.Interfaces;
using HttpClientFactory = SAPLink.Handler.Connection.HttpClientFactory;

namespace SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Inventory;

public class ItemsService : IEntityService<RequestResult<ProductResponseModel>, ItemMasterData>
{
    private static DepartmentService _departmentService;
    private readonly VendorsService _vendorsHandler;

    private readonly Clients _client;
    private readonly Credentials _credentials;
    private readonly Subsidiaries _subsidiary;

    public ItemsService(Clients client, DepartmentService departmentService, VendorsService vendorsHandler)
    {
        _departmentService = departmentService;
        _vendorsHandler = vendorsHandler;

        _client = client;
        _credentials = _client.Credentials.FirstOrDefault();
        _subsidiary = _credentials.Subsidiaries.FirstOrDefault();
    }

    public async Task<RequestResult<ProductResponseModel>> GetByCodeAsync(string itemCode)
    {
        var result = new RequestResult<ProductResponseModel>();
        var query = _credentials.BackOfficeUri;
        var resource = $"/inventory?action=inventorygetitems&filter=(alu,eq,{itemCode})AND(sbssid,eq,{_subsidiary.SID})&cols=sid,upc,rowversion,sbssid,alu";

        var body = @"
                                   {
                                    ""data"": [
                                           {
                                             ""activestoresid"": """ + _subsidiary.ActiveStoreSid + @""",
                                             ""activepricelevelsid"": """ + _subsidiary.ActivePriceLevelid + @""",
                                             ""activeseasonsid"": """ + _subsidiary.ActiveSeasonSid + @"""
                                           
                                            }
                                     ]
                                  }";


        result.Response = await HttpClientFactory.InitializeAsync(query, resource, Method.POST, body);

        if (result.Response.StatusCode == HttpStatusCode.OK)
        {
            var productData = JsonConvert.DeserializeObject<Response<ProductResponseModel>>(result.Response.Content).Data.ToList();
            if (productData.Any() && productData.Count > 0)
            {
                var product = productData.FirstOrDefault();
                result.EntityList.Add(product);
                result.Status = Enums.StatusType.Success;
            }
        }
        else
        {
            result.Status = Enums.StatusType.NotFound;
            result.Message = $"Product Sid Is Null or Empty. \r\n \r\n {result.Response.Content} \r\n";
        }

        return result;
    }



    public async Task<RequestResult<ProductResponseModel>> Sync(string body, string sid = "", Enums.UpdateType updateType = Enums.UpdateType.InitialDepartment)
    {
        var result = new RequestResult<ProductResponseModel>();
        var query = _credentials.BackOfficeUri;
        const string resource = "/inventory?action=InventorySaveItems";

        result.Response = await HttpClientFactory.InitializeAsync(query, resource, Method.POST, body);

        if (result.Response.StatusCode == HttpStatusCode.OK)
        {
            var productData = JsonConvert.DeserializeObject<Response<ProductResponseModel>>(result.Response.Content).Data.ToList();
            if (productData.Any() && productData.Count > 0)
            {
                var product = productData.FirstOrDefault();
                result.EntityList.Add(product);
                result.Status = Enums.StatusType.Success;
            }
        }
        else
        {
            result.Status = Enums.StatusType.Failed;
            result.Message = $"Cant Add Product. \r\n \r\n {result.Response.Content} \r\n";
        }
        return result;
    }

    public IRestResponse AddProducts(List<ItemMasterData> items)
    {
        try
        {
            var query = _credentials.BackOfficeUri;
            var resource = "/dcs";
            var body = CreateProductsPayload(items);

            var response = HttpClientFactory.InitializeAsync(query, resource, Method.POST, body).Result;

            // message = "Error Message" + response.ErrorMessage + "\r\r \r\n " + body;
            return response;
        }
        catch (Exception ex)
        {
            //message = $"Response Error Message: {response.ErrorMessage} \r\n \r\n ex {ex.Message}";
        }

        return null;
    }

    private string CreateProductsPayload(List<ItemMasterData> itemGroupsList)
    {
        var dcsList = new List<Product>();

        foreach (var item in itemGroupsList)
        {
            dcsList.Add(new Product
            {
                //Originapplication = "RProPrismWeb",
                //Sbssid = _subsidiary.SID,
                //Active = 1,
                //Dname = item.ItemGroupName,
                //Regional = false,
                //Dcscode = item.ItemGroupCode,
                //Publishstatus = 2,
                //D = item.ItemGroupCode,
                //C = "",
                //S = "",
                //Dlongname = "",
                //Cname = "",
                //Clongname = "",
                //Sname = "",
                //Slongname = "",
                //Unknown Property : patternsid 
                //Useqtydecimals = 1,
                //Taxcodesid = "",
                //Marginpctg = 1,
                //Markuppctg = 1,
                //Coefficient = 1,
                //Margintype = 1,
                //Marginvalue = 1,
            });
        }

        OdataPrism<Product> root = new OdataPrism<Product>
        {
            Data = dcsList,
        };

        return JsonConvert.SerializeObject(root);
    }

    public async Task<string> CreateEntityPayload(ItemMasterData item)
    {
        // PrimaryItemDefinition variables
        var departmentResult = await _departmentService.GetByCodeAsync(item.ItemGroupCode);
        var department = departmentResult.EntityList.FirstOrDefault();

        var vendorResult = await _vendorsHandler.GetByCodeAsync(item.CardCode);
        var vendor = vendorResult.EntityList.FirstOrDefault();

        var subsidiarySid = _subsidiary.SID.ToString();
        var activePriceLevelSid = _subsidiary.ActivePriceLevelid;
        var activeSeasonSid = _subsidiary.ActiveSeasonSid;
        //var activestoreSid = _subsidiary.ActiveStoreSid;

        //var name = item.IsTaxable.Equals("Y") ? "TAXABLE" : "EXEMPT";
        //var taxCodeSid = _subsidiary.TaxCodes.Find(x => x.Taxname.ToUpper() == name.ToUpper()).Sid;
        //var taxCodeSid = prismHandler.GetTaxCodeSidByName(name).Sid;

        var name = item.IsTaxable.Equals("Y") ? "TAXABLE" : "EXEMPT";
        var taxCodeSid = await GetTaxCodeSid(name);


        string itemName50Char;
        if (item.ItemName.Length > 50)
        {
            var itemNameOrg = item.ItemName;
            itemName50Char = itemNameOrg.Substring(0, Math.Min(itemNameOrg.Length, 50));
        }
        else
            itemName50Char = item.ItemName;


        string itemForeignName50Char;
        if (item.ItemName.Length > 50)
        {
            var itemForeignNameOrg = item.ForeignName;
            itemForeignName50Char = itemForeignNameOrg.Substring(0, Math.Min(itemForeignNameOrg.Length, 50));
        }
        else
            itemForeignName50Char = item.ForeignName;


        var productsList = new OdataPrism<Product>()
        {
            Data = new List<Product>()
            {
                new()
                {
                    OriginApplication = "RProPrismWeb",
                    PrimaryItemDefinition = new PrimaryItemDefinition
                    {
                        Dcssid = department.Sid,
                        Vendsid = vendor.Sid,
                        Description1 = item.DesigGroupName,
                        Description2 = item.Size,
                        Attribute = item.ColorCode,
                        Itemsize = item.SalesUoM,
                        //Sid = null


                        UDF3 = itemName50Char,
                        UDF4 = itemForeignName50Char,
                        UDF5 = item.OrignGroupName,
                        UDF6 = item.Sticker,
                        UDF7 = item.StickerForeign,
                        UDF8 = item.TypeGroupName,
                        UDF9 = item.BarCode,
                        UDF10 = item.ItemsPerSaleUoM,
                        UDF11 = item.AveragePrice.ToString(),
                        UDF12 = item.SalesPrice,

                    },
                    InventoryItems = new[]
                    {
                        new InventoryItem
                        {
                            Sbssid = subsidiarySid,
                            Alu = item.ItemCode,
                            Active = item.Active,
                            Attribute = item.ColorCode,
                            ItemSize = item.SalesUoM,

                            Dcssid = department.Sid,
                            Vendsid = vendor.Sid, //
                            Description1 = item.DesigGroupName,
                            Description2 = item.Size, //
                            Description3 = item.ColorGroup, //
                            Description4 = item.ProductGroupName, //
                            Longdescription = item.ItemName + " - " + item.ForeignName, //
                            Lastrcvdcost = (decimal)item.AveragePrice,
                            //Price = item.PriceListCode,

                            Taxcodesid = taxCodeSid, //

                            Noninventory = item.InvntoryItem,
                            Dcscode = department.DcsCode, //

                            TEXT1 = item.ItemName,
                            TEXT2 = item.ForeignName,
                            TEXT3 = item.InventoryUoM,

                            //Spif = 0,

                            //Itemsize = 1,
                            Useqtydecimals = 2,
                            //QtyPerCase = item.ItemsPerSaleUoM,

                            Regional = false,
                            Activepricelevelsid = activePriceLevelSid,
                            Activeseasonsid = activeSeasonSid,
                            //Activestoresid = activestoreSid,


                            #region Not Mandatory

                            //Upc = null,
                            //Maxdiscperc1 = 100,
                            //Maxdiscperc2 = 100,
                            //Serialtype = 0,
                            //Lottype = 0,


                            //Actstrprice = 10,
                            //Actstrpricewt = 12,
                            //Actstrohqty = 2,

                            //Invnquantity = new Invnquantity[]
                            //{
                            //   new Invnquantity()
                            //   {
                            //       Qty = 2,
                            //       Invnsbsitemsid = 0,
                            //       Sbssid = subsidiaryId,
                            //       Storesid = storeSid,
                            //       Minqty = 0,
                            //       Maxqty = 0,
                            //   }
                            //},
                            //Invnprice = new Invnprice[] {
                            //    new Invnprice()
                            //    {
                            //        Price = 12,
                            //        Invnsbsitemsid = 0,
                            //        Sbssid = subsidiaryId,
                            //        Pricelvlsid = priceLevelSid,
                            //        Seasonsid = seasonSid,
                            //    }
                            //},

                            #endregion
                        },
                    },
                    //UpdateStyleDefinition = false,
                    //UpdateStyleCost = false,
                    //UpdateStylePrice = false,
                    //DefaultReasonSidForQtyMemo = "663852140000104707",
                    //DefaultReasonSidForCostMemo = "663852140000104707",
                    //DefaultReasonSidForPriceMemo = "663852140000104707",
                }
            }
        };
        var body = JsonConvert.SerializeObject(productsList);

        return body;
    }

    public async Task<string> CreateUpdatePayload(ItemMasterData item, long productSid)
    {
        // PrimaryItemDefinition variables
        var departmentResult = await _departmentService.GetByCodeAsync(item.ItemGroupCode);
        var department = departmentResult.EntityList.FirstOrDefault();

        var vendorResult = await _vendorsHandler.GetByCodeAsync(item.CardCode);
        var vendor = vendorResult.EntityList.FirstOrDefault();

        var subsidiarySid = _subsidiary.SID.ToString();
        var activePriceLevelSid = _subsidiary.ActivePriceLevelid;
        var activeSeasonSid = _subsidiary.ActiveSeasonSid;



        var name = item.IsTaxable.Equals("Y") ? "TAXABLE" : "EXEMPT";
        var taxCodeSid = await GetTaxCodeSid(name);

        if (taxCodeSid.IsNullOrEmpty())
            taxCodeSid = _subsidiary.ActiveTaxCode;

        //var activestoreSid = _subsidiary.ActiveStoreSid;


        string itemName50Char;
        if (item.ItemName.Length > 50)
        {
            var itemNameOrg = item.ItemName;
            itemName50Char = itemNameOrg.Substring(0, Math.Min(itemNameOrg.Length, 50));
        }
        else
            itemName50Char = item.ItemName;


        string itemForeignName50Char;
        if (item.ItemName.Length > 50)
        {
            var itemForeignNameOrg = item.ForeignName;
            itemForeignName50Char = itemForeignNameOrg.Substring(0, Math.Min(itemForeignNameOrg.Length, 50));
        }
        else
            itemForeignName50Char = item.ForeignName;

        var productsList = new OdataPrism<Product>()
        {
            Data = new List<Product>()
            {
                new()
                {
                    OriginApplication = "RProPrismWeb",
                    //Sid = productSid,
                    PrimaryItemDefinition = new PrimaryItemDefinition
                    {
                        Dcssid = department.Sid,
                        Vendsid = vendor.Sid,
                        Description1 = item.DesigGroupName,
                        Description2 = item.Size,
                        Attribute = item.ColorCode,
                        Itemsize = item.SalesUoM,
                        Sid = productSid.ToString(),


                        UDF3 = itemName50Char,
                        UDF4 = itemForeignName50Char,
                        UDF5 = item.OrignGroupName,
                        UDF6 = item.Sticker,
                        UDF7 = item.StickerForeign,
                        UDF8 = item.TypeGroupName,
                        UDF9 = item.BarCode,
                        UDF10 = item.ItemsPerSaleUoM,
                        UDF11 = item.AveragePrice.ToString(),
                        UDF12 = item.SalesPrice,

                    },
                    InventoryItems = new[]
                    {
                        new InventoryItem
                        {
                            Sbssid = subsidiarySid,
                            Alu = item.ItemCode,
                            Active = item.Active,
                            Attribute = item.ColorCode,
                            ItemSize = item.SalesUoM,

                            Dcssid = department.Sid,
                            Vendsid = vendor.Sid, //
                            Description1 = item.DesigGroupName,
                            Description2 = item.Size, //
                            Description3 = item.ColorGroup, //
                            Description4 = item.ProductGroupName, //
                            Longdescription = item.ItemName + " - " + item.ForeignName, //
                            Lastrcvdcost = (decimal)item.AveragePrice,
                            //Price = item.PriceListCode,
                            Sid = productSid.ToString(),

                            Taxcodesid = taxCodeSid, //

                            Noninventory = item.InvntoryItem,
                            Dcscode = department.DcsCode, //
                            TEXT1 = item.ItemName,
                            TEXT2 = item.ForeignName,
                            TEXT3 = item.InventoryUoM,

                            //Spif = 0,

                            //Itemsize = 1,
                            Useqtydecimals = 2,
                            //QtyPerCase = item.ItemsPerSaleUoM,

                            Regional = false,
                            Activepricelevelsid = activePriceLevelSid,
                            Activeseasonsid = activeSeasonSid,
                            //Activestoresid = storeSid,


                            #region Not Mandatory

                            //Upc = null,
                            //Maxdiscperc1 = 100,
                            //Maxdiscperc2 = 100,
                            //Serialtype = 0,
                            //Lottype = 0,


                            //Actstrprice = 10,
                            //Actstrpricewt = 12,
                            //Actstrohqty = 2,

                            //Invnquantity = new Invnquantity[]
                            //{
                            //   new Invnquantity()
                            //   {
                            //       Qty = 2,
                            //       Invnsbsitemsid = 0,
                            //       Sbssid = subsidiaryId,
                            //       Storesid = storeSid,
                            //       Minqty = 0,
                            //       Maxqty = 0,
                            //   }
                            //},
                            //Invnprice = new Invnprice[] {
                            //    new Invnprice()
                            //    {
                            //        Price = 12,
                            //        Invnsbsitemsid = 0,
                            //        Sbssid = subsidiaryId,
                            //        Pricelvlsid = priceLevelSid,
                            //        Seasonsid = seasonSid,
                            //    }
                            //},

                            #endregion
                        },
                    },
                    //UpdateStyleDefinition = false,
                    //UpdateStyleCost = false,
                    //UpdateStylePrice = false,
                    //DefaultReasonSidForQtyMemo = "663852140000104707",
                    //DefaultReasonSidForCostMemo = "663852140000104707",
                    //DefaultReasonSidForPriceMemo = "663852140000104707",
                }
            }
        };
        var body = JsonConvert.SerializeObject(productsList);

        return body;
    }

    private async Task<string> GetTaxCodeSid(string name)
    {
        var taxCodesProduction = new List<TaxCodes>()
        {
            new("664651377000183746", 664651285000113257, 0, "Taxable", true),
            new("664651377000185747", 664651285000113257, 2, "Exempt", false),
            new("664651377000185748", 664651285000113257, 3, "Luxury", false),
        };

        var taxCodesTest = new List<TaxCodes>()
        {
            new("663852140000157746", 674650600000126277, 1, "TAXABLE", true),
            new("663852140000157747", 674650600000126277, 2, "EXEMPT", false),
            new("663852140000158748", 674650600000126277, 3, "LUXURY", false),
        };

        var taxCodesLocal = new List<TaxCodes>()
        {
            new("665151925000173747", 665151872000149257, 0, "Taxable", true),
            new("665151925000174748", 665151872000149257, 1, "Exempt", false),
            new("665151925000176749", 665151872000149257, 2, "Luxury", false),
        };

        var taxCodeSid = "";
        if (_subsidiary.Name.Contains("Production"))
        {
            taxCodeSid = taxCodesProduction.Find(x => x.TaxName.ToUpper() == name.ToUpper()).Sid;
        }

        if (_subsidiary.Name.Contains("Test"))
        {
            taxCodeSid = taxCodesTest.Find(x => x.TaxName.ToUpper() == name.ToUpper()).Sid;
        }

        if (_subsidiary.Name.Contains("Local"))
        {
            taxCodeSid = taxCodesLocal.Find(x => x.TaxName.ToUpper() == name.ToUpper()).Sid;
        }

        return taxCodeSid;
    }
}