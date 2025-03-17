using UnityEngine;

class Board
{
    public ChessPiece[,] Cells = new ChessPiece[8, 8];

    public Board()
    {
        string[] backRow = { "Rook", "Knight", "Bishop", "Queen", "King", "Bishop", "Knight", "Rook" };

        for (int i = 0; i < 8; i++)
        {
            Cells[7, i] = CreatePiece(backRow[i], "White", 7, i);
            Cells[6, i] = CreatePiece("Pawn", "White", 6, i);

            Cells[1, i] = CreatePiece("Pawn", "Black", 1, i);
            Cells[0, i] = CreatePiece(backRow[i], "Black", 0, i);
        }

        for (int i = 2; i < 6; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Cells[i, j] = null;
            }
        }
    }

    public ChessPiece CreatePiece(string type, string color, int x, int y)
    {
        switch (type)
        {
            case "Pawn": return new Pawn(color, x, y);
            case "Rook": return new Rook(color, x, y);
            case "Queen": return new Queen(color, x, y);
            case "King": return new King(color, x, y);
            case "Knight": return new Knight(color, x, y);
            case "Bishop": return new Bishop(color, x, y);
            default: return null;
        }
    }
}
