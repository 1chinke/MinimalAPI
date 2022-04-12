using MinimalAPI.Mediatr.Queries.KullaniciQueries;
using MinimalAPI.Responses;
using MediatR;
using System.Net;
using MinimalAPI.Infrastructure.Repository.Queries;

namespace MinimalAPI.Mediatr.Handlers.KullaniciHandlers;

public class GetPeopleHnd : IRequestHandler<GetAllKullaniciQry, KullanicilarResponse>
{

    private readonly IKullaniciQryRepo _repo;

    public GetPeopleHnd(IKullaniciQryRepo repo)
    {
        _repo = repo;

    }

    public async Task<KullanicilarResponse> Handle(GetAllKullaniciQry request, CancellationToken cancellationToken)
    {

        try
        {
            var result = await _repo.GetAll();
            return new KullanicilarResponse(result);
        }
        catch (Exception ex)
        {
            return new KullanicilarResponse(StatusCode: HttpStatusCode.BadRequest, Error: ex.Message);
        }
    }
}
