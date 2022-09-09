using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChessNetWork
{
    public class King : ChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int xTileCount, int yTileCount)
        {
            List<Vector2Int> r = new List<Vector2Int>();
            int direction = (team == 0) ? 1 : -1;

            #region  Right
            //Right
            if (xIndex + 1 < xTileCount)
            {
                if (board[xIndex + 1, yIndex] == null)
                    r.Add(new Vector2Int(xIndex + 1, yIndex));
                else if (board[xIndex + 1, yIndex].team != team)
                    r.Add(new Vector2Int(xIndex + 1, yIndex));
            }

            //Top Right
            if (yIndex + 1 < yTileCount)
            {
                if (board[xIndex + 1, yIndex + 1] == null)
                    r.Add(new Vector2Int(xIndex + 1, yIndex + 1));
                else if (board[xIndex + 1, yIndex + 1].team != team)
                    r.Add(new Vector2Int(xIndex + 1, yIndex + 1));
            }
            //Bottom Right
            if (yIndex - 1 >= 0)
            {
                if (board[xIndex + 1, yIndex - 1] == null)
                    r.Add(new Vector2Int(xIndex + 1, yIndex - 1));
                else if (board[xIndex + 1, yIndex - 1].team != team)
                    r.Add(new Vector2Int(xIndex + 1, yIndex - 1));
            }
            #endregion
            #region Left
            //Left
            if (xIndex - 1 >= 0)
            {
                if (board[xIndex - 1, yIndex] == null)
                    r.Add(new Vector2Int(xIndex - 1, yIndex));
                else if (board[xIndex - 1, yIndex].team != team)
                    r.Add(new Vector2Int(xIndex - 1, yIndex));
            }

            //Top Left
            if (yIndex + 1 < yTileCount)
            {
                if (board[xIndex - 1, yIndex + 1] == null)
                    r.Add(new Vector2Int(xIndex - 1, yIndex + 1));
                else if (board[xIndex - 1, yIndex + 1].team != team)
                    r.Add(new Vector2Int(xIndex - 1, yIndex + 1));
            }
            //Bottom Left
            if (yIndex - 1 >= 0)
            {
                if (board[xIndex - 1, yIndex - 1] == null)
                    r.Add(new Vector2Int(xIndex - 1, yIndex - 1));
                else if (board[xIndex - 1, yIndex - 1].team != team)
                    r.Add(new Vector2Int(xIndex - 1, yIndex - 1));
            }
            #endregion
            #region Up
            if (yIndex + 1 < yTileCount)
                if (board[xIndex, yIndex + 1] == null || board[xIndex, yIndex + 1].team != team)
                    r.Add(new Vector2Int(xIndex, yIndex + 1));
            #endregion
            #region Down
            if (yIndex - 1 >= 0)
                if (board[xIndex, yIndex - 1] == null || board[xIndex, yIndex - 1].team != team)
                    r.Add(new Vector2Int(xIndex, yIndex - 1));
            #endregion

            
            return r;
        }
    }
}