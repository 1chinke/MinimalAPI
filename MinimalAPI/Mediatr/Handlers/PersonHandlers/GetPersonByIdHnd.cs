using MediatR;
using MinimalAPI.Mediatr.Queries.PersonQueries;
using MinimalAPI.Infrastructure.Repository;
using MinimalAPI.Responses;
using System.Net;

namespace MinimalAPI.Mediatr.Handlers.PersonHandlers;

public class GetPersonByIdHnd : IRequestHandler<GetPersonById, PersonResponse>
{
    private readonly IPersonRepo _repo;

    public GetPersonByIdHnd(IPersonRepo repo)
    {
        _repo = repo;
    }

    public async Task<PersonResponse> Handle(GetPersonById request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _repo.GetById(request.Id);

            if (result == null)
            {
                return new PersonResponse(StatusCode: HttpStatusCode.NotFound);
            }

            return new PersonResponse(result);
        } catch (Exception ex)
        {
            return new PersonResponse(StatusCode: HttpStatusCode.BadRequest, Error: ex.Message);
        }
        
    }
}
