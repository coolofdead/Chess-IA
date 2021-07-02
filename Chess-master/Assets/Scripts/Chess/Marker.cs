using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker {

    public bool IsChoosable { get { return Type != MarkerType.CHECK; } }
    public MarkerType Type { set; get; }
    public bool IsCastle { set; get; }
    public bool IsEnPassant { set; get; }
    public Field EnPassantField { set; get; }

    public Marker(MarkerType type)
    {
        this.Type = type;
    }

    public void Reset()
    {
        IsCastle = false;
        IsEnPassant = false;
    }
}

public enum MarkerType
{
    MOVE,
    CAPTURE,
    CHECK
}