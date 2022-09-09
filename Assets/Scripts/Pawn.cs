using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChessNetWork
{
    public class Pawn : ChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int xTileCount, int yTileCount)
        {
            List<Vector2Int> r = new List<Vector2Int>();
            int direction = (team == 0) ? 1 : -1;

            if (board[xIndex, yIndex + direction] == null)
                r.Add(new Vector2Int(xIndex, yIndex + direction));

            if (board[xIndex, yIndex + direction] == null)
            {
                if (team == 0 && yIndex == 1 && board[xIndex, yIndex + direction * 2] == null)
                    r.Add(new Vector2Int(xIndex, yIndex + direction * 2));
                if (team == 1 && yIndex == 6 && board[xIndex, yIndex + direction * 2] == null)
                    r.Add(new Vector2Int(xIndex, yIndex + direction * 2));
            }

            if (xIndex != xTileCount - 1)
                if (board[xIndex + 1, yIndex + direction] != null && board[xIndex + 1, yIndex + direction].team != team)
                {
                    r.Add(new Vector2Int(xIndex + 1, yIndex + direction));
                }
            if (xIndex != 0)
                if (board[xIndex - 1, yIndex + direction] != null && board[xIndex - 1, yIndex + direction].team != team)
                {
                    r.Add(new Vector2Int(xIndex - 1, yIndex + direction));
                }

            return r;
        }
        public override SpecialMove GetSpecialMoves(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> availableMoves)
        {
            int direction = (team == 0) ? 1 : -1;
            if (moveList.Count > 0)
            {
                Vector2Int[] lastMove = moveList[moveList.Count - 1];
                if (board[lastMove[1].x, lastMove[1].y].pieceType == ChessPiecesType.Pawn)
                {
                    if (Mathf.Abs(lastMove[0].y - lastMove[1].y) == 2)
                    {
                        if (board[lastMove[1].x, lastMove[1].y].team != team)
                        {
                            if (lastMove[1].y == yIndex)
                            {
                                if (lastMove[1].x == xIndex - 1)
                                {
                                    availableMoves.Add(new Vector2Int(xIndex - 1, yIndex + direction));
                                    return SpecialMove.EnPassant;
                                }
                                if (lastMove[1].x == xIndex + 1)
                                {
                                    availableMoves.Add(new Vector2Int(xIndex + 1, yIndex + direction));
                                    return SpecialMove.EnPassant;
                                }
                            }
                        }
                    }
                }
            }
            return SpecialMove.None;
        }
    }
}