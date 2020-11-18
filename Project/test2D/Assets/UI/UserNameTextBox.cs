using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class UserNameTextBox : MonoBehaviour
{
    PlayFabUserProfiel m_PlayFabUserProfiel = null;
    private bool IsNameSet = false;
    // Start is called before the first frame update
    void Start()
    {
        m_PlayFabUserProfiel = GameObject.Find("PlayFabUserProfiel").GetComponent<PlayFabUserProfiel>();
    }

    // Update is called once per frame
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
                    gameObject.GetComponent<InputField>().text = displayName;
                    IsNameSet = true;
                }
            }
        }
    }
}
