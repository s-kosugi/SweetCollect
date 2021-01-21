using UnityEngine;
using UnityEngine.UI;

public class AchievementDisablePopup : MonoBehaviour
{
    [SerializeField] AchievementSceneManager manager = default;
    Button button = default;
    bool oldEnable = true;

    void Start()
    {
        button = GetComponent<Button>();
        if (manager == default)
            manager = GameObject.Find("AchievementSceneManager").GetComponent<AchievementSceneManager>();
    }


    void LateUpdate()
    {
        // ポップアップ表示中はボタンを無効化する
        if (manager.state == AchievementSceneManager.STATE.POPUP)
        {
            button.enabled = false;
            oldEnable = false;
        }
        else
        {
            // ポップアップからそうでない状態に切り替わった時に一度だけボタンを有効化する
            if (oldEnable == false)
            {
                button.enabled = true;
                oldEnable = true;

            }
        }
    }
}
