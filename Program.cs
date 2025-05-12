using Carter;
using LiteDB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddSingleton<ILiteDatabase, LiteDatabase>(x => new LiteDatabase("\"Filename=encurtador.db;Connection=shared\""));

var app = builder.Build();


//app.MapGet("/", () => "Hello World!");
app.UseStaticFiles();
app.MapCarter();

app.Run();
