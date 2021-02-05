using UnityEngine;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField] TitleManager titleManager = default;
    Button button = default;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        // ステージセレクト操作状態以外はボタンを無効にする
        if (titleManager.state == TitleManager.STATE.STAGESELECT_CONTROL)
        {
            button.enabled = true;
        }
        else
        {
            button.enabled = false;
        }
    }
}
