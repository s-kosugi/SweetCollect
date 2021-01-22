using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementButton : MonoBehaviour
{
    private AchievementParent parent = default;
    [SerializeField] TextMeshProUGUI title = default;
    [SerializeField] TextMeshProUGUI progress = default;
    [SerializeField] Color reachColor = default;
    [SerializeField] Color defaultColor = default;
    [SerializeField] DisableWaitConnectButton disableWait = default;
    [SerializeField] DisableSceneFadeButton disableSceneFade = default;
    [SerializeField] Image noticeIcon = default;

    /// <summary>
    /// 実績達成済みかどうか
    /// </summary>
    public bool ReachAchievement = false;
    void Start()
    {
        parent = transform.parent.GetComponent<AchievementParent>();

        // ボタン無効化スクリプトにシーンを教える
        AchievementSceneManager achievementScene = GameObject.Find("AchievementSceneManager").GetComponent<AchievementSceneManager>();
        disableWait.scene = achievementScene;
        disableSceneFade.scene = achievementScene;
    }

    private void Update()
    {
        // 通知アイコンの有効化
        EnableNoticeIcon();

        // 解放状態によってテキストの色を変更する
        if (ReachAchievement)
        {
            title.color = reachColor;
            progress.color = reachColor;
        }
        else
        {
            title.text = "？？？？？";
            title.color = defaultColor;
            progress.color = defaultColor;
        }
    }

    /// <summary>
    /// 押された情報の送信
    /// </summary>
    public void SendPushData()
    {
        // ボタンを押されたのでどのIDが押されたかを親へ教える
        parent.UpdateDescript(this.name, ReachAchievement);

        // ボタンが押されたので通知を削除する
        UserDataRecord record;
        PlayFabPlayerData playerData = parent.GetPlayerData();
        if (playerData.m_isGet && playerData.m_Data.TryGetValue("NOTICE_" + gameObject.name, out record))
        {
            if (record.Value == "TRUE")
            {
                playerData.SetPlayerData("NOTICE_" + gameObject.name,"FALSE");
                noticeIcon.enabled = false;
            }
        }
    }

    /// <summary>
    /// 通知アイコンの有効化
    /// </summary>
    void EnableNoticeIcon()
    {
        UserDataRecord record;
        PlayFabPlayerData playerData = parent.GetPlayerData();
        if (playerData.m_isGet && playerData.m_Data.TryGetValue("NOTICE_" + gameObject.name, out record))
        {
            if (noticeIcon.enabled != true && record.Value == "TRUE")
            {
                noticeIcon.enabled = true;
            }
            else if(record.Value == "FALSE")
            {
                noticeIcon.enabled = false;
            }
        }
        else
        {
            noticeIcon.enabled = false;
        }
    }
}
