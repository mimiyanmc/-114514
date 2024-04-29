using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class launch : MonoBehaviourPunCallbacks
{
    public static launch Instance;
    // Start is called before the first frame update
    [SerializeField] TMP_InputField roomNameinputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject StartGameButton;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Debug.Log("will wcnm");
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("wcnming");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {
        MenuManager.Instance.openMenu("title");
        Debug.Log("wcnm");
        
    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameinputField.name))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameinputField.text);
        MenuManager.Instance.openMenu("loading");
    }
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.openMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform t in playerListContent) { 
            Destroy(t.gameObject);
        }
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<playerListItem>().setUp(players[i]);
        }
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Create Failed" + message;
        MenuManager.Instance.openMenu("error");
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
    /*public void DisConnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }
    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.LeaveRoom();
        while(PhotonNetwork.InRoom)
        {
            yield return null;
        }
        PhotonNetwork.LoadLevel(0);
    }*/
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.openMenu("loading");
    }
    public void JoinRoom(RoomInfo Info)
    {
        PhotonNetwork.JoinRoom(Info.Name);
        MenuManager.Instance.openMenu("loading");


    }
    public override void OnLeftRoom()
    {
        MenuManager.Instance.openMenu("title");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform transform in roomListContent)
        {
            Destroy(transform.gameObject);
        }
        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().setUp(roomList[i]);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab,playerListContent).GetComponent<playerListItem>().setUp(newPlayer);
    }
 
}
