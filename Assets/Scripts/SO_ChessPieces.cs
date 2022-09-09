using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChessNetWork
{
    [CreateAssetMenu(menuName = "Theme/Pieces")]
    public class SO_ChessPieces : ScriptableObject
    {
        public GameObject[] chessPieces;
        public PieceType chessPieceType;
    }
}