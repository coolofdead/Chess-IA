using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public Knight(Vector2Int position, bool isWhite) : base(position, isWhite, PieceType.N)
    {
    }

    public override bool CanMakeMove(Move move)
    {
        return move.IsMoveLShaped();
    }
}
