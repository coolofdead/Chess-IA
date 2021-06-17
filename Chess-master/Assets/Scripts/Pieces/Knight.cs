using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public Knight(Vector2Int position, bool isWhite) : base(position, isWhite, PieceType.N)
    {
    }

    public Knight(Vector2Int position, bool isWhite, PieceType type, bool MadeFirstMove = false) : base(position, isWhite, type, MadeFirstMove)
    {
    }

    public override bool CanMakeMove(Move move)
    {
        return move.IsMoveLShaped();
    }

    public override Piece Clone()
    {
        return new Knight(position, this.IsWhite, this.Type, this.MadeFirstMove);
    }
}
