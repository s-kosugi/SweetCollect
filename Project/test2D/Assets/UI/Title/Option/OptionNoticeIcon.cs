using PlayFab.ClientModels;
using UnityEngine;

public class OptionNoticeIcon : MonoBehaviour
{
    [SerializeField] PlayFabPlayerData playerData = default;
    private bool isInit = false;


    void Update()
    {
        // 初回値取得時にオブジェクトを非表示にするかを設定
        SetFirstActive();
    }
    void SetFirstActive()
    {
        if (playerData.m_isGet && !isInit)
        {
            UserDataRecord record = default;
            if (playerData.m_Data.TryGetValue(PlayerDataName.NOTICE_OPTION, out record))
            {
                // 通知アイコンが表示状態を見て表示
                if (record.Value != "TRUE")
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                gameObject.SetActive(false);
            }

            isInit = true;
        }
    }

    /// <summary>
    /// ボタンがタップされたらアイコンを非表示にする
    /// </summary>
    public void TapOptionButton()
    {
        gameObject.SetActive(false);
    }
}
