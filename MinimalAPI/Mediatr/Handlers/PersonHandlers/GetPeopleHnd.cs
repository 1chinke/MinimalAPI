using MinimalAPI.Mediatr.Queries.PersonQueries;
using MinimalAPI.Responses;
using MediatR;
using System.Net;
using MinimalAPI.Infrastructure.Repository.Queries;

namespace MinimalAPI.Mediatr.Handlers.PersonHandlers;

public class GetPeopleHnd : IRequestHandler<GetPeopleQry, PeopleResponse>
{

    private readonly IPersonQryRepo _repo;

    public GetPeopleHnd(IPersonQryRepo repo)
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
