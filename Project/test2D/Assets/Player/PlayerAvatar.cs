using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    private PlayFabPlayerData m_PlayerData = null;
    private SpriteRenderer m_SpriteRenderer = null;
    public bool m_isAvatarChange { get; private set; }
    [SerializeField] PlayFabWaitConnect waitConnect = default;

    void Start()
    {
        // プレイヤーデータを取得
        m_PlayerData = GameObject.Find("PlayFabPlayerData").GetComponent<PlayFabPlayerData>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_isAvatarChange = false;
    }

    void Update()
    {
        CheckAvatarData();
    }
    private void CheckAvatarData()
    {
        if (!m_isAvatarChange)
        {
            // 通信待ちしていないかどうか。
            if (!waitConnect.IsWait())
            {
                // プレイヤーデータを取得して衣服を変更する
                Sprite sprite = Resources.Load<Sprite>("Player\\" + m_PlayerData.m_Data[PlayerDataName.ECLOTHES].Value);
                if (sprite)
                {
                    m_SpriteRenderer.sprite = sprite;
                }
                m_isAvatarChange = true;
                Debug.Log("PlayerAvaterChanged");

                return;
            }
        }
    }

    /// <summary>
    /// アバターの更新
    /// </summary>
    public void UpdateAvatar()
    {
        m_isAvatarChange = false;
    }
}
