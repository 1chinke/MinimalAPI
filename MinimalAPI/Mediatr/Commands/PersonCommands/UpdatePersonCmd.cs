using MinimalAPI.Models;
using MinimalAPI.Responses;
using MediatR;

namespace MinimalAPI.Mediatr.Commands.PersonCommands;
public record UpdatePersonCmd (Person Model) : IRequest<GenericResponse>;
