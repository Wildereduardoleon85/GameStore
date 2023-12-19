using GameStore.Api.Entities;
using GameStore.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
  const string GET_GAME_ENDPOINT_NAME = "GetGame";

  public static RouteGroupBuilder MapGamesEndpoints(
    this IEndpointRouteBuilder routes
  )
  {

    RouteGroupBuilder group = routes.MapGroup("/games")
      .WithParameterValidation();

    group.MapGet(
      "/", async ([FromServices] IGamesRepository repository) =>
      {
        var games = await repository.GetAllAsync();
        var parsedGames = games.Select(game => game.AsDto());

        return Results.Ok(parsedGames);
      }
    );

    group.MapGet(
      "/{id}",
      async (
        [FromServices] IGamesRepository repository,
        [FromRoute] int id
      ) =>
    {
      var game = await repository.GetAsync(id);

      return game is null ? Results.NotFound() : Results.Ok(game.AsDto());
    })
      .WithName(GET_GAME_ENDPOINT_NAME);

    group.MapPost(
      "/",
      async (
        [FromServices] IGamesRepository repository,
        [FromBody] CreateGameDto gameDto
      ) =>
    {

      Game game = new()
      {
        Name = gameDto.Name,
        Genre = gameDto.Genre,
        ReleaseDate = gameDto.ReleaseDate,
        Price = gameDto.Price,
        ImageUri = gameDto.ImageUri
      };

      await repository.CreateAsync(game);

      return Results.CreatedAtRoute(
        GET_GAME_ENDPOINT_NAME,
        new { id = game.Id },
        game
      );
    });

    group.MapPut(
      "/{id}",
      async (
        [FromServices] IGamesRepository repository,
        [FromRoute] int id,
        [FromBody] UpdatedGameDto updatedGameDto
      ) =>
    {
      var existingGame = await repository.GetAsync(id);

      if (existingGame is null)
      {
        return Results.NotFound();
      }

      existingGame.Name = updatedGameDto.Name;
      existingGame.Genre = updatedGameDto.Genre;
      existingGame.Price = updatedGameDto.Price;
      existingGame.ReleaseDate = updatedGameDto.ReleaseDate;
      existingGame.ImageUri = updatedGameDto.ImageUri;

      await repository.UpdateAsync(existingGame);

      return Results.NoContent();
    });

    group.MapDelete(
      "/{id}",
      async (
        [FromServices] IGamesRepository repository,
        [FromRoute] int id
      ) =>
    {
      var game = await repository.GetAsync(id);

      if (game is not null)
      {
        await repository.DeleteAsync(id);
      }

      return Results.NoContent();
    });

    return group;
  }
}