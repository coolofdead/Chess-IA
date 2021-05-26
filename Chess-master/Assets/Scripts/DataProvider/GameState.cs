using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GameState
{
    public int state;
    public string action;
    public int reward;
    public int nextState;
}