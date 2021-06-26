using System.Collections;
using System.Collections.Generic;

public interface IDataProvidable
{
    IEnumerable<ChessGameHistory> GetGamesHistoryForPlayer(string playerName, int startYear);
}
