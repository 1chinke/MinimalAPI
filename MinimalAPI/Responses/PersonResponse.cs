using MinimalAPI.Models;
using System.Net;

namespace MinimalAPI.Responses;

public record PersonResponse(Person Result = null, HttpStatusCode StatusCode = HttpStatusCode.OK, string Error = "");
