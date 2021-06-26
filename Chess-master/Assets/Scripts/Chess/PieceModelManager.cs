using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PieceModelManager : MonoBehaviour {

    public Chess chess;
    public Tilemap tilemap;

    [Header("White")]
    public TileBase kingWhite;
    public TileBase queenWhite;
    public TileBase bishopWhite;
    public TileBase knightWhite;
    public TileBase rookWhite;
    public TileBase pawnWhite;

    [Header("Black")]
    public TileBase kingBlack;
    public TileBase queenBlack;
    public TileBase bishopBlack;
    public TileBase knightBlack;
    public TileBase rookBlack;
    public TileBase pawnBlack;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 positionRaw = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int position = new Vector2Int((int)positionRaw.x, (int)positionRaw.y);
            
            Field field = chess.GetField(position);
            if (field != null)
            {
                chess.OnPieceChosen(chess.GetField(position));
            }
        }
    }

    public void ShowPieceModel(Field field)
    {
        tilemap.SetTile(tilemap.WorldToCell(field.Get3DPosition()), PieceToTileBase(field.Piece));
    }

    public void ShowNoPieceModel(Field field)
    {
        tilemap.SetTile(tilemap.WorldToCell(field.Get3DPosition()), null);
    }

    public TileBase PieceToTileBase(Piece piece)
    {
        if (piece.IsWhite)
        {
            switch (piece.Type)
            {
                case PieceType.K:
                    return kingWhite;

                case PieceType.Q:
                    return queenWhite;

                case PieceType.B:
                    return bishopWhite;

                case PieceType.N:
                    return knightWhite;

                case PieceType.R:
                    return rookWhite;

                case PieceType.P:
                    return pawnWhite;
            }
        }

        else
        {
            switch (piece.Type)
            {
                case PieceType.K:
                    return kingBlack;

                case PieceType.Q:
                    return queenBlack;

                case PieceType.B:
                    return bishopBlack;

                case PieceType.N:
                    return knightBlack;

                case PieceType.R:
                    return rookBlack;

                case PieceType.P:
                    return pawnBlack;
            }
        }

        return null;
    }
}
