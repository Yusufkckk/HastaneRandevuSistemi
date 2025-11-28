# 🏥 Özel Koçak Hastanesi - Randevu Yönetim Sistemi

Özel Koçak Hastanesi Randevu Sistemi, hastaların online randevu almasını kolaylaştıran ve hastane yönetiminin doktor, poliklinik ve randevu süreçlerini dijital ortamda yönetmesini sağlayan kapsamlı bir web projesidir.

## 🛠️ Teknik Altyapı (Tech Stack)

Proje geliştirilirken **MVC Mimarisi** yapısına sadık kalınmış, veri bütünlüğü ve güvenlik ön planda tutulmuştur.

* **Backend:** C#, ASP.NET Core 8.0 MVC
* **Veritabanı:** MSSQL Server
* **ORM:** Entity Framework Core (Code First Yaklaşımı)
* **Frontend:** HTML5, CSS3, Bootstrap 5, JavaScript (jQuery & AJAX)
* **Güvenlik:** SHA-256 Hashing, Session Yönetimi, CSRF Koruması

## 💻 Temel Özellikler

Sistem **Yönetici (Admin)** ve **Hasta** olmak üzere 2 temel rol üzerinden kurgulanmıştır.

### 1. Yönetim Paneli (Admin Dashboard)
Hastane yöneticisinin kuş bakışı tüm sistemi izlediği ve yönettiği alandır.

* 📊 **Dashboard:** Toplam doktor, hasta, randevu ve departman istatistiklerinin grafiksel özeti.
* 👨‍⚕️ **Doktor Yönetimi:** Doktor ekleme, düzenleme, silme ve **fotoğraf yükleme** işlemleri.
* 🏥 **Departman (Poliklinik) Yönetimi:** Yeni bölüm açma ve düzenleme işlemleri.
* 🕒 **Takvim & Çalışma Saati:** Doktorların hangi gün ve saatlerde çalıştığının ayarlanması.
* 📢 **Duyuru Yönetimi:** Ana sayfada görünen duyuruların eklenip kaldırılması.
* 📅 **Randevu Takibi:** Sistemdeki tüm randevuları listeleme ve gerektiğinde iptal etme yetkisi.

### 2. Hasta Modülü
Hastaların sisteme üye olup randevu süreçlerini yönettiği alandır.

* 🔐 **Güvenli Giriş & Kayıt:** SHA-256 şifreleme algoritması ile güvenli üyelik.
* 📅 **Akıllı Randevu Alma:**
    * Poliklinik seçimine göre **AJAX** ile sayfa yenilenmeden doktor listeleme.
    * Geçmiş tarihe veya doktorun çalışmadığı saate randevu almayı engelleyen validasyonlar.
    * Çakışan randevuların otomatik kontrolü.
* ❌ **Randevularım:** Aktif ve geçmiş randevuları görüntüleme; ileri tarihli randevuları iptal edebilme.
* 👤 **Profil Yönetimi:** Şifre ve iletişim bilgilerini güncelleme.

### 3. Genel Özellikler (Arayüz)
* 🎨 **Modern Tasarım:** Bootstrap 5 ile geliştirilmiş, mobil uyumlu (responsive) arayüz.
* 🖼️ **Dinamik İçerik:** Doktorlar sayfasında veritabanından çekilen görseller (Yüklü değilse cinsiyete göre otomatik avatar atama).
* 🏠 **Kurumsal Sayfalar:** Hakkımızda, Doktorlarımız ve İletişim bölümleri.

---

## 📷 Ekran Görüntüleri

Projenin arayüzünden bazı kareler:

### 1. Ana Sayfa
Kullanıcı dostu arayüz ile hastane duyurularının ve hızlı işlem menülerinin sunumu.
<img width="1907" height="913" alt="Image" src="https://github.com/user-attachments/assets/4c232c12-0788-4654-962c-0119c072de61" />

### 2. Hakkımızda
<img width="1909" height="892" alt="Image" src="https://github.com/user-attachments/assets/76aed874-db05-413b-aab0-f5548ca0b24e" />

### 3. Randevu Alma Ekranı (Dinamik Filtreleme)
Hastanın doktor ve saat seçimi yaparak onayladığı ekran.
<img width="1904" height="896" alt="Image" src="https://github.com/user-attachments/assets/62acf10c-f026-41ea-948b-4edc709380d5" />

### 4. Hasta Randevularım Paneli
Hastanın geçmiş ve gelecek randevularını yönetebildiği, iptal işlemi yapabildiği ekran.
<img width="1919" height="911" alt="Image" src="https://github.com/user-attachments/assets/89a898c3-b66b-4d51-878d-86e228d99b87" />

### 5. Admin Dashboard
Yöneticilerin sistemi tam kontrolle yönetebildiği, sade ve anlaşılır yönetim paneli.
<img width="1906" height="899" alt="Image" src="https://github.com/user-attachments/assets/14a87c6f-f4e6-4f76-b7f9-b04ec6b5c9b1" />

### 6. Doktor Yönetimi ve Ekleme Formu
<img width="1920" height="914" alt="Image" src="https://github.com/user-attachments/assets/21e7a389-3f87-461b-b005-2842cbb8d03b" />

<img width="1923" height="914" alt="Image" src="https://github.com/user-attachments/assets/ae49c08d-4c7b-4cfe-8986-87af5f463641" />

### 7. Doktorlarımız
Hastanemizde görev yapan uzmanların listelendiği dinamik kart yapısı.
<img width="1921" height="914" alt="Image" src="https://github.com/user-attachments/assets/2cfa1632-1e6e-4926-8414-52f1ec9c43f8" />
