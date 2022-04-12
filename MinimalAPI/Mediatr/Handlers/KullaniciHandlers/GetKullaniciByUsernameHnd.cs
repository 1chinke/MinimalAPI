using MediatR;
using MinimalAPI.Mediatr.Queries.KullaniciQueries;
using MinimalAPI.Responses;
using System.Net;
using MinimalAPI.Infrastructure.Repository.Queries;

namespace MinimalAPI.Mediatr.Handlers.KullaniciHandlers;

public class GetKullaniciByUsernameHnd : IRequestHandler<GetKullaniciByUsernameQry, KullaniciResponse>
{
    private readonly IKullaniciQryRepo _repo;

    public GetKullaniciByUsernameHnd(IKullaniciQryRepo repo)
    {
        _repo = repo;
    }

    public async Task<KullaniciResponse> Handle(GetKullaniciByUsernameQry request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _repo.GetByUsername(request.Username);

            if (result == null)
            {
                return new KullaniciResponse(StatusCode: HttpStatusCode.NotFound);
            }

            return new KullaniciResponse(result);
        }
        catch (Exception ex)
        {
            return new KullaniciResponse(StatusCode: HttpStatusCode.BadRequest, Error: ex.Message);
        }

    }
}
