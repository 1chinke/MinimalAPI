using System.Net;

namespace MinimalAPI.Responses;

public record GenericResponse(object Result = null, HttpStatusCode StatusCode = HttpStatusCode.OK, string Error = "");