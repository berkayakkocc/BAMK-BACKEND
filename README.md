# BAMK Backend API

E-commerce platformu için geliştirilmiş modern .NET 9 Web API projesi.

## 🏗️ Mimari

- **Clean Architecture** prensiplerine uygun katmanlı yapı
- **Repository Pattern** + **Unit of Work**
- **Result Pattern** ile structured error handling
- **JWT Authentication** sistemi
- **Entity Framework Core** ile veritabanı yönetimi

## 🚀 Özellikler

- ✅ JWT Authentication & Authorization
- ✅ User Management (CRUD)
- ✅ Auto Migration
- ✅ Structured Error Handling
- ✅ Swagger Documentation
- ✅ CORS Configuration
- ✅ Clean Code Principles
- ✅ Repository Pattern
- ✅ Fluent Validation
- ✅ AutoMapper

## 🛠️ Teknolojiler

- **.NET 9**
- **Entity Framework Core**
- **SQL Server**
- **AutoMapper**
- **FluentValidation**
- **BCrypt.Net**
- **Swagger/OpenAPI**
- **JWT Bearer Authentication**

## 📁 Proje Yapısı

```
BAMK-Backend/
├── BAMK.API/                 # Web API Katmanı
│   ├── Controllers/          # API Controllers
│   ├── Services/            # API Services
│   ├── DTOs/               # Data Transfer Objects
│   └── Configuration/       # JWT Settings
├── BAMK.Application/         # Uygulama Katmanı
│   ├── Services/           # Business Logic
│   └── DTOs/              # Application DTOs
├── BAMK.Core/               # Temel Katman
│   ├── Common/             # Result Pattern, Error Handling
│   ├── Entities/           # Base Entities
│   └── Interfaces/         # Generic Repository
├── BAMK.Domain/             # Domain Katmanı
│   ├── Entities/           # Domain Models
│   └── Interfaces/         # Unit of Work
└── BAMK.Infrastructure/     # Altyapı Katmanı
    ├── Data/              # DbContext, Migrations
    ├── Repositories/      # Repository Implementations
    └── Extensions/        # Service Extensions
```

## 🚀 Kurulum

1. Repository'yi klonlayın
```bash
git clone https://github.com/KULLANICI_ADINIZ/BAMK-Backend.git
cd BAMK-Backend
```

2. SQL Server'ı başlatın

3. `appsettings.json`'da connection string'i güncelleyin:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=BAMK_DB;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

4. Projeyi çalıştırın:
```bash
cd BAMK-Backend
dotnet restore
dotnet build
dotnet run --project BAMK.API
```

## 📚 API Dokümantasyonu

Uygulama çalıştıktan sonra Swagger UI'a erişin:
- **Development:** https://localhost:7000/swagger
- **HTTP:** http://localhost:5000/swagger

## 🔐 Test Kullanıcısı

- **Email:** test@bamk.com
- **Password:** 123456

## 📋 API Endpoints

### Authentication
- `POST /api/auth/login` - Kullanıcı girişi
- `POST /api/auth/register` - Kullanıcı kaydı
- `POST /api/auth/logout` - Kullanıcı çıkışı

### Health Check
- `GET /api/health` - Sistem durumu

## 🏛️ Clean Architecture Katmanları

### 1. **Core Layer**
- Result Pattern implementation
- Error handling
- Base entities
- Generic repository interface

### 2. **Domain Layer**
- Business entities (User, TShirt, Order, etc.)
- Domain interfaces
- Business rules

### 3. **Application Layer**
- Business logic services
- DTOs
- Application interfaces

### 4. **Infrastructure Layer**
- Entity Framework implementation
- Repository implementations
- Database context
- Migrations

### 5. **API Layer**
- Controllers
- Authentication
- Swagger documentation
- CORS configuration

## 🔧 Result Pattern

Proje modern Result Pattern kullanır:

```csharp
// Success
Result<UserDto>.Success(user)

// Error
Result<UserDto>.Failure(Error.UserNotFound(email))

// Validation Error
Result.ValidationFailure(ValidationError.Required("email"))
```

## 🛡️ Error Handling

Structured error handling sistemi:

- **Error Codes:** Enum-based error categorization
- **Validation Errors:** Field-specific validation
- **HTTP Status Codes:** Proper API responses
- **Error Metadata:** Additional error context

## 📝 Lisans

MIT License

## 🤝 Katkıda Bulunma

1. Fork edin
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Commit edin (`git commit -m 'Add some AmazingFeature'`)
4. Push edin (`git push origin feature/AmazingFeature`)
5. Pull Request oluşturun

## 📞 İletişim

Proje hakkında sorularınız için issue açabilirsiniz.

---

**Not:** Bu proje eğitim amaçlı geliştirilmiştir ve production kullanımı için ek güvenlik önlemleri alınmalıdır.
