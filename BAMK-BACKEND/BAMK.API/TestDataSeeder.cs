using BAMK.Application.DTOs.User;
using BAMK.Application.Services;
using BAMK.Core.Common;

namespace BAMK.API
{
    public class TestDataSeeder
    {
        private readonly IUserService _userService;

        public TestDataSeeder(IUserService userService)
        {
            _userService = userService;
        }

        public async Task SeedTestUserAsync()
        {
            try
            {
                // Test kullanıcısının var olup olmadığını kontrol et
                var existingUser = await _userService.GetByEmailAsync("test@bamk.com");
                if (existingUser.IsSuccess)
                {
                    Console.WriteLine("✅ Test kullanıcısı zaten mevcut");
                    return;
                }

                // Test kullanıcısı oluştur
                var testUser = new CreateUserDto
                {
                    Email = "test@bamk.com",
                    Password = "123456",
                    FirstName = "Test",
                    LastName = "User",
                    PhoneNumber = "+905551234567"
                };

                var result = await _userService.CreateAsync(testUser);
                if (result.IsSuccess)
                {
                    Console.WriteLine("✅ Test kullanıcısı başarıyla oluşturuldu");
                }
                else
                {
                    Console.WriteLine($"❌ Test kullanıcısı oluşturulamadı: {result.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test kullanıcısı oluşturma hatası: {ex.Message}");
            }
        }
    }
}
