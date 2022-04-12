using MinimalAPI.Mediatr.Commands.PersonCommands;
using MinimalAPI.Infrastructure.Database;
using MinimalAPI.Responses;
using MediatR;
using System.Net;
using MinimalAPI.Infrastructure.Repository.Commands;

namespace MinimalAPI.Mediatr.Handlers.PersonHandlers;

public class UpdatePersonelHnd : IRequestHandler<UpdatePersonCmd, GenericResponse>
{
    private readonly IConnectionManager _connectionManager;
    private readonly IPersonCmdRepo _repo;

    public UpdatePersonelHnd(IConnectionManager connectionManager, IPersonCmdRepo repo)
    {
        _connectionManager = connectionManager;
        _repo = repo;
    }

    public async Task<GenericResponse> Handle(UpdatePersonCmd request, CancellationToken cancellationToken)
    {
        using var transaction = _connectionManager.GetConnection().BeginTransaction();
        try
        {
            var result = await _repo.Update(request.Model);

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
