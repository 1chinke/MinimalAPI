using MinimalAPI.Mediatr.Commands.PersonCommands;
using MinimalAPI.Infrastructure.Database;
using MinimalAPI.Responses;
using MediatR;
using System.Net;

namespace MinimalAPI.Mediatr.Handlers.PersonHandlers;

public class DeletePersonHnd : IRequestHandler<DeletePerson, GenericResponse>
{

    private readonly IConnectionManager _connectionManager;
    private readonly IPersonRepo _repo;

    public DeletePersonHnd(IConnectionManager connectionManager, IPersonRepo repo)
    {
        _connectionManager = connectionManager;
        _repo = repo;

    }

    public async Task<GenericResponse> Handle(DeletePerson request, CancellationToken cancellationToken)
    {
        using var transaction = _connectionManager.GetConnection().BeginTransaction();
        try
        {
            var result = await _repo.Delete(request.Id);
            transaction.Commit();

            if (result == 0)
            {
                return new GenericResponse(StatusCode: HttpStatusCode.BadRequest, Error: "Kayıt silinemedi.");
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
