using UnityEngine;

class Board
{
    public ChessPiece[,] Cells = new ChessPiece[8, 8];
    public GameObject[] whitePrefabs;
    public GameObject[] blackPrefabs;

    public Board(GameObject[] whitePrefabs, GameObject[] blackPrefabs)
    {
        string[] backRow = { "Rook", "Knight", "Bishop", "Queen", "King", "Bishop", "Knight", "Rook" };
        this.whitePrefabs = whitePrefabs;
        this.blackPrefabs = blackPrefabs;

        for (int i = 0; i < 8; i++)
        {
            Cells[7, i] = CreatePiece(backRow[i], "White", i, 7);
            Cells[6, i] = CreatePiece("Pawn", "White", i, 6);

            Cells[1, i] = CreatePiece("Pawn", "Black", i, 1);
            Cells[0, i] = CreatePiece(backRow[i], "Black", i, 0);
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
        ChessPiece piece = null;

        switch (type)
        {
            case "Pawn": piece = new Pawn(color, x, y, true); break;
            case "Rook": piece = new Rook(color, x, y, true); break;
            case "Queen": piece = new Queen(color, x, y, true); break;
            case "King": piece = new King(color, x, y, true); break;
            case "Knight": piece = new Knight(color, x, y, true); break;
            case "Bishop": piece = new Bishop(color, x, y, true); break;
        }

        return piece;
    }
}
