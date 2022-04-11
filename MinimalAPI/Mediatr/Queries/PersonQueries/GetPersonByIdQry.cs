using MediatR;
using MinimalAPI.Responses;

namespace MinimalAPI.Mediatr.Queries.PersonQueries;

public record GetPersonByIdQry(string Id) : IRequest<PersonResponse>;
