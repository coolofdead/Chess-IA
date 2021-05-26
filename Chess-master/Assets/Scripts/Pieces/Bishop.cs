using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public Bishop(Vector2Int position, bool isWhite) : base(position, isWhite, PieceType.B)
    {
    }

    public override bool CanMakeMove(Move move)
    {
        return move.IsMoveSideWays();
    }
}
