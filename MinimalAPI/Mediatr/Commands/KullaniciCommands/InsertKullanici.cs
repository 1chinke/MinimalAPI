using MinimalAPI.Models;
using MinimalAPI.Responses;
using MediatR;

namespace MinimalAPI.Mediatr.Commands.KullaniciCommands;

public record InsertKullanici(Kullanici Model) : IRequest<GenericResponse>;
