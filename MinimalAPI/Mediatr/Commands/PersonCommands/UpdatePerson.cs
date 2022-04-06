using MinimalAPI.Models;
using MinimalAPI.Responses;
using MediatR;

namespace MinimalAPI.Mediatr.Commands.PersonCommands;
public record UpdatePerson (int Id, Person Model) : IRequest<GenericResponse>;
