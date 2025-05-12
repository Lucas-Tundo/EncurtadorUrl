using Carter;
using EncurtadorURL.Models;
using LiteDB;
using UrlShortnerVideo;

namespace UrlShortnerVideo.CarterModules;

public class PagesModule : ICarterModule {
    public void AddRoutes(IEndpointRouteBuilder app) {
        // GET /
        app.MapGet("/", async (HttpContext ctx) => {
            ctx.Response.ContentType = "text/html";
            ctx.Response.StatusCode = 200;
            await ctx.Response.SendFileAsync("wwwroot/index.html");
        });

        // GET /{chunck}
        app.MapGet("/{chunck}", async (HttpContext ctx, ILiteDatabase db) => {
            var chunck = ctx.Request.RouteValues["chunck"]?.ToString();

            if (string.IsNullOrWhiteSpace(chunck)) {
                ctx.Response.Redirect("/");
                return;
            }

            var shortUrl = db.GetCollection<UrlEntry>("urls")
                             .FindOne(x => x.Chunck == chunck);

            if (shortUrl is null)
                ctx.Response.Redirect("/");
            else
                ctx.Response.Redirect(shortUrl.Url);

            await Task.CompletedTask;
        });
    }
}
