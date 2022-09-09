using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChessNetWork
{
    public class Bishop : ChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int xTileCount, int yTileCount)
        {
            List<Vector2Int> r = new List<Vector2Int>();
            int direction = (team == 0) ? 1 : -1;

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
            return r;
        }
    }
}