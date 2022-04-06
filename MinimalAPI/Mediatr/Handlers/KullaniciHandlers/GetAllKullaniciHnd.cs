using MinimalAPI.Mediatr.Queries.KullaniciQueries;
using MinimalAPI.Repository;
using MinimalAPI.Responses;
using MediatR;

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
            return new KullanicilarResponse(StatusCode: 400, Error: ex.Message);
        }
    }
}
