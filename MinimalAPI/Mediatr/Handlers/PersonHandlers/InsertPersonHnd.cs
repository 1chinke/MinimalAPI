using MinimalAPI.Mediatr.Commands.PersonCommands;
using MinimalAPI.Infrastructure.Database;
using MinimalAPI.Responses;
using MediatR;
using System.Net;

namespace MinimalAPI.Mediatr.Handlers.PersonHandlers;

public class InsertPersonHnd : IRequestHandler<InsertPerson, GenericResponse>
{
    private readonly IConnectionManager _connectionManager;
    private readonly IPersonRepo _repo;

    public InsertPersonHnd(IConnectionManager connectionManager, IPersonRepo repo)
    {
        _connectionManager = connectionManager;
        _repo = repo;
    }

    public async Task<GenericResponse> Handle(InsertPerson request, CancellationToken cancellationToken)
    { 
        try
        {
            using var transaction = _connectionManager.GetConnection().BeginTransaction();
            try
            {
                var result = await _repo.Insert(request.Model.Id, request.Model.FirstName, request.Model.LastName);
                transaction.Commit();

                if (result == 0)
                {
                    return new GenericResponse(StatusCode: HttpStatusCode.BadRequest, Error: "Kaydedilemedi.");
                }


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
}
