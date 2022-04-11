using MinimalAPI.Responses;
using MediatR;


namespace MinimalAPI.Mediatr.Commands.PersonCommands;

public record DeletePersonCmd(string Id) : IRequest<GenericResponse>;  
