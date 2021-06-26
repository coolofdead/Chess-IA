using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker {

    public bool IsChoosable { get; }
    public MarkerType Type { get; }
    public bool IsCastle { set; get; }
    public bool IsEnPassant { set; get; }
    public Field EnPassantField { set; get; }

    public Marker(MarkerType type)
    {
        if (type == MarkerType.CHECK)
        {
            this.IsChoosable = false;
        }

        else
        {
            this.IsChoosable = true;
        }

        this.Type = type;
    }
}

public enum MarkerType
{
    MOVE,
    CAPTURE,
    CHECK
}