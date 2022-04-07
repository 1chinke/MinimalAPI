using MinimalAPI.Responses;
using MediatR;
using MinimalAPI.Infrastructure.Integration;
using System.Net;
using MinimalAPI.Mediatr.Queries.HavaTahminiQueries;

namespace MinimalAPI.Mediatr.Handlers.HavaTahminiHandlers;

public class GetHavaTahminiHnd : IRequestHandler<GetHavaTahmini, GenericResponse>
{

    private readonly IHavaTahminiSvc _havaTahminSvc;

    public GetHavaTahminiHnd(IHavaTahminiSvc havaTahminiSvc)
    {
        _havaTahminSvc = havaTahminiSvc;
    }

    public async Task<GenericResponse> Handle(GetHavaTahmini request, CancellationToken cancellationToken)
    {

        try
        {
            var result = await _havaTahminSvc.Get(request.ApplicableDate);

            if (result.StatusCode == HttpStatusCode.OK) {
                return new GenericResponse(result.Result);
            } 
            else
            {
                return new GenericResponse(StatusCode: result.StatusCode, Error: result.Error);
            }
        }
        catch (Exception ex)
        {
            return new GenericResponse(StatusCode: HttpStatusCode.BadRequest, Error: ex.Message);
        }
    }
}
