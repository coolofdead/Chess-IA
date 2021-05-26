using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Game game;

    public IA ia;
    private Dictionary<int, float> winratesByGamestate;

    private void Start()
    {
        IDataProvidable dataProvidable = (new ChessDataProviderFactory()).GetDataProvidable();

        foreach (ChessGameHistory matchHistory in dataProvidable.GetGamesHistoryForPlayer("magnuscarlsen", 2010))
        {
            Debug.Log(matchHistory.Result);

            // true == white | false == black
            bool playAsWhite = matchHistory.Result != ChessGameResult.DRAW ? matchHistory.Result == ChessGameResult.WHITE_WIN : true;

            foreach (KeyValuePair<bool, string> move in matchHistory)
            {
                
            }

            //var winner = game.GetWinner() == Game.Player.Player1 ? firstPlayer : secondPlayer;
            //if (winner is IA)
            //    (winner as IA).AddGameStateToGameHistory(new GameState { state = 0, reward = 1 }, true);
        }
    }
}
