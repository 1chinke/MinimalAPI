using MinimalAPI.Mediatr.Commands.KullaniciCommands;
using MinimalAPI.Infrastructure.Repository;
using MinimalAPI.Responses;
using MediatR;
using System.Net;

namespace MinimalAPI.Mediatr.Handlers.KullaniciHandlers;

public class DeleteKullaniciHnd : IRequestHandler<DeleteKullanici, GenericResponse>
{

    private readonly IKullaniciRepo _repo;
    private readonly IConnectionManager _connectionManager;

    public DeleteKullaniciHnd(IConnectionManager connectionManager, IKullaniciRepo repo)
    {
        _connectionManager = connectionManager;
        _repo = repo;
    }

    public async Task<GenericResponse> Handle(DeleteKullanici request, CancellationToken cancellationToken)
    {
        
        using var transaction = _connectionManager.GetConnection().BeginTransaction();
        try
        {
            var result = await _repo.Delete(request.Username);
            transaction.Commit();

            if (result == 0)
            {
                return new GenericResponse(StatusCode: HttpStatusCode.BadRequest , Error: "Kayıt silinemedi.");
            }

            return new GenericResponse("Kayıt başarıyla silindi.");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return new GenericResponse(StatusCode: HttpStatusCode.BadRequest, Error: ex.Message);
        }
    }
}
