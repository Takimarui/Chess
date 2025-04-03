using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPun
{
    public static GameManager Instance;

    private bool isWhiteTurn = true;
    private string playerColor;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerColor = PlayerPrefs.GetString("PlayerColor", "White");
        SetupBoardView();
    }
    private void SetupBoardView()
    {
        if (playerColor == "Black")
        {
            RotateBoard();
        }
    }
    private void RotateBoard()
    {
        GameObject board = GameObject.Find("ChessBoard");
        if (board != null)
        {
            board.transform.Rotate(0, 180, 0);
        }
    }

    private void EndTurn()
    {
        isWhiteTurn = !isWhiteTurn;
        photonView.RPC("RpcEndTurn", RpcTarget.All, isWhiteTurn);
    }
    public void NextTurn()
    {
        EndTurn();
    }
    public bool IsWhiteTurn => isWhiteTurn;

    [PunRPC]
    private void RpcEndTurn(bool isWhiteTurn)
    {
        this.isWhiteTurn = isWhiteTurn;
        Debug.Log("End Turn, isWhiteTurn: " + isWhiteTurn);
    }
    public bool CanMovePiece(ChessPiece piece)
    {
        return piece.Color == playerColor &&
               ((isWhiteTurn && piece.Color == "White") || (!isWhiteTurn && piece.Color == "Black"));
    }
}
