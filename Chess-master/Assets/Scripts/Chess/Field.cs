using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field {

    public Vector2Int Position { get; private set; }
    public Piece Piece { get; set; }
    public Marker Marker { get; set; }

    public bool edited = false;

    private static List<Move> movesPool = new List<Move> {
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
        new Move(),
    };
    private static int indexMovePool = 0;
    private static Move currentMoveFromPool
    {
        get
        {
            indexMovePool = indexMovePool + 1 >= movesPool.Count ? 0 : indexMovePool + 1;
            return movesPool[indexMovePool];
        }
    }

    public string AlphaNumericPosition
    {
        get {
            List<string> xName = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h" };
            List<string> yName = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8" };
            return xName[Position.x] + yName[Position.y];
        }
    }

    public Field(int x, int y)
    {
        this.Position = new Vector2Int(x, y);
        this.Piece = null;
    }

    public bool IsOccupied()
    {
        return Piece != null;
    }

    public Move GetMoveTo(Field field)
    {
        var move = currentMoveFromPool;
        move.Set(field.Position - this.Position);
        return move;
    }

    public Vector3Int Get3DPosition()
    {
        return new Vector3Int(Position.x, Position.y, 0);
    }
}
