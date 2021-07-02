using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece  {

    protected Vector2Int position;
    public Vector2Int Position { get { return position; } }
    protected bool hasMooved;
    public bool MadeFirstMove { get; set; }

    public bool IsWhite { get; set; }
    public PieceType Type { get; }

    private static List<Piece> piecesPool = new List<Piece>() {
        new Bishop(new Vector2Int(), true),
        new Bishop(new Vector2Int(), true),
        new Bishop(new Vector2Int(), false),
        new Bishop(new Vector2Int(), false),
        new Knight(new Vector2Int(), true),
        new Knight(new Vector2Int(), true),
        new Knight(new Vector2Int(), false),
        new Knight(new Vector2Int(), false),
        new Rook(new Vector2Int(), true),
        new Rook(new Vector2Int(), true),
        new Rook(new Vector2Int(), false),
        new Rook(new Vector2Int(), false),
        new Queen(new Vector2Int(), true),
        new Queen(new Vector2Int(), false),
        new Queen(new Vector2Int(), true),
        new Queen(new Vector2Int(), false),
        new Queen(new Vector2Int(), true),
        new Queen(new Vector2Int(), false),
        new King(new Vector2Int(), true),
        new King(new Vector2Int(), false),
        new Pawn(new Vector2Int(), true),
        new Pawn(new Vector2Int(), true),
        new Pawn(new Vector2Int(), true),
        new Pawn(new Vector2Int(), true),
        new Pawn(new Vector2Int(), true),
        new Pawn(new Vector2Int(), true),
        new Pawn(new Vector2Int(), true),
        new Pawn(new Vector2Int(), true),
        new Pawn(new Vector2Int(), false),
        new Pawn(new Vector2Int(), false),
        new Pawn(new Vector2Int(), false),
        new Pawn(new Vector2Int(), false),
        new Pawn(new Vector2Int(), false),
        new Pawn(new Vector2Int(), false),
        new Pawn(new Vector2Int(), false),
        new Pawn(new Vector2Int(), false),
    };

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

    public static void Release(Piece piece, bool d = false)
    {
        piecesPool.Add(piece);
    }

    public static Piece Create(Vector2Int position, bool isWhite, PieceType type, bool MadeFirstMove = false)
    {
        Piece p = piecesPool.Find((Piece piece) => piece.Type == type && piece.IsWhite == isWhite);

        piecesPool.Remove(p);

        p.position = position;
        p.MadeFirstMove = MadeFirstMove;

        return p;
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
    K = 99,
    Q = 10,
    B = 4,
    N = 3,
    R = 5,
    P = 1,
    None = 0,
}