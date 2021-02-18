using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ランキングボタンの自動無効化クラス
/// </summary>
public class RankingDisableButton : MonoBehaviour
{
    [SerializeField] RankingSceneManager sceneManager = default;
    Button button = default;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        // メイン状態以外はボタンを無効化
        if (sceneManager.state != RankingSceneManager.STATE.MAIN)
        {
            button.enabled = false;
        }
    }
}
