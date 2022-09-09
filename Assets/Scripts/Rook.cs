using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChessNetWork
{
    public class Rook : ChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int xTileCount, int yTileCount)
        {
            List<Vector2Int> r = new List<Vector2Int>();
            int direction = (team == 0) ? 1 : -1;
            //DownWard
            for (int i = yIndex - 1; i >= 0; i--)
            {
                if (board[xIndex, i] == null)
                    r.Add(new Vector2Int(xIndex, i));

                if (board[xIndex, i] != null)
                {
                    if (board[xIndex, i].team != team)
                    {
                        r.Add(new Vector2Int(xIndex, i));
                    }
                    break;
                }
            }

            //Upward
            for (int i = yIndex + 1; i < yTileCount; i++)
            {
                if (board[xIndex, i] == null)
                    r.Add(new Vector2Int(xIndex, i));

                if (board[xIndex, i] != null)
                {
                    if (board[xIndex, i].team != team)
                    {
                        r.Add(new Vector2Int(xIndex, i));
                    }
                    break;
                }

            }

            //LeftSide
            for (int i = xIndex - 1; i >= 0; i--)
            {
                if (board[i, yIndex] == null)
                    r.Add(new Vector2Int(i, yIndex));

                if (board[i, yIndex] != null)
                {
                    if (board[i, yIndex].team != team)
                    {
                        r.Add(new Vector2Int(i, yIndex));
                    }
                    break;
                }

            }

            //RightSide
            for (int i = xIndex + 1; i < xTileCount; i++)
            {
                if (board[i, yIndex] == null)
                    r.Add(new Vector2Int(i, yIndex));

                if (board[i, yIndex] != null)
                {
                    if (board[i, yIndex].team != team)
                    {
                        r.Add(new Vector2Int(i, yIndex));
                    }
                    break;
                }

            }
            return r;
        }
    }
}