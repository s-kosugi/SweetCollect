using UnityEngine;

public class AchievementButton : MonoBehaviour
{
    private AchievementParent parent = default;
    /// <summary>
    /// 実績達成済みかどうか
    /// </summary>
    public bool ReachAchievement = false;
    void Start()
    {
        parent = transform.parent.GetComponent<AchievementParent>();
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
