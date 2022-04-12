using MinimalAPI.Models;

namespace MinimalAPI.Infrastructure.Repository.Commands;

public interface IKullaniciCmdRepo
{
    Task<int> Delete(string username);
    Task<int> Insert(Kullanici model);
    Task<int> Update(string username, Kullanici model);
    Task<int> InsertKullaniciRol(string username, IEnumerable<Rol> roller);
}
