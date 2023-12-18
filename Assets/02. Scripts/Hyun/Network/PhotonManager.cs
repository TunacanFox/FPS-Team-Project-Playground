using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using System;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager instance;
    private readonly string _version = "0.0.1";
    public string userID;
    public string userPW;
    public event Action<List<RoomInfo>> onRoomListUpdate;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        PhotonNetwork.GameVersion = _version;

        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public bool TrySetUserID(string id, string pw) // my sql �� �����ϱ� 
    {
        if (string.IsNullOrEmpty(id) ||
            string.IsNullOrEmpty(pw))
        {
            Debug.LogWarning("[PhotonManager] : Failed to set user ID. id or pw is empty");
            return false;
        }

        userID = id;
        userPW = pw;
        PhotonNetwork.NickName = id; // to do => ������ ���̽����� �� id�� nickname �����;���. ������ ���� �α����̹Ƿ� �г��� ����â ����.
        return true;
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        if (PhotonNetwork.InLobby)
        {
            SetCustomProperty(PhotonNetwork.LocalPlayer, new Hashtable
        {
            {CustomPropertyKeys.PLAYER_STATE_IN_ROOM, PlayerStateInRoom.NotInRoom}
        }
        );
            Debug.Log($"[PhotonNetwork] : {PhotonNetwork.NickName} is joined lobby");
        }
    }

    public override void OnCreatedRoom() // Ŭ���̾�Ʈ�� CreatRoom �Լ� ȣ�� => �������� Ŭ���̾�Ʈ���� ���� ��������ٰ� �ݹ��ϴ� ��. 
    {
        base.OnCreatedRoom();
        // to do => Room Scene ��ȯ�̳� UI�� ��� ��
        PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.LocalPlayer); // ���� ���� ���� ���� �÷��̾ �������� ! CreatRoom�� ȣ���� Ŭ���̾�Ʈ
        Debug.Log($"[PhotonManager] : Created Room.Master : {PhotonNetwork.LocalPlayer.NickName}");
    }

    public override void OnJoinedRoom() // �ٸ� �÷��̾ ���� �濡 ����
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.InRoom)
        {
            SetCustomProperty(PhotonNetwork.LocalPlayer, new Hashtable
        {
            {CustomPropertyKeys.PLAYER_STATE_IN_ROOM, PlayerStateInRoom.NotReady}
        }
        );
            Debug.Log($"[PhotonManager] : Successfully joined room, Total players : {PhotonNetwork.CurrentRoom.PlayerCount}");
            // to do => Room Scene ��ȯ�̳� UI�� ��� ��
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SetCustomProperty(PhotonNetwork.LocalPlayer, new Hashtable
        {
            {CustomPropertyKeys.PLAYER_STATE_IN_ROOM, PlayerStateInRoom.NotInRoom}
        }
        );
        // to do => move to local lobby.
    }

    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        // to do => Move to Login.
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log($"[PhotonManager] : {newPlayer.NickName} entered room.");
        // �濡 ���� �÷��̾��� ���¸� Idle�� ����.        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log($"[PhotonManager] : {otherPlayer.NickName} left room.");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        onRoomListUpdate?.Invoke(roomList);
    }

    private void SetCustomProperty(Player player, Hashtable hashtable)
    {
        player.SetCustomProperties(hashtable);
    }

}
