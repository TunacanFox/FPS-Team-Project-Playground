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

    public bool TrySetUserID(string id, string pw) // my sql 로 관리하기 
    {
        if (string.IsNullOrEmpty(id) ||
            string.IsNullOrEmpty(pw))
        {
            Debug.LogWarning("[PhotonManager] : Failed to set user ID. id or pw is empty");
            return false;
        }

        userID = id;
        userPW = pw;
        PhotonNetwork.NickName = id; // to do => 데이터 베이스에서 이 id로 nickname 가져와야함. 없으면 최초 로그인이므로 닉네임 설정창 띄우기.
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

    public override void OnCreatedRoom() // 클라이언트가 CreatRoom 함수 호출 => 서버에서 클라이언트에게 방이 만들어졌다고 콜백하는 것. 
    {
        base.OnCreatedRoom();
        // to do => Room Scene 전환이나 UI를 띄울 것
        PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.LocalPlayer); // 방을 만든 현재 로컬 플레이어를 방장으로 ! CreatRoom을 호출한 클라이언트
        Debug.Log($"[PhotonManager] : Created Room.Master : {PhotonNetwork.LocalPlayer.NickName}");
    }

    public override void OnJoinedRoom() // 다른 플레이어가 만든 방에 참가
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
            // to do => Room Scene 전환이나 UI를 띄울 것
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
        // 방에 들어온 플레이어의 상태를 Idle로 만듬.        
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
