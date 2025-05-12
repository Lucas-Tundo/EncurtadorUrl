using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carter;

namespace EncurtadorURL.CarterModules;

public class PagesModule : ICarterModule {
    public void AddRoutes(IEndpointRouteBuilder app) {
        app.MapGet("/", async (HttpContext ctx) => {
            ctx.Response.ContentType = "text/html";
            ctx.Response.StatusCode = 200;
            await ctx.Response.SendFileAsync("wwwroot/index.html");
        });
    }
}
