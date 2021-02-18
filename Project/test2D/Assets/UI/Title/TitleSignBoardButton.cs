using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 看板内ボタンクラス
/// </summary>
public class TitleSignBoardButton : MonoBehaviour
{
    [SerializeField] TitleManager titleManager = default;
    Button button = default;
    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        // タイトルが看板操作状態以外の時はボタンを無効化する
        if (titleManager.state != TitleManager.STATE.SIGNBOARD_CONTROL)
        {
            button.enabled = false;
        }
        else
        {
            button.enabled = true;
        }
    }
}
