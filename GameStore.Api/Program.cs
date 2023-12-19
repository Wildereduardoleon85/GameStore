using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);
var sqlServerConnectionString =
  builder.Configuration.GetConnectionString("GameStoreContext");

builder.Services.AddSingleton<IGamesRepository, InMemGamesRepository>();
builder.Services.AddSqlServer<GameStoreContext>(sqlServerConnectionString);

var app = builder.Build();

app.MapGamesEndpoints();

app.Run();
