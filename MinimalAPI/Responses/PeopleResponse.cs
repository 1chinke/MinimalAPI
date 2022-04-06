using MinimalAPI.Models;

namespace MinimalAPI.Responses;

public record PeopleResponse(IEnumerable<Person> Result = null, int StatusCode = 200, string Error = "");
