using BAMK.Application.DTOs.User;
using BAMK.Application.DTOs.TShirt;
using BAMK.Application.DTOs.ProductDetail;
using BAMK.Application.DTOs.Question;
using BAMK.Application.DTOs.Order;
using BAMK.Application.DTOs.Cart;
using BAMK.Application.Services;
using BAMK.Core.Common;
using BAMK.Infrastructure.Data;

namespace BAMK.API
{
    public class TestDataSeeder
    {
        private readonly IUserService _userService;
        private readonly ITShirtService _tShirtService;
        private readonly IProductDetailService _productDetailService;
        private readonly IQuestionService _questionService;
        private readonly IOrderService _orderService;
        private readonly BAMK.Application.Services.ICartService _cartService;
        private readonly BAMKDbContext _context;

        public TestDataSeeder(
            IUserService userService,
            ITShirtService tShirtService,
            IProductDetailService productDetailService,
            IQuestionService questionService,
            IOrderService orderService,
            BAMK.Application.Services.ICartService cartService,
            BAMKDbContext context)
        {
            _userService = userService;
            _tShirtService = tShirtService;
            _productDetailService = productDetailService;
            _questionService = questionService;
            _orderService = orderService;
            _cartService = cartService;
            _context = context;
        }

        public async Task SeedAllTestDataAsync()
        {
            Console.WriteLine("🌱 Test Data Seeder başlatılıyor...");
            Console.WriteLine(new string('=', 60));
            
            var startTime = DateTime.UtcNow;
            var successCount = 0;
            var totalSteps = 7;
            
            try
            {
                // 1. Kullanıcılar
                Console.WriteLine("\n📋 Adım 1/7: Kullanıcılar oluşturuluyor...");
                if (await SeedTestUsersAsync())
                {
                    await _context.SaveChangesAsync();
                    Console.WriteLine("💾 Kullanıcı değişiklikleri kaydedildi");
                    successCount++;
                    Console.WriteLine("✅ Kullanıcılar başarıyla oluşturuldu");
                }
                else
                {
                    Console.WriteLine("❌ Kullanıcılar oluşturulamadı");
                }
                
                // 2. TShirt'ler
                Console.WriteLine("\n📋 Adım 2/7: TShirt'ler oluşturuluyor...");
                if (await SeedTestTShirtsAsync())
                {
                    await _context.SaveChangesAsync();
                    Console.WriteLine("💾 TShirt değişiklikleri kaydedildi");
                    successCount++;
                    Console.WriteLine("✅ TShirt'ler başarıyla oluşturuldu");
                }
                else
                {
                    Console.WriteLine("❌ TShirt'ler oluşturulamadı");
                }
                
                // 3. ProductDetails
                Console.WriteLine("\n📋 Adım 3/7: ProductDetail'ler oluşturuluyor...");
                if (await SeedTestProductDetailsAsync())
                {
                    await _context.SaveChangesAsync();
                    Console.WriteLine("💾 ProductDetail değişiklikleri kaydedildi");
                    successCount++;
                    Console.WriteLine("✅ ProductDetail'ler başarıyla oluşturuldu");
                }
                else
                {
                    Console.WriteLine("❌ ProductDetail'ler oluşturulamadı");
                }
                
                // 4. Sorular
                Console.WriteLine("\n📋 Adım 4/7: Sorular oluşturuluyor...");
                if (await SeedTestQuestionsAsync())
                {
                    await _context.SaveChangesAsync();
                    Console.WriteLine("💾 Soru değişiklikleri kaydedildi");
                    successCount++;
                    Console.WriteLine("✅ Sorular başarıyla oluşturuldu");
                }
                else
                {
                    Console.WriteLine("❌ Sorular oluşturulamadı");
                }
                
                // 5. Siparişler
                Console.WriteLine("\n📋 Adım 5/7: Siparişler oluşturuluyor...");
                if (await SeedTestOrdersAsync())
                {
                    await _context.SaveChangesAsync();
                    Console.WriteLine("💾 Sipariş değişiklikleri kaydedildi");
                    successCount++;
                    Console.WriteLine("✅ Siparişler başarıyla oluşturuldu");
                }
                else
                {
                    Console.WriteLine("❌ Siparişler oluşturulamadı");
                }
                
                // 6. Cevaplar
                Console.WriteLine("\n📋 Adım 6/7: Cevaplar oluşturuluyor...");
                if (await SeedTestAnswersAsync())
                {
                    await _context.SaveChangesAsync();
                    Console.WriteLine("💾 Cevap değişiklikleri kaydedildi");
                    successCount++;
                    Console.WriteLine("✅ Cevaplar başarıyla oluşturuldu");
                }
                else
                {
                    Console.WriteLine("❌ Cevaplar oluşturulamadı");
                }
                
                // 7. Sepetler
                Console.WriteLine("\n📋 Adım 7/7: Sepetler oluşturuluyor...");
                if (await SeedTestCartsAsync())
                {
                    await _context.SaveChangesAsync();
                    Console.WriteLine("💾 Sepet değişiklikleri kaydedildi");
                    successCount++;
                    Console.WriteLine("✅ Sepetler başarıyla oluşturuldu");
                }
                else
                {
                    Console.WriteLine("❌ Sepetler oluşturulamadı");
                }
                
                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;
                
                Console.WriteLine("\n" + new string('=', 60));
                Console.WriteLine($"🎉 Test Data Seeder tamamlandı!");
                Console.WriteLine($"📊 İstatistikler:");
                Console.WriteLine($"   • Başarılı adımlar: {successCount}/{totalSteps}");
                Console.WriteLine($"   • Toplam süre: {duration.TotalSeconds:F2} saniye");
                Console.WriteLine($"   • Başlangıç: {startTime:HH:mm:ss}");
                Console.WriteLine($"   • Bitiş: {endTime:HH:mm:ss}");
                Console.WriteLine(new string('=', 60));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Test Data Seeder hatası: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        private async Task<bool> SeedTestUsersAsync()
        {
            try
            {
                Console.WriteLine("👥 Kullanıcı verileri kontrol ediliyor...");
                
                var users = new List<CreateUserDto>
                {
                    new CreateUserDto
                    {
                        Email = "admin@bamk.com",
                        Password = "admin123",
                        FirstName = "Admin",
                        LastName = "User",
                        PhoneNumber = "+905551111111"
                    },
                    new CreateUserDto
                    {
                        Email = "test@bamk.com",
                        Password = "123456",
                        FirstName = "Test",
                        LastName = "User",
                        PhoneNumber = "+905551234567"
                    },
                    new CreateUserDto
                    {
                        Email = "customer@bamk.com",
                        Password = "customer123",
                        FirstName = "Customer",
                        LastName = "User",
                        PhoneNumber = "+905552222222"
                    },
                    new CreateUserDto
                    {
                        Email = "ahmet@example.com",
                        Password = "password123",
                        FirstName = "Ahmet",
                        LastName = "Yılmaz",
                        PhoneNumber = "+905553333333"
                    },
                    new CreateUserDto
                    {
                        Email = "ayse@example.com",
                        Password = "password123",
                        FirstName = "Ayşe",
                        LastName = "Demir",
                        PhoneNumber = "+905554444444"
                    }
                };

                var createdCount = 0;
                foreach (var user in users)
                {
                    try
                    {
                        var existingUser = await _userService.GetByEmailAsync(user.Email);
                        if (!existingUser.IsSuccess)
                        {
                            var result = await _userService.CreateAsync(user);
                            if (result.IsSuccess)
                            {
                                Console.WriteLine($"✅ Kullanıcı oluşturuldu: {user.FirstName} {user.LastName}");
                                createdCount++;
                            }
                            else
                            {
                                Console.WriteLine($"❌ Kullanıcı oluşturulamadı: {user.FirstName} {user.LastName} - {result.Error?.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"ℹ️ Kullanıcı zaten mevcut: {user.FirstName} {user.LastName}");
                            createdCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Kullanıcı oluşturma hatası: {user.FirstName} {user.LastName} - {ex.Message}");
                    }
                }

                Console.WriteLine($"📊 Kullanıcı işlemi tamamlandı: {createdCount}/{users.Count}");
                return createdCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Kullanıcı oluşturma hatası: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> SeedTestTShirtsAsync()
        {
            try
            {
                Console.WriteLine("👕 TShirt verileri kontrol ediliyor...");
                
                // Mevcut TShirt'leri kontrol et
                var existingTShirts = await _tShirtService.GetAllAsync();
                if (existingTShirts.IsSuccess && existingTShirts.Value != null && existingTShirts.Value.Any())
                {
                    var existingCount = existingTShirts.Value.Count();
                    Console.WriteLine($"ℹ️ {existingCount} TShirt zaten mevcut");
                    
                    // Eğer 15 veya daha fazla TShirt varsa, yeni ekleme
                    if (existingCount >= 15)
                    {
                        Console.WriteLine("✅ Yeterli TShirt mevcut, yeni ekleme yapılmıyor");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ Sadece {existingCount} TShirt var, {15 - existingCount} tane daha ekleniyor");
                    }
                }
                
                var tShirts = new List<CreateTShirtDto>
                {
                    new CreateTShirtDto
                    {
                        Name = "Klasik Beyaz T-Shirt",
                        Description = "Rahat ve şık klasik beyaz t-shirt",
                        Price = 99.99m,
                        Color = "Beyaz",
                        Size = "M",
                        StockQuantity = 50,
                        ImageUrl = "https://example.com/white-tshirt.jpg",
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "Siyah Basic T-Shirt",
                        Description = "Her gardıropta olması gereken siyah basic t-shirt",
                        Price = 89.99m,
                        Color = "Siyah",
                        Size = "L",
                        StockQuantity = 30,
                        ImageUrl = "https://example.com/black-tshirt.jpg",
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "Mavi Denim T-Shirt",
                        Description = "Denim kumaştan yapılmış dayanıklı t-shirt",
                        Price = 129.99m,
                        Color = "Mavi",
                        Size = "XL",
                        StockQuantity = 25,
                        ImageUrl = "https://example.com/blue-tshirt.jpg",
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "Kırmızı Spor T-Shirt",
                        Description = "Spor aktiviteleri için ideal nefes alabilir t-shirt",
                        Price = 79.99m,
                        Color = "Kırmızı",
                        Size = "S",
                        StockQuantity = 40,
                        ImageUrl = "https://example.com/red-tshirt.jpg",
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "Yeşil Organik T-Shirt",
                        Description = "Organik pamuktan üretilmiş çevre dostu t-shirt",
                        Price = 149.99m,
                        Color = "Yeşil",
                        Size = "M",
                        StockQuantity = 20,
                        ImageUrl = "https://example.com/green-tshirt.jpg",
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "Turuncu Yaz T-Shirt",
                        Description = "Yaz ayları için parlak turuncu t-shirt",
                        Price = 69.99m,
                        Color = "Turuncu",
                        Size = "L",
                        StockQuantity = 35,
                        ImageUrl = "https://example.com/orange-tshirt.jpg",
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "Mor Premium T-Shirt",
                        Description = "Premium kalite mor t-shirt",
                        Price = 159.99m,
                        Color = "Mor",
                        Size = "XL",
                        StockQuantity = 15,
                        ImageUrl = "https://example.com/purple-tshirt.jpg",
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "Gri Casual T-Shirt",
                        Description = "Günlük kullanım için ideal gri t-shirt",
                        Price = 59.99m,
                        Color = "Gri",
                        Size = "M",
                        StockQuantity = 60,
                        ImageUrl = "https://example.com/gray-tshirt.jpg",
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "Pembe Şık T-Shirt",
                        Description = "Şık ve zarif pembe t-shirt",
                        Price = 89.99m,
                        Color = "Pembe",
                        Size = "S",
                        StockQuantity = 25,
                        ImageUrl = "https://example.com/pink-tshirt.jpg",
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "Lacivert İş T-Shirt",
                        Description = "İş ortamı için uygun lacivert t-shirt",
                        Price = 119.99m,
                        Color = "Lacivert",
                        Size = "L",
                        StockQuantity = 45,
                        ImageUrl = "https://example.com/navy-tshirt.jpg",
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "Sarı Enerjik T-Shirt",
                        Description = "Enerjik ve canlı sarı t-shirt",
                        Price = 79.99m,
                        Color = "Sarı",
                        Size = "M",
                        StockQuantity = 30,
                        ImageUrl = "https://example.com/yellow-tshirt.jpg",
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "Kahverengi Doğal T-Shirt",
                        Description = "Doğal ve sakin kahverengi t-shirt",
                        Price = 94.99m,
                        Color = "Kahverengi",
                        Size = "XL",
                        StockQuantity = 20,
                        ImageUrl = "https://example.com/brown-tshirt.jpg",
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "Turkuaz Deniz T-Shirt",
                        Description = "Deniz mavisi turkuaz t-shirt",
                        Price = 109.99m,
                        Color = "Turkuaz",
                        Size = "L",
                        StockQuantity = 28,
                        ImageUrl = "https://example.com/turquoise-tshirt.jpg",
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "Bordo Lüks T-Shirt",
                        Description = "Lüks ve şık bordo t-shirt",
                        Price = 179.99m,
                        Color = "Bordo",
                        Size = "M",
                        StockQuantity = 12,
                        ImageUrl = "https://example.com/burgundy-tshirt.jpg",
                        IsActive = true
                    },
                    new CreateTShirtDto
                    {
                        Name = "Koyu Yeşil Orman T-Shirt",
                        Description = "Orman yeşili doğa dostu t-shirt",
                        Price = 139.99m,
                        Color = "Koyu Yeşil",
                        Size = "XL",
                        StockQuantity = 18,
                        ImageUrl = "https://example.com/dark-green-tshirt.jpg",
                        IsActive = true
                    }
                };

                var createdCount = 0;
                foreach (var tShirt in tShirts)
                {
                    try
                    {
                        var result = await _tShirtService.CreateAsync(tShirt);
                        if (result.IsSuccess)
                        {
                            Console.WriteLine($"✅ TShirt oluşturuldu: {tShirt.Name} (ID: {result.Value?.Id})");
                            createdCount++;
                        }
                        else
                        {
                            Console.WriteLine($"❌ TShirt oluşturulamadı: {tShirt.Name} - {result.Error?.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ TShirt oluşturma hatası: {tShirt.Name} - {ex.Message}");
                    }
                }

                Console.WriteLine($"📊 TShirt işlemi tamamlandı: {createdCount}/{tShirts.Count}");
                return createdCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ TShirt oluşturma hatası: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> SeedTestProductDetailsAsync()
        {
            try
            {
                Console.WriteLine("📋 ProductDetail verileri kontrol ediliyor...");
                
                // TShirt'leri al
                Console.WriteLine("🔍 TShirt'ler alınıyor...");
                var tShirts = await _tShirtService.GetAllAsync();
                Console.WriteLine($"TShirt service sonucu: IsSuccess={tShirts.IsSuccess}");
                
                if (!tShirts.IsSuccess)
                {
                    Console.WriteLine($"❌ TShirt service hatası: {tShirts.Error?.Message}");
                    return false;
                }
                
                if (tShirts.Value == null)
                {
                    Console.WriteLine("❌ TShirt Value null");
                    return false;
                }
                
                if (!tShirts.Value.Any())
                {
                    Console.WriteLine("❌ TShirt listesi boş");
                    return false;
                }

                var tShirtList = tShirts.Value.ToList();
                Console.WriteLine($"✅ {tShirtList.Count} TShirt bulundu");
                
                // TShirt detaylarını yazdır
                foreach (var tshirt in tShirtList)
                {
                    Console.WriteLine($"   TShirt: ID={tshirt.Id}, Name={tshirt.Name}");
                }

                // Mevcut ProductDetail'leri kontrol et
                var existingProductDetails = await _productDetailService.GetAllAsync();
                if (existingProductDetails.IsSuccess && existingProductDetails.Value != null && existingProductDetails.Value.Any())
                {
                    var existingCount = existingProductDetails.Value.Count();
                    Console.WriteLine($"ℹ️ {existingCount} ProductDetail zaten mevcut");
                    
                    // Eğer 15 veya daha fazla ProductDetail varsa, yeni ekleme
                    if (existingCount >= 15)
                    {
                        Console.WriteLine("✅ Yeterli ProductDetail mevcut, yeni ekleme yapılmıyor");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ Sadece {existingCount} ProductDetail var, {15 - existingCount} tane daha ekleniyor");
                    }
                }

                var productDetails = new List<CreateProductDetailDto>();
                
                // Her TShirt için ProductDetail oluştur
                foreach (var tShirt in tShirtList)
                {
                    productDetails.Add(new CreateProductDetailDto
                    {
                        TShirtId = tShirt.Id,
                        Material = "100% Pamuk",
                        CareInstructions = "30°C'de yıkayın, ütüleyin",
                        Brand = "BAMK",
                        Origin = "Türkiye",
                        Weight = "180g",
                        Dimensions = "Göğüs: 50cm, Boy: 70cm",
                        Features = "Nefes alabilir, yumuşak dokulu",
                        AdditionalInfo = "Çevre dostu üretim",
                        IsActive = true
                    });
                }

                var createdCount = 0;
                Console.WriteLine($"📝 {productDetails.Count} ProductDetail oluşturulmaya çalışılıyor...");
                
                foreach (var productDetail in productDetails)
                {
                    try
                    {
                        Console.WriteLine($"🔄 ProductDetail oluşturuluyor: TShirt ID {productDetail.TShirtId}");
                        Console.WriteLine($"   Material: {productDetail.Material}");
                        Console.WriteLine($"   Brand: {productDetail.Brand}");
                        
                        var result = await _productDetailService.CreateAsync(productDetail);
                        Console.WriteLine($"Service sonucu: IsSuccess={result.IsSuccess}");
                        
                        if (result.IsSuccess)
                        {
                            Console.WriteLine($"✅ ProductDetail oluşturuldu: TShirt ID {productDetail.TShirtId} (ID: {result.Value?.Id})");
                            createdCount++;
                        }
                        else
                        {
                            Console.WriteLine($"❌ ProductDetail oluşturulamadı: TShirt ID {productDetail.TShirtId}");
                            Console.WriteLine($"   Hata: {result.Error?.Message}");
                            Console.WriteLine($"   Hata Kodu: {result.Error?.Code}");
                            Console.WriteLine($"   Hata Detayı: {result.Error?.ToString()}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ ProductDetail oluşturma hatası: TShirt ID {productDetail.TShirtId}");
                        Console.WriteLine($"   Exception: {ex.Message}");
                        Console.WriteLine($"   Stack Trace: {ex.StackTrace}");
                    }
                }

                Console.WriteLine($"📊 ProductDetail işlemi tamamlandı: {createdCount}/{productDetails.Count}");
                return createdCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ProductDetail oluşturma hatası: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> SeedTestQuestionsAsync()
        {
            try
            {
                Console.WriteLine("❓ Question verileri kontrol ediliyor...");
                
                // Test kullanıcısını al
                var testUser = await _userService.GetByEmailAsync("test@bamk.com");
                if (!testUser.IsSuccess || testUser.Value == null)
                {
                    Console.WriteLine("❌ Test kullanıcısı bulunamadı");
                    return false;
                }

                // Mevcut soruları kontrol et
                var existingQuestions = await _questionService.GetAllAsync();
                if (existingQuestions.IsSuccess && existingQuestions.Value != null && existingQuestions.Value.Any())
                {
                    Console.WriteLine($"ℹ️ {existingQuestions.Value.Count()} soru zaten mevcut");
                    return true;
                }

                var questions = new List<CreateQuestionDto>
                {
                    new CreateQuestionDto
                    {
                        QuestionTitle = "T-Shirt bedenleri hakkında",
                        QuestionContent = "T-shirt bedenleri nasıl? Küçük gelir mi? Hangi bedeni seçmeliyim?",
                        UserId = testUser.Value.Id
                    },
                    new CreateQuestionDto
                    {
                        QuestionTitle = "Kargo süresi ne kadar?",
                        QuestionContent = "Kargo ne kadar sürer? Hangi şehirlere gönderiyorsunuz?",
                        UserId = testUser.Value.Id
                    },
                    new CreateQuestionDto
                    {
                        QuestionTitle = "Ödeme yöntemleri",
                        QuestionContent = "Hangi ödeme yöntemlerini kabul ediyorsunuz? Kredi kartı ile ödeme yapabilir miyim?",
                        UserId = testUser.Value.Id
                    },
                    new CreateQuestionDto
                    {
                        QuestionTitle = "İade politikası",
                        QuestionContent = "Ürünleri iade edebilir miyim? Koşulları neler?",
                        UserId = testUser.Value.Id
                    },
                    new CreateQuestionDto
                    {
                        QuestionTitle = "Kumaş kalitesi",
                        QuestionContent = "T-shirt'lerin kumaş kalitesi nasıl? Yıkandığında çeker mi?",
                        UserId = testUser.Value.Id
                    }
                };

                var createdCount = 0;
                foreach (var question in questions)
                {
                    try
                    {
                        var result = await _questionService.CreateAsync(question);
                        if (result.IsSuccess)
                        {
                            Console.WriteLine($"✅ Soru oluşturuldu: {question.QuestionTitle}");
                            createdCount++;
                        }
                        else
                        {
                            Console.WriteLine($"❌ Soru oluşturulamadı: {question.QuestionTitle} - {result.Error?.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Soru oluşturma hatası: {question.QuestionTitle} - {ex.Message}");
                    }
                }

                Console.WriteLine($"📊 Soru işlemi tamamlandı: {createdCount}/{questions.Count}");
                return createdCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Soru oluşturma hatası: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> SeedTestOrdersAsync()
        {
            try
            {
                Console.WriteLine("📦 Order verileri kontrol ediliyor...");
                
                // Test kullanıcısını al
                var testUser = await _userService.GetByEmailAsync("test@bamk.com");
                if (!testUser.IsSuccess || testUser.Value == null)
                {
                    Console.WriteLine("❌ Test kullanıcısı bulunamadı");
                    return false;
                }

                // TShirt'leri al
                var tShirts = await _tShirtService.GetAllAsync();
                if (!tShirts.IsSuccess || tShirts.Value == null || !tShirts.Value.Any())
                {
                    Console.WriteLine("❌ TShirt bulunamadı");
                    return false;
                }

                var tShirtList = tShirts.Value.ToList();
                Console.WriteLine($"✅ {tShirtList.Count} TShirt bulundu");

                // Mevcut siparişleri kontrol et
                var existingOrders = await _orderService.GetAllAsync();
                if (existingOrders.IsSuccess && existingOrders.Value != null && existingOrders.Value.Any())
                {
                    Console.WriteLine($"ℹ️ {existingOrders.Value.Count()} sipariş zaten mevcut");
                    return true;
                }

                var orders = new List<CreateOrderDto>();
                
                // İlk sipariş - 2 farklı TShirt
                if (tShirtList.Count >= 2)
                {
                    orders.Add(new CreateOrderDto
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
                    });
                }

                // İkinci sipariş - tek TShirt
                if (tShirtList.Count >= 1)
                {
                    orders.Add(new CreateOrderDto
                    {
                        UserId = testUser.Value.Id,
                        OrderItems = new List<CreateOrderItemDto>
                        {
                            new CreateOrderItemDto
                            {
                                TShirtId = tShirtList[0].Id,
                                Quantity = 3
                            }
                        }
                    });
                }

                var createdCount = 0;
                foreach (var order in orders)
                {
                    try
                    {
                        var result = await _orderService.CreateAsync(order);
                        if (result.IsSuccess)
                        {
                            Console.WriteLine($"✅ Sipariş oluşturuldu: ID {result.Value?.Id}");
                            createdCount++;
                        }
                        else
                        {
                            Console.WriteLine($"❌ Sipariş oluşturulamadı: {result.Error?.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Sipariş oluşturma hatası: {ex.Message}");
                    }
                }

                Console.WriteLine($"📊 Sipariş işlemi tamamlandı: {createdCount}/{orders.Count}");
                return createdCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Sipariş oluşturma hatası: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> SeedTestCartsAsync()
        {
            try
            {
                Console.WriteLine("🛒 Cart verileri kontrol ediliyor...");
                
                // Kullanıcıları al
                var users = await _userService.GetAllAsync();
                if (!users.IsSuccess || users.Value == null || !users.Value.Any())
                {
                    Console.WriteLine("❌ Kullanıcı bulunamadı");
                    return false;
                }

                // TShirt'leri al
                var tShirts = await _tShirtService.GetAllAsync();
                if (!tShirts.IsSuccess || tShirts.Value == null || !tShirts.Value.Any())
                {
                    Console.WriteLine("❌ TShirt bulunamadı");
                    return false;
                }

                var userList = users.Value.ToList();
                var tShirtList = tShirts.Value.ToList();
                Console.WriteLine($"✅ {userList.Count} kullanıcı ve {tShirtList.Count} TShirt bulundu");

                var cartCreatedCount = 0;
                var maxUsersToProcess = Math.Min(userList.Count, 3); // En fazla 3 kullanıcı

                for (int i = 0; i < maxUsersToProcess; i++)
                {
                    var user = userList[i];
                    
                    try
                    {
                        Console.WriteLine($"🛒 Cart oluşturuluyor: {user.FirstName} {user.LastName} (ID: {user.Id})");
                        
                        // Cart'ı kontrol et
                        var cartResult = await _cartService.GetCartAsync(user.Id);
                        Console.WriteLine($"Cart service sonucu: IsSuccess={cartResult.IsSuccess}");
                        
                        if (!cartResult.IsSuccess)
                        {
                            Console.WriteLine($"❌ Cart alınamadı: {user.FirstName} {user.LastName} - {cartResult.Error?.Message}");
                            Console.WriteLine($"   Hata Kodu: {cartResult.Error?.Code}");
                            Console.WriteLine($"   Hata Detayı: {cartResult.Error?.ToString()}");
                            continue; // Bu kullanıcıyı atla
                        }
                        else
                        {
                            Console.WriteLine($"✅ Cart alındı: {user.FirstName} {user.LastName}");
                            Console.WriteLine($"   Cart ID: {cartResult.Value?.Id}");
                            Console.WriteLine($"   Cart Items: {cartResult.Value?.CartItems?.Count ?? 0}");
                            
                            // Eğer cart'ta ürün varsa atla
                            if (cartResult.Value?.CartItems?.Any() == true)
                            {
                                Console.WriteLine($"ℹ️ Cart zaten dolu: {user.FirstName} {user.LastName} ({cartResult.Value.CartItems.Count} ürün)");
                                cartCreatedCount++;
                                continue;
                            }
                        }

                        // Rastgele ürünleri sepete ekle
                        var random = new Random();
                        var cartItemCount = random.Next(1, Math.Min(3, tShirtList.Count + 1));
                        var addedItems = 0;

                        Console.WriteLine($"📦 {cartItemCount} ürün sepete ekleniyor...");

                        for (int j = 0; j < cartItemCount; j++)
                        {
                            var randomTShirt = tShirtList[random.Next(tShirtList.Count)];
                            var quantity = random.Next(1, 3);

                            var addToCartDto = new AddToCartDto
                            {
                                TShirtId = randomTShirt.Id,
                                Quantity = quantity
                            };

                            Console.WriteLine($"   Ürün ekleniyor: {randomTShirt.Name} (ID: {randomTShirt.Id}, Miktar: {quantity})");
                            
                            var addResult = await _cartService.AddToCartAsync(user.Id, addToCartDto);
                            Console.WriteLine($"   AddToCart sonucu: IsSuccess={addResult.IsSuccess}");
                            
                            if (addResult.IsSuccess)
                            {
                                addedItems++;
                                Console.WriteLine($"   ✅ Ürün eklendi");
                            }
                            else
                            {
                                Console.WriteLine($"   ❌ Ürün eklenemedi: {addResult.Error?.Message}");
                            }
                        }

                        if (addedItems > 0)
                        {
                            Console.WriteLine($"✅ Cart oluşturuldu: {user.FirstName} {user.LastName} ({addedItems} ürün)");
                            cartCreatedCount++;
                        }
                        else
                        {
                            Console.WriteLine($"⚠️ Cart oluşturulamadı: {user.FirstName} {user.LastName} (hiç ürün eklenemedi)");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Cart oluşturma hatası: {user.FirstName} {user.LastName} - {ex.Message}");
                    }
                }

                Console.WriteLine($"📊 Cart işlemi tamamlandı: {cartCreatedCount} cart oluşturuldu");
                return cartCreatedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Cart oluşturma hatası: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> SeedTestAnswersAsync()
        {
            try
            {
                Console.WriteLine("💬 Answer verileri kontrol ediliyor...");
                
                // Soruları al
                var questions = await _questionService.GetAllAsync();
                if (!questions.IsSuccess || questions.Value == null || !questions.Value.Any())
                {
                    Console.WriteLine("❌ Soru bulunamadı, cevap oluşturulamıyor");
                    return false;
                }

                var questionList = questions.Value.ToList();
                Console.WriteLine($"✅ {questionList.Count} soru bulundu");

                // Kullanıcıları al
                var users = await _userService.GetAllAsync();
                if (!users.IsSuccess || users.Value == null || !users.Value.Any())
                {
                    Console.WriteLine("❌ Kullanıcı bulunamadı, cevap oluşturulamıyor");
                    return false;
                }

                var userList = users.Value.ToList();
                Console.WriteLine($"✅ {userList.Count} kullanıcı bulundu");

                // Mevcut cevapları kontrol et
                var existingAnswers = await _questionService.GetAnswersByQuestionIdAsync(questionList[0].Id);
                if (existingAnswers.IsSuccess && existingAnswers.Value != null && existingAnswers.Value.Any())
                {
                    Console.WriteLine($"ℹ️ {existingAnswers.Value.Count()} cevap zaten mevcut");
                    return true;
                }

                var answers = new List<CreateAnswerDto>();
                
                // Her soru için 1-2 cevap oluştur
                foreach (var question in questionList.Take(3)) // İlk 3 soru için
                {
                    // Rastgele kullanıcı seç
                    var random = new Random();
                    var randomUser = userList[random.Next(userList.Count)];
                    
                    answers.Add(new CreateAnswerDto
                    {
                        AnswerContent = $"Bu soruya cevap: {question.QuestionTitle} için detaylı açıklama yapabilirim. Bu konuda deneyimim var ve yardımcı olabilirim.",
                        QuestionId = question.Id,
                        UserId = randomUser.Id
                    });

                    // İkinci cevap (eğer farklı kullanıcı varsa)
                    if (userList.Count > 1)
                    {
                        var secondUser = userList.Where(u => u.Id != randomUser.Id).FirstOrDefault();
                        if (secondUser != null)
                        {
                            answers.Add(new CreateAnswerDto
                            {
                                AnswerContent = $"Alternatif görüş: {question.QuestionTitle} konusunda farklı bir yaklaşım önerebilirim. Bu yöntem daha etkili olabilir.",
                                QuestionId = question.Id,
                                UserId = secondUser.Id
                            });
                        }
                    }
                }

                var createdCount = 0;
                Console.WriteLine($"📝 {answers.Count} cevap oluşturulmaya çalışılıyor...");
                
                foreach (var answer in answers)
                {
                    try
                    {
                        Console.WriteLine($"🔄 Cevap oluşturuluyor: Soru ID {answer.QuestionId}, Kullanıcı ID {answer.UserId}");
                        
                        var result = await _questionService.CreateAnswerAsync(answer);
                        if (result.IsSuccess)
                        {
                            Console.WriteLine($"✅ Cevap oluşturuldu: ID {result.Value?.Id}");
                            createdCount++;
                        }
                        else
                        {
                            Console.WriteLine($"❌ Cevap oluşturulamadı: {result.Error?.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Cevap oluşturma hatası: {ex.Message}");
                    }
                }

                Console.WriteLine($"📊 Cevap işlemi tamamlandı: {createdCount}/{answers.Count}");
                return createdCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Cevap oluşturma hatası: {ex.Message}");
                return false;
            }
        }

        public async Task SeedTestUserAsync()
        {
            await SeedTestUsersAsync();
        }
    }
}