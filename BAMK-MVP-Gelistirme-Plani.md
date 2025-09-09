# 🚀 BAMK MVP Geliştirme Planı

## 📋 Proje Özeti

**BAMK MVP**, genç ve moda takipçilerine yönelik trend t-shirt satan, alışveriş odaklı bir e-ticaret platformunun minimum viable product versiyonu.

## 👥 Ekip Rolleri
- **Berkay** - Backend Developer (.NET Core)
- **Altay** - Frontend Developer (React.js)
- **Mert** - Mobile Developer (React Native)
- **Kerim** - UI/UX Designer

## 🎯 MVP Hedefleri

### Ana Hedefler
- ✅ 2-3 ayda çalışan platform
- ✅ Temel alışveriş deneyimi
- ✅ Basit soru-cevap sistemi
- ✅ Mobil uyumlu tasarım
- ✅ Güvenli ödeme sistemi

### MVP Özellikleri
1. **Temel T-Shirt Kataloğu** (5-10 ürün)
2. **Kullanıcı Kayıt/Giriş Sistemi**
3. **Sepet ve Sipariş Yönetimi**
4. **Basit Soru-Cevap Sistemi**
5. **Ödeme Sistemi (Iyzico)**
6. **Responsive Tasarım**

## 🛠️ Teknoloji Stack

### Backend
- **.NET Core 8**: Ana backend framework
- **ASP.NET Core Web API**: RESTful API
- **Entity Framework Core**: ORM
- **SQL Server**: Veritabanı
- **JWT**: Authentication
- **Swagger**: API dokümantasyonu

### Frontend
- **React.js 18**: Ana frontend framework
- **TypeScript**: Tip güvenliği
- **Material-UI**: UI bileşenleri
- **Redux Toolkit**: State management
- **Axios**: HTTP client
- **React Router**: Sayfa yönlendirme

### Veritabanı
- **SQL Server**: Ana veritabanı
- **Redis**: Cache ve session

## 📅 Geliştirme Zaman Çizelgesi

### Faz 1: Proje Kurulumu (1. Hafta)
- [ ] Backend proje kurulumu (.NET Core)
- [ ] Frontend proje kurulumu (React.js)
- [ ] Veritabanı tasarımı ve kurulumu
- [ ] Git repository kurulumu
- [ ] Development environment setup

### Faz 2: Backend Geliştirme (2-3. Hafta)
- [ ] Entity Framework modelleri
- [ ] API Controller'ları
- [ ] Authentication sistemi
- [ ] Temel CRUD operasyonları
- [ ] Swagger dokümantasyonu

### Faz 3: Frontend Geliştirme (3-4. Hafta)
- [ ] Ana sayfa tasarımı
- [ ] Ürün listesi sayfası
- [ ] Ürün detay sayfası
- [ ] Sepet sayfası
- [ ] Kullanıcı kayıt/giriş sayfaları

### Faz 4: Soru-Cevap Sistemi (4-5. Hafta)
- [ ] Soru-cevap backend API'leri
- [ ] Soru-cevap frontend bileşenleri
- [ ] Soru sorma formu
- [ ] Cevap verme sistemi
- [ ] Soru-cevap listesi

### Faz 5: Ödeme Sistemi (5-6. Hafta)
- [ ] Iyzico entegrasyonu
- [ ] Sipariş yönetimi
- [ ] Ödeme sayfası
- [ ] Sipariş takip sistemi
- [ ] Email bildirimleri

### Faz 6: Test ve Deployment (6-8. Hafta)
- [ ] Unit testler
- [ ] Integration testler
- [ ] E2E testler
- [ ] Performance testleri
- [ ] Production deployment
- [ ] Domain ve SSL kurulumu

## 🗄️ Veritabanı Tasarımı

### Temel Tablolar

#### Users Tablosu
```sql
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(255) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);
```

#### TShirts Tablosu
```sql
CREATE TABLE TShirts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(10,2) NOT NULL,
    Color NVARCHAR(50) NOT NULL,
    Size NVARCHAR(10) NOT NULL,
    Material NVARCHAR(100),
    ImageUrl NVARCHAR(500),
    StockQuantity INT DEFAULT 0,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);
```

#### Questions Tablosu
```sql
CREATE TABLE Questions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TShirtId INT FOREIGN KEY REFERENCES TShirts(Id),
    UserId INT FOREIGN KEY REFERENCES Users(Id),
    QuestionText NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    IsAnswered BIT DEFAULT 0
);
```

#### Answers Tablosu
```sql
CREATE TABLE Answers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    QuestionId INT FOREIGN KEY REFERENCES Questions(Id),
    UserId INT FOREIGN KEY REFERENCES Users(Id),
    AnswerText NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    IsHelpful BIT DEFAULT 0
);
```

#### Orders Tablosu
```sql
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(Id),
    TotalAmount DECIMAL(10,2) NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Pending',
    PaymentStatus NVARCHAR(50) DEFAULT 'Pending',
    ShippingAddress NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);
```

## 🎨 UI/UX Tasarım Sistemi

### Renk Paleti
```css
:root {
  --primary-color: #8b5cf6;      /* Mor - Ana renk */
  --secondary-color: #a855f7;    /* Açık mor */
  --accent-color: #ec4899;       /* Pembe aksan */
  --success-color: #10b981;      /* Yeşil */
  --warning-color: #f59e0b;      /* Turuncu */
  --error-color: #ef4444;        /* Kırmızı */
  --neutral-50: #faf5ff;         /* Açık mor-gri */
  --neutral-900: #1e1b4b;        /* Koyu mor */
}
```

### Ana Sayfa Tasarımı
- **Hero Section**: Trend t-shirt'lerin büyük görselleri
- **Trend Kategorileri**: Renk, model, stil kategorileri
- **Popüler Ürünler**: En çok satan t-shirt'ler
- **Yeni Trendler**: Son eklenen ürünler
- **Soru-Cevap Bölümü**: Sık sorulan sorular

## 🔧 Geliştirme Araçları

### Backend Araçları
- **Visual Studio 2022**: IDE
- **SQL Server Management Studio**: Veritabanı yönetimi
- **Postman**: API testleri
- **Swagger UI**: API dokümantasyonu

### Frontend Araçları
- **Visual Studio Code**: IDE
- **Chrome DevTools**: Debug
- **React Developer Tools**: React debug
- **Redux DevTools**: State debug

### Genel Araçlar
- **Git**: Version control
- **GitHub**: Repository hosting
- **Azure**: Cloud hosting
- **Slack**: Ekip iletişimi

## 📊 MVP Başarı Metrikleri

### Teknik Metrikler
- **Uptime**: %99+ hedef
- **Response Time**: <500ms API
- **Error Rate**: <1%
- **Test Coverage**: >70%

### İş Metrikleri
- **Günlük Ziyaretçi**: 100+ hedef
- **Dönüşüm Oranı**: %2+ hedef
- **Ortalama Sipariş Değeri**: 100+ TL hedef
- **Kullanıcı Memnuniyeti**: 4+ yıldız hedef

## 🚀 Deployment Stratejisi

### Development Environment
- **Local Development**: Her geliştirici kendi makinesinde
- **Git Branches**: Feature branch'ler
- **Code Review**: Pull request'ler

### Staging Environment
- **Test Server**: Azure App Service
- **Test Database**: Azure SQL Database
- **Test Domain**: staging.bamk.com

### Production Environment
- **Production Server**: Azure App Service
- **Production Database**: Azure SQL Database
- **Production Domain**: bamk.com
- **SSL Certificate**: Let's Encrypt

## 📞 İletişim ve Koordinasyon

### Günlük Toplantılar
- **Daily Standup**: Her gün saat 10:00
- **Sprint Planning**: Her 2 haftada bir
- **Retrospective**: Her sprint sonunda

### İletişim Kanalları
- **Slack**: Günlük iletişim
- **GitHub**: Kod inceleme
- **Trello**: Görev takibi
- **Zoom**: Video toplantılar

## 🎯 Sonraki Aşamalar (Post-MVP)

### Faz 2: Gelişmiş Özellikler (3-6 ay)
- [ ] AI destekli trend analizi
- [ ] Mobil uygulama (React Native)
- [ ] Sosyal medya entegrasyonu
- [ ] Gelişmiş arama ve filtreleme
- [ ] Kullanıcı profil sistemi

### Faz 3: Ölçeklendirme (6-12 ay)
- [ ] B2B satıcı paneli
- [ ] API marketplace
- [ ] Uluslararası genişleme
- [ ] Gelişmiş analitik

---

**Başlangıç Tarihi**: Aralık 2024  
**Hedef Bitiş Tarihi**: Şubat 2025  
**Durum**: Planlama Aşamasında

*Bu plan sürekli güncellenmektedir. Proje ilerledikçe yeni özellikler ve güncellemeler eklenecektir.*
