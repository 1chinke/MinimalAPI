using MinimalAPI.Responses;
using MediatR;

namespace MinimalAPI.Mediatr.Commands.KullaniciCommands;

public record DeleteKullanici(string Username) : IRequest<GenericResponse>;

