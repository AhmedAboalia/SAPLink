using SAPLink.Core.Models;
using SAPLink.Core.Models.Prism.Inventory.Products;
using SAPLink.Core.Models.Prism.Receiving;
using SAPLink.Core.Models.SAP.Documents;

namespace SAPLink.Handler.Prism.Interfaces;

public interface IReceivingService
{
    Task<ReceivingResponseDto> GenerateVoucherSid(string storeSid);
    Task<Responses> AddConsolidateItem(string body, string receivingSid);
    string CreateAddConsolidateItemPayload(ProductResponseModel product, string sid, Line line);

    Task<Responses> AddReceiving(ReceivingResponseDto receiving, string rowVersion, string trackingNo, string note, string storeCode);
}