using UnityEngine;
namespace ChessNetWork
{
    [CreateAssetMenu(menuName = "Theme/Board")]
    public class SO_BoardTheme : ScriptableObject
    {

        public SquareColours lightSquares;
        public SquareColours darkSquares;

        [System.Serializable]
        public struct SquareColours
        {
            public Color normal;
            public Color legal;
            public Color selected;
            public Color moveFromHighlight;
            public Color moveToHighlight;
        }
    }
}