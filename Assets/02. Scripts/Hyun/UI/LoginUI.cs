using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField] TMP_InputField _id;
    [SerializeField] TMP_InputField _pw;
    [SerializeField] Button _login;

    private void Awake()
    {
        _login.onClick.AddListener(() =>
        {
            if (PhotonManager.instance.TrySetUserID(_id.text, _pw.text))
            {
                SceneManager.LoadScene("Lobby");
                PhotonNetwork.JoinLobby();
            }
            else
            {
                // to do pop up alert window that id or pw is wrong.
            }
        });
    }
}
