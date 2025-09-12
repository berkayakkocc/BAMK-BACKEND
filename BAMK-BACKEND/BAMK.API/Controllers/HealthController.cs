using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BAMK.API;
using BAMK.Application.Services;
using BAMK.Infrastructure.Data;

namespace BAMK.API.Controllers
{
    public class HealthController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ITShirtService _tShirtService;
        private readonly IProductDetailService _productDetailService;
        private readonly IQuestionService _questionService;
        private readonly IOrderService _orderService;
        private readonly BAMK.Application.Services.ICartService _cartService;
        private readonly BAMKDbContext _context;

        public HealthController(
            ILogger<HealthController> logger,
            IUserService userService,
            ITShirtService tShirtService,
            IProductDetailService productDetailService,
            IQuestionService questionService,
            IOrderService orderService,
            BAMK.Application.Services.ICartService cartService,
            BAMKDbContext context) : base(logger)
        {
            _userService = userService;
            _tShirtService = tShirtService;
            _productDetailService = productDetailService;
            _questionService = questionService;
            _orderService = orderService;
            _cartService = cartService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var healthData = new
            {
                status = "Healthy",
                timestamp = DateTime.UtcNow,
                version = "1.0.0",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                uptime = DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime(),
                memory = GC.GetTotalMemory(false),
                gcCollections = new
                {
                    gen0 = GC.CollectionCount(0),
                    gen1 = GC.CollectionCount(1),
                    gen2 = GC.CollectionCount(2)
                }
            };
            
            return SuccessResponse(healthData, "BAMK API is running!");
        }

        [HttpGet("detailed")]
        public IActionResult GetDetailed()
        {
            try
            {
                var healthData = new
                {
                    status = "Healthy",
                    timestamp = DateTime.UtcNow,
                    version = "1.0.0",
                    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                    uptime = DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime(),
                    memory = new
                    {
                        total = GC.GetTotalMemory(false),
                        workingSet = Environment.WorkingSet,
                        privateMemory = Environment.WorkingSet
                    },
                    gcCollections = new
                    {
                        gen0 = GC.CollectionCount(0),
                        gen1 = GC.CollectionCount(1),
                        gen2 = GC.CollectionCount(2)
                    },
                    threads = System.Diagnostics.Process.GetCurrentProcess().Threads.Count,
                    handles = System.Diagnostics.Process.GetCurrentProcess().HandleCount
                };
                
                return SuccessResponse(healthData, "Detailed health check completed!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Detailed health check failed");
                return ErrorResponse("Health check failed", 500);
            }
        }

        [HttpPost("test-seeder")]
        public async Task<IActionResult> TestSeeder()
        {
            try
            {
                var seeder = new TestDataSeeder(
                    _userService,
                    _tShirtService,
                    _productDetailService,
                    _questionService,
                    _orderService,
                    _cartService,
                    _context);

                await seeder.SeedAllTestDataAsync();

                return SuccessResponse<object>(null, "Test data seeder çalıştırıldı!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Test seeder failed");
                return ErrorResponse($"Test seeder hatası: {ex.Message}", 500);
            }
        }

        [HttpGet("check-data")]
        public async Task<IActionResult> CheckData()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                var tShirts = await _tShirtService.GetAllAsync();
                var productDetails = await _productDetailService.GetAllAsync();
                var questions = await _questionService.GetAllAsync();
                var orders = await _orderService.GetAllAsync();

                var dataCount = new
                {
                    users = users.IsSuccess ? users.Value?.Count() ?? 0 : 0,
                    tShirts = tShirts.IsSuccess ? tShirts.Value?.Count() ?? 0 : 0,
                    productDetails = productDetails.IsSuccess ? productDetails.Value?.Count() ?? 0 : 0,
                    questions = questions.IsSuccess ? questions.Value?.Count() ?? 0 : 0,
                    orders = orders.IsSuccess ? orders.Value?.Count() ?? 0 : 0
                };

                return SuccessResponse(dataCount, "Veri durumu kontrol edildi!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Data check failed");
                return ErrorResponse($"Veri kontrolü hatası: {ex.Message}", 500);
            }
        }
    }
}
