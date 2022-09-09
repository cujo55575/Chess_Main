using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChessNetWork
{
    public class Knight : ChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int xTileCount, int yTileCount)
        {
            List<Vector2Int> r = new List<Vector2Int>();
            int direction = (team == 0) ? 1 : -1;

            //Top Right
            int x = xIndex + 1;
            int y = yIndex + 2;
            if (x < xTileCount && y < yTileCount)
                if (board[x, y] == null || board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

            x = xIndex + 2;
            y = yIndex + 1;
            if (x < xTileCount && y < yTileCount)
                if (board[x, y] == null || board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

            //Top Left
            x = xIndex - 1;
            y = yIndex + 2;
            if (x >= 0 && y < yTileCount)
                if (board[x, y] == null || board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

            x = xIndex - 2;
            y = yIndex + 1;
            if (x >= 0 && y < yTileCount)
                if (board[x, y] == null || board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

            //bottom right

            x = xIndex + 1;
            y = yIndex - 2;
            if (x < xTileCount && y >= 0)
                if (board[x, y] == null || board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

            x = xIndex + 2;
            y = yIndex - 1;
            if (x < xTileCount && y >= 0)
                if (board[x, y] == null || board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

            //bottom Left
            x = xIndex - 1;
            y = yIndex - 2;
            if (x >= 0 && y >= 0)
                if (board[x, y] == null || board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

            x = xIndex - 2;
            y = yIndex - 1;
            if (x >= 0 && y >= 0)
                if (board[x, y] == null || board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));
            return r;
        }
    }
}