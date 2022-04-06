using MinimalAPI.Responses;
using MediatR;
using MinimalAPI.Models;

namespace MinimalAPI.Mediatr.Commands.PersonCommands;

public record InsertPerson(Person Model) : IRequest<GenericResponse>;  
