using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightingManager : MonoBehaviour {

    public Chess chess;
    public TileBase moveMarker;
    public TileBase captureMarker;
    public TileBase checkMarker;

    [HideInInspector]
    public Tilemap tilemap;

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
                if (field.Marker != null)
                {
                    chess.OnMarkerChosen(chess.GetField(position));
                }
            }
        }
    }

    public void ShowOption(Field field)
    {
        switch (field.Marker.Type)
        {
            case MarkerType.MOVE:
                tilemap.SetTile(tilemap.WorldToCell(field.Get3DPosition()), moveMarker);
                break;

            case MarkerType.CAPTURE:
                tilemap.SetTile(tilemap.WorldToCell(field.Get3DPosition()), captureMarker);
                break;

            case MarkerType.CHECK:
                tilemap.SetTile(tilemap.WorldToCell(field.Get3DPosition()), checkMarker);
                break;
        }
    }

    public void ClearAllOptions()
    {
        tilemap.ClearAllTiles();
    }
}
