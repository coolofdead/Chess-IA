using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Chess : MonoBehaviour {

    [Header("UI")]
    public Text player1Waiting;
    public Text player2Waiting;
    public Text player1Playing;
    public Text player2Playing;
    public Tilemap player1Pieces;
    public Tilemap player2Pieces;
    public GameObject promotionTiles;
    public GameObject promotionPanel;
    public GameObject endScreen;
    public Text endScreenInfo;

    public HighlightingManager highlightingManager;
    public PieceModelManager pieceModelManager;

    private Field selectedField = null;
    private Field promoteField = null;
    public bool isWhiteAtTurn = true;
    public static int Turn { private set; get; }

    private Field[,] fields = new Field[8, 8];

    public override int GetHashCode()
    {
        string chessField = "";

        foreach (Field f in fields)
        {
            chessField += f.Piece == null ? "" : f.Piece.ToString() + f.AlphaNumericPosition;
        }

        Debug.Log(chessField);

        return chessField.GetHashCode();
    }

    private void Start()
    {
        for (int x = 0; x <= fields.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= fields.GetUpperBound(1); y++)
            {
                fields[x, y] = new Field(x, y);
            }
        }

        GetHashCode();

        SetUpField();

        player1Waiting.enabled = false;
        player1Playing.enabled = true;
        player2Waiting.enabled = true;
        player2Playing.enabled = false;
    }

    public Field GetField(Vector2Int position)
    {
        if (fields.GetUpperBound(0) >= position.x && fields.GetUpperBound(1) >= position.y)
        {
            return fields[position.x, position.y];
        }

        return null;
    }

    public Field GetKingField(bool isWhite)
    {
        foreach (Field field in fields)
        {
            if (field.IsOccupied() && field.Piece.Type == PieceType.K && field.Piece.IsWhite == isWhite)
            {
                return field;
            }
        }

        return null;
    }

    private Field GetKing(bool isWhite)
    {
        foreach (Field field in fields)
        {
            if (field.Piece == null)
                continue;

            if (field.Piece.IsKing() && field.Piece.IsWhite == isWhite)
                return field;
        }

        return null;
    }

    //EVENT
    public void OnPieceChosen(Field chosenField)
    {
        if (chosenField.Piece == null)
            return;

        if (chosenField.Piece.IsWhite == isWhiteAtTurn)
        {
            this.selectedField = chosenField;
            //Clear move and capture marks
            ClearMarks(false);
            Piece piece = chosenField.Piece;

            foreach (Field to in fields)
            {
                // Castle
                if (CanCastle(chosenField, to))
                {
                    to.Marker = new Marker(MarkerType.MOVE);
                    to.Marker.IsCastle = true;
                    highlightingManager.ShowOption(to);
                }

                if (CanEnPassant(chosenField, to))
                {
                    var enPassantField = fields[to.Position.x, to.Position.y + (isWhiteAtTurn ? 1 : -1)];
                    enPassantField.Marker = new Marker(MarkerType.CAPTURE);
                    enPassantField.Marker.IsEnPassant = true;
                    enPassantField.Marker.EnPassantField = to;
                    highlightingManager.ShowOption(enPassantField);
                }

                foreach (Field moveField in GetMoveFieldsAdvanced(chosenField))
                {
                    moveField.Marker = new Marker(MarkerType.MOVE);
                    highlightingManager.ShowOption(moveField);
                }

                foreach (Field captureField in GetCaptureFieldsAdvanced(chosenField, false))
                {
                    captureField.Marker = new Marker(MarkerType.CAPTURE);
                    highlightingManager.ShowOption(captureField);
                }
            }
        }
    }

    private List<Field> GetMoveFieldsAdvanced(Field from)
    {
        List<Field> potentialFields = new List<Field>();

        foreach (Field to in fields)
        {
            if (IsMoveGenerallyValid(from, to))
            {
                to.Piece = from.Piece;
                from.Piece = null;
                if (!WouldBeCheck(to.Piece.IsWhite))
                {
                    potentialFields.Add(to);
                }
                from.Piece = to.Piece;
                to.Piece = null;
            }
        }

        return potentialFields;
    }

    private List<Field> GetMoveFields(Field from)
    {
        List<Field> potentialFields = new List<Field>();

        foreach (Field to in fields)
        {
            if (IsMoveGenerallyValid(from, to))
            {
                potentialFields.Add(to);
            }
        }

        return potentialFields;
    }

    private bool IsMoveGenerallyValid(Field from, Field to)
    {
        if (to != from)
        {
            Move move = from.GetMoveTo(to);

            //If field to got is not occupied
            if (!to.IsOccupied())
            {
                //Check if one can move to the field
                if (from.Piece.CanMakeMove(move))
                {
                    //Check if there is no piece between (except knight)
                    if (!IsAnyPieceInTheWay(from, move))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private List<Field> GetCaptureFieldsAdvanced(Field from, bool ignoreChess)
    {
        List<Field> potentialFields = new List<Field>();

        foreach (Field to in fields)
        {
            if (IsCaptureGenerallyValid(from, to))
            {
                Piece toCopy = to.Piece;
                to.Piece = from.Piece;
                from.Piece = null;
                if (ignoreChess || !WouldBeCheck(to.Piece.IsWhite))
                {
                    potentialFields.Add(to);
                }
                from.Piece = to.Piece;
                to.Piece = toCopy;
            }
        }

        return potentialFields;
    }

    private List<Field> GetCaptureFields(Field from)
    {
        List<Field> potentialFields = new List<Field>();

        foreach (Field to in fields)
        {
            if (IsCaptureGenerallyValid(from, to))
            {
                potentialFields.Add(to);
            }
        }

        return potentialFields;
    }

    public bool IsCaptureGenerallyValid(Field from, Field to)
    {
        //Check if field is not current field
        if (to != from)
        {
            Move move = from.GetMoveTo(to);

            //If field to got is not occupied
            if (to.IsOccupied())
            {
                //Check if one can move to the field
                if (from.Piece.CanMakeCaptureMove(move))
                {
                    //Check if opponents piece
                    if (to.Piece.IsWhite != from.Piece.IsWhite)
                    {
                        //Check if there is no piece in the way
                        if (!IsAnyPieceInTheWay(from, move))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private bool WouldBeCheck(bool isWhite)
    {
        foreach (Field from in fields)
        {
            if (from.IsOccupied())
            {
                if (GetCaptureFields(from).Contains(GetKingField(isWhite)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CanMakeAnyLegalMove(bool isWhite)
    {
        foreach (Field field in fields)
        {
            if (field.IsOccupied() && field.Piece.IsWhite == isWhite)
            {
                if (GetCaptureFieldsAdvanced(field, false).Count > 0 || GetMoveFieldsAdvanced(field).Count > 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CanEnPassant(Field from, Field to)
    {
        if (from.Piece == null || to.Piece == null)
            return false;

        if (from.Piece.Type != PieceType.P || to.Piece.Type != PieceType.P)
            return false;

        if (!((Pawn)to.Piece).HasMovedForwardTwoLastMove)
            return false;

        return from.Position.y == to.Position.y && (Mathf.Abs(from.Position.x - to.Position.x) == 1);
    }

    private bool CanCastle(Field king, Field rook)
    {
        if (king.Piece == null || rook.Piece == null)
            return false;

        if (!king.Piece.IsKing() || rook.Piece.Type != PieceType.R)
            return false;

        if (king.Piece.HasMooved() || rook.Piece.HasMooved())
            return false;

        return !IsAnyPieceInTheWay(king, king.GetMoveTo(rook));
    }

    private bool IsAnyPieceInTheWay(Field start, Move move)
    {
        if (start.Piece.Type != PieceType.N)
        {
            foreach (Move part in move.GetMoveParts())
            {
                Field nextField = GetField(start.Position + part.GetVector());
                if (nextField.Position != start.Position + move.GetVector() && nextField.IsOccupied())
                {
                    return true;
                }
            }
        }

        return false;
    }

    //EVENT
    public void OnMarkerChosen(Field field)
    {
        var previousGameState = GetHashCode();

        if (field.Marker != null)
        {
            switch (field.Marker.Type)
            {
                case MarkerType.MOVE:
                    selectedField.Piece.MadeFirstMove = true;
                    selectedField.Piece.Move(field.Position);

                    // Castle
                    if (field.Marker.IsCastle)
                    {
                        Castle(GetKing(isWhiteAtTurn), field);
                        Turn++;
                    }
                    else
                    {
                        MovePiece(selectedField, field);
                    }
                    break;

                case MarkerType.CAPTURE:
                    var fieldToMove = field.Marker.IsEnPassant ? field.Marker.EnPassantField : field;

                    selectedField.Piece.MadeFirstMove = true;
                    selectedField.Piece.Move(fieldToMove.Position);
                    SendPieceToGraveyard(fieldToMove);
                    MovePiece(selectedField, field);
                    break;

                default:
                    return;
            }

            ClearMarks(true);

            //Check if pawn can promote
            if (field.Piece.CanPromote())
            {
                promoteField = field;
                promotionTiles.SetActive(true);
                promotionPanel.SetActive(true);
            }
        }

        // Nécessaire ?
        //var gameState = new GameState
        //{
        //    state = previousGameState,
        //    nextState = GetHashCode(), // Set as 0, it'll be set once the next player play a move or stay as 0 if the game is over
        //    action = field.Piece.Type.ToString() + field.AlphaNumericPosition,
        //    reward = !CanMakeAnyLegalMove(!isWhiteAtTurn) ? 0 : -1
        //};

        //Check if enemy king field is check
        Field kingField = GetKingField(!isWhiteAtTurn);
        if (WouldBeCheck(kingField.Piece.IsWhite))
        {
            kingField.Marker = new Marker(MarkerType.CHECK);
            highlightingManager.ShowOption(kingField);

            if (!CanMakeAnyLegalMove(!isWhiteAtTurn))
            {
                ShowEndScreen(false, isWhiteAtTurn);
            }
        }
        else
        {
            if (!CanMakeAnyLegalMove(!isWhiteAtTurn))
            {
                ShowEndScreen(true, false);
            }
        }

        if (field.Marker == null)
        {
            player1Waiting.enabled = !player1Waiting.enabled;
            player1Playing.enabled = !player1Playing.enabled;
            player2Waiting.enabled = !player2Waiting.enabled;
            player2Playing.enabled = !player2Playing.enabled;

            isWhiteAtTurn = !isWhiteAtTurn;
        }
    }

    private void SetUpField()
    {
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                Field fieldW = fields[x, y];
                Field fieldB = fields[x, 7 - y];

                if (y == 1)
                {
                    fieldW.Piece = new Pawn(fieldW.Position, true);
                    fieldB.Piece = new Pawn(fieldB.Position, false);
                }

                else if (x == 0 || x == 7)
                {
                    fieldW.Piece = new Rook(fieldW.Position, true);
                    fieldB.Piece = new Rook(fieldB.Position, false);
                }

                else if (x == 1 || x == 6)
                {
                    fieldW.Piece = new Knight(fieldW.Position, true);
                    fieldB.Piece = new Knight(fieldB.Position, false);
                }

                else if (x == 2 || x == 5)
                {
                    fieldW.Piece = new Bishop(fieldW.Position, true);
                    fieldB.Piece = new Bishop(fieldB.Position, false);
                }

                else if (x == 3)
                {
                    fieldW.Piece = new Queen(fieldW.Position, true);
                    fieldB.Piece = new Queen(fieldB.Position, false);
                }

                else if (x == 4)
                {
                    fieldW.Piece = new King(fieldW.Position, true);
                    fieldB.Piece = new King(fieldB.Position, false);
                }

                CreatePiece(fieldW);
                CreatePiece(fieldB);
            }
        }
    }

    private void CreatePiece(Field field)
    {
        pieceModelManager.ShowPieceModel(field);
    }

    private void MovePiece(Field now, Field after)
    {
        after.Piece = now.Piece;
        pieceModelManager.ShowPieceModel(after);
        pieceModelManager.ShowNoPieceModel(now);
        now.Piece = null;

        Turn++;
    }

    private void Castle(Field king, Field rook)
    {
        Vector2Int newKingPos;
        Vector2Int newRookPos;

        // Castle king side
        if (rook.Position.x > king.Position.x)
        {
            newKingPos = new Vector2Int(rook.Position.x - 1, rook.Position.y);
            newRookPos = new Vector2Int(king.Position.x + 1, king.Position.y);
        }
        else
        {
            newKingPos = new Vector2Int(rook.Position.x + 2, rook.Position.y);
            newRookPos = new Vector2Int(king.Position.x - 1, king.Position.y);
        }

        fields[newKingPos.x, newKingPos.y].Piece = king.Piece;
        fields[newRookPos.x, newRookPos.y].Piece = rook.Piece;

        pieceModelManager.ShowPieceModel(fields[newKingPos.x, newKingPos.y]);
        pieceModelManager.ShowPieceModel(fields[newRookPos.x, newRookPos.y]);

        pieceModelManager.ShowNoPieceModel(king);
        pieceModelManager.ShowNoPieceModel(rook);
    }

    private void SendPieceToGraveyard(Field now)
    {
        if (isWhiteAtTurn)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (player2Pieces.GetTile(new Vector3Int(x, y, 0)) == null)
                    {
                        player2Pieces.SetTile(new Vector3Int(x, y, 0), pieceModelManager.PieceToTileBase(now.Piece));
                        pieceModelManager.ShowNoPieceModel(now);
                        now.Piece = null;
                        return;
                    }
                }
            }

        }

        else
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (player1Pieces.GetTile(new Vector3Int(x, y, 0)) == null)
                    {
                        player1Pieces.SetTile(new Vector3Int(x, y, 0), pieceModelManager.PieceToTileBase(now.Piece));
                        pieceModelManager.ShowNoPieceModel(now);
                        now.Piece = null;
                        return;
                    }
                }
            }
        }
    }

    private void CreateMark(Field field)
    {
        highlightingManager.ShowOption(field);
    }

    private void ClearMarks(bool full)
    {
        highlightingManager.ClearAllOptions();
        foreach (Field field in fields)
        {
            if (field.Marker != null)
            {
                if (field.Marker.IsChoosable || full)
                {
                    field.Marker = null;
                }

                else
                {
                    highlightingManager.ShowOption(field);
                }
            }
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene(2);
    }

    public void Surrender()
    {
        //Current player has lost
        ShowEndScreen(false, !isWhiteAtTurn);
    }

    public void Revance()
    {
        SceneManager.LoadScene(7);
    }

    private void ShowEndScreen(bool draw, bool hasWhiteWon)
    {
        if (draw)
        {
            endScreenInfo.text = "Draw";
        }

        else
        {
            if (hasWhiteWon)
            {
                endScreenInfo.text = "Player1 won";
            }

            else
            {
                endScreenInfo.text = "Player2 won";
            }
        }

        endScreen.SetActive(true);
    }

    public void SelectPromotion(int type)
    {
        promotionTiles.SetActive(false);
        promotionPanel.SetActive(false);

        pieceModelManager.ShowNoPieceModel(promoteField);

        switch (type)
        {
            case 1:
                promoteField.Piece = new Queen(promoteField.Position, !isWhiteAtTurn);
                break;

            case 2:
                promoteField.Piece = new Knight(promoteField.Position, !isWhiteAtTurn);
                break;

            case 3:
                promoteField.Piece = new Rook(promoteField.Position, !isWhiteAtTurn);
                break;

            case 4:
                promoteField.Piece = new Bishop(promoteField.Position, !isWhiteAtTurn);
                break;
        }

        CreatePiece(promoteField);

        selectedField = promoteField;
        OnMarkerChosen(promoteField);
    }
}