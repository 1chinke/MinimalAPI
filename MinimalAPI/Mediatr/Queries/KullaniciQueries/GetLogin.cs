using MediatR;
using MinimalAPI.Models;
using MinimalAPI.Responses;

namespace MinimalAPI.Mediatr.Queries.KullaniciQueries;

public record GetLogin(UserLogin Login) : IRequest<LoginResponse>;

