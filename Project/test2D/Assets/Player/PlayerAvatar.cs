using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    [SerializeField] PlayFabPlayerData playerData = default;
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    private SpriteRenderer m_SpriteRenderer = null;
    public bool m_isAvatarChange { get; private set; }

    void Start()
    {
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
            if (playerData.isGet && !waitConnect.IsWait())
            {
                UserDataRecord record;
                // プレイヤーデータを取得して衣服を変更する
                if (playerData.data.TryGetValue(PlayerDataName.ECLOTHES, out record))
                {
                    Sprite sprite = Resources.Load<Sprite>("Player\\" + record.Value);
                    if (sprite)
                    {
                        m_SpriteRenderer.sprite = sprite;
                        Debug.Log("PlayerAvaterChanged");
                    }
                }
                m_isAvatarChange = true;
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

    /// <summary>
    /// アバターの更新
    /// </summary>
    /// <param name="itemID">服のID</param>
    public void UpdateAvatar(string itemID)
    {
        Sprite sprite = Resources.Load<Sprite>("Player\\" + itemID);
        if (sprite)
        {
            m_SpriteRenderer.sprite = sprite;
            Debug.Log("PlayerAvaterChanged");
        }
    }
}
