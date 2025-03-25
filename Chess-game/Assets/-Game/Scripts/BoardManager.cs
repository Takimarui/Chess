using Photon.Pun;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class BoardManager : MonoBehaviourPun
{
    [SerializeField] private PhotonView _view;

    private Board _board;
    private ChessPiece _selectedPiece;

    // 0 - Pawn, 1 - Rook, 2 - Knight, 3 - Bishop, 4 - Queen, 5 - King
    public GameObject[] whitePrefabs;
    public GameObject[] blackPrefabs;

    private void Start()
    {
        _board = new Board(whitePrefabs, blackPrefabs);
        if (PhotonNetwork.IsMasterClient)
        {
            DrawBoard();
        }
    }

    private void Update()
    {
        if (_board == null) return;

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
                    photonView.RPC("RpcMovePiece", RpcTarget.All, _selectedPiece.X, _selectedPiece.Y, x, y);
                    GameManager.Instance.NextTurn();
                }
                UpdatePieceTransparency(_selectedPiece, false);
                _selectedPiece = null;
            }
        }
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
                if (piece != null && piece.pieceObject == null)
                {
                    GameObject prefab = GetPrefab(piece);
                    if (prefab != null)
                    {
                        piece.pieceObject = PhotonNetwork.Instantiate(
                            prefab.name,
                            new Vector3(j, i, 0),
                            Quaternion.identity,
                            0
                        );
                        piece.pieceObject.tag = "ChessPiece";

                        var sync = piece.pieceObject.GetComponent<ChessPieceSync>();
                        if (sync == null)
                        {
                            sync = piece.pieceObject.AddComponent<ChessPieceSync>();
                        }
                        sync.SyncPosition(j, i);
                    }
                }
            }
        }
    }


    public void UpdatePieceTransparency(ChessPiece selectedPiece, bool visibility = true)
    {
        if (selectedPiece == null || selectedPiece.pieceObject == null) return;

        SpriteRenderer renderer = selectedPiece.pieceObject.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            Color color = renderer.color;
            color.a = visibility ? 0.8f : 1f;
            renderer.color = color;
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

    [PunRPC]
    private void RpcMovePiece(int startX, int startY, int targetX, int targetY)
    {
        ChessPiece piece = _board.Cells[startY, startX];
        if (piece == null) return;

        ChessPiece targetPiece = _board.Cells[targetY, targetX];
        if (targetPiece != null && targetPiece.pieceObject != null)
        {
            PhotonNetwork.Destroy(targetPiece.pieceObject);
        }

        _board.Cells[startY, startX] = null;
        _board.Cells[targetY, targetX] = piece;
        piece.X = targetX;
        piece.Y = targetY;

        if (piece.pieceObject != null)
        {
            var sync = piece.pieceObject.GetComponent<ChessPieceSync>();
            if (sync != null)
            {
                sync.SyncPosition(targetX, targetY);
            }
        }
    }
}