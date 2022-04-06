using MediatR;
using MinimalAPI.Responses;

namespace MinimalAPI.Mediatr.Queries.KullaniciQueries;

public record GetKullaniciByUsername(string Username) : IRequest<KullaniciResponse>;

