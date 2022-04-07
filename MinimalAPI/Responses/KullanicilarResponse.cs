using MinimalAPI.Models;
using System.Net;

namespace MinimalAPI.Responses;

public record KullanicilarResponse(IEnumerable<Kullanici> Result = null, HttpStatusCode StatusCode = HttpStatusCode.OK, string Error = "");

