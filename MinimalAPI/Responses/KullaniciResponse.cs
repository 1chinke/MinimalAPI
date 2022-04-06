using MinimalAPI.Models;

namespace MinimalAPI.Responses;

public record KullaniciResponse(Kullanici Result = null, int StatusCode = 200, string Error = "");

