using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carter;
using Carter.ModelBinding;
using EncurtadorURL.Models;
using LiteDB;

namespace EncurtadorURL.CarterModules;

public class UrlsModule : ICarterModule {
    public void AddRoutes(IEndpointRouteBuilder app) {
        // POST /urls
        app.MapPost("/urls", async (HttpContext ctx, ILiteDatabase db) => {
            var data = await ctx.Request.ReadFromJsonAsync<UrlEntry>();
            if (data is null || string.IsNullOrWhiteSpace(data.OriginalUrl)) {
                ctx.Response.StatusCode = 400;
                await ctx.Response.WriteAsync("URL inválida.");
                return;
            }

            data.ShortCode = Guid.NewGuid().ToString("N")[..6];

            var col = db.GetCollection<UrlEntry>("urls");
            col.Insert(data);

            await ctx.Response.WriteAsJsonAsync(new {
                shortUrl = $"https://localhost:5050/{data.ShortCode}"
            });
        });

        // GET /Exemplo: //https://localhost:5050/{chunk}
        app.MapGet("/{chunk}", (string chunk, ILiteDatabase db) => {
            var col = db.GetCollection<UrlEntry>("urls");
            var entry = col.FindOne(x => x.ShortCode == chunk);

            return entry is not null
                ? Results.Redirect(entry.OriginalUrl)
                : Results.NotFound("URL não encontrada.");
        });
    }
}

