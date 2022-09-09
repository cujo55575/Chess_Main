using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChessNetWork
{
    public class Queen : ChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int xTileCount, int yTileCount)
        {
            List<Vector2Int> r = new List<Vector2Int>();
            int direction = (team == 0) ? 1 : -1;
            #region Horizontal and Vertical Moves
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
            #endregion
            #region  Diagonal Moves
            //Top right
            for (int x = xIndex + 1, y = yIndex + 1; x < xTileCount && y < yTileCount; x++, y++)
            {
                if (board[x, y] == null)
                    r.Add(new Vector2Int(x, y));
                else
                {
                    if (board[x, y].team != team)
                        r.Add(new Vector2Int(x, y));
                    break;
                }
            }
            //Top left
            for (int x = xIndex - 1, y = yIndex + 1; x >= 0 && y < yTileCount; x--, y++)
            {
                if (board[x, y] == null)
                    r.Add(new Vector2Int(x, y));
                else
                {
                    if (board[x, y].team != team)
                        r.Add(new Vector2Int(x, y));
                    break;
                }
            }

            //Bottom Right
            for (int x = xIndex + 1, y = yIndex - 1; x < xTileCount && y >= 0; x++, y--)
            {
                if (board[x, y] == null)
                    r.Add(new Vector2Int(x, y));
                else
                {
                    if (board[x, y].team != team)
                        r.Add(new Vector2Int(x, y));
                    break;
                }
            }

            //Bottom Left
            for (int x = xIndex - 1, y = yIndex - 1; x >= 0 && y >= 0; x--, y--)
            {
                if (board[x, y] == null)
                    r.Add(new Vector2Int(x, y));
                else
                {
                    if (board[x, y].team != team)
                        r.Add(new Vector2Int(x, y));
                    break;
                }
            }
            #endregion
            return r;
        }
    }
}