using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move {

    public int X { get; set; }
    public int Y { get; set; }

    public Vector2Int GetVector()
    {
        return new Vector2Int(X, Y);
    }

    public Move(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public Move(Vector2Int vector2Int)
    {
        this.X = vector2Int.x;
        this.Y = vector2Int.y;
    }

    public List<Move> GetMoveParts()
    {
        List<Move> moves = new List<Move>();
        Move currentMove = new Move(0, 0);
        for (int step = 0; step < Mathf.Abs(this.X) || step < Mathf.Abs(this.Y); step++)
        {
            if (step < Mathf.Abs(this.X))
            {
                currentMove.X += (int)Mathf.Sign(this.X);
            }

            if (step < Mathf.Abs(this.Y))
            {
                currentMove.Y += (int)Mathf.Sign(this.Y);
            }

            moves.Add(new Move(currentMove.X, currentMove.Y));
        }

        moves.RemoveAt(moves.Count - 1);
        return moves;
    }

    public bool IsMoveSideWays()
    {
        //Seitliche Bewegung
        if (Mathf.Abs(X) == Mathf.Abs(Y))
        {
            return true;
        }

        return false;
    }

    public bool IsMoveHorizontal()
    {
        //Horizontale Bewegung
        if (Y == 0)
        {
            return true;
        }

        return false;
    }

    public bool IsMoveVertical()
    {
        //Vertikale Bewegung
        if (X == 0)
        {
            return true;
        }

        return false;
    }

    public bool IsMoveLegthOne()
    {
        //Eine Bewegung um ein Feld
        if (Mathf.Abs(X) <= 1 && Mathf.Abs(Y) <= 1)
        {
            return true;
        }

        return false;
    }

    public bool IsMoveLengthTwo()
    {
        //Eine Bewegung um twi Felder
        if (Mathf.Abs(X) <= 2 && Mathf.Abs(Y) <= 2)
        {
            return true;
        }

        return false;
    }

    public bool IsMoveForward(bool isWhite)
    {
        if (isWhite)
        {
            if (Y > 0)
            {
                return true;
            }
        }

        else
        {
            if (Y < 0)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsMoveLShaped()
    {
        if (Mathf.Abs(X) == 2 && Mathf.Abs(Y) == 1)
        {
            return true;
        }

        if (Mathf.Abs(X) == 1 && Mathf.Abs(Y) == 2)
        {
            return true;
        }

        return false;
    }
}