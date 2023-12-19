using GameStore.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
  public static async Task IinitializeDbAsync(this IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
    await dbContext.Database.MigrateAsync();
  }

  public static IServiceCollection AddRepositories(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    var sqlServerConnectionString =
      configuration.GetConnectionString("GameStoreContext");

    services
      .AddSqlServer<GameStoreContext>(sqlServerConnectionString)
      .AddScoped<IGamesRepository, EntityFrameworkGamesRepository>();

    return services;
  }
}