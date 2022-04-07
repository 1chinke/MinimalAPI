using MinimalAPI.Mediatr.Commands.KullaniciCommands;
using MinimalAPI.Infrastructure.Repository;
using MinimalAPI.Responses;
using MediatR;
using System.Security.Cryptography;
using System.Text;
using System.Net;

namespace MinimalAPI.Mediatr.Handlers.KullaniciHandlers;

public class InsertKullaniciHnd : IRequestHandler<InsertKullanici, GenericResponse>
{
    private readonly IConnectionManager _connectioManager;
    private readonly IKullaniciRepo _repo;
    private readonly IConfiguration _config;

    public InsertKullaniciHnd(IConnectionManager connectioManager, IKullaniciRepo repo, IConfiguration config)
    {
        _connectioManager = connectioManager;
        _repo = repo;
        _config = config;
    }

    public async Task<GenericResponse> Handle(InsertKullanici request, CancellationToken cancellationToken)
    {
        try
        {
            using var transaction = _connectioManager.GetConnection().BeginTransaction();
            try
            {
                request.Model.Password = CreatePasswordHash(request.Model.Password);

                var result = await _repo.Insert(request.Model);                

                if (result == 0)
                {
                    transaction.Rollback();
                    return new GenericResponse(StatusCode: HttpStatusCode.BadRequest, Error: "Kaydedilemedi.");
                }

                transaction.Commit();
                return new GenericResponse("Başarıyla kaydedildi.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new GenericResponse(StatusCode: HttpStatusCode.BadRequest, Error: ex.Message);
            }
        }
        catch (Exception ex)
        {
            return new GenericResponse(StatusCode: HttpStatusCode.BadRequest, Error: ex.Message);
        }

    }

    private string CreatePasswordHash(string password)
    {        
        using var hmac = new HMACSHA512();
        hmac.Key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);


        //return Encoding.UTF8.GetString(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
}
