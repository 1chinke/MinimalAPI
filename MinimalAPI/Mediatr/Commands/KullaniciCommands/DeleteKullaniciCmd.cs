using MinimalAPI.Responses;
using MediatR;

namespace MinimalAPI.Mediatr.Commands.KullaniciCommands;

public record DeleteKullaniciCmd(string Username) : IRequest<GenericResponse>;

