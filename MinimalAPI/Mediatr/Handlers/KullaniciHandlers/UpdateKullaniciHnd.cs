using MinimalAPI.Mediatr.Commands.KullaniciCommands;
using MinimalAPI.Infrastructure.Database;
using MinimalAPI.Responses;
using MediatR;
using System.Net;

namespace MinimalAPI.Mediatr.Handlers.KullaniciHandlers;

public class UpdateKullanicielHnd : IRequestHandler<UpdateKullanici, GenericResponse>
{

    private readonly IKullaniciRepo _repo;
    private readonly IConnectionManager _connectionManager;

    public UpdateKullanicielHnd(IConnectionManager connectionManager, IKullaniciRepo repo)
    {
        _connectionManager = connectionManager;
        _repo = repo;
    }

    public async Task<GenericResponse> Handle(UpdateKullanici request, CancellationToken cancellationToken)
    {
        using var transaction = _connectionManager.GetConnection().BeginTransaction();
        try
        {
            var result = await _repo.Update(request.Username, request.Model);

            if (result == 0)
            {
                transaction.Rollback();
                return new GenericResponse(StatusCode: HttpStatusCode.NotFound);
            }

            transaction.Commit();

            return new GenericResponse("Güncelleme yapıldı.");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return new GenericResponse(StatusCode: HttpStatusCode.BadRequest, Error: ex.Message);
        }
    }
}
