using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class UserNameTextBox : MonoBehaviour
{
    [SerializeField] TitleManager titleManager = default;
    PlayFabUserProfiel m_PlayFabUserProfiel = null;
    InputField inputField = default;
    private bool IsNameSet = false;

    void Start()
    {
        m_PlayFabUserProfiel = GameObject.Find("PlayFabUserProfiel").GetComponent<PlayFabUserProfiel>();
        inputField = GetComponent<InputField>();
    }


    void Update()
    {
        // PlayFabから名前未設定の場合はテキストに名前をセットする
        if (!IsNameSet)
        {
            if (m_PlayFabUserProfiel.isGet)
            {
                string displayName = m_PlayFabUserProfiel.DisplayName;
                if (displayName != "")
                {
                    // ログインしてたらユーザーネームをセットする
                    inputField.text = displayName;
                    IsNameSet = true;
                }
            }
        }
        // メイン状態以外は入力を受け付けない
        if (titleManager.fadeState != BaseScene.FADE_STATE.NONE)
        {
            inputField.enabled = false;
        }
        else
        {
            inputField.enabled = true;
        }
    }
}
