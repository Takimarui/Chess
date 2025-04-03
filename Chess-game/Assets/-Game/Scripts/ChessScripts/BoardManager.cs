using Photon.Pun;
using UnityEngine;

public class BoardManager : MonoBehaviourPun
{
    [SerializeField] private PhotonView _view;
    [SerializeField] private MusicManager musicManager;

    private Board _board;
    private ChessPiece _selectedPiece;

    public GameObject[] whitePrefabs;
    public GameObject[] blackPrefabs;

    private bool isPlayerWhite;

    private void Start()
    {
        _board = new Board(whitePrefabs, blackPrefabs);

        object hostColor;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("HostColor", out hostColor))
        {
            string hostColorStr = hostColor.ToString();
            isPlayerWhite = (PhotonNetwork.IsMasterClient && hostColorStr == "White") ||
                            (!PhotonNetwork.IsMasterClient && hostColorStr == "Black");
        }

        DrawBoard(isPlayerWhite ? "White" : "Black");
        SetupCamera();
    }

    private void SetupCamera()
    {
        Camera mainCamera = Camera.main;
        if (isPlayerWhite && mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(3.5f, 3.5f, -10);
            mainCamera.transform.Rotate(0, 0, 180);
        }
    }

    private void Update()
    {
        if (_board == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePosition.x);
            int y = Mathf.RoundToInt(mousePosition.y);

            if (!IsValidCell(x, y)) return;

            if (_selectedPiece == null)
            {
                _selectedPiece = _board.Cells[y, x];

                if (_selectedPiece != null)
                {
                    if (_selectedPiece.Color == (isPlayerWhite ? "White" : "Black") &&
                        ((_selectedPiece.Color == "White" && GameManager.Instance.IsWhiteTurn) ||
                         (_selectedPiece.Color == "Black" && !GameManager.Instance.IsWhiteTurn)))
                    {
                        UpdatePieceTransparency(_selectedPiece);
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

    private bool IsValidCell(int x, int y)
    {
        return x >= 0 && x < 8 && y >= 0 && y < 8;
    }

    public bool IsCellEmpty(int x, int y)
    {
        return IsValidCell(x, y) && _board.Cells[y, x] == null;
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
                if (_board.Cells[y, startX] != null) return false;
            }
        }
        else if (startY == targetY)
        {
            int minX = Mathf.Min(startX, targetX);
            int maxX = Mathf.Max(startX, targetX);
            for (int x = minX + 1; x < maxX; x++)
            {
                if (_board.Cells[startY, x] != null) return false;
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
                if (_board.Cells[y, x] != null) return false;
                x += xDirection;
                y += yDirection;
            }
        }

        return true;
    }
    public ChessPiece GetPieceAt(int x, int y)
    {
        return _board.Cells[y, x];
    }

    private void DrawBoard(string playerColor)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                ChessPiece piece = _board.Cells[i, j];
                if (piece != null && piece.pieceObject == null && piece.Color == playerColor)
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
    private string PromotePawn(int x, int y, bool isWhite)
    {
        ChessPiece pawn = _board.Cells[y, x];
        if (pawn.pieceObject != null)
        {
            PhotonNetwork.Destroy(pawn.pieceObject);
        }

        GameObject queenPrefab = isWhite ? whitePrefabs[4] : blackPrefabs[4];
        GameObject queenObject = PhotonNetwork.Instantiate(queenPrefab.name, new Vector3(x, y, 0), Quaternion.identity);

        ChessPiece queen = new Queen(pawn.Color, x, y, true);
        queen.pieceObject = queenObject;

        _board.Cells[y, x] = queen;

        return queenObject.GetComponent<PhotonView>().ViewID.ToString();
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

        if (piece is Pawn)
        {
            bool isWhite = piece.Color == "White";
            if ((isWhite && targetY == 0) || (!isWhite && targetY == 7))
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    string queenId = PromotePawn(targetX, targetY, isWhite);
                    photonView.RPC("RpcPromotePawn", RpcTarget.Others, targetX, targetY, isWhite, queenId);
                }
                musicManager.musicOfTheMove();
                return;
            }
        }

        if (piece is King && Mathf.Abs(targetX - startX) == 2)
        {
            int rookStartX = (targetX > startX) ? startX + 3 : startX - 4;
            int rookTargetX = (targetX > startX) ? targetX - 1 : targetX + 1;

            ChessPiece rook = _board.Cells[startY, rookStartX];

            if (rook is Rook)
            {
                photonView.RPC("RpcMoveRook", RpcTarget.All, rookStartX, startY, rookTargetX, startY);
            }
        }

        if (piece.pieceObject != null)
        {
            var sync = piece.pieceObject.GetComponent<ChessPieceSync>();
            if (sync != null)
            {
                sync.SyncPosition(targetX, targetY);
            }
        }
        musicManager.musicOfTheMove();
    }

    [PunRPC]
    private void RpcMoveRook(int startX, int startY, int targetX, int targetY)
    {
        ChessPiece rook = _board.Cells[startY, startX];
        if (rook == null || !(rook is Rook)) return;

        _board.Cells[startY, startX] = null;
        _board.Cells[targetY, targetX] = rook;
        rook.X = targetX;
        rook.Y = targetY;

        if (rook.pieceObject != null)
        {
            var sync = rook.pieceObject.GetComponent<ChessPieceSync>();
            if (sync != null)
            {
                sync.SyncPosition(targetX, targetY);
            }
        }
    }


    [PunRPC]
    private void RpcPromotePawn(int x, int y, bool isWhite, string queenId)
    {
        ChessPiece pawn = _board.Cells[y, x];
        if (pawn != null && pawn.pieceObject != null)
        {
            int viewID = pawn.pieceObject.GetComponent<PhotonView>().ViewID;
            photonView.RPC("RpcDestroyPiece", RpcTarget.All, viewID);
        }

        GameObject queenObject = PhotonView.Find(int.Parse(queenId)).gameObject;

        ChessPiece queen = new Queen(isWhite ? "White" : "Black", x, y, true);
        queen.pieceObject = queenObject;

        _board.Cells[y, x] = queen;
    }
    [PunRPC]
    private void RpcDestroyPiece(int viewID)
    {
        GameObject piece = PhotonView.Find(viewID)?.gameObject;
        if (piece != null)
        {
            Destroy(piece);
        }
    }
}
