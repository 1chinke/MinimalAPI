using MinimalAPI.Models;
using System.Net;

namespace MinimalAPI.Responses;

public record KullaniciResponse(Kullanici Result = null, HttpStatusCode StatusCode = HttpStatusCode.OK, string Error = "");

