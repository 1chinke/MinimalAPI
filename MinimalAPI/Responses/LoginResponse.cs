using MinimalAPI.Models;
using System.Security.Claims;

namespace MinimalAPI.Responses;

public record LoginResponse(Claim[] Claims = null, Kullanici Kullanici = null, string Token = null, int StatusCode = 200, string Error = "");


