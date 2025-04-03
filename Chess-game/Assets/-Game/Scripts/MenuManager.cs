using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string _version = "1";

    [SerializeField] private MusicManager _musicManager;

    [SerializeField] private TMP_InputField _roomCodeInputField;
    [SerializeField] private TMP_Text _roomCodeDisplay;
    [SerializeField] private Button _createRoomButton, _joinRoomButton, _generateCodeButton;

    [SerializeField] private GameObject _loadingMenu;
    [SerializeField] private TMP_Text _loadingText;

    private string roomCode;
    private string baseText = "Loading";

    private void Awake()
    {
        _loadingMenu.SetActive(true);
        StartCoroutine(AnimateDots());

        AppSettings appSettings = new AppSettings
        {
            AppVersion = _version,
            AppIdRealtime = "ea1fd268-0573-4fa9-8de5-6ddaf7ebecdd"
        };

        PhotonNetwork.ConnectUsingSettings(appSettings);

        _createRoomButton.onClick.AddListener(CreateRoom);
        _joinRoomButton.onClick.AddListener(JoinRoom);
        _generateCodeButton.onClick.AddListener(GenerateCode);
    }
    IEnumerator AnimateDots()
    {
        int dotCount = 0;

        while (true)
        {
            _loadingText.text = baseText + new string('.', dotCount);
            dotCount = (dotCount + 1) % 4;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void GenerateCode()
    {
        roomCode = System.Guid.NewGuid().ToString().Substring(0, 6);
        _roomCodeDisplay.text = roomCode;
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
            _musicManager.StartFadeOut();

            string hostColor = Random.value > 0.5f ? "White" : "Black";

            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 2;
            options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "HostColor", hostColor } };
            options.CustomRoomPropertiesForLobby = new string[] { "HostColor" };

            PhotonNetwork.CreateRoom(roomCode, options, null);
        }
        else
        {
            Debug.LogError("Not connected to Photon Master Server yet!");
        }
    }

    private void JoinRoom()
    {
        string inputCode = _roomCodeInputField.text;
        if (string.IsNullOrEmpty(inputCode))
        {
            Debug.LogError("Room code is empty!");
            return;
        }

        if (PhotonNetwork.IsConnectedAndReady)
        {
            _musicManager.StartFadeOut();
            PhotonNetwork.JoinRoom(inputCode);
        }
        else
        {
            Debug.LogError("Not connected to Photon Master Server yet!");
        }
    }

    public override void OnConnectedToMaster()
    {
        _loadingMenu.SetActive(false);
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected to Master Server. Ready to create or join rooms.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);

        string hostColor = (string)PhotonNetwork.CurrentRoom.CustomProperties["HostColor"];
        string playerColor = PhotonNetwork.IsMasterClient ? hostColor : (hostColor == "White" ? "Black" : "White");
        PlayerPrefs.SetString("PlayerColor", playerColor);

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
