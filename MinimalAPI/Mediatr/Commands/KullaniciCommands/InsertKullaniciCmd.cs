using MinimalAPI.Models;
using MinimalAPI.Responses;
using MediatR;

namespace MinimalAPI.Mediatr.Commands.KullaniciCommands;

public record InsertKullaniciCmd(Kullanici Model) : IRequest<GenericResponse>;
