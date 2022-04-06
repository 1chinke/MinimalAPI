using MinimalAPI.Mediatr.Commands.PersonCommands;
using MinimalAPI.Repository;
using MinimalAPI.Responses;
using MediatR;


namespace MinimalAPI.Mediatr.Handlers.PersonHandlers;

public class UpdatePersonelHnd : IRequestHandler<UpdatePerson, GenericResponse>
{

    private readonly IPersonRepo _repo;

    public UpdatePersonelHnd(IPersonRepo repo)
    {
        _repo = repo;
    }

    public async Task<GenericResponse> Handle(UpdatePerson request, CancellationToken cancellationToken)
    {
        using var transaction = _repo.GetConnection().BeginTransaction();
        try
        {
            var result = await _repo.Update(request.Id, request.Model);

            if (result == 0)
            {
                transaction.Rollback();
                return new GenericResponse(StatusCode: 404);
            }

            transaction.Commit();

            return new GenericResponse("Güncelleme yapıldı.");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return new GenericResponse(StatusCode: 400, Error: ex.Message);
        }
    }
}
