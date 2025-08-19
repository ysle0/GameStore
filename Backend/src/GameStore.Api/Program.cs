using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.MapGet("/", () => "Welcome to the Game Store API!");

app.Run();
