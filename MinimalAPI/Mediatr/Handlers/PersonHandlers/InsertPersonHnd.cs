using MinimalAPI.Mediatr.Commands.PersonCommands;
using MinimalAPI.Infrastructure.Database;
using MinimalAPI.Responses;
using MediatR;
using System.Net;
using MinimalAPI.Models;
using MinimalAPI.Infrastructure.Repository.Commands;

namespace MinimalAPI.Mediatr.Handlers.PersonHandlers;

public class InsertPersonHnd : IRequestHandler<InsertPersonCmd, GenericResponse>
{
    private readonly IConnectionManager _connectionManager;
    private readonly IPersonCmdRepo _repo;

    public InsertPersonHnd(IConnectionManager connectionManager, IPersonCmdRepo repo)
    {
        _connectionManager = connectionManager;
        _repo = repo;
    }

    public async Task<GenericResponse> Handle(InsertPersonCmd request, CancellationToken cancellationToken)
    { 
        try
        {
            using var transaction = _connectionManager.GetConnection().BeginTransaction();
            try
            {
                Person model = new()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName
                };
             
                var result = await _repo.Insert(model);

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
