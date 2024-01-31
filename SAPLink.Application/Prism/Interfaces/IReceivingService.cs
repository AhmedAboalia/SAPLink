using SAPLink.Domain.Models.Prism.Merchandise.Inventory;
using SAPLink.Domain.Models.Prism.Receiving;
using SAPLink.Domain.SAP.Documents;

namespace SAPLink.Application.Prism.Interfaces;

public interface IReceivingService
{
    Task<ReceivingResponseDto> GenerateVoucherSid(string storeSid);
    Task<IRestResponse> AddConsolidateItem(string body, string receivingSid);
    string CreateAddConsolidateItemPayload(ProductResponseModel product, string sid, Line line);

    Task<IRestResponse> AddReceiving(ReceivingResponseDto receiving, string rowVersion, string trackingNo, string note, string storeCode);
}