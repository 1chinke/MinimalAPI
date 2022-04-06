Minimal API kullanarak Mediatr ile CQRS ve Pipeline Behavior örnek proje

Farklar: Artık controller yok, onun yerine Program.cs içerisinde doğrudan API end pointlerini kodluyoruz. Örnek:

![image](https://user-images.githubusercontent.com/42934024/161954909-72c1127f-881c-40f1-a6d1-e31ff3202c3a.png)

Örnekte görüldüğü gibi mediator dependency injection artık constructor ile değil doğrudan methodun parametresi ile yapılıyor.

Ayrıca bu projede Authentication ve Authorization da kullanılıyor. login olmadan diğer end pointleri kullanmanız mümkün değil.

Test için: (normal kullanıcı) 
Username: norm
Password: norm

ve (admin)

Username: adem
Password: adem

kullanabilirsiniz.

Aşağıdaki paketleri nuget üzerinden yüklemeniz gerekir:

![image](https://user-images.githubusercontent.com/42934024/161954361-cac42e46-d77f-4aab-9c33-6929266e613f.png)

Swagger'a erişmek için URL'in sonuna /swagger/index.html eklenmelidir. Örnek: https://localhost:7068/swagger/index.html
