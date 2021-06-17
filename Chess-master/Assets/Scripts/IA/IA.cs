using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IA
{
    private const int MAX_DEPTH = 2;

    private MovePredicted bestMove;
    private int bestScore;

    private Chess chess;
    public readonly bool playAsWhite;

    public struct MovePredicted
    {
        public Vector2Int piecePosition;
        public Vector2Int moveFromPiece;
    }

    public IA(Chess chess, bool playAsWhite)
    {
        this.chess = chess;
        this.playAsWhite = playAsWhite;
    }

    private int FindNextMove(Field[,] board, bool whiteTurn, int currentDepth = 0, int score = 0)
    {
        if (currentDepth >= MAX_DEPTH)
            return score;

        if (score < bestScore)
            return score;

        chess.fields = board;

        //Debug.Log("Turn " + (whiteTurn ? "White" : "Black"));

        // Get all IA pieces
        var pieces = chess.GetEveryPiecesForPlayer(whiteTurn);

        // Choose piece on field
        foreach (Field piece in pieces)
        {
            chess.fields = board;
            chess.isWhiteAtTurn = whiteTurn;

            // Check every moves available for that piece
            chess.OnPieceChosen(piece);

            var moves = chess.GetAllFieldWithMarker();

            // Check mate for one player
            if (!chess.CanMakeAnyLegalMove(whiteTurn))
            {
                return 999 * (whiteTurn == playAsWhite ? 1 : -1);
            }

            foreach (Field pieceMove in moves)
            {
                if (bestMove.piecePosition.x == -1 && bestMove.piecePosition.y == -1 && whiteTurn == playAsWhite)
                {
                    //Debug.Log($"DEFAULT MOVE: {piece.Piece.ToString()} at {piece.Piece.Position} et move {pieceMove.Position}");
                    bestMove.piecePosition = new Vector2Int(piece.Position.x, piece.Position.y);
                    bestMove.moveFromPiece = new Vector2Int(pieceMove.Position.x, pieceMove.Position.y);
                }

                if (pieceMove.Marker.Type == MarkerType.CAPTURE)
                {
                    // Translate move to score
                    score += (int)pieceMove.Piece.Type * (whiteTurn == playAsWhite ? 1 : -1);

                    var t = whiteTurn ? "white" : "black";
                    //Debug.Log($"{piece.Piece} at {piece.Position} eat a {pieceMove.Piece} at {pieceMove.Position} for a score of {score} during turn {t}");
                }

                if (pieceMove.Marker.Type == MarkerType.CAPTURE && whiteTurn == playAsWhite)
                {
                    //Debug.Log($"IA {piece.Piece} at {piece.Position} eat a {pieceMove.Piece} at {pieceMove.Position} for a score of {score}");
                }

                var boardClone = CloneBoard(board);
                chess.fields = boardClone;

                Field pieceMoveClone = boardClone[pieceMove.Position.x, pieceMove.Position.y];
                Field pieceClone = boardClone[piece.Position.x, piece.Position.y];

                // Play that move
                chess.OnMarkerChosen(pieceMoveClone, pieceClone);

                // Recu with opponent pieces
                int newScore = FindNextMove(boardClone, !whiteTurn, currentDepth + 1, score);

                //Debug.Log($"Turn {currentDepth} for {(whiteTurn ? "White" : "Black")} score: {newScore}");

                // Set best move
                if (newScore > bestScore && playAsWhite == whiteTurn)
                {
                    //Debug.Log("New Best move " + bestMove.moveFromPiece + " with score: " + score);
                    bestMove.piecePosition = new Vector2Int(piece.Position.x, piece.Position.y);
                    bestMove.moveFromPiece = new Vector2Int(pieceMove.Position.x, pieceMove.Position.y);
                    this.bestScore = newScore;
                }
            }
        }

        return score;
    }

    private Field[,] CloneBoard(Field[,] board)
    {
        Field[,] cloneBoard = new Field[8, 8];

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                cloneBoard[x,y] = board[x,y].Clone();
            }
        }

        return cloneBoard;
    }

    public MovePredicted GetNextMove()
    {
        bestScore = -9999;
        bestMove = new MovePredicted() { piecePosition = new Vector2Int(-1, -1) };

        var baseBoard = chess.fields;

        // Clone current board
        Field[,] cloneBoard = CloneBoard(baseBoard);

        FindNextMove(cloneBoard, playAsWhite);

        chess.fields = baseBoard;
        chess.isWhiteAtTurn = playAsWhite;

        //Debug.Log(bestMove.piecePosition);
        //Debug.Log(bestMove.moveFromPiece);

        chess.RefreshChess();

        return bestMove;
    }
}
