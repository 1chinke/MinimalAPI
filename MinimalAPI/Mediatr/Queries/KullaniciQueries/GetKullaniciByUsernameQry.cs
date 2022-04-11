using MediatR;
using MinimalAPI.Responses;

namespace MinimalAPI.Mediatr.Queries.KullaniciQueries;

public record GetKullaniciByUsernameQry(string Username) : IRequest<KullaniciResponse>;

