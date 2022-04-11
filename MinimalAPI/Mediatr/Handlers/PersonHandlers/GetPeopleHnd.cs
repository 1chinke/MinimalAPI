using MinimalAPI.Mediatr.Queries.PersonQueries;
using MinimalAPI.Infrastructure.Database;
using MinimalAPI.Responses;
using MediatR;
using System.Net;

namespace MinimalAPI.Mediatr.Handlers.PersonHandlers;

public class GetPeopleHnd : IRequestHandler<GetPeopleQry, PeopleResponse>
{

    private readonly IPersonRepo _repo;

    public GetPeopleHnd(IPersonRepo repo)
    {
        _repo = repo;

    }

    public async Task<PeopleResponse> Handle(GetPeopleQry request, CancellationToken cancellationToken)
    {
       
        try
        {
            var result = await _repo.GetAll();
            return new PeopleResponse(result);
        }
        catch (Exception ex)
        {
            return new PeopleResponse(StatusCode: HttpStatusCode.BadRequest, Error: ex.Message);
        }
    }
}
