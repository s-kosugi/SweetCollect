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

    [SerializeField] Sprite Normal = null;
    [SerializeField] Sprite Santa = null;
    [SerializeField] Sprite PastryChef = null;

    // Start is called before the first frame update
    void Start()
    {
        m_AvatarData = transform.Find("PlayFabEclothesData").GetComponent<PlayFabPlayerData>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_isAvatarChange = false;
    }

    // Update is called once per frame
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
                switch (m_AvatarData.m_Value)
                {
                    case "001_NORMAL": m_SpriteRenderer.sprite = Normal; break;
                    case "002_SANTA": m_SpriteRenderer.sprite = Santa; break;
                    case "003_PASTRYCHEF": m_SpriteRenderer.sprite = PastryChef; break;

                    default: m_SpriteRenderer.sprite = Normal; break;
                }
                m_isAvatarChange = true;
            }
            m_Timer += Time.deltaTime;
            // タイムアウトしたらデフォルトの服にする
            if (m_Timer >= TIMEOUT)
            {
                m_isAvatarChange = true;
            }
        }
    }
}
