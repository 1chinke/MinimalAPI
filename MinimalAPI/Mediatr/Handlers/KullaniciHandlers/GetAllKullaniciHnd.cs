using MinimalAPI.Mediatr.Queries.KullaniciQueries;
using MinimalAPI.Infrastructure.Repository;
using MinimalAPI.Responses;
using MediatR;
using System.Net;

namespace MinimalAPI.Mediatr.Handlers.KullaniciHandlers;

public class GetPeopleHnd : IRequestHandler<GetAllKullanici, KullanicilarResponse>
{

    private readonly IKullaniciRepo _repo;

    public GetPeopleHnd(IKullaniciRepo repo)
    {
        _repo = repo;

    }

    public async Task<KullanicilarResponse> Handle(GetAllKullanici request, CancellationToken cancellationToken)
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
