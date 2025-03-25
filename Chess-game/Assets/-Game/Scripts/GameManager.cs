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

    public void EndTurn()
    {
        isWhiteTurn = !isWhiteTurn;
        photonView.RPC("RpcEndTurn", RpcTarget.All, isWhiteTurn);
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
