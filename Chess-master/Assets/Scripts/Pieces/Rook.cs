using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public Rook(Vector2Int position, bool isWhite) : base(position, isWhite, PieceType.R)
    {
    }

    public Rook(Vector2Int position, bool isWhite, PieceType type, bool MadeFirstMove = false) : base(position, isWhite, type, MadeFirstMove)
    {
    }

    public override bool CanMakeMove(Move move)
    {
        return move.IsMoveHorizontal() || move.IsMoveVertical();
    }

    public override Piece Clone()
    {
        return new Rook(position, this.IsWhite, this.Type, this.MadeFirstMove);
    }
}
