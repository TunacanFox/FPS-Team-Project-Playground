using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    // ���� ����
    private readonly string version = "1.0f";

    // ����� ���̵�
    private string _userID = "hyun";

    private void Awake()
    {
        // ���� room�� �����鿡�� �ڵ����� scene �� �ε�
        PhotonNetwork.AutomaticallySyncScene = true;

        // ���� ������ �������� ���� ���
        PhotonNetwork.GameVersion = version;

        // ���� ���̵� �Ҵ�
        PhotonNetwork.NickName = _userID;

        // ���� ������ ��� Ƚ�� ����, �ʴ� 30ȸ
        Debug.Log(PhotonNetwork.SendRate);

        // ���� ����
        PhotonNetwork.ConnectUsingSettings();
    }

    // ���� ������ ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnConnectedToMaster() // �κ� ���Դ� �� ���θ� ��Ÿ���� �Ӽ��� ����� �� ����?
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"[PhotonNetwork.InLobby] = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby(); // �κ� ����
    }

    // �κ� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedLobby()
    {
        Debug.Log($"[PhotonNetwork.InLobby] = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom(); // ������ �� �� ������ �濡 ������ => ���� ��ġ ����ŷ
    }

    // ������ room ���忡 �������� ��� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // returnCode = �����ڵ�, message = ���� �޼���
        Debug.Log($"JoinRandom Failed {returnCode} : {message}");

        // Room�� �Ӽ��� ����
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;  // �ִ� ������ �� ����, ���� ���� �������� �ִ� 20����
        ro.IsOpen = true;    // Room�� open ����
        ro.IsVisible = true; // ���������� �����������?

        // Room ����
        PhotonNetwork.CreateRoom("My Room", ro);
    }

    // Room ������ �Ϸ�� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnCreatedRoom()
    {
        Debug.Log($"Created Room");
        Debug.Log($"Room name : {PhotonNetwork.CurrentRoom.Name}");
    }

    // Room�� ������ �� ȣ��Ǵ� �ݹ� �Լ�
    //public override void OnJoinedRoom()
    //{
    //    Debug.Log($"PhotonNetwork.InRoom : {PhotonNetwork.InRoom}"); // Room�� ����Ǿ����� �� True, �ƴ϶�� False�� ��ȯ��.
    //    Debug.Log($"Player Count : {PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}");

    //    // Room�� ������ Player ������ Ȯ��
    //    foreach (var player in PhotonNetwork.CurrentRoom.Players)
    //    {
    //        Debug.Log($"No.{player.Value.ActorNumber} : {player.Value.NickName}");
    //    }

    //    // Player Spawn Point Info
    //    Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
    //    int index = Random.Range(1, points.Length);

    //    // Player Spawn
    //    PhotonNetwork.Instantiate("Player", points[index].position, points[index].rotation, 0);
    //}

}
