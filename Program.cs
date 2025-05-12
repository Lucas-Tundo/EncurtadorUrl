using Carter;
using LiteDB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddSingleton<ILiteDatabase, LiteDatabase>(x => new LiteDatabase("short.db"));
builder.Services.AddSingleton<ILiteDatabase>(_ =>
    new LiteDatabase("Filename=shortner.db;Connection=shared"));


var app = builder.Build();


//app.MapGet("/", () => "Hello World!");
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapCarter();

app.Run();
