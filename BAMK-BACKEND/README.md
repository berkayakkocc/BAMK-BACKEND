# 🚀 BAMK Backend - E-commerce API

## 📊 Proje Durumu
- **MVP Tamamlanma Oranı:** %100 ✅
- **Son Güncelleme:** 2024-12-09
- **Backend Durumu:** ✅ Tamamlandı

---

## 🎯 Bugün Yapılan Güncellemeler (2024-12-09)

### 🆕 Yeni Özellikler

#### 1. ProductDetail Entity Sistemi
- ✅ **ProductDetail entity** oluşturuldu (2024-12-09)
- ✅ **TShirt ile 1:1 ilişki** kuruldu (2024-12-09)
- ✅ **CRUD servisleri** implementasyonu (2024-12-09)
- ✅ **API Controller** oluşturuldu (2024-12-09)
- ✅ **DTOs ve AutoMapper** profili (2024-12-09)

#### 2. Veritabanı İyileştirmeleri
- ✅ **Clean Code** property isimleri (2024-12-09)
- ✅ **Navigation property** düzeltmeleri (2024-12-09)
- ✅ **Foreign key** ilişkileri optimize edildi (2024-12-09)
- ✅ **Migration** oluşturuldu ve uygulandı (2024-12-09)

#### 3. Test Data Sistemi Tamamen Yenilendi
- ✅ **TestDataSeeder** tamamen yeniden yazıldı (2024-12-09)
- ✅ **15 TShirt** çeşitli renklerde (2024-12-09)
- ✅ **15 ProductDetail** her TShirt için (2024-12-09)
- ✅ **Answer sistemi** sorulara cevaplar (2024-12-09)
- ✅ **Cart sistemi** düzeltildi ve çalışır hale getirildi (2024-12-09)
- ✅ **Akıllı tekrar kontrolü** - aynı verileri tekrar eklemiyor (2024-12-09)
- ✅ **Temiz logging** - gereksiz loglar kaldırıldı (2024-12-09)

#### 4. Cart Sistemi Düzeltmeleri
- ✅ **Cart Service** SaveChangesAsync eksiklikleri düzeltildi (2024-12-09)
- ✅ **Entity Framework Include** hataları çözüldü (2024-12-09)
- ✅ **Cart oluşturma** ve **ürün ekleme** çalışır hale getirildi (2024-12-09)

---

## 🏗️ Mimari Yapı

### 📁 Proje Katmanları
```
BAMK-BACKEND/
├── BAMK.API/                 # Web API Layer
├── BAMK.Application/         # Application Layer
├── BAMK.Domain/             # Domain Layer
├── BAMK.Infrastructure/     # Infrastructure Layer
└── BAMK.Core/              # Core Layer
```

### 🗄️ Veritabanı Yapısı

#### Entities
- **User** - Kullanıcı bilgileri
- **TShirt** - T-shirt ürünleri
- **ProductDetail** - Ürün detay bilgileri (YENİ!)
- **Order** - Sipariş bilgileri
- **OrderItem** - Sipariş kalemleri
- **Question** - Sorular
- **Answer** - Cevaplar

#### İlişkiler
- User → Orders (1:N)
- User → Questions (1:N)
- User → Answers (1:N)
- TShirt → ProductDetail (1:1) (YENİ!)
- TShirt → OrderItems (1:N)
- Order → OrderItems (1:N)
- Question → Answers (1:N)

---

## 🚀 API Endpoints

### 🔐 Authentication (3 endpoint)
- `POST /api/auth/login` - Giriş yap
- `POST /api/auth/register` - Kayıt ol
- `POST /api/auth/logout` - Çıkış yap

### 👤 Users (4 endpoint)
- `GET /api/users` - Tüm kullanıcılar
- `GET /api/users/{id}` - Kullanıcı detayı
- `POST /api/users` - Yeni kullanıcı
- `PUT /api/users/{id}` - Kullanıcı güncelle

### 👕 TShirts (8 endpoint)
- `GET /api/tshirts` - Tüm t-shirt'ler
- `GET /api/tshirts/{id}` - T-shirt detayı
- `POST /api/tshirts` - Yeni t-shirt
- `PUT /api/tshirts/{id}` - T-shirt güncelle
- `DELETE /api/tshirts/{id}` - T-shirt sil
- `GET /api/tshirts/active` - Aktif t-shirt'ler
- `GET /api/tshirts/color/{color}` - Renk bazlı filtreleme
- `GET /api/tshirts/size/{size}` - Beden bazlı filtreleme
- `PUT /api/tshirts/{id}/stock` - Stok güncelleme

### 📋 ProductDetails (7 endpoint) - YENİ!
- `GET /api/productdetail` - Tüm ürün detayları
- `GET /api/productdetail/{id}` - Ürün detayı
- `GET /api/productdetail/tshirt/{tshirtId}` - TShirt'e göre detay
- `POST /api/productdetail` - Yeni ürün detayı
- `PUT /api/productdetail/{id}` - Ürün detayı güncelle
- `PUT /api/productdetail/{id}/status` - Durum güncelle
- `DELETE /api/productdetail/{id}` - Ürün detayı sil

### 📦 Orders (8 endpoint)
- `GET /api/orders` - Tüm siparişler
- `GET /api/orders/{id}` - Sipariş detayı
- `POST /api/orders` - Yeni sipariş
- `PUT /api/orders/{id}/status` - Sipariş durumu güncelle
- `PUT /api/orders/{id}/payment-status` - Ödeme durumu güncelle
- `GET /api/orders/user/{userId}` - Kullanıcı siparişleri
- `GET /api/orders/{id}/total` - Sipariş toplamı
- `DELETE /api/orders/{id}` - Sipariş sil

### ❓ Questions (12 endpoint)
- `GET /api/question` - Tüm sorular
- `GET /api/question/{id}` - Soru detayı
- `GET /api/question/user/{userId}` - Kullanıcı soruları
- `POST /api/question` - Yeni soru
- `PUT /api/question/{id}` - Soru güncelle
- `DELETE /api/question/{id}` - Soru sil
- `PUT /api/question/{id}/activate` - Soru aktifleştir
- `PUT /api/question/{id}/deactivate` - Soru deaktifleştir
- `POST /api/question/{id}/answers` - Cevap ekle
- `GET /api/question/{id}/answers` - Soru cevapları
- `GET /api/question/answers/{id}` - Cevap detayı
- `PUT /api/question/answers/{id}` - Cevap güncelle
- `DELETE /api/question/answers/{id}` - Cevap sil

---

## 🛠️ Teknoloji Stack

### Backend
- **.NET 9** (YENİ!)
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server Express**
- **AutoMapper**
- **Fluent Validation**
- **JWT Authentication**
- **Swagger/OpenAPI**

### Architecture
- **Clean Architecture**
- **Repository Pattern**
- **Unit of Work Pattern**
- **Dependency Injection**
- **DTO Pattern**

---

## 🚀 Hızlı Başlangıç

### 1. Projeyi Çalıştırma
```bash
# Projeyi klonla
git clone <repository-url>
cd BAMK-BACKEND

# Veritabanını oluştur
dotnet ef database update --project BAMK.Infrastructure --startup-project BAMK.API

# Projeyi çalıştır
dotnet run --project BAMK.API
```

### 2. API Test Etme
- **Swagger UI:** https://localhost:5240/swagger
- **Base URL:** https://localhost:5240/api

### 3. Test Kullanıcıları
- **Admin:** admin@bamk.com / admin123
- **Test:** test@bamk.com / 123456
- **Customer:** customer@bamk.com / customer123

---

## 📊 Veritabanı Bilgileri

### Connection String
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=BAMK_DB_Dev;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### Migrations
- **InitialCreate** - Temel tablolar
- **AddTestDataSupport** - Test verisi desteği
- **AddProductDetailEntity** - ProductDetail entity
- **UpdateEntityPropertiesAndNavigationRelations** - Property ve ilişki güncellemeleri

---

## 🎯 Bugün Yapılan Değişiklikler Detayı

### 1. ProductDetail Entity
```csharp
public class ProductDetail : BaseEntity
{
    public int TShirtId { get; set; }
    public string Material { get; set; }
    public string CareInstructions { get; set; }
    public string Brand { get; set; }
    public string Origin { get; set; }
    public string Weight { get; set; }
    public string Dimensions { get; set; }
    public string Features { get; set; }
    public string AdditionalInfo { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation Properties
    public virtual TShirt TShirt { get; set; }
}
```

### 2. Clean Code Property İsimleri
- `Order.Status` → `Order.OrderStatus`
- `Order.Notes` → `Order.OrderNotes`
- `Question.Title` → `Question.QuestionTitle`
- `Question.Content` → `Question.QuestionContent`
- `Answer.Content` → `Answer.AnswerContent`
- `Answer.IsAccepted` → `Answer.IsAcceptedAnswer`

### 3. Test Data Güncellemeleri
```csharp
// Siyah TShirt'ler için ImageUrl eklendi
new CreateTShirtDto
{
    Name = "BAMK Classic Black",
    Description = "Klasik siyah BAMK t-shirt",
    ImageUrl = "https://i.hizliresim.com/et5qfcq.jpg", // YENİ!
    // ... diğer özellikler
}
```

---

## 📈 Performans ve Güvenlik

### ✅ Implemented
- JWT Authentication
- Password Hashing
- CORS Configuration
- Input Validation
- Error Handling
- Repository Pattern
- AutoMapper

### 🔄 Gelecek Özellikler
- Rate Limiting
- Caching (Redis)
- Logging (Serilog)
- Health Checks
- API Versioning

---

## 🧪 Test Coverage

### ✅ Test Data
- **5 Test kullanıcısı** (Admin, Test, Customer, Ahmet, Ayşe) (2024-12-09)
- **15 Test TShirt'i** (15 farklı renk ve model) (2024-12-09)
- **15 ProductDetail** (her TShirt için detaylı bilgi) (2024-12-09)
- **5 Test sorusu** (çeşitli konularda) (2024-12-09)
- **6 Test cevabı** (sorulara verilen cevaplar) (2024-12-09)
- **2 Test siparişi** (farklı ürünlerle) (2024-12-09)
- **3 Test sepeti** (rastgele ürünlerle) (2024-12-09)

### 🔄 Gelecek Testler
- Unit Tests
- Integration Tests
- API Tests
- Performance Tests

---

## 📝 Notlar

### Database
- **Type:** SQL Server Express
- **Name:** BAMK_DB_Dev
- **Auto Migration:** ✅ Enabled
- **Seed Data:** ✅ Available

### API
- **Port:** 5240 (HTTP)
- **Swagger:** https://localhost:5240/swagger
- **Authentication:** JWT Bearer Token

---

## 🎯 Sonraki Adımlar

1. **Frontend Geliştirme** (React/Next.js)
2. **Ödeme Sistemi** (İyzico/Stripe)
3. **Production Deployment** (Azure/AWS)
4. **Domain & SSL** kurulumu
5. **Monitoring & Logging** implementasyonu

---

**Son Güncelleme:** 2024-12-09  
**Durum:** Backend MVP %100 tamamlandı ✅  
**Bugünkü Geliştirme:** 
- ✅ ProductDetail sistemi tamamen çalışır hale getirildi (2024-12-09)
- ✅ TestDataSeeder tamamen yeniden yazıldı ve optimize edildi (2024-12-09)
- ✅ Cart sistemi düzeltildi ve çalışır hale getirildi (2024-12-09)
- ✅ 15 TShirt + 15 ProductDetail + Answer + Cart test verileri eklendi (2024-12-09)
- ✅ Akıllı tekrar kontrolü ve temiz logging sistemi (2024-12-09)
