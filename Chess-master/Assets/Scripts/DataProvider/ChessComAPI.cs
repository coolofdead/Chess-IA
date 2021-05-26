using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ChessComAPI : IDataProvidable
{
    public IEnumerable<ChessGameHistory> GetGamesHistoryForPlayer(string playerName, int startYear)
    {
        //UnityWebRequest www = UnityWebRequest.Get("https://api.chess.com/pub/player/magnuscarlsen/games/2020/12");

        //www.SendWebRequest();
        //while (!www.isDone)
        //{
        //}

        //if (www.isNetworkError)
        //{
        //    Debug.Log(www.error);
        //}
        //else
        //{
        //    // Show results as text
        //    Debug.Log(www.downloadHandler.text);
        //
        //    // Json serialise result
        //}

        yield return new ChessGameHistory(File.ReadAllText("C:/Users/Thomas/Downloads/MagnusCarlsen_vs_JSP.json"));
    }
}
