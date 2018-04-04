
# Book Store E-commerce / Multi suppliers
Merhabalar,
Eski bir projeyi belki birilerine lazım olur diyerekten paylaştim. 
Genel hatlarıyla çalışan bir proje ama tabiki bi sürü eksik ve hata var :)

Detaylı proje bilgisi için: https://github.com/WowSyler/Book-Store-E-commerce-Multi-suppliers/blob/master/doc.pdf

## Ana sayfa 
http://gobookstore.ozankck.com
## Suppliers Link
* Kayit
http://gobookstore.ozankck.com/Suppliers/Register
* Giriş
http://gobookstore.ozankck.com/Suppliers/LoginSupp

## Admin link
http://gobookstore.ozankck.com/admin

## ekran görüntüleri
* [AnaSayfa](https://i.hizliresim.com/W73MAY.png)
* [Satıcı panel görüntüsü](https://i.hizliresim.com/Rnz7L1.png)
* [Admin panel görüntüsü](https://i.hizliresim.com/z0oPvY.png)
* [Tek satıcılı bir kitap](https://i.hizliresim.com/G9M8Q2.png)
* [Birden fazla Satıcılı bir kitap](https://i.hizliresim.com/6JWA7P.png)


## Database baglantı
* Proje içindeki "Web.config" dosyasının içinde kendi database bilgilerinizle yapılandırma yapmanız gerek. 
* Tabi ilk önce "SQL 2014 Database Table settings..." dosyasındaki sql kodlarını databasenizde calıştırmanız gerek.
* databaseip, userid, pass
* <code> add name="bookStoreDBEntities" connectionString="metadata=res://*/Models.Model1.csdl|res://*/Models.Model1.ssdl|res://*/Models.Model1.msl;
       provider=System.Data.SqlClient;provider connection string=&quot;
       data source=databaseip;
       initial catalog=newbookStoreDB;
       persist security info=True;
       user id=userid;
       password=pass;
       multipleactiveresultsets=True;
       application name=EntityFramework&quot;" providerName="System.Data.EntityClient" </code>
