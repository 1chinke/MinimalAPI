using MinimalAPI.Models;
using System.Net;

namespace MinimalAPI.Responses;

public record PeopleResponse(IEnumerable<Person> Result = null, HttpStatusCode StatusCode = HttpStatusCode.OK, string Error = "");
