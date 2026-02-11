using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BAMK.API.Configuration;
using BAMK.API.DTOs.Sap;
using BAMK.Application.DTOs.Order;
using BAMK.Core.Common;
using Microsoft.Extensions.Options;

namespace BAMK.API.Services
{
    public class SapIntegrationService : ISapIntegrationService
    {
        private readonly HttpClient _httpClient;
        private readonly SapIntegrationSettings _settings;
        private readonly ILogger<SapIntegrationService> _logger;

        public SapIntegrationService(
            HttpClient httpClient,
            IOptions<SapIntegrationSettings> settings,
            ILogger<SapIntegrationService> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<Result<SapOrderSyncResultDto>> SyncOrderAsync(OrderDto orderDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_settings.BaseUrl))
                {
                    return Result<SapOrderSyncResultDto>.Failure(
                        Error.Create(ErrorCode.InvalidOperation, "SAP BaseUrl ayarı eksik"));
                }

                var payload = new SapOrderSyncRequestDto
                {
                    OrderId = orderDto.Id,
                    UserId = orderDto.UserId,
                    UserEmail = orderDto.UserEmail,
                    TotalAmount = orderDto.TotalAmount,
                    PaymentStatus = orderDto.PaymentStatus,
                    OrderStatus = orderDto.Status,
                    CreatedAt = orderDto.CreatedAt,
                    Items = orderDto.OrderItems.Select(item => new SapOrderSyncItemDto
                    {
                        ProductId = item.TShirtId,
                        ProductName = item.TShirtName,
                        Color = item.TShirtColor,
                        Size = item.TShirtSize,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    }).ToList()
                };

                var requestUri = _settings.OrderSyncEndpoint;
                var requestJson = JsonSerializer.Serialize(payload);

                using var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
                {
                    Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrWhiteSpace(_settings.ApiKey))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
                }

                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "SAP order sync başarısız. OrderId: {OrderId}, StatusCode: {StatusCode}, Response: {Response}",
                        orderDto.Id,
                        (int)response.StatusCode,
                        responseBody);

                    return Result<SapOrderSyncResultDto>.Failure(
                        Error.Create(ErrorCode.InvalidOperation, "SAP servisine sipariş gönderilemedi"));
                }

                var result = new SapOrderSyncResultDto
                {
                    IsSuccess = true,
                    StatusCode = (int)response.StatusCode,
                    Message = "Sipariş SAP sistemine başarıyla iletildi",
                    RawResponse = responseBody
                };

                return Result<SapOrderSyncResultDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SAP order sync sırasında hata oluştu. OrderId: {OrderId}", orderDto.Id);
                return Result<SapOrderSyncResultDto>.Failure(
                    Error.Create(ErrorCode.InternalServerError, "SAP entegrasyonu sırasında hata oluştu"));
            }
        }
    }
}
