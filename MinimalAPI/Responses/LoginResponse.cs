using MinimalAPI.Models;
using System.Net;
using System.Security.Claims;

namespace MinimalAPI.Responses;

public record LoginResponse(Claim[] Claims = null, Kullanici Kullanici = null, string Token = null, HttpStatusCode StatusCode = HttpStatusCode.OK, string Error = "");


