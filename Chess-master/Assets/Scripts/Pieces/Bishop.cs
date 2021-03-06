using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public Bishop(Vector2Int position, bool isWhite) : base(position, isWhite, PieceType.B)
    {
    }

    public Bishop(Vector2Int position, bool isWhite, PieceType type, bool MadeFirstMove = false) : base(position, isWhite, type, MadeFirstMove)
    {
    }

    public override bool CanMakeMove(Move move)
    {
        return move.IsMoveSideWays();
    }

    public override Piece Clone()
    {
        return new Bishop(position, this.IsWhite, this.Type, this.MadeFirstMove);
    }
}
