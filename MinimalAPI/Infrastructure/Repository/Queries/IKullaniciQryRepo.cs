using MinimalAPI.Models;

namespace MinimalAPI.Infrastructure.Repository.Queries;

public interface IKullaniciQryRepo
{
    Task<IEnumerable<Kullanici>> GetAll();
    Task<Kullanici> GetByUsername(String username);
    Task<Kullanici> Login(String username, String password);   
    Task<IEnumerable<Rol>> GetKullaniciRoles(string username);
    
}
