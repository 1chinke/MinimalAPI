using MediatR;
using MinimalAPI.Responses;

namespace MinimalAPI.Mediatr.Queries.PersonQueries;

public record GetPersonById(int Id) : IRequest<PersonResponse>;
