using MinimalAPI.Mediatr.Queries.KullaniciQueries;
using MinimalAPI.Infrastructure.Database;
using MinimalAPI.Responses;
using MediatR;
using System.Net;

namespace MinimalAPI.Mediatr.Handlers.KullaniciHandlers;

public class GetPeopleHnd : IRequestHandler<GetAllKullaniciQry, KullanicilarResponse>
{

    private readonly IKullaniciRepo _repo;

    public GetPeopleHnd(IKullaniciRepo repo)
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
