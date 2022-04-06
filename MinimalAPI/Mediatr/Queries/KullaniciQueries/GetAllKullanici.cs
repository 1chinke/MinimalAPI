using MediatR;
using MinimalAPI.Responses;

namespace MinimalAPI.Mediatr.Queries.KullaniciQueries;

public record GetAllKullanici() : IRequest<KullanicilarResponse>;


