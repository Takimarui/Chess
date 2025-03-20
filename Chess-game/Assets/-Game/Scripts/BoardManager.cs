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
        if (Input.GetMouseButtonDown(1))
        {
            ResetBoard();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePosition.x);
            int y = Mathf.RoundToInt(mousePosition.y);


            if (_selectedPiece == null)
            {
                _selectedPiece = _board.Cells[y, x];


                if (_selectedPiece != null)
                {
                    if ((_selectedPiece.Color == "White" && GameManager.Instance.IsWhiteTurn) ||
                    (_selectedPiece.Color == "Black" && !GameManager.Instance.IsWhiteTurn))
                    {
                        UpdatePieceTransparency(_selectedPiece); ;
                    }
                    else
                    {
                        _selectedPiece = null;
                    }
                }
            }
            else
            {
                if (_selectedPiece.CanMove(x, y, this))
                {
                MovePiece(_selectedPiece, x, y);
                GameManager.Instance.NextTurn();
                }
                _selectedPiece = null;
                UpdatePieceTransparency(null);
            }
        }
    }

    public void MovePiece(ChessPiece piece, int targetX, int targetY)
    {
        ChessPiece targetPiece = _board.Cells[targetY, targetX];

        if (targetPiece != null)
        {
            Destroy(targetPiece.pieceObject);
            _board.Cells[targetY, targetX] = null;
        }

        _board.Cells[piece.Y, piece.X] = null;
        _board.Cells[targetY, targetX] = piece;

        piece.X = targetX;
        piece.Y = targetY;

        piece.pieceObject.transform.position = new Vector3(piece.X, piece.Y, 0);
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
        ChessPiece piece = _board.Cells[y, x];
        return piece != null ? piece.Color : null;
    }

    public bool IsPathClear(int startX, int startY, int targetX, int targetY)
    {
        if (startX == targetX)
        {
            int minY = Mathf.Min(startY, targetY);
            int maxY = Mathf.Max(startY, targetY);

            for (int y = minY + 1; y < maxY; y++)
            {
                if (_board.Cells[y, startX] != null)
                {
                    return false;
                }
            }
        }
        else if (startY == targetY)
        {
            int minX = Mathf.Min(startX, targetX);
            int maxX = Mathf.Max(startX, targetX);

            for (int x = minX + 1; x < maxX; x++)
            {
                if (_board.Cells[startY, x] != null)
                {
                    return false;
                }
            }
        }
        else if (Mathf.Abs(startX - targetX) == Mathf.Abs(startY - targetY))
        {
            int xDirection = targetX > startX ? 1 : -1;
            int yDirection = targetY > startY ? 1 : -1;

            int x = startX + xDirection;
            int y = startY + yDirection;

            while (x != targetX && y != targetY)
            {
                if (_board.Cells[y, x] != null)
                {
                    return false;
                }

                x += xDirection;
                y += yDirection;
            }
        }

        return true;
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
                        piece.pieceObject.tag = "ChessPiece";
                    }
                }
            }
        }
    }

    public void UpdatePieceTransparency(ChessPiece selectedPiece)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ChessPiece"))
        {
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                Color color = renderer.color;
                color.a = (selectedPiece != null && obj != selectedPiece.pieceObject) ? 0.9f : 1f;
                renderer.color = color;
            }
        }
    }

    private void ResetBoard()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ChessPiece"))
        {
            Destroy(obj);
        }

        _board = new Board(whitePrefabs, blackPrefabs);
        DrawBoard();
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
