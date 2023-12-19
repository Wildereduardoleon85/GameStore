using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public class InMemGamesRepository : IGamesRepository
{
  private readonly List<Game> games = new()
  {
    new Game()
    {
      Id = 1,
      Name = "Street Fighter II",
      Genre = "Fighting",
      Price = 19.99m,
      ReleaseDate = new DateTime(
        1991, 2, 1, 0, 0, 0,
        DateTimeKind.Unspecified
      ),
      ImageUri = "https://placehold.co/100"
    },
    new Game()
    {
      Id = 2,
      Name = "Final Fantasy Tactics",
      Genre = "RPG",
      Price = 8.99m,
      ReleaseDate = new DateTime(
        1998, 5, 20, 0, 0, 0,
        DateTimeKind.Unspecified
      ),
      ImageUri = "https://placehold.co/100"
    },
    new Game()
    {
      Id = 3,
      Name = "FIFA 23",
      Genre = "Sports",
      Price = 69.99m,
      ReleaseDate = new DateTime(
        2023, 10, 12, 0, 0, 0,
        DateTimeKind.Unspecified
      ),
      ImageUri = "https://placehold.co/100"
    },
  };

  public IEnumerable<Game> GetAll()
  {
    return games;
  }

  public Game? Get(int id)
  {
    return games.Find(game => game.Id == id);
  }

  public void Create(Game game)
  {
    game.Id = games.Max(game => game.Id) + 1;
    games.Add(game);
  }

  public void Update(Game updatedGame)
  {
    var index = games.FindIndex(game => game.Id == updatedGame.Id);
    games[index] = updatedGame;
  }

  public void Delete(int id)
  {
    var index = games.FindIndex(game => game.Id == id);
    games.RemoveAt(index);
  }

}
