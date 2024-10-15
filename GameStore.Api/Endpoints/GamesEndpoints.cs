using GameStore.Api.Dtos;

namespace GameStore.Api.EndPoints;

public static class GamesEndpoints
{
    const string GetGameEndPointName = "GetGame";

    private static List<GameDto> games = [
        new (
        1,
        "Street Fighting",
        "Fighting",
        19.99M,
        new DateOnly(1992, 7, 15)
    ),
    new(
      2,
        "NFS Game",
        "NFS",
        59.99M,
        new DateOnly(2010, 5, 20)
    ),
     new(
      3,
        "FIFA 2023",
        "FIF",
        109.99M,
        new DateOnly(2010, 5, 20)
    ),
];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games");
        //GET /Games
        group.MapGet("/", () => games);
        //GET /games/{id}
        group.MapGet("/{id}", (int id) =>
        {
            GameDto? game = games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameEndPointName);


        //POST /games

        group.MapPost("/", (CreateGameDto newGame) =>
        {
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleasedDate
            );
            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndPointName, new { Id = game.Id }, game);
        });


        //PUT /games

        group.MapPut("/{id}", (int id, UpdateGameDto updateGameDto) =>
        {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }
            games[index] = new(
                id,
                updateGameDto.Name,
                updateGameDto.Genre,
                updateGameDto.Price,
                updateGameDto.ReleasedDate
            );

            return Results.NoContent();
        });

        //DELETE /games/{id}
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;
    }
}