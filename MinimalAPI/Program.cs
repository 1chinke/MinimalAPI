using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalAPI.Mediatr;
using MinimalAPI.Mediatr.Behaviors;
using MinimalAPI.Mediatr.Commands.KullaniciCommands;
using MinimalAPI.Mediatr.Commands.PersonCommands;
using MinimalAPI.Mediatr.Queries.KullaniciQueries;
using MinimalAPI.Mediatr.Queries.PersonQueries;
using MinimalAPI.Models;
using MinimalAPI.Infrastructure.Database;
using MinimalAPI.Validators.Api;
using MinimalAPI.Validators.Domain;
using Serilog;
using Serilog.Events;
using System.Text;
using MinimalAPI.Infrastructure.Integration;
using MinimalAPI.Mediatr.Queries.HavaTahminiQueries;
using System.Net;


// Serilogu iki aþamalý olarak yapýlandýrýyoruz.
// 1) ASP.NET Core uygulamasýnýn baþlatýlmasý
// 2) Loglama ayarlarýnýn appsettings.json dosyasýndan okunarak uygulamanýn diðer aþamalrýnda kullanýmý

// Burasý birinci kýsým. Henüz uygulama baþlangýç aþamasýnda olduðu için sadece console'a eriþimizi var.

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();


try
{
    Log.Information("Web host baþlatýlýyor....");
    var builder = WebApplication.CreateBuilder(args);

    // Serilog ikinci kýsým: Ayarlarý appsettings.json dosyasýndan alýyoruz.
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());


    DateTime time = DateTime.ParseExact("2022-04-07", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

    Log.Information($"Time is: {time}", time);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "Bearer Authentication with JWT Token",
            Type = SecuritySchemeType.Http
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                new List<string>()
            }

        });
    });

    //Jwt
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateActor = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = "localhost:6379";
    });


    builder.Services.AddAuthorization();

    //vmo: DI'lar buraya ekleniyor
    builder.Services.AddSingleton<IConnectionManager, ConnectionManager>();
    builder.Services.AddSingleton<IPersonRepo, PersonRepo>();
    builder.Services.AddSingleton<IKullaniciRepo, KullaniciRepo>();
    builder.Services.AddSingleton<IRolRepo, RolRepo>();
    builder.Services.AddSingleton<IHavaTahminiSvc, HavaTahminiSvc>();

    builder.Services.AddMediatR(typeof(MediatrEntryPoint).Assembly); //bunun için nugetten mediatr.dependencyinjection paketini eklemek gerekiyor.
                                                                     //MediatrEntryPoint: Mediatr dizinindeki boþ class
    builder.Services.AddValidatorsFromAssembly(typeof(DomainValidationEntryPoint).Assembly);
    builder.Services.AddMemoryCache();

    //Mediatr pipeline behavior DI. Buradaki sýraya göre pipelinedaki her bir behavior çalýþýyor
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

    builder.Services.AddHttpClient();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    ////////////////////////HAVA TAHMÝNÝ//////////////////////

    app.MapGet("/havaTahmini",
    async (string tarih, IMediator mediator, CancellationToken cancel) =>
        {
            var result = await mediator.Send(new GetHavaTahminiQry(tarih), cancel);

            Log.Information($"result: {result.Result}");

            return result.StatusCode switch
            {
                HttpStatusCode.OK => Results.Ok(result),
                HttpStatusCode.NotFound => Results.NotFound("Hava tahmini bulunamadý."),
                _ => Results.BadRequest(result),
            };
        });

    /////////////////////////////PERSON/////////////////////////// 

    app.MapGet("/allPerson",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        async (IMediator mediator, CancellationToken cancel) =>
    {
        var result = await mediator.Send(new GetPeopleQry(), cancel);

        return result.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(result),
            HttpStatusCode.NotFound => Results.NotFound("Kiþi bulunamadý."),
            _ => Results.BadRequest(result),
        };
    });

    app.MapGet("/personById",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    async (string id, IMediator mediator, CancellationToken cancel) =>
    {
        var result = await mediator.Send(new GetPersonByIdQry(id), cancel);

        return result.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(result),
            HttpStatusCode.NotFound => Results.NotFound("Kiþi bulunamadý."),
            _ => Results.BadRequest(result),
        };
    });

    app.MapPost("/createPerson",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    async (Person model, IMediator mediator, CancellationToken cancel) =>
    {
        var errors = ValidatePerson(model);

        if (errors.Count > 0)
        {
            return Results.BadRequest(errors);
        }

        var result = await mediator.Send(new InsertPersonCmd(model.FirstName, model.LastName), cancel);

        return result.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(result),
            HttpStatusCode.NotFound => Results.NotFound("Kiþi bulunamadý."),
            _ => Results.BadRequest(result),
        };

    });

    app.MapPut("/updatePerson",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    async (Person model, IMediator mediator, CancellationToken cancel) =>
    {
        var errors = ValidatePerson(model);

        if (errors.Count > 0)
        {
            return Results.BadRequest(errors);
        }

        var result = await mediator.Send(new UpdatePersonCmd(model), cancel);

        return result.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(result),
            HttpStatusCode.NotFound => Results.NotFound("Kiþi bulunamadý."),
            _ => Results.BadRequest(result),
        };

    });

    app.MapDelete("/deletePerson",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    async (string id, IMediator mediator, CancellationToken cancel) =>
    {
        var result = await mediator.Send(new DeletePersonCmd(id), cancel);

        return result.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(result),
            HttpStatusCode.NotFound => Results.NotFound("Kiþi bulunamadý."),
            _ => Results.BadRequest(result),
        };

    });


    /////////////////////////////KULLANICI/////////////////////////// 

    app.MapGet("/allKullanici",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    async (IMediator mediator, CancellationToken cancel) =>
    {
        var result = await mediator.Send(new GetAllKullaniciQry(), cancel);

        return result.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(result),
            HttpStatusCode.NotFound => Results.NotFound("Kullanýcý bulunamadý."),
            _ => Results.BadRequest(result),
        };
    });

    app.MapGet("/kullaniciByUsername",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    async (string username, IMediator mediator, CancellationToken cancel) =>
    {
        var result = await mediator.Send(new GetKullaniciByUsernameQry(username), cancel);

        return result.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(result),
            HttpStatusCode.NotFound => Results.NotFound("Kullanýcý bulunamadý."),
            _ => Results.BadRequest(result),
        };
    });

    app.MapPost("/createKullanici",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    async (Kullanici model, IMediator mediator, CancellationToken cancel) =>
    {
        var errors = ValidateKullanici(model);

        if (errors.Count > 0)
        {
            return Results.BadRequest(errors);
        }

        var result = await mediator.Send(new InsertKullaniciCmd(model), cancel);

        return result.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(result),
            HttpStatusCode.NotFound => Results.NotFound("Kullanýcý bulunamadý."),
            _ => Results.BadRequest(result),
        };

    });

    app.MapPut("/updateKullanici",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    async (string username, Kullanici model, IMediator mediator, CancellationToken cancel) =>
    {
        var errors = ValidateKullanici(model);

        if (errors.Count > 0)
        {
            return Results.BadRequest(errors);
        }

        var result = await mediator.Send(new UpdateKullaniciCmd(username, model), cancel);

        return result.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(result),
            HttpStatusCode.NotFound => Results.NotFound("Kullanýcý bulunamadý."),
            _ => Results.BadRequest(result),
        };

    });

    app.MapDelete("/deleteKullanici",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    async (string username, IMediator mediator, CancellationToken cancel) =>
    {
        var result = await mediator.Send(new DeleteKullaniciCmd(username), cancel);

        return result.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(result),
            HttpStatusCode.NotFound => Results.NotFound("Kullanýcý bulunamadý."),
            _ => Results.BadRequest(result),
        };

    });

    app.MapPost("/login", async (UserLogin login, IMediator mediator, CancellationToken cancel) =>
    {
        var result = await mediator.Send(new GetLoginQry(login), cancel);

        return result.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(result),
            HttpStatusCode.NotFound => Results.NotFound("Kullanýcý adý ya da parola hatalý."),
            _ => Results.BadRequest(result),
        };

    });


    app.UseAuthentication();
    app.UseAuthorization();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var ex = context.Features.Get<IExceptionHandlerFeature>();

                var detail = "";
                var status = StatusCodes.Status500InternalServerError;
                var title = "HATA!!!";
                if (ex != null)
                {
                    if (ex.Error is ValidationException)
                    {
                        status = StatusCodes.Status400BadRequest;
                        title = "Veri Doðrulama Hatasý!";
                        detail = ex.Error.Message;
                    }
                    else
                    {
                        detail = ex.Error.Message;
                    }
                }
                else
                {
                    detail = "Beklenmeyen hata. Lütfen RequestId ile birlikte teknik desteðe bildirin";
                }

                var pd = new ProblemDetails
                {
                    Title = title,
                    Status = status,
                    Detail = detail,
                };

                pd.Extensions.Add("RequestId", context.TraceIdentifier);
                pd.Extensions.Add("User", context.User);

            context.Response.StatusCode = status;

                await context.Response.WriteAsJsonAsync(pd, pd.GetType(), null, contentType: "application/problem+json");
            });
        });
    }


    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

List<FluentValidation.Results.ValidationFailure> ValidatePerson(Person model)
{
    PersonValidator validator = new();

    var validatorResult = validator.Validate(model);

    return validatorResult.Errors;

}

List<FluentValidation.Results.ValidationFailure> ValidateKullanici(Kullanici model)
{
    KullaniciValidator validator = new();

    var validatorResult = validator.Validate(model);

    return validatorResult.Errors;

}

return 0;







