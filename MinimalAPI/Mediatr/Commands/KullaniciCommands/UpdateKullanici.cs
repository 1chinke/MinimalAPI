using MinimalAPI.Models;
using MinimalAPI.Responses;
using MediatR;

namespace MinimalAPI.Mediatr.Commands.KullaniciCommands;

public record UpdateKullanici(string Username, Kullanici Model) : IRequest<GenericResponse>;

