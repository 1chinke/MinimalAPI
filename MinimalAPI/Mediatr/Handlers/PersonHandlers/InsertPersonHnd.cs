using MinimalAPI.Mediatr.Commands.PersonCommands;
using MinimalAPI.Repository;
using MinimalAPI.Responses;
using MediatR;

namespace MinimalAPI.Mediatr.Handlers.PersonHandlers;

public class InsertPersonHnd : IRequestHandler<InsertPerson, GenericResponse>
{
    private readonly IPersonRepo _repo;

    public InsertPersonHnd(IPersonRepo repo)
    {
        _repo = repo;
    }

    public async Task<GenericResponse> Handle(InsertPerson request, CancellationToken cancellationToken)
    { 
        try
        {
            using var transaction = _repo.GetConnection().BeginTransaction();
            try
            {
                var result = await _repo.Insert(request.Model.Id, request.Model.FirstName, request.Model.LastName);
                transaction.Commit();

                if (result == 0)
                {
                    return new GenericResponse(StatusCode: 400, Error: "Kaydedilemedi.");
                }


                return new GenericResponse("Başarıyla kaydedildi.");
            }

            catch (Exception ex)
            {
                transaction.Rollback();
                return new GenericResponse(StatusCode: 400, Error: ex.Message);
            }
        }
        catch (Exception ex)
        {
            return new GenericResponse(StatusCode: 400, Error: ex.Message);
        }

    }
}
