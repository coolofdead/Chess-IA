using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public Queen(Vector2Int position, bool isWhite) : base(position, isWhite, PieceType.Q)
    {
    }

    public override bool CanMakeMove(Move move)
    {
        return move.IsMoveVertical() || move.IsMoveHorizontal() || move.IsMoveSideWays();
    }
}
