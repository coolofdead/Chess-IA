using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
[CreateAssetMenu(fileName = "IA", menuName = "ScriptableObjects/IA", order = 1)]
public class IA : ScriptableObject, IPlayer
{
    private Dictionary<int, float> winratesByGamestate = new Dictionary<int, float>();

    public IA()
    {
        // Train
    }

    public void AddGameStateToGameHistory(GameState gameState, bool wasWinningMove = false)
    {
        //// If not first move
        //if (currentGameHistory.Count > 0)
        //{
        //    var lastGameState = currentGameHistory.Last(); // Pop last game state
        //    currentGameHistory.Remove(lastGameState); // Remove it from list (can't modify directly bc it's a struct)

        //    lastGameState.nextState = gameState.state; // Set the nextState value
        //    if (wasWinningMove)
        //        lastGameState.reward = gameState.reward; // Set the nextState value

        //    currentGameHistory.Add(lastGameState); // Then re insert it
        //}
    
        //// If it's the winning move then we don't add it to the list bc it's already in there
        //if (!wasWinningMove)
        //{
        //    currentGameHistory.Add(gameState);
        //}
    }

    public void Train()
    {
        //// Flip current game history
        //currentGameHistory.Reverse();

        //// Loop over each game states and apply formula
        //foreach (GameState gameState in currentGameHistory)
        //{
        //    if (winratesByGamestate.ContainsKey(gameState.state) == false)
        //        winratesByGamestate.Add(gameState.state, 0);

        //    if (winratesByGamestate.ContainsKey(gameState.nextState) == false)
        //        winratesByGamestate.Add(gameState.nextState, 0);

        //    // Store in value function the <state, value> foreach game state
        //    if (gameState.reward == 0) // Meaning it is the last game state (win or loose)
        //    {
        //        winratesByGamestate[gameState.state] = winratesByGamestate[gameState.state] + LEARNING_RATE * (winratesByGamestate[gameState.nextState] - winratesByGamestate[gameState.state]);
        //    }
        //    else
        //    {
        //        winratesByGamestate[gameState.state] = winratesByGamestate[gameState.state] + LEARNING_RATE * ((float)gameState.reward - winratesByGamestate[gameState.state]);
        //    }
        //}

        //// Reset current game history
        //currentGameHistory.Clear();
    }

    private int GetStickByExperience()
    {
        var stickActions = new int[] { 1, 2, 3 };
        int bestAction = 1; // Default one
        float bestActionEstimatedWinrate = Mathf.Infinity;

        foreach (int stickNumber in stickActions)
        {
            //if (
            //    currentGame.Stick - stickNumber > 0 &&
            //    winratesByGamestate.ContainsKey(currentGame.Stick - stickNumber) &&
            //    (winratesByGamestate[currentGame.Stick - stickNumber] < bestActionEstimatedWinrate)
            //)
            //{
            //    bestAction = stickNumber;
            //    bestActionEstimatedWinrate = winratesByGamestate[currentGame.Stick - stickNumber];
            //}
        }

        return bestAction;
    }

    public int GetMoveToPick()
    {
        return GetStickByExperience();
    }

    public string GetName()
    {
        return "IA";
    }
}
