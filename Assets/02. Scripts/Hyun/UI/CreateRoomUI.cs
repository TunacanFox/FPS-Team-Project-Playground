using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateRoomUI : MonoBehaviour
{
    [SerializeField] TMP_InputField _roomName;
    [SerializeField] Button _confirm;
    [SerializeField] Button _cancel;

    private void Start()
    {
        _confirm.onClick.AddListener(() =>
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 8;
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.CleanupCacheOnLeave = true;
            if(PhotonNetwork.CreateRoom(_roomName.text, roomOptions, TypedLobby.Default))
            {
                SceneManager.LoadScene("Room");
            }
        });

        _cancel.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

    }
}
