# Toplantı Randevu Sistemi (TRS 1.1)

C# Windows Forms ve .NET Framework 4.7.2 ile geliştirilmiş masaüstü toplantı/randevu yönetimi uygulaması.

## Özellikler
- Yönetici ve kullanıcı girişi
- Toplantı oluşturma ve katılımcı belirleme
- Oluşturulan toplantıları listeleme
- Toplantıları düzenleme ve silme
- Kullanıcı profil/bildirim alanları

## Gereksinimler
- Windows
- Visual Studio 2019/2022 ( .NET desktop development iş yükü)
- .NET Framework 4.7.2 Developer Pack
- SQL Server (LocalDB veya SQL Server Express dahil)

## Kurulum
1. Depoyu klonlayın ve `TRS 1.1.sln` dosyasını Visual Studio ile açın.
2. SQL Server üzerinde `database/schema.sql` dosyasını çalıştırın.
3. `TRS 1.1/App.config` içindeki `Data Source=localhost` değerini kendi SQL Server örneğinize göre değiştirin (ör. `localhost\SQLEXPRESS`).
4. Projeyi derleyip çalıştırın.
5. Yönetici/kullanıcı kayıtlarını kendi veritabanınızda oluşturun. Depoda varsayılan parola veya gerçek kullanıcı verisi bulunmaz.

> Not: Uygulamanın mevcut kodu `admin`, `users` ve `top_olustur` tablolarını bekliyor. Sağlanan SQL dosyası kaynak arşivindeki şema tanımlarının temizlenmiş halidir; proje kaynak kodundaki tüm veritabanı gereksinimleriyle birebir doğrulanmış bir migration paketi değildir. İlk çalıştırmadan önce tabloları/alanları kontrol edin.

## Teknoloji
- C#
- Windows Forms
- .NET Framework 4.7.2
- ADO.NET / SQL Server

## Arayüz stili
Uygulama web uygulaması değildir; CSS dosyası kullanmaz. Arayüz Windows Forms Designer dosyalarında (`*.Designer.cs`) tanımlanır. Ana renkler koyu gri `#242424` ve turuncu `#E75E38`; yazı tipi ağırlıklı olarak Arial'dır.

## Ekran görüntüleri (kaynak koduna dayalı önizleme)

Aşağıdaki görseller, ilgili Windows Forms `*.Designer.cs` dosyalarındaki form ölçüleri, kontrol koordinatları, metinler, renkler ve mevcut logo kaynağı esas alınarak hazırlanmıştır. Bunlar Windows'ta çalışan uygulamadan alınmış gerçek ekran görüntüleri değildir; bazı işletim sistemi kontrollerinin (özellikle DataGridView ve standart giriş alanları) görünümü yaklaşık olarak çizilmiştir.

- [Tüm ekranlar](screenshots/00-tum-ekranlar.png)
- [Giriş ekranı](screenshots/01-giris-ekrani.png)
- [Kullanıcı ana sayfası](screenshots/02-kullanici-anasayfa.png)
- [Yönetici ana sayfası](screenshots/03-yonetici-anasayfa.png)
- [Toplantı oluşturma](screenshots/04-toplanti-olusturma.png)
- [Toplantı düzenleme](screenshots/05-toplanti-duzenleme.png)
- [Giriş ekranındaki orijinal logo kaynağı](screenshots/form1-logo-resource.png)
