using MediatR;
using MinimalAPI.Models;
using MinimalAPI.Responses;

namespace MinimalAPI.Mediatr.Queries.KullaniciQueries;

public record GetLoginQry(UserLogin Login) : IRequest<LoginResponse>;

