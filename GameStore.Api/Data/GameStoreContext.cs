using System.Reflection;
using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

/// <summary>
/// Command for migrations:
/// 1. To make a migration:
///   in powershell: `dotnet ef migrations add InitialCreate --output-dir Data\Migrations`
///   - Breaking down the command:
///   "InitialCreate": this is the name of the file
///   "--output-dir": Tha path where the file will be created
///  
/// 2. To run maked migrations:
///   in powershell: `dotnet ef database update`
/// </summary>

public class GameStoreContext : DbContext
{
  public GameStoreContext(DbContextOptions<GameStoreContext> options)
    : base(options)
  {
  }

  public DbSet<Game> Games => Set<Game>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}