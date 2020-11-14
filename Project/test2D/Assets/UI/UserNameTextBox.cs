﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class UserNameTextBox : MonoBehaviour
{
    GameObject playFabManager = null;
    private bool IsNameSet = false;
    // Start is called before the first frame update
    void Start()
    {
        playFabManager = GameObject.Find("PlayFabManager");
    }

    // Update is called once per frame
    void Update()
    {
        // PlayFabから名前未設定の場合はテキストに名前をセットする
        if (!IsNameSet)
        {
            // Playfabにログイン済みかを確認する
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                string displayName = playFabManager.GetComponent<PlayFabUserProfiel>().displayName;
                if (displayName != "")
                {
                    // ログインしてたらユーザーネームをセットする
                    gameObject.GetComponent<InputField>().text = displayName;
                    IsNameSet = true;
                }
            }
        }
    }
}
