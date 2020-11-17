using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingText : MonoBehaviour
{
    private PlayFabLeaderBoard m_PlayFabLeaderBoard = null;
    private TextMeshProUGUI m_Text = null;
    private bool IsGetRank = false;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayFabLeaderBoard = GameObject.Find("PlayFabLeaderBoard").GetComponent<PlayFabLeaderBoard>();
        m_Text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGetRank)
        {
            if (m_PlayFabLeaderBoard != null )
            {
                string oldText = m_Text.text;
                // ランキング取得関数がデリゲートで非同期の為、更新で参照する。
                m_Text.text = m_PlayFabLeaderBoard.m_RankingText;

                // テキストがリーダーボードによって更新済みになった場合
                if (oldText != m_Text.text)
                {
                    IsGetRank = true;
                }
            }
            else
            {
                Debug.LogError("m_PlayFabLeaderBoard is null");
            }
        }
    }
}