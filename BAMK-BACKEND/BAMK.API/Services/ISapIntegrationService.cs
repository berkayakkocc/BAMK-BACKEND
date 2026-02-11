using BAMK.API.DTOs.Sap;
using BAMK.Application.DTOs.Order;
using BAMK.Core.Common;

namespace BAMK.API.Services
{
    public interface ISapIntegrationService
    {
        Task<Result<SapOrderSyncResultDto>> SyncOrderAsync(OrderDto orderDto);
    }
}
