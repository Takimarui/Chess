using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class BoardManager : MonoBehaviour
{
    private Board _board;

    // 0 - Pawn, 1 - Rook, 2 - Knight, 3 - Bishop, 4 - Queen, 5 - King
    public GameObject[] whitePrefabs;
    public GameObject[] blackPrefabs;

    private void Start()
    {
        _board = new Board();
        DrawBoard();
    }

    private void DrawBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            { 
                ChessPiece piece = _board.Cells[i, j];
                if (piece != null)
                {
                    GameObject pieceObject = GetPrefab(piece);

                    if (pieceObject != null)
                    {
                        Instantiate(pieceObject, new Vector3(j, i, 0), Quaternion.identity);
                    }
                }
            }
        }
    }

    private GameObject GetPrefab(ChessPiece piece)
    {
        int index = GetPieceIndex(piece);

        return piece.Color == "White" ? whitePrefabs[index] : blackPrefabs[index];
    }

    private int GetPieceIndex(ChessPiece piece)
    {
        switch (piece)
        {
            case Pawn _: return 0;
            case Rook _: return 1;
            case Knight _: return 2;
            case Bishop _: return 3;
            case Queen _: return 4;
            case King _: return 5;
            default: return -1;
        }
    }

}
