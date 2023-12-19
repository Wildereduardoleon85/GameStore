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
      "/", ([FromServices] IGamesRepository repository) =>
      {
        IEnumerable<GameDto> games = repository.GetAll().Select(game => game.AsDto());
        return Results.Ok(games);
      }
    );

    group.MapGet(
      "/{id}",
      (
        [FromServices] IGamesRepository repository,
        [FromRoute] int id
      ) =>
    {
      Game? game = repository.Get(id);

      return game is null ? Results.NotFound() : Results.Ok(game.AsDto());
    })
      .WithName(GET_GAME_ENDPOINT_NAME);

    group.MapPost(
      "/",
      (
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

      repository.Create(game);

      return Results.CreatedAtRoute(
        GET_GAME_ENDPOINT_NAME,
        new { id = game.Id },
        game
      );
    });

    group.MapPut(
      "/{id}",
      (
        [FromServices] IGamesRepository repository,
        [FromRoute] int id,
        [FromBody] UpdatedGameDto updatedGameDto
      ) =>
    {
      Game? existingGame = repository.Get(id);

      if (existingGame is null)
      {
        return Results.NotFound();
      }

      existingGame.Name = updatedGameDto.Name;
      existingGame.Genre = updatedGameDto.Genre;
      existingGame.Price = updatedGameDto.Price;
      existingGame.ReleaseDate = updatedGameDto.ReleaseDate;
      existingGame.ImageUri = updatedGameDto.ImageUri;

      repository.Update(existingGame);

      return Results.NoContent();
    });

    group.MapDelete(
      "/{id}",
      (
        [FromServices] IGamesRepository repository,
        [FromRoute] int id
      ) =>
    {
      Game? game = repository.Get(id);

      if (game is not null)
      {
        repository.Delete(id);
      }

      return Results.NoContent();
    });

    return group;
  }
}