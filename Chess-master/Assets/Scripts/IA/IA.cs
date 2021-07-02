using System.Collections.Generic;
using UnityEngine;
using System;

public class IA
{
    private const int MAX_DEPTH = 2;
    private int b = 0;

    private MovePredicted bestMove;
    private int bestScore;

    private Chess chess;
    public readonly bool playAsWhite;

    public struct MovePredicted
    {
        public Vector2Int piecePosition;
        public Vector2Int moveFromPiece;
    }

    public struct UglyMove
    {
        public PieceType piece;
        public bool whitePiece;
        public bool madeFirstMove;
        public Vector2Int fromPosition;
        public Vector2Int toPosition;
        public MarkerType moveType;
        public PieceType capturedPiece;
        public bool whiteCapturedPiece;
        public bool madeFirstMoveCapturedPiece;
    }

    public IA(Chess chess, bool playAsWhite)
    {
        this.chess = chess;
        this.playAsWhite = playAsWhite;
    }

    System.Diagnostics.Stopwatch everyPiecesWatch = new System.Diagnostics.Stopwatch();
    System.Diagnostics.Stopwatch allFieldMarkerWatch = new System.Diagnostics.Stopwatch();
    System.Diagnostics.Stopwatch pieceChoosenWatch = new System.Diagnostics.Stopwatch();
    System.Diagnostics.Stopwatch markerChoosenWatch = new System.Diagnostics.Stopwatch();
    
    private int FindNextMove(Field[,] board, bool whiteTurn, int currentDepth = 0, int score = 0)
    {
        if (currentDepth >= MAX_DEPTH)
            return score;

        if (score < bestScore)
            return score;

        b++;

        everyPiecesWatch.Start();
        // Get all IA pieces
        var pieces = chess.GetEveryPiecesForPlayer(whiteTurn);
        everyPiecesWatch.Stop();

        var min = 0;

        // Choose piece on field
        foreach (Field piece in pieces)
        {
            chess.isWhiteAtTurn = whiteTurn;

            pieceChoosenWatch.Start();
            // Check every moves available for that piece
            chess.OnPieceChosen(piece, true);
            pieceChoosenWatch.Stop();

            allFieldMarkerWatch.Start();
            var moves = chess.GetAllFieldWithMarker();
            allFieldMarkerWatch.Stop();

            // Check mate for one player
            if (!chess.CanMakeAnyLegalMove(whiteTurn))
            {
                return 999 * (whiteTurn == playAsWhite ? 1 : -1);
            }

            //Debug.Log($"{t} {piece.Piece.ToString()} at {piece.Position} a {moves.Count} moves");

            foreach (Field pieceMove in moves)
            {
                chess.isWhiteAtTurn = whiteTurn;

                pieceChoosenWatch.Start();
                chess.OnPieceChosen(piece, true);
                pieceChoosenWatch.Stop();
                //Debug.Log(c + " " + piece.Piece.ToString() + " to " + pieceMove.Position);

                if (bestMove.piecePosition.x == -1 && bestMove.piecePosition.y == -1 && whiteTurn == playAsWhite)
                {
                    //Debug.Log($"DEFAULT MOVE: {piece.Piece.ToString()} at {piece.Piece.Position} et move {pieceMove.Position}");
                    bestMove.piecePosition = new Vector2Int(piece.Position.x, piece.Position.y);
                    bestMove.moveFromPiece = new Vector2Int(pieceMove.Position.x, pieceMove.Position.y);
                }

                if (pieceMove.Marker.Type == MarkerType.CAPTURE)
                {
                    if (pieceMove.Piece == null)
                    {
                        //Debug.Log($"{piece.Piece.IsWhite} {piece.Piece.ToString()} {piece.Position} essaye de manger à {pieceMove.Position}");
                        continue;
                    }
                    //Debug.Log($"{t} can capture {pieceMove.Piece} at {pieceMove.Position} with {piece.Piece} at {piece.Position}");
                }

                UglyMove uglyMove = new UglyMove()
                {
                    piece = piece.Piece.Type,
                    whitePiece = piece.Piece.IsWhite,
                    madeFirstMove = piece.Piece.MadeFirstMove,
                    fromPosition = new Vector2Int(piece.Position.x, piece.Position.y),
                    toPosition = new Vector2Int(pieceMove.Position.x, pieceMove.Position.y),
                    moveType = pieceMove.Marker.Type,
                    capturedPiece = pieceMove.Marker.Type == MarkerType.CAPTURE ? pieceMove.Piece.Type : PieceType.None,
                    whiteCapturedPiece = pieceMove.Marker.Type == MarkerType.CAPTURE ? pieceMove.Piece.IsWhite : false,
                    madeFirstMoveCapturedPiece = pieceMove.Marker.Type == MarkerType.CAPTURE ? pieceMove.Piece.MadeFirstMove : false,
                };

                markerChoosenWatch.Start();
                // Play that move
                chess.OnMarkerChosen(pieceMove, piece);
                markerChoosenWatch.Stop();

                // Recu with opponent pieces
                var caputedPieceValue = uglyMove.capturedPiece == PieceType.None ? 0 : (int)uglyMove.capturedPiece * (whiteTurn == playAsWhite ? 1 : -1);

                int newScore = FindNextMove(board, !whiteTurn, currentDepth + 1, score + caputedPieceValue);

                min = newScore < min ? newScore : min;

                if (uglyMove.moveType == MarkerType.MOVE)
                {
                    Piece.Release(board[uglyMove.toPosition.x, uglyMove.toPosition.y].Piece);

                    board[uglyMove.toPosition.x, uglyMove.toPosition.y].Piece = null;
                    piece.Piece = Piece.Create(uglyMove.fromPosition, uglyMove.whitePiece, uglyMove.piece, uglyMove.madeFirstMove);
                }
                if (uglyMove.moveType == MarkerType.CAPTURE)
                {
                    Piece.Release(chess.fields[pieceMove.Position.x, pieceMove.Position.y].Piece, true);

                    piece.Piece = Piece.Create(uglyMove.fromPosition, uglyMove.whitePiece, uglyMove.piece, uglyMove.madeFirstMove);
                    board[uglyMove.toPosition.x, uglyMove.toPosition.y].Piece = Piece.Create(uglyMove.toPosition, uglyMove.whiteCapturedPiece, uglyMove.capturedPiece, uglyMove.madeFirstMoveCapturedPiece);
                }

                // Set best move
                if (newScore > bestScore && playAsWhite == whiteTurn && currentDepth == 0)
                {
                    //Debug.Log("New Best move " + t + " " + piece.Piece.ToString() + " at " + piece.Position + " move to " + pieceMove.Position + " with new score: " + newScore + " and score " + score + " at depth " + currentDepth);
                    bestMove.piecePosition = new Vector2Int(piece.Position.x, piece.Position.y);
                    bestMove.moveFromPiece = new Vector2Int(pieceMove.Position.x, pieceMove.Position.y);
                    this.bestScore = newScore;
                }
            }
        }

        return min;
    }

    public MovePredicted GetNextMove()
    {
        b = 0;
        bestScore = -9999;
        bestMove = new MovePredicted() { piecePosition = new Vector2Int(-1, -1) };

        var turn = Chess.Turn;

        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

        stopWatch.Start();

        FindNextMove(chess.fields, playAsWhite);

        stopWatch.Stop();

        Debug.Log($"{stopWatch.Elapsed.TotalSeconds} secs");
        Debug.Log(everyPiecesWatch.Elapsed.TotalSeconds);
        Debug.Log(allFieldMarkerWatch.Elapsed.TotalSeconds);
        Debug.Log(pieceChoosenWatch.Elapsed.TotalSeconds);
        Debug.Log(markerChoosenWatch.Elapsed.TotalSeconds);

        Debug.Log("----------------");

        Debug.Log(Chess.generalyValidWatch.Elapsed.TotalSeconds);
        Debug.Log(Chess.wouldBeCheckWatch.Elapsed.TotalSeconds);

        Chess.Turn = turn;
        chess.isWhiteAtTurn = playAsWhite;

        Debug.Log("Il y a " + b + " branches éxplorés");

        //Debug.Log(bestMove.piecePosition);
        //Debug.Log(bestMove.moveFromPiece);

        chess.RefreshChess();

        return bestMove;
    }
}
