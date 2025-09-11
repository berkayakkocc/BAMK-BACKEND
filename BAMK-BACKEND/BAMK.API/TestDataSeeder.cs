using BAMK.Application.DTOs.User;
using BAMK.Application.DTOs.TShirt;
using BAMK.Application.DTOs.ProductDetail;
using BAMK.Application.DTOs.Question;
using BAMK.Application.DTOs.Order;
using BAMK.Application.Services;
using BAMK.Core.Common;

namespace BAMK.API
{
    public class TestDataSeeder
    {
        private readonly IUserService _userService;
        private readonly ITShirtService _tShirtService;
        private readonly IProductDetailService _productDetailService;
        private readonly IQuestionService _questionService;
        private readonly IOrderService _orderService;

        public TestDataSeeder(
            IUserService userService,
            ITShirtService tShirtService,
            IProductDetailService productDetailService,
            IQuestionService questionService,
            IOrderService orderService)
        {
            _userService = userService;
            _tShirtService = tShirtService;
            _productDetailService = productDetailService;
            _questionService = questionService;
            _orderService = orderService;
        }

        public async Task SeedAllTestDataAsync()
        {
            Console.WriteLine("🌱 Test verileri ekleniyor...");
            
            await SeedTestUsersAsync();
            await SeedTestTShirtsAsync();
            await SeedTestProductDetailsAsync();
            await SeedTestQuestionsAsync();
            await SeedTestOrdersAsync();
            
            Console.WriteLine("✅ Tüm test verileri başarıyla eklendi!");
        }

        public async Task SeedTestUsersAsync()
        {
            try
            {
                // Admin kullanıcısı
                var adminUser = new CreateUserDto
                {
                    Email = "admin@bamk.com",
                    Password = "admin123",
                    FirstName = "Admin",
                    LastName = "User",
                    PhoneNumber = "+905551111111"
                };

                var existingAdmin = await _userService.GetByEmailAsync("admin@bamk.com");
                if (!existingAdmin.IsSuccess)
                {
                    await _userService.CreateAsync(adminUser);
                    Console.WriteLine("✅ Admin kullanıcısı oluşturuldu");
                }

                // Test kullanıcısı
                var testUser = new CreateUserDto
                {
                    Email = "test@bamk.com",
                    Password = "123456",
                    FirstName = "Test",
                    LastName = "User",
                    PhoneNumber = "+905551234567"
                };

                var existingTest = await _userService.GetByEmailAsync("test@bamk.com");
                if (!existingTest.IsSuccess)
                {
                    await _userService.CreateAsync(testUser);
                    Console.WriteLine("✅ Test kullanıcısı oluşturuldu");
                }

                // Müşteri kullanıcısı
                var customerUser = new CreateUserDto
                {
                    Email = "customer@bamk.com",
                    Password = "customer123",
                    FirstName = "Customer",
                    LastName = "User",
                    PhoneNumber = "+905552222222"
                };

                var existingCustomer = await _userService.GetByEmailAsync("customer@bamk.com");
                if (!existingCustomer.IsSuccess)
                {
                    await _userService.CreateAsync(customerUser);
                    Console.WriteLine("✅ Müşteri kullanıcısı oluşturuldu");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Kullanıcı oluşturma hatası: {ex.Message}");
            }
        }

        public async Task SeedTestTShirtsAsync()
        {
            try
            {
                var tShirts = new List<CreateTShirtDto>
                {
                    new CreateTShirtDto
                    {
                        Name = "BAMK Classic Black",
                        Description = "Klasik siyah BAMK t-shirt",
                        Price = 99.99m,
                        Color = "Siyah",
                        Size = "M",
                        StockQuantity = 50,
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "BAMK Classic White",
                        Description = "Klasik beyaz BAMK t-shirt",
                        Price = 99.99m,
                        Color = "Beyaz",
                        Size = "M",
                        StockQuantity = 45,
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "BAMK Classic Red",
                        Description = "Klasik kırmızı BAMK t-shirt",
                        Price = 99.99m,
                        Color = "Kırmızı",
                        Size = "L",
                        StockQuantity = 30,
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "BAMK Classic Blue",
                        Description = "Klasik mavi BAMK t-shirt",
                        Price = 99.99m,
                        Color = "Mavi",
                        Size = "XL",
                        StockQuantity = 25,
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "BAMK Premium Black",
                        Description = "Premium siyah BAMK t-shirt",
                        Price = 149.99m,
                        Color = "Siyah",
                        Size = "M",
                        StockQuantity = 20,
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "BAMK Premium White",
                        Description = "Premium beyaz BAMK t-shirt",
                        Price = 149.99m,
                        Color = "Beyaz",
                        Size = "L",
                        StockQuantity = 15,
                        IsActive = true
                    }
                };

                foreach (var tShirt in tShirts)
                {
                    var result = await _tShirtService.CreateAsync(tShirt);
                    if (result.IsSuccess)
                    {
                        Console.WriteLine($"✅ TShirt oluşturuldu: {tShirt.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ TShirt oluşturma hatası: {ex.Message}");
            }
        }

        public async Task SeedTestProductDetailsAsync()
        {
            try
            {
                // TShirt'leri al
                var tShirts = await _tShirtService.GetAllAsync();
                if (!tShirts.IsSuccess) return;

                var tShirtList = tShirts.Value.ToList();
                if (tShirtList.Count == 0) return;

                var productDetails = new List<CreateProductDetailDto>
                {
                    new CreateProductDetailDto
                    {
                        TShirtId = tShirtList[0].Id, // BAMK Classic Black
                        Material = "100% Pamuk",
                        CareInstructions = "30°C'de yıkayın, ütülemeyin, kuru temizleme yapmayın",
                        Brand = "BAMK",
                        Origin = "Türkiye",
                        Weight = "200g",
                        Dimensions = "M: 50x70cm, L: 52x72cm, XL: 54x74cm",
                        Features = "Nefes alabilir, yumuşak, dayanıklı, çevre dostu",
                        AdditionalInfo = "Ekolojik üretim, organik pamuk kullanımı, adil ticaret",
                        IsActive = true
                    },
                    new CreateProductDetailDto
                    {
                        TShirtId = tShirtList[1].Id, // BAMK Classic White
                        Material = "100% Pamuk",
                        CareInstructions = "30°C'de yıkayın, ütülemeyin, kuru temizleme yapmayın",
                        Brand = "BAMK",
                        Origin = "Türkiye",
                        Weight = "200g",
                        Dimensions = "M: 50x70cm, L: 52x72cm, XL: 54x74cm",
                        Features = "Nefes alabilir, yumuşak, dayanıklı, çevre dostu",
                        AdditionalInfo = "Ekolojik üretim, organik pamuk kullanımı, adil ticaret",
                        IsActive = true
                    },
                    new CreateProductDetailDto
                    {
                        TShirtId = tShirtList[2].Id, // BAMK Classic Red
                        Material = "100% Pamuk",
                        CareInstructions = "30°C'de yıkayın, ütülemeyin, kuru temizleme yapmayın",
                        Brand = "BAMK",
                        Origin = "Türkiye",
                        Weight = "200g",
                        Dimensions = "M: 50x70cm, L: 52x72cm, XL: 54x74cm",
                        Features = "Nefes alabilir, yumuşak, dayanıklı, çevre dostu",
                        AdditionalInfo = "Ekolojik üretim, organik pamuk kullanımı, adil ticaret",
                        IsActive = true
                    },
                    new CreateProductDetailDto
                    {
                        TShirtId = tShirtList[3].Id, // BAMK Classic Blue
                        Material = "100% Pamuk",
                        CareInstructions = "30°C'de yıkayın, ütülemeyin, kuru temizleme yapmayın",
                        Brand = "BAMK",
                        Origin = "Türkiye",
                        Weight = "200g",
                        Dimensions = "M: 50x70cm, L: 52x72cm, XL: 54x74cm",
                        Features = "Nefes alabilir, yumuşak, dayanıklı, çevre dostu",
                        AdditionalInfo = "Ekolojik üretim, organik pamuk kullanımı, adil ticaret",
                        IsActive = true
                    },
                    new CreateProductDetailDto
                    {
                        TShirtId = tShirtList[4].Id, // BAMK Premium Black
                        Material = "100% Organik Pamuk",
                        CareInstructions = "30°C'de yıkayın, ütülemeyin, kuru temizleme yapmayın, doğal deterjan kullanın",
                        Brand = "BAMK Premium",
                        Origin = "Türkiye",
                        Weight = "220g",
                        Dimensions = "M: 50x70cm, L: 52x72cm, XL: 54x74cm",
                        Features = "Premium kalite, nefes alabilir, yumuşak, dayanıklı, çevre dostu, anti-bakteriyel",
                        AdditionalInfo = "Premium ekolojik üretim, %100 organik pamuk, adil ticaret, GOTS sertifikalı",
                        IsActive = true
                    },
                    new CreateProductDetailDto
                    {
                        TShirtId = tShirtList[5].Id, // BAMK Premium White
                        Material = "100% Organik Pamuk",
                        CareInstructions = "30°C'de yıkayın, ütülemeyin, kuru temizleme yapmayın, doğal deterjan kullanın",
                        Brand = "BAMK Premium",
                        Origin = "Türkiye",
                        Weight = "220g",
                        Dimensions = "M: 50x70cm, L: 52x72cm, XL: 54x74cm",
                        Features = "Premium kalite, nefes alabilir, yumuşak, dayanıklı, çevre dostu, anti-bakteriyel",
                        AdditionalInfo = "Premium ekolojik üretim, %100 organik pamuk, adil ticaret, GOTS sertifikalı",
                        IsActive = true
                    }
                };

                foreach (var productDetail in productDetails)
                {
                    var result = await _productDetailService.CreateAsync(productDetail);
                    if (result.IsSuccess)
                    {
                        Console.WriteLine($"✅ ProductDetail oluşturuldu: TShirt ID {productDetail.TShirtId}");
                    }
                    else
                    {
                        Console.WriteLine($"❌ ProductDetail oluşturulamadı: TShirt ID {productDetail.TShirtId} - {result.Error?.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ProductDetail oluşturma hatası: {ex.Message}");
            }
        }

        public async Task SeedTestQuestionsAsync()
        {
            try
            {
                // Test kullanıcısını al
                var testUser = await _userService.GetByEmailAsync("test@bamk.com");
                if (!testUser.IsSuccess) return;

                var questions = new List<CreateQuestionDto>
                {
                    new CreateQuestionDto
                    {
                        QuestionTitle = "T-Shirt bedenleri hakkında",
                        QuestionContent = "T-shirt bedenleri nasıl? Küçük gelir mi?",
                        UserId = testUser.Value.Id
                    },
                    new CreateQuestionDto
                    {
                        QuestionTitle = "Kargo süresi",
                        QuestionContent = "Kargo ne kadar sürer? Hangi şehirlere gönderiyorsunuz?",
                        UserId = testUser.Value.Id
                    },
                    new CreateQuestionDto
                    {
                        QuestionTitle = "Ödeme yöntemleri",
                        QuestionContent = "Hangi ödeme yöntemlerini kabul ediyorsunuz?",
                        UserId = testUser.Value.Id
                    },
                    new CreateQuestionDto
                    {
                        QuestionTitle = "İade politikası",
                        QuestionContent = "Ürünleri iade edebilir miyim? Koşulları neler?",
                        UserId = testUser.Value.Id
                    }
                };

                foreach (var question in questions)
                {
                    var result = await _questionService.CreateAsync(question);
                    if (result.IsSuccess)
                    {
                        Console.WriteLine($"✅ Soru oluşturuldu: {question.QuestionTitle}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Soru oluşturma hatası: {ex.Message}");
            }
        }

        public async Task SeedTestOrdersAsync()
        {
            try
            {
                // Test kullanıcısını al
                var testUser = await _userService.GetByEmailAsync("test@bamk.com");
                if (!testUser.IsSuccess) return;

                // TShirt'leri al
                var tShirts = await _tShirtService.GetAllAsync();
                if (!tShirts.IsSuccess) return;

                var tShirtList = tShirts.Value.ToList();
                if (tShirtList.Count < 2) return;

                var orders = new List<CreateOrderDto>
                {
                    new CreateOrderDto
                    {
                        UserId = testUser.Value.Id,
                        OrderItems = new List<CreateOrderItemDto>
                        {
                            new CreateOrderItemDto
                            {
                                TShirtId = tShirtList[0].Id,
                                Quantity = 2
                            },
                            new CreateOrderItemDto
                            {
                                TShirtId = tShirtList[1].Id,
                                Quantity = 1
                            }
                        }
                    },
                    new CreateOrderDto
                    {
                        UserId = testUser.Value.Id,
                        OrderItems = new List<CreateOrderItemDto>
                        {
                            new CreateOrderItemDto
                            {
                                TShirtId = tShirtList[2].Id,
                                Quantity = 1
                            }
                        }
                    }
                };

                foreach (var order in orders)
                {
                    var result = await _orderService.CreateAsync(order);
                    if (result.IsSuccess)
                    {
                        Console.WriteLine($"✅ Sipariş oluşturuldu: {result.Value.Id}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Sipariş oluşturma hatası: {ex.Message}");
            }
        }

        public async Task SeedTestUserAsync()
        {
            await SeedTestUsersAsync();
        }
    }
}
