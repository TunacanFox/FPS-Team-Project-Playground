using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    // 게임 버전
    private readonly string version = "1.0f";

    // 사용자 아이디
    private string _userID = "hyun";

    private void Awake()
    {
        // 같은 room의 유저들에게 자동으로 scene 을 로딩
        PhotonNetwork.AutomaticallySyncScene = true;

        // 같은 버전의 유저끼리 접속 허용
        PhotonNetwork.GameVersion = version;

        // 유저 아이디 할당
        PhotonNetwork.NickName = _userID;

        // 포톤 서버와 통신 횟수 설정, 초당 30회
        Debug.Log(PhotonNetwork.SendRate);

        // 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster() // 로비에 들어왔는 지 여부를 나타내는 속성을 출력할 수 있음?
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"[PhotonNetwork.InLobby] = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby(); // 로비 입장
    }

    // 로비에 접속 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"[PhotonNetwork.InLobby] = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom(); // 생성된 방 중 랜덤한 방에 입장함 => 랜덤 매치 메이킹
    }

    // 랜덤한 room 입장에 실패했을 경우 호출되는 콜백 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // returnCode = 에러코드, message = 에러 메세지
        Debug.Log($"JoinRandom Failed {returnCode} : {message}");

        // Room의 속성을 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;  // 최대 접속자 수 설정, 포톤 무료 버전에서 최대 20명임
        ro.IsOpen = true;    // Room의 open 여부
        ro.IsVisible = true; // 공개방인지 비공개방인지?

        // Room 생성
        PhotonNetwork.CreateRoom("My Room", ro);
    }

    // Room 생성이 완료된 후 호출되는 콜백 함수
    public override void OnCreatedRoom()
    {
        Debug.Log($"Created Room");
        Debug.Log($"Room name : {PhotonNetwork.CurrentRoom.Name}");
    }

    // Room에 입장한 후 호출되는 콜백 함수
    //public override void OnJoinedRoom()
    //{
    //    Debug.Log($"PhotonNetwork.InRoom : {PhotonNetwork.InRoom}"); // Room에 입장되어있을 시 True, 아니라면 False를 반환함.
    //    Debug.Log($"Player Count : {PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}");

    //    // Room에 접속한 Player 정보를 확인
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
