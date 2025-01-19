using Microsoft.AspNetCore.Http.Extensions;
using Pingu.Application.UrlShortenerService;
using Pingu.Application.UseCases.ShortenUrl;

namespace Pingu.API.Endpoints;

public static class UrlShortenerEndpoint
{
    public static IEndpointRouteBuilder AddUrlShortenerEndpoint(this IEndpointRouteBuilder endpoinBuilder)
    {
        var urlShortenerEndpoint = endpoinBuilder.MapGroup("/url-shortener");

        urlShortenerEndpoint.MapPost("/shorten", async (HttpContext context, IUrlShortenerService urlShortenerService, ShortenUrlRequest request) =>
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
        });

        return urlShortenerEndpoint;
    }
}