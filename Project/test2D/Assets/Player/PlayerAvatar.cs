using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    private PlayFabPlayerData m_AvatarData = null;
    private SpriteRenderer m_SpriteRenderer = null;
    public bool m_isAvatarChange { get; private set; }
    [SerializeField] PlayFabWaitConnect waitConnect = default;

    void Start()
    {
        // 着用している衣服データを取得
        m_AvatarData = GameObject.Find("PlayFabEclothesData").GetComponent<PlayFabPlayerData>();
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
            // 通信待ちしていないかどうか。※一旦通信タイムアウトは考えない
            if (!waitConnect.IsWait())
            {
                Sprite sprite = Resources.Load<Sprite>("Player\\" + m_AvatarData.m_Value);
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
