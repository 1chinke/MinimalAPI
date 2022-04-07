using MediatR;
using MinimalAPI.Mediatr.Queries.KullaniciQueries;
using MinimalAPI.Infrastructure.Repository;
using MinimalAPI.Responses;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;
using System.Net;

namespace MinimalAPI.Mediatr.Handlers.KullaniciHandlers;

public class GetLoginHnd : IRequestHandler<GetLogin, LoginResponse>
{
    private readonly IKullaniciRepo _repo;
    private readonly IConfiguration _config;

    public GetLoginHnd(IKullaniciRepo repo, IConfiguration config)
    {
        _repo = repo;
        _config = config;
    }

    public async Task<LoginResponse> Handle(GetLogin request, CancellationToken cancellationToken)
    {
        try
        {
            var passwordHash = CreatePasswordHash(request.Login.Password);
            var kullanici = await _repo.Login(request.Login.Username, passwordHash);

            if (kullanici == null)
            {
                return new LoginResponse(StatusCode: HttpStatusCode.NotFound, Error: "Kullanıcı ya da parola hatalı");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, kullanici.Username),
                //new Claim(ClaimTypes.Role, kullanici.Role),
                new Claim(ClaimTypes.Email, kullanici.EmailAddress),
                new Claim(ClaimTypes.GivenName, kullanici.FirstName),
                new Claim(ClaimTypes.Surname, kullanici.LastName)
            };

            var roller = await _repo.GetKullaniciRoles(kullanici.Username);

            foreach (var rol in roller)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol.Adi));
            }

            var token = new JwtSecurityToken
            (
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponse(claims.ToArray(), kullanici, tokenString, HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return new LoginResponse(StatusCode: HttpStatusCode.BadRequest, Error: ex.Message);
        }

    }

    private string CreatePasswordHash(string password)
    {
        using var hmac = new HMACSHA512();
        hmac.Key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);


        //return Encoding.UTF8.GetString(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
}

