Minimal API kullanarak Mediatr ile CQRS ve Pipeline Behavior örnek proje

Farklar: Artık controller yok, onun yerine Program.cs içerisinde doğrudan API end pointlerini kodluyoruz. Örnek:

![image](https://user-images.githubusercontent.com/42934024/161954909-72c1127f-881c-40f1-a6d1-e31ff3202c3a.png)

Örnekte görüldüğü gibi mediator dependency injection artık constructor ile değil doğrudan methodun parametresi ile yapılıyor.

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

![image](https://user-images.githubusercontent.com/42934024/162229555-aca78cac-cbf8-4f53-b2b4-b32cecfa7f8d.png)


Swagger'a erişmek için URL'in sonuna /swagger/index.html eklenmelidir. Örnek: https://localhost:7068/swagger/index.html
