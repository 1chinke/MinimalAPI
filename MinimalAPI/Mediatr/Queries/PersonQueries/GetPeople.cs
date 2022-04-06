using MinimalAPI.Responses;
using MediatR;

namespace MinimalAPI.Mediatr.Queries.PersonQueries;

public record GetPeople() : IRequest<PeopleResponse>;

