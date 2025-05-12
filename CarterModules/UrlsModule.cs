using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carter;
using Carter.ModelBinding;
using EncurtadorURL.Models;
using LiteDB;
using NanoidDotNet;

namespace EncurtadorURL.CarterModules;

public class UrlsModule : ICarterModule {
    public void AddRoutes(IEndpointRouteBuilder app) {
        // POST /urls
        app.MapPost("/urls", async (HttpContext ctx, ILiteDatabase db) => {
            var data = await ctx.Request.ReadFromJsonAsync<UrlEntry>();
            if (data is null || string.IsNullOrWhiteSpace(data.Url)) {
                ctx.Response.StatusCode = 400;
                await ctx.Response.WriteAsync("URL inválida.");
                return;
            }

            data.Chunck = Nanoid.Generate(size: 9);

            var col = db.GetCollection<UrlEntry>("urls");
            col.Insert(data);

            await ctx.Response.WriteAsJsonAsync(new {
                shortUrl = $"https://localhost:5053/{data.Chunck}"
            });
        });

        // GET /Exemplo: //https://localhost:5050/{chunck}
        app.MapGet("/{chunck}", (string chunck, ILiteDatabase db) => {
            var col = db.GetCollection<UrlEntry>("urls");
            var entry = col.FindOne(x => x.Chunck == chunck);
            

            return entry is not null
                ? Results.Redirect(entry.Url)
                : Results.NotFound("URL não encontrada.");
        });
    }
}

