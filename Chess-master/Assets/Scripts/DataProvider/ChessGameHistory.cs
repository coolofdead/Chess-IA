using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text.RegularExpressions;

public class ChessGameHistory : IEnumerable<KeyValuePair<bool, string>>
{
    private const string REGEX_MATCH_PGN = @"\b(?<=\s)\w{0,2}[a-z]\d\b";
    private const string REGEX_MATCH_RESULT = @"(white|black)"":{""rating"":\d+,""result"":""win";

    private MatchCollection moves;
    public readonly ChessGameResult Result;

    public ChessGameHistory(string jsonHistory)
    {
        moves = (new Regex(REGEX_MATCH_PGN)).Matches(jsonHistory);
        var regex = (new Regex(REGEX_MATCH_RESULT)).Match(jsonHistory);

        string matchResult = regex.Groups[1].Value.ToLower();

        Result = matchResult == "white" ? ChessGameResult.WHITE_WIN : matchResult == "black" ? ChessGameResult.BLACK_WIN : ChessGameResult.DRAW;
    }

    public IEnumerator<KeyValuePair<bool, string>> GetEnumerator()
    {
        bool isWhite = true;

        foreach (Match move in moves)
            yield return new KeyValuePair<bool, string>(isWhite, move.Value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}

public enum ChessGameResult
{
    BLACK_WIN,
    WHITE_WIN,
    DRAW,
}