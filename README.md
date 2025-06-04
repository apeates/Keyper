# Keyper 🔐

Keyper, ASP.NET Core MVC mimarisi kullanılarak geliştirilen bir **şifre kasası** uygulamasıdır. SPA (Single Page Application) yapısıyla modern ve kullanıcı dostu bir arayüz sunar.

## 📌 Hakkında

Bu proje, kullanıcıların site giriş bilgilerini güvenli şekilde saklayabileceği bir **şifre kasası (password manager)** uygulamasıdır. Geliştirme sürecinde hem **frontend (Bootstrap, jQuery, SPA mimarisi)** hem de **backend (ASP.NET Core API + MVC yapısı)** üzerine odaklandım.  
Projede kullanıcı kimlik doğrulaması, şifreleme, token ile giriş ve veritabanı işlemleri gibi güvenlik odaklı birçok bileşeni entegre ettim.

Amaç, kullanıcıların tüm şifrelerini merkezi bir yerde saklayabileceği modern, responsive ve güvenli bir sistem geliştirmekti.

## 🧩 Özellikler

- Kullanıcı kayıt ve giriş işlemleri (JWT destekli)
- Kayıtlı siteler için şifre yönetimi:
  - Listeleme
  - Yeni şifre ekleme
  - Şifreleri güncelleme
  - Şifre silme
- Şifre göster/gizle toggle
- SPA mimarisi (AJAX + jQuery)
- SQLite veritabanı ile çalışır
- Bootstrap ile responsive tasarım

## 🧱 Teknolojiler

- ASP.NET Core MVC
- Entity Framework Core
- SQLite
- jQuery & AJAX
- Bootstrap 5

## 🔧 Kurulum

### 1. Klonla

```bash
git clone https://github.com/kullaniciadi/Keyper.git
cd Keyper
