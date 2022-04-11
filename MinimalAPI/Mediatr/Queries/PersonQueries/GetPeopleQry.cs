using MinimalAPI.Responses;
using MediatR;

namespace MinimalAPI.Mediatr.Queries.PersonQueries;

public record GetPeopleQry() : IRequest<PeopleResponse>;

