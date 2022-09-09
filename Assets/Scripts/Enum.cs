namespace ChessNetWork
{
    public enum ChessPiecesType
    {
        None = 0,
        Pawn = 1,
        Rook = 2,
        Knight = 3,
        Bishop = 4,
        Queen = 5,
        King = 6
    }
    public enum PieceType
    {
        White = 0,
        Black = 1

    }
    public enum SpecialMove
    {
        None = 0,
        EnPassant,
        Castling,
        Promotion
    }

}