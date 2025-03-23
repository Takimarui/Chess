using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPun
{
    public static GameManager Instance;

    private bool isWhiteTurn = true;

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
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("I am the host!");
            InitializeBoard();
        }
    }

    private void InitializeBoard()
    {
        photonView.RPC("RpcSetupBoard", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RpcSetupBoard()
    {
        Debug.Log("Setting up chess board");
    }

    public void MovePiece(int startX, int startY, int targetX, int targetY)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RpcMovePiece", RpcTarget.All, startX, startY, targetX, targetY);
        }
    }

    [PunRPC]
    private void RpcMovePiece(int startX, int startY, int targetX, int targetY)
    {
        Debug.Log($"Moving piece from ({startX}, {startY}) to ({targetX}, {targetY})");
    }

    public void EndTurn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isWhiteTurn = !isWhiteTurn;
            photonView.RPC("RpcEndTurn", RpcTarget.All, isWhiteTurn);
        }
    }

    [PunRPC]
    private void RpcEndTurn(bool isWhiteTurn)
    {
        this.isWhiteTurn = isWhiteTurn;
        Debug.Log("End Turn, isWhiteTurn: " + isWhiteTurn);
    }

    public void NextTurn()
    {
        EndTurn();
    }

    public bool IsWhiteTurn => isWhiteTurn;
}
