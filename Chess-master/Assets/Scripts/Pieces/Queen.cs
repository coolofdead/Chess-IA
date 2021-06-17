using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public Queen(Vector2Int position, bool isWhite) : base(position, isWhite, PieceType.Q)
    {
    }

    public Queen(Vector2Int position, bool isWhite, PieceType type, bool MadeFirstMove = false) : base(position, isWhite, type, MadeFirstMove)
    {
    }

    public override bool CanMakeMove(Move move)
    {
        return move.IsMoveVertical() || move.IsMoveHorizontal() || move.IsMoveSideWays();
    }

    public override Piece Clone()
    {
        return new Queen(position, this.IsWhite, this.Type, this.MadeFirstMove);
    }
}
