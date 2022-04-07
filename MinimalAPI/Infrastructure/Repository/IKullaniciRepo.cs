using MinimalAPI.Models;
using System.Data;

namespace MinimalAPI.Infrastructure.Database;

public interface IKullaniciRepo
{
    Task<int> Delete(string username);
    Task<IEnumerable<Kullanici>> GetAll();
    Task<Kullanici> GetByUsername(String username);
    Task<Kullanici> Login(String username, String password);
    Task<int> Insert(Kullanici model);
    Task<int> Update(string username, Kullanici model);
    Task<int> InsertKullaniciRol(string username, IEnumerable<Rol> roller);
    Task<IEnumerable<Rol>> GetKullaniciRoles(string username);
    
}
