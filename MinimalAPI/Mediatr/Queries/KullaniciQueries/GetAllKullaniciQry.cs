using MediatR;
using MinimalAPI.Responses;

namespace MinimalAPI.Mediatr.Queries.KullaniciQueries;

public record GetAllKullaniciQry() : IRequest<KullanicilarResponse>;


