# ScholarBridge 🎓🌉

ScholarBridge, burs arayan başarılı öğrenciler ile burs veren vakıfları (kurumları) ve hayırsever bağışçıları güvenli, şeffaf ve hızlı bir şekilde bir araya getiren modern bir **Burs Yönetim Platformu**dur. 

Bu proje, ASP.NET Core MVC mimarisi kullanılarak geliştirilmiş, dinamik rol yönetimi ve onay mekanizmalarına sahip tam kapsamlı bir web uygulamasıdır.

---

## 🚀 Öne Çıkan Özellikler

Uygulama, gelişmiş bir rol tabanlı yetkilendirme (Role-Based Authorization) sistemiyle yönetilmektedir:

*   **🎓 Öğrenci Modülü:**
    *   Kişisel ve akademik profil oluşturma (GPA, Üniversite, Bölüm vb.).
    *   Gerekli resmi belgeleri (Transkript, Öğrenci Belgesi, Yurt/İkametgah belgesi) sisteme yükleme.
    *   Aktif burs ilanlarını inceleme ve tek tıkla başvuru yapma.
    *   Başvuruların anlık durumunu ("Bekliyor", "Onaylandı", "Reddedildi") takip etme.

*   **🏢 Kurum (Vakıf) Modülü:**
    *   Kurumsal profil oluşturma ve Vergi Numarası ile doğrulama talebi.
    *   Yeni burs ilanları yayınlama ve düzenleme.
    *   Öğrenciler tarafından yapılan başvuruları, yüklenen resmi evrakları inceleme.
    *   Başvuruları onaylama veya reddetme mekanizması.

*   **🤝 Bağışçı Modülü:**
    *   Sisteme hızlı kayıt olma ve profil yönetimi.
    *   Vakıflar ve öğrenciler için bağış süreçlerine katılım altyapısı.

*   **👑 Yönetici (Admin) Modülü:**
    *   Sisteme kaydolan kurumların (vakıfların) güvenilirliğini doğrulamak için **Onaylama / Reddetme** paneli.
    *   Genel sistem istatistikleri ve kullanıcı yönetimi.

---

## 🛠️ Kullanılan Teknolojiler ve Kütüphaneler

*   **Backend:** .NET 8.0 & ASP.NET Core MVC
*   **Database ORM:** Entity Framework Core
*   **Veritabanı:** MS SQL Server
*   **Authentication:** Cookie-Based Authentication (Rol tabanlı yetkilendirme ile)
*   **Frontend:** HTML5, CSS3, Javascript, Bootstrap, Responsive Layout


---

## 📦 Kurulum ve Çalıştırma

Projeyi yerel bilgisayarınızda çalıştırmak için aşağıdaki adımları takip edebilirsiniz:

### 1. Gereksinimler
*   [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   [MS SQL Server & SSMS](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### 2. Projeyi Klonlayın
```bash
git clone https://github.com/buraktk48/ScholarBridge.git
cd ScholarBridge
```

### 3. Veritabanı Bağlantısı (ConnectionString)
`ScholarBridge/appsettings.json` dosyasını açın ve kendi SQL Server bağlantı adresinize göre düzenleyin:
```json
{
  "ConnectionStrings": {
    "baglan": "Server=YOUR_SERVER_NAME;Database=ScholarBridgeDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 4. Projeyi Derleyin ve Çalıştırın
Terminalde proje klasörünün (`ScholarBridge/ScholarBridge`) içerisindeyken şu komutları çalıştırın:

```bash
# Bağımlılıkları geri yükleyin
dotnet restore

# Projeyi derleyin
dotnet build

# Uygulamayı başlatın
dotnet run
```
Uygulama çalıştıktan sonra terminalde belirtilen adresi (örn: `http://localhost:5000` veya `https://localhost:5001`) tarayıcınızda açarak platformu test edebilirsiniz.

---

## 📂 Proje Klasör Yapısı

```text
ScholarBridge/
├── Controllers/          # İstekleri karşılayan ve iş mantığını yöneten sınıflar
├── Models/               # Veritabanı tabloları, EF Core Context ve ViewModel sınıfları
├── Views/                # Arayüz tasarımları (Razor Pages - .cshtml)
│   ├── Account/          # Giriş, Kayıt ekranları
│   ├── Admin/            # Admin yönetim paneli sayfaları
│   ├── Student/          # Öğrenci profil ve başvuru sayfaları
│   ├── Organization/     # Kurum yönetim sayfaları
│   └── Shared/           # Ortak kullanılan Layout şablonları
├── wwwroot/              # CSS, JS, Görsel ve Yüklenen öğrenci belgelerinin tutulduğu klasör
└── Program.cs            # Uygulama başlangıç ayarları ve middleware yapılandırmaları
```

---

