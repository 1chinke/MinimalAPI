using MediatR;
using MinimalAPI.Responses;

namespace MinimalAPI.Mediatr.Queries.KullaniciQueries;

public record GetLogin(string Username, string Password) : IRequest<LoginResponse>;

