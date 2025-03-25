using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string _version = "1";

    [SerializeField] private TMP_InputField roomCodeInputField;
    [SerializeField] private TMP_Text roomCodeDisplay;
    [SerializeField] private Button createRoomButton, joinRoomButton, generateCodeButton;

    private string roomCode;

    private void Awake()
    {
        AppSettings appSettings = new AppSettings
        {
            AppVersion = _version,
            AppIdRealtime = "ea1fd268-0573-4fa9-8de5-6ddaf7ebecdd"
        };

        PhotonNetwork.ConnectUsingSettings(appSettings);

        createRoomButton.onClick.AddListener(CreateRoom);
        joinRoomButton.onClick.AddListener(JoinRoom);
        generateCodeButton.onClick.AddListener(GenerateCode);
    }

    private void GenerateCode()
    {
        roomCode = System.Guid.NewGuid().ToString().Substring(0, 6);
        roomCodeDisplay.text = roomCode;
        Debug.Log(roomCode);

        GUIUtility.systemCopyBuffer = roomCode;
    }

    private void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomCode))
        {
            Debug.LogError("Room code is empty!");
            return;
        }

        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.CreateRoom(roomCode, new RoomOptions { MaxPlayers = 2 }, null);
        }
        else
        {
            Debug.LogError("Not connected to Photon Master Server yet!");
        }
    }

    private void JoinRoom()
    {
        string inputCode = roomCodeInputField.text;
        if (string.IsNullOrEmpty(inputCode))
        {
            Debug.LogError("Room code is empty!");
            return;
        }

        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRoom(inputCode);
        }
        else
        {
            Debug.LogError("Not connected to Photon Master Server yet!");
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected to Master Server. Ready to create or join rooms.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to create room: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
    }
}
