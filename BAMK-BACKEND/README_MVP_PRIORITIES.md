# BAMK Backend MVP Öncelik Listesi

## 📊 Mevcut Durum
- **MVP Tamamlanma Oranı:** %50
- **Tahmini Kalan Süre:** 2-4 gün
- **Son Güncelleme:** 2024-09-09

## 🎯 MVP Hedefleri
1. **Kullanıcı Yönetimi** - Tam çalışır durumda
2. **T-Shirt Yönetimi** - CRUD operations
3. **Sipariş Sistemi** - Temel sipariş işlemleri
4. **Soru-Cevap Sistemi** - Temel Q&A işlemleri

---

## 🔥 YÜKSEK ÖNCELİK (1-2 gün)

### 1. Veritabanı Entegrasyonu
- [x] **Entity Framework Migrations oluşturma**
  - `dotnet ef migrations add InitialCreate` ✅
  - `dotnet ef database update` ✅
  - Test veritabanı bağlantısı ✅
  - Auto Migration özelliği eklendi ✅

- [ ] **UserService'i veritabanı ile entegre etme**
  - AuthService'de gerçek veritabanı sorguları
  - Password hashing ile user kaydetme
  - Email kontrolü ve validation

### 2. T-Shirt Yönetimi
- [ ] **TShirtService implementasyonu**
  - CRUD operations (Create, Read, Update, Delete)
  - Repository pattern kullanımı
  - AutoMapper entegrasyonu

- [ ] **TShirtController oluşturma**
  - GET /api/tshirts - Tüm t-shirt'leri listele
  - GET /api/tshirts/{id} - Tek t-shirt getir
  - POST /api/tshirts - Yeni t-shirt oluştur
  - PUT /api/tshirts/{id} - T-shirt güncelle
  - DELETE /api/tshirts/{id} - T-shirt sil

---

## 🟡 ORTA ÖNCELİK (2-3 gün)

### 3. Sipariş Sistemi
- [ ] **OrderService implementasyonu**
  - Sipariş oluşturma
  - Sipariş durumu güncelleme
  - Kullanıcı siparişlerini listeleme

- [ ] **OrderController oluşturma**
  - POST /api/orders - Yeni sipariş oluştur
  - GET /api/orders - Kullanıcı siparişlerini listele
  - GET /api/orders/{id} - Sipariş detayı
  - PUT /api/orders/{id}/status - Sipariş durumu güncelle

### 4. Soru-Cevap Sistemi
- [ ] **QuestionService implementasyonu**
  - Soru oluşturma ve listeleme
  - Cevap ekleme
  - Soru kategorileri

- [ ] **QuestionController oluşturma**
  - GET /api/questions - Soruları listele
  - POST /api/questions - Yeni soru oluştur
  - POST /api/questions/{id}/answers - Cevap ekle

---

## 🟢 DÜŞÜK ÖNCELİK (1-2 gün)

### 5. API İyileştirmeleri
- [ ] **Error Handling iyileştirmeleri**
  - Global exception handling
  - Custom error responses
  - Logging iyileştirmeleri

- [ ] **API Dokümantasyonu**
  - Swagger annotations
  - API response examples
  - Authentication documentation

### 6. Performance ve Güvenlik
- [ ] **Caching implementasyonu**
  - Memory caching
  - Response caching

- [ ] **Rate Limiting**
  - API rate limiting
  - Brute force protection

---

## 📋 Günlük Takip Listesi

### Gün 1: Veritabanı Entegrasyonu
- [x] Migrations oluştur ✅
- [x] Veritabanı bağlantısını test et ✅
- [x] Auto Migration özelliği eklendi ✅
- [ ] UserService'i veritabanı ile entegre et
- [ ] AuthService'i güncelle

### Gün 2: T-Shirt Yönetimi
- [ ] TShirtService oluştur
- [ ] TShirtController oluştur
- [ ] CRUD operations test et
- [ ] Swagger'da test et

### Gün 3: Sipariş Sistemi
- [ ] OrderService oluştur
- [ ] OrderController oluştur
- [ ] Sipariş işlemlerini test et

### Gün 4: Soru-Cevap Sistemi
- [ ] QuestionService oluştur
- [ ] QuestionController oluştur
- [ ] Q&A işlemlerini test et

### Gün 5: Test ve İyileştirmeler
- [ ] Tüm API'leri test et
- [ ] Error handling iyileştir
- [ ] Performance optimizasyonu

---

## 🚀 Hızlı Başlangıç Komutları

```bash
# Veritabanı migrations
cd Backend/BAMK.API
dotnet ef migrations add InitialCreate
dotnet ef database update

# Projeyi çalıştır
dotnet run

# Swagger'ı aç
https://localhost:7000/swagger
```

---

## 📝 Notlar

- **Test Kullanıcısı:** test@bamk.com / 123456
- **Database:** SQL Server LocalDB Express
- **Port:** 7000 (HTTPS), 5000 (HTTP)
- **Swagger:** https://localhost:7000/swagger

---

## ✅ Tamamlanan İşlemler

- [x] Clean Architecture yapısı
- [x] JWT Authentication sistemi
- [x] Repository Pattern + Unit of Work
- [x] Fluent Validation
- [x] AutoMapper
- [x] Swagger dokümantasyonu
- [x] CORS yapılandırması
- [x] Domain entities (User, TShirt, Order, Question, Answer)
- [x] BaseEntity ve Core katmanı
- [x] Dependency Injection yapılandırması
- [x] Entity Framework Migrations oluşturuldu
- [x] Veritabanı bağlantısı test edildi
- [x] Auto Migration özelliği eklendi
- [x] JWT SecretKey yapılandırması

---

**Son Güncelleme:** 2024-09-09  
**Durum:** MVP'ye %50 tamamlandı
