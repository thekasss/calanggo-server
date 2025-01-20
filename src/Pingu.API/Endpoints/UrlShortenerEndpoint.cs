using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

using Pingu.Application.Interfaces;
using Pingu.Application.UseCases.ShortenUrl;

namespace Pingu.API.Endpoints;

public static class UrlShortenerEndpoint
{
    public static IEndpointRouteBuilder AddUrlShortenerEndpoint(this IEndpointRouteBuilder endpoinBuilder)
    {
        var urlShortenerEndpoint = endpoinBuilder.MapGroup("/url-shortener");

        // POST /url-shortener/shorten
        urlShortenerEndpoint.MapPost("/shorten", HandleShortenUrl)
            .WithDescription("The endpoint to shorten a URL")
            .Produces<ProblemDetails>(400)
            .Produces<ShortenUrlResponse>(201);


        return urlShortenerEndpoint;
    }
    #region [Endpoints Handlers]

    // POST /url-shortener/shorten
    private static async Task<IResult> HandleShortenUrl(HttpContext context, IUrlShortenerService urlShortenerService, ShortenUrlRequest request)
    {
        var result = await urlShortenerService.CreateShortenedUrl(request.OriginalUrl, request.CreatedBy, request.ExpiresAt);
        if (result.IsSuccess == false)
        {
            return Results.Problem(
                title: "An error occurred while shortening the URL",
                statusCode: result.Error!.Code,
                detail: result.Error!.Message);
        }

        string shortenedUrl = context.Request.GetDisplayUrl().Replace(context.Request.Path, $"/{result.Value!.ShortCode}");
        var response = new ShortenUrlResponse(result.Value.OriginalUrl, shortenedUrl, result.Value.ShortCode, result.Value.ExpiresAt);
        return Results.Created(shortenedUrl, response);
    }

    #endregion
}