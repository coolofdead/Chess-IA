using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field {

    public Vector2Int Position { get; private set; }
    public Piece Piece { get; set; }
    public Marker Marker { get; set; }

    public bool edited = false;

    public string AlphaNumericPosition
    {
        get {
            List<string> xName = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h" };
            List<string> yName = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8" };
            return xName[Position.x] + yName[Position.y];
        }
    }

    public Field(int x, int y)
    {
        this.Position = new Vector2Int(x, y);
        this.Piece = null;
    }

    public Field Clone()
    {
        Field clone = new Field(Position.x, Position.y);

        clone.Piece = Piece?.Clone();
        clone.Marker = Marker?.Clone();

        return clone;
    }

    public bool IsOccupied()
    {
        return Piece != null;
    }

    public Move GetMoveTo(Field field)
    {
        return new Move(field.Position - this.Position);
    }

    public Vector3Int Get3DPosition()
    {
        return new Vector3Int(Position.x, Position.y, 0);
    }
}
