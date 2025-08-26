namespace GameStore.Api.Data;

public class GameDataLogger(
    GameStoreData data,
    ILogger<GameDataLogger> logger
) {
    public void PrintGames() {
        foreach (var game in data.GetAllGames()) {
            logger.LogInformation(
                "Game Id: {gameId} | GameName: {gameName} ",
                game.Id,
                game.Name
            );
        }
    }
}
