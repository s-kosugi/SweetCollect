using UnityEngine;
using UnityEngine.UI;

public class DisableSceneFadeButton : MonoBehaviour
{
    [SerializeField] BaseScene scene = default;
    [SerializeField] bool StartAutoEnabled = true;
    Button button = default;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        // シーン遷移中はボタンを無効化する
        if (scene.fadeState != BaseScene.FADE_STATE.NONE )
        {
            button.enabled = false;
        }
        else
        {
            // シーンがフェード状態から切り替わった時に1度だけボタンを有効化させる
            if (StartAutoEnabled == true)
            {
                button.enabled = true;

                StartAutoEnabled = false;
            }
        }
    }
}
