using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public King(Vector2Int position, bool isWhite) : base(position, isWhite, PieceType.K)
    {
    }

    public King(Vector2Int position, bool isWhite, PieceType type, bool MadeFirstMove = false) : base(position, isWhite, type, MadeFirstMove)
    {
    }

    public override bool CanMakeMove(Move move)
    {
        if (move.IsMoveLegthOne())
        {
            return move.IsMoveHorizontal() || move.IsMoveVertical() || move.IsMoveSideWays();
        }

        return false;
    }

    public override Piece Clone()
    {
        return new King(position, this.IsWhite, this.Type, this.MadeFirstMove);
    }
}
