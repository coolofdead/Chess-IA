using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public King(Vector2Int position, bool isWhite) : base(position, isWhite, PieceType.K)
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
}
