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
using MinimalAPI.Repository;
using MinimalAPI.Validators.Api;
using MinimalAPI.Validators.Domain;
using Serilog;
using Serilog.Events;
using System.Text;

// Serilogu iki a�amal� olarak yap�land�r�yoruz.
// 1) ASP.NET Core uygulamas�n�n ba�lat�lmas�
// 2) Loglama ayarlar�n�n appsettings.json dosyas�ndan okunarak uygulaman�n di�er a�amalr�nda kullan�m�

// Buras� birinci k�s�m. Hen�z uygulama ba�lang�� a�amas�nda oldu�u i�in sadece console'a eri�imizi var.

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();


try
{
    Log.Information("Web host ba�lat�l�yor....");
    var builder = WebApplication.CreateBuilder(args);

    // Serilog ikinci k�s�m: Ayarlar� appsettings.json dosyas�ndan al�yoruz.
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

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
    builder.Services.AddAuthorization();

    //vmo: DI'lar buraya ekleniyor
    builder.Services.AddSingleton<IPersonRepo, PersonRepo>();
    builder.Services.AddSingleton<IKullaniciRepo, KullaniciRepo>();

    builder.Services.AddMediatR(typeof(MediatrEntryPoint).Assembly); //bunun i�in nugetten mediatr.dependencyinjection paketini eklemek gerekiyor.
                                                                     //MediatrEntryPoint: Mediatr dizinindeki bo� class
    builder.Services.AddValidatorsFromAssembly(typeof(DomainValidationEntryPoint).Assembly);

    //Mediatr pipeline behavior DI. Buradaki s�raya g�re pipelinedaki her bir behavior �al���yor
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    /////////////////////////////PERSON/////////////////////////// 

    app.MapGet("/allPerson",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        async (IMediator mediator, CancellationToken cancel) =>
    {
        var result = await mediator.Send(new GetPeople(), cancel);

        return result.StatusCode switch
        {
            200 => Results.Ok(result),
            404 => Results.NotFound("Ki�i bulunamad�."),
            _ => Results.BadRequest(result),
        };
    });

    app.MapGet("/personById",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    async (int id, IMediator mediator, CancellationToken cancel) =>
    {
        var result = await mediator.Send(new GetPersonById(id), cancel);

        return result.StatusCode switch
        {
            200 => Results.Ok(result),
            404 => Results.NotFound("Ki�i bulunamad�."),
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

        var result = await mediator.Send(new InsertPerson(model), cancel);

        return result.StatusCode switch
        {
            200 => Results.Ok(result),
            404 => Results.NotFound("Ki�i bulunamad�."),
            _ => Results.BadRequest(result),
        };

    });

    app.MapPut("/updatePerson",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    async (int id, Person model, IMediator mediator, CancellationToken cancel) =>
    {
        var errors = ValidatePerson(model);

        if (errors.Count > 0)
        {
            return Results.BadRequest(errors);
        }

        var result = await mediator.Send(new UpdatePerson(id, model), cancel);

        return result.StatusCode switch
        {
            200 => Results.Ok(result),
            404 => Results.NotFound("Ki�i bulunamad�."),
            _ => Results.BadRequest(result),
        };

    });

    app.MapDelete("/deletePerson",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    async (int id, IMediator mediator, CancellationToken cancel) =>
    {
        var result = await mediator.Send(new DeletePerson(id), cancel);

        return result.StatusCode switch
        {
            200 => Results.Ok(result),
            404 => Results.NotFound("Ki�i bulunamad�."),
            _ => Results.BadRequest(result),
        };

    });


    /////////////////////////////KULLANICI/////////////////////////// 

    app.MapGet("/allKullanici",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    async (IMediator mediator, CancellationToken cancel) =>
    {
        var result = await mediator.Send(new GetAllKullanici(), cancel);

        return result.StatusCode switch
        {
            200 => Results.Ok(result),
            404 => Results.NotFound("Kullan�c� bulunamad�."),
            _ => Results.BadRequest(result),
        };
    });

    app.MapGet("/kullaniciByUsername",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    async (string username, IMediator mediator, CancellationToken cancel) =>
    {
        var result = await mediator.Send(new GetKullaniciByUsername(username), cancel);

        return result.StatusCode switch
        {
            200 => Results.Ok(result),
            404 => Results.NotFound("Kullan�c� bulunamad�."),
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

        var result = await mediator.Send(new InsertKullanici(model), cancel);

        return result.StatusCode switch
        {
            200 => Results.Ok(result),
            404 => Results.NotFound("Kullan�c� bulunamad�."),
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

        var result = await mediator.Send(new UpdateKullanici(username, model), cancel);

        return result.StatusCode switch
        {
            200 => Results.Ok(result),
            404 => Results.NotFound("Kullan�c� bulunamad�."),
            _ => Results.BadRequest(result),
        };

    });

    app.MapDelete("/deleteKullanici",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    async (string username, IMediator mediator, CancellationToken cancel) =>
    {
        var result = await mediator.Send(new DeleteKullanici(username), cancel);

        return result.StatusCode switch
        {
            200 => Results.Ok(result),
            404 => Results.NotFound("Kullan�c� bulunamad�."),
            _ => Results.BadRequest(result),
        };

    });

    app.MapPost("/login", async (UserLogin login, IMediator mediator, CancellationToken cancel) =>
    {
        var result = await mediator.Send(new GetLogin(login), cancel);

        if (result.StatusCode == 200)
        {
            return Results.Ok(result.Token);
        }
        else if (result.StatusCode == 404)
        {
            return Results.NotFound("Kullan�c� ad� ya da parola hatal�.");
        }
        else
        {
            return Results.BadRequest(result);
        }

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
                        title = "Veri Do�rulama Hatas�!";
                        detail = ex.Error.Message;
                    }
                    else
                    {
                        detail = ex.Error.Message;
                    }
                }
                else
                {
                    detail = "Beklenmeyen hata. L�tfen RequestId ile birlikte teknik deste�e bildirin";
                }

                var pd = new ProblemDetails
                {
                    Title = title,
                    Status = status,
                    Detail = detail,
                };

                pd.Extensions.Add("RequestId", context.TraceIdentifier);
            //pd.Extensions.Add("User", context.User);

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






