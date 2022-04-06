using MinimalAPI.Models;

namespace MinimalAPI.Responses;

public record PersonResponse(Person Result = null, int StatusCode = 200, string Error = "");
