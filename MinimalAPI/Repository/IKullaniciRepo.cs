﻿using MinimalAPI.Models;
using System.Data;

namespace MinimalAPI.Repository;

public interface IKullaniciRepo
{
    Task<int> Delete(string username);
    Task<IEnumerable<Kullanici>> GetAll();
    Task<Kullanici> GetByUsername(String username);
    Task<Kullanici> Login(String username, String password);
    Task<int> Insert(Kullanici model);
    Task<int> Update(string username, Kullanici model);

    IDbConnection GetConnection();
}
