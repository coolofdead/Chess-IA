using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public Rook(Vector2Int position, bool isWhite) : base(position, isWhite, PieceType.R)
    {
    }

    public override bool CanMakeMove(Move move)
    {
        return move.IsMoveHorizontal() || move.IsMoveVertical();
    }
}
