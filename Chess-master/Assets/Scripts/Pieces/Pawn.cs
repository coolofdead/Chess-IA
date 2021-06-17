using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public bool hasMovedForwardTwoLastMove;
    public bool HasMovedForwardTwoLastMove { private set { hasMovedForwardTwoLastMove = value; } get { return  hasMovedForwardTwoLastMove && turnMoved == Chess.Turn - 1; } }
    private int turnMoved;

    public Pawn(Vector2Int position, bool isWhite) : base(position, isWhite, PieceType.P)
    {
    }

    public Pawn(Vector2Int position, bool isWhite, PieceType type, bool MadeFirstMove = false) : base(position, isWhite, type, MadeFirstMove)
    {
    }

    public override Piece Clone()
    {
        return new Pawn(position, this.IsWhite, this.Type, this.MadeFirstMove);
    }

    public override bool CanMakeMove(Move move)
    {
        if (move.IsMoveForward(IsWhite) && move.IsMoveVertical())
        {
            if (MadeFirstMove)
            {
                return move.IsMoveLegthOne();
            }

            return move.IsMoveLegthOne() || move.IsMoveLengthTwo();
        }

        return false;
    }

    public override bool CanMakeCaptureMove(Move move)
    {
        return move.IsMoveForward(IsWhite) && move.IsMoveLegthOne() && move.IsMoveSideWays();
    }

    public override bool CanPromote()
    {
        if (IsWhite)
        {
            if (position.y == 7)
            {
                return true;
            }
        }

        else
        {
            if (position.y == 0)
            {
                return true;
            }
        }

        return false;
    }

    public override void Move(Vector2Int newPosition)
    {
        if (Mathf.Abs(newPosition.y - position.y) == 2)
        {
            HasMovedForwardTwoLastMove = true;
            turnMoved = Chess.Turn;
        }

        base.Move(newPosition);
    }
}
