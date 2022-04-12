using MinimalAPI.Responses;
using MediatR;

namespace MinimalAPI.Mediatr.Commands.PersonCommands;

public record InsertPersonCmd(string FirstName, string LastName) : IRequest<GenericResponse>;
