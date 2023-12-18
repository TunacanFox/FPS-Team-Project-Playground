using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListUI : MonoBehaviour
{
    [SerializeField] Button _create;
    [SerializeField] Button _join;
    public RoomInfo selected
    {
        get
        {
            if(_selected != null &&
                _selected.RemovedFromList)
            {
                selected = null;
            }
            return _selected;
        }
        set
        {
            _join.interactable = value != null;
        }
    }
    private RoomInfo _selected;
    [SerializeField] RoomListSlot _slotPrefab;
    List<RoomListSlot> _slots = new List<RoomListSlot>();
    [SerializeField] Transform _content;
    private void Start()
    {
        PhotonManager.instance.onRoomListUpdate += Refresh;
        
    }

    private void OnDisable()
    {
        PhotonManager.instance.onRoomListUpdate -= Refresh;
    }


    public void Refresh(List<RoomInfo> roomInfos)
    {
        for(int i = _slots.Count - 1; i >= 0; i--)
        {
            Destroy(_slots[i]);
        }

        RoomListSlot slot;
        for(int i = 0; i< roomInfos.Count; i++)
        {
            slot = Instantiate(_slotPrefab, _content);
            slot.roomListUI = this;
            slot.Refresh(roomInfos[i]);
        }
    }
}
