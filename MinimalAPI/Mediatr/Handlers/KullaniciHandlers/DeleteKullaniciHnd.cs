using MinimalAPI.Mediatr.Commands.KullaniciCommands;
using MinimalAPI.Infrastructure.Database;
using MinimalAPI.Responses;
using MediatR;
using System.Net;
using MinimalAPI.Infrastructure.Repository.Commands;

namespace MinimalAPI.Mediatr.Handlers.KullaniciHandlers;

public class DeleteKullaniciHnd : IRequestHandler<DeleteKullaniciCmd, GenericResponse>
{

    private readonly IKullaniciCmdRepo _repo;
    private readonly IConnectionManager _connectionManager;

    public DeleteKullaniciHnd(IConnectionManager connectionManager, IKullaniciCmdRepo repo)
    {
        _connectionManager = connectionManager;
        _repo = repo;
    }

    public async Task<GenericResponse> Handle(DeleteKullaniciCmd request, CancellationToken cancellationToken)
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
