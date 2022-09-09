using System.Collections.Generic;
using UnityEngine;
namespace ChessNetWork
{
    public class ChessPiece : MonoBehaviour
    {
        public ChessPiecesType pieceType;
        public int xIndex;
        public int yIndex;
        public int team;

        private Vector3 desiredPos;
        private Vector3 desiredScale = new Vector3(2, 2, 1);
        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * 10);
            transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * 10);
        }
        #region Position and Scale
        public virtual void SetPos(Vector3 pos, bool force = false)
        {
            desiredPos = pos;
            if (force) transform.position = desiredPos;
        }
        public virtual void SetScale(Vector3 scale, bool force = false)
        {
            desiredScale = scale;
            if (force) transform.localScale = desiredScale;
        }
        #endregion
        #region piece moving
        public virtual List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int xTileCount, int yTileCount)
        {
            List<Vector2Int> r = new List<Vector2Int>();
            return r;
        }
        #endregion
        public virtual SpecialMove GetSpecialMoves(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> availableMoves)
        {
            return SpecialMove.None;
        }
    }
}
