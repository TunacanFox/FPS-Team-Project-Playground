using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListSlot : MonoBehaviour
{
    public RoomListUI roomListUI;
    [SerializeField] TMP_Text _name;
    [SerializeField] Button _select;
    RoomInfo _roomInfo;


    public void Refresh(RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        _name.text = roomInfo.Name;

        _select.onClick.RemoveAllListeners();
        _select.onClick.AddListener(() =>
        {
            roomListUI.selected = _roomInfo;
        });
    }

}