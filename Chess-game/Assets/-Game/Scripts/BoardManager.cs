using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class BoardManager : MonoBehaviour
{
    private Board _board;
    private ChessPiece _selectedPiece;

    // 0 - Pawn, 1 - Rook, 2 - Knight, 3 - Bishop, 4 - Queen, 5 - King
    public GameObject[] whitePrefabs;
    public GameObject[] blackPrefabs;

    private void Start()
    {
        _board = new Board(whitePrefabs, blackPrefabs);
        DrawBoard();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePosition.x);
            int y = Mathf.RoundToInt(mousePosition.y);

            Debug.Log($"mousePosition : {x};{y}");

            if (_selectedPiece == null)
            {
                _selectedPiece = _board.Cells[y, x];
                Debug.Log($"_selectedPiece : {_selectedPiece.X};{_selectedPiece.Y}");

                if (_selectedPiece != null)
                {
                    Debug.Log($"{_selectedPiece.GetType().Name} ({_selectedPiece.Color})");
                }
            }
            else
            {
                if (_selectedPiece.CanMove(x, y, this))
                {
                MovePiece(_selectedPiece, x, y);
                }
                _selectedPiece = null;
            }
        }
    }

    public void MovePiece(ChessPiece piece, int targetX, int targetY)
    {
        _board.Cells[piece.Y, piece.X] = null;
        _board.Cells[targetY, targetX] = piece;

        piece.X = targetX;
        piece.Y = targetY;

        piece.pieceObject.transform.position = new Vector3(piece.X, piece.Y, 0);
        Debug.Log($"{piece.GetType().Name} : {piece.Y}, {piece.Y}");
    }

    public bool IsCellEmpty(int x, int y)
    {
        if (x < 0 || x >= 8 || y < 0 || y >= 8)
        {
            return false;
        }
        return _board.Cells[y, x] == null;
    }

    public string GetPieceColor(int x, int y)
    {
        ChessPiece piece = _board.Cells[x, y];
        return piece != null ? piece.Color : null;
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

                    if (pieceObject != null && piece.pieceObject == null)
                    {
                        piece.pieceObject = Instantiate(pieceObject, new Vector3(j, i, 0), Quaternion.identity);
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
