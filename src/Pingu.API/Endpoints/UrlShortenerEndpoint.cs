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
            .ProducesProblem(400)
            .Produces<ShortenUrlResponse>(201);

        // GET /url-shortener/{shortCode}
        urlShortenerEndpoint.MapGet("/{shortCode}", HandleGetShortenedUrl)
            .WithDescription("The endpoint to get the original URL from a shortened URL")
            .ProducesProblem(400)
            .Produces(301);


        return urlShortenerEndpoint;
    }
    #region [Endpoints Handlers]

    // POST /url-shortener/shorten
    private static async Task<IResult> HandleShortenUrl(HttpContext context, IUrlShortenerService urlShortenerService, ShortenUrlRequest request)
    {
        var result = await urlShortenerService.CreateShortenedUrl(request.OriginalUrl, request.ExpiresAt);
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

    // GET /url-shortener/{shortCode}
    private static async Task<IResult> HandleGetShortenedUrl(HttpContext context, IUrlShortenerService urlShortenerService, string shortCode)
    {
        var result = await urlShortenerService.GetShortenedUrl(shortCode);
        return result.IsSuccess == false
            ? Results.Problem(
                title: "An error occurred while getting the original URL",
                statusCode: result.Error!.Code,
                detail: result.Error!.Message)
            : Results.Redirect(result.Value!.OriginalUrl, permanent: true);
    }

    #endregion
}