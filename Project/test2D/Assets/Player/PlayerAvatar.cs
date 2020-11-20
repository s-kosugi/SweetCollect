using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    private PlayFabPlayerData m_AvatarData = null;
    private SpriteRenderer m_SpriteRenderer = null;
    private float m_Timer = 0;
    private float TIMEOUT = 1.5f;
    public bool m_isAvatarChange { get; private set; }


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
            // アバターのデータ取得完了済みかどうか。※一旦通信タイムアウトは考えない
            if (m_AvatarData.m_isGet)
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
            m_Timer += Time.deltaTime;
            // タイムアウトしたらデフォルトの服にする
            if (m_Timer >= TIMEOUT)
            {
                Debug.Log("PlayerAvaterTimeOut");
                m_isAvatarChange = true;
            }
        }
    }
}
