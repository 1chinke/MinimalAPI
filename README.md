Minimal API kullanarak Mediatr ile CQRS ve Pipeline Behavior örnek proje

Farklar: Artık controller yok, onun yerine Program.cs içerisinde doğrudan API end pointlerini kodluyoruz. Örnek:

![image](https://user-images.githubusercontent.com/42934024/161954909-72c1127f-881c-40f1-a6d1-e31ff3202c3a.png)

Örnekte görüldüğü gibi mediator dependency injection artık constructor ile değil doğrudan methodun parametresi ile yapılıyor.

Uygulamayı çalıştırabilmek için bir lokal bir redis sunucusuna ihtiyaç var. Bunun için ya redisi bilgisayarınıza kurmanız ya da docker image'i indirip çalıştımanız gerekiyor. Benim önerim docker image'i kullanmanız. Docker image için aşağıdaki komutu çalıştırabilirsiniz:

docker run -d --restart unless-stopped --name cache-redis -p 6379:6379 redis redis-server

Ayrıca bu projede Authentication ve Authorization da kullanılıyor. login olmadan diğer end pointleri kullanmanız mümkün değil.

Login den dönen token kopyalanarak swagger'daki "Authorize" kısmına yapıştırılmalı. Ancak tokenın başındaki ve sonundaki " (çift tırnak) karakterleri silinmeli aksi takdirde invalid token hatası alırsınız.

Postman'de Authorization'da "Bearer Token" seçilip, Token kısmına yine çift tırnak olmadan yapıştırılmalı

Test için: (normal kullanıcı) 
Username: norm
Password: norm

ve (admin)

Username: adem
Password: adem

kullanabilirsiniz.

Aşağıdaki paketleri nuget üzerinden yüklemeniz gerekir:

![image](https://user-images.githubusercontent.com/42934024/162677865-dfdce8db-fac3-47eb-a7fd-d31a8940d78d.png)




TODO: (veyahut Ödev :))
Kullanıcı rollerinin çoklu bir şekilde girilmesi
