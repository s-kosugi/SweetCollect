using TMPro;
using UnityEngine;

public class AchievementButton : MonoBehaviour
{
    private AchievementParent parent = default;
    [SerializeField] TextMeshProUGUI title = default;
    [SerializeField] TextMeshProUGUI progress = default;
    [SerializeField] Color reachColor = default;
    [SerializeField] Color defaultColor = default;
    /// <summary>
    /// 実績達成済みかどうか
    /// </summary>
    public bool ReachAchievement = false;
    void Start()
    {
        parent = transform.parent.GetComponent<AchievementParent>();
    }

    private void Update()
    {
        // 解放状態によってテキストの色を変更する
        if (ReachAchievement)
        {
            title.color = reachColor;
            progress.color = reachColor;
        }
        else
        {
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
        parent.UpdateDescript(this.name,ReachAchievement);
    }
}
