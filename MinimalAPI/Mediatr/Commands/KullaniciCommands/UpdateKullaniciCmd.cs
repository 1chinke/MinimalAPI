using MinimalAPI.Models;
using MinimalAPI.Responses;
using MediatR;

namespace MinimalAPI.Mediatr.Commands.KullaniciCommands;

public record UpdateKullaniciCmd(string Username, Kullanici Model) : IRequest<GenericResponse>;

