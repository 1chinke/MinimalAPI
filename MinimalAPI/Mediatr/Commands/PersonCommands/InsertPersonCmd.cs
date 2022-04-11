using MinimalAPI.Responses;
using MediatR;
using MinimalAPI.Models;

namespace MinimalAPI.Mediatr.Commands.PersonCommands;

public record InsertPersonCmd(string FirstName, string LastName) : IRequest<GenericResponse>;
