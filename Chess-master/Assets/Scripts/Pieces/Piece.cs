using System;
using UnityEngine;

public abstract class Piece  {

    protected Vector2Int position;
    public Vector2Int Position { get { return position; } }
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

    public Piece()
    {

    }

    public Piece(Vector2Int position, bool isWhite, PieceType type, bool MadeFirstMove = false)
    {
        this.position = position;
        this.IsWhite = isWhite;
        this.MadeFirstMove = MadeFirstMove;
        this.Type = type;
    }

    public abstract Piece Clone();

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

    public override string ToString()
    {
        switch (this.Type)
        {
            case PieceType.B:
                return "Bishop";
            case PieceType.K:
                return "King";
            case PieceType.N:
                return "Knight";
            case PieceType.P:
                return "Pawn";
            case PieceType.Q:
                return "Queen";
            case PieceType.R:
                return "Rook";
            default:
                return "???";
        }
    }
}

public enum PieceType
{
    K = 999,
    Q = 10,
    B = 4,
    N = 3,
    R = 5,
    P = 1
}