using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    public static Dictionary<string, PlayerSetup> spawned = new Dictionary<string, PlayerSetup>();
    public string nickName;

    public TextMeshPro nickNameText;
    private PhotonView _photonView;
    public Dictionary<string, object> customData = new Dictionary<string, object>();


    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        spawned.Add(_photonView.Owner.UserId, this);
    }

    private void OnDestroy()
    {
        spawned.Remove(_photonView.Owner.UserId);
        /*
        ArgumentNullException: Value cannot be null.
        Parameter name: key
        System.Collections.Generic.Dictionary`2[TKey,TValue].Remove (TKey key) (at <b89873cb176e44a995a4781c7487d410>:0)
        PlayerSetup.OnDestroy () (at Assets/02. Scripts/Lee/PlayerSetup.cs:25)
         */
    }

    [PunRPC]
    public void SetNickname(string _name)
    {
        nickName = _name;

        nickNameText.text = nickName;
    }
    
}
