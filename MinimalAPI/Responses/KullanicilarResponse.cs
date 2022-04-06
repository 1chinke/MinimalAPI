using MinimalAPI.Models;

namespace MinimalAPI.Responses;

public record KullanicilarResponse(IEnumerable<Kullanici> Result = null, int StatusCode = 200, string Error = "");

