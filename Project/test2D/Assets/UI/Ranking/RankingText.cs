using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingText : MonoBehaviour
{
    private GameObject PlayFabManager = null;
    private bool IsGetRank = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayFabManager = GameObject.Find("PlayFabManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGetRank)
        {
            if (PlayFabManager != null )
            {
                TextMeshProUGUI textComp = gameObject.GetComponent<TextMeshProUGUI>();

                string oldText = textComp.text;
                // ランキング取得関数がデリゲートで非同期の為、更新で参照する。
                textComp.text = PlayFabManager.GetComponent<PlayFabLeaderBoard>().m_RankingText;

                // テキストがリーダーボードによって更新済みになった場合
                if (oldText != textComp.text)
                {
                    IsGetRank = true;
                }
            }
            else
            {
                Debug.Log("PlayFabManager is null");
            }
        }
    }
}