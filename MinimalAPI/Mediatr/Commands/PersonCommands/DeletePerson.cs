using MinimalAPI.Responses;
using MediatR;


namespace MinimalAPI.Mediatr.Commands.PersonCommands;

public record DeletePerson(int Id) : IRequest<GenericResponse>;  
