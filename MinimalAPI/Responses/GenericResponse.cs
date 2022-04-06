namespace MinimalAPI.Responses;

public record GenericResponse(object Result = null, int StatusCode = 200, string Error = "");