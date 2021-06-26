using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece  {

    protected Vector2Int position;
    protected bool hasMooved;
    public bool MadeFirstMove { get; set; }

    public bool IsWhite { get; set; }
    public PieceType Type { get; }

    public bool IsKing()
    {
        return Type == PieceType.K;
    }

    public bool HasMooved()
    {
        return hasMooved;
    }

    public Piece(Vector2Int position, bool isWhite, PieceType type)
    {
        this.position = position;
        this.IsWhite = isWhite;
        this.MadeFirstMove = false;
        this.Type = type;
    }

    public virtual bool CanMakeCaptureMove(Move move)
    {
        return CanMakeMove(move);
    }

    public abstract bool CanMakeMove(Move move);

    public virtual bool CanPromote()
    {
        return false;
    }

    public virtual void Move(Vector2Int newPosition)
    {
        hasMooved = true;
        this.position = newPosition;
    }
}

public enum PieceType
{
    K = 0,
    Q = 1,
    B = 2,
    N = 3,
    R = 4,
    P = 5
}