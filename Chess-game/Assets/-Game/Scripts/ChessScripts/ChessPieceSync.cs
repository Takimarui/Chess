using Photon.Pun;
using UnityEngine;

public class ChessPieceSync : MonoBehaviourPun
{
    private Vector3 _targetPosition;
    private bool _isSyncing;
    private float _moveSpeed = 10f;

    public void SyncPosition(int x, int y)
    {
        _targetPosition = new Vector3(x, y, 0);
        _isSyncing = true;

        if (photonView.IsMine)
        {
            photonView.RPC("RpcUpdatePosition", RpcTarget.Others, x, y);
        }
    }

    [PunRPC]
    private void RpcUpdatePosition(int x, int y)
    {
        _targetPosition = new Vector3(x, y, 0);
        _isSyncing = true;
    }

    private void Update()
    {
        if (_isSyncing)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _moveSpeed);

            if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
            {
                _isSyncing = false;
                transform.position = _targetPosition;   
            }
        }
    }
}