using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// リトライボタンクラス
/// </summary>
public class GameMainRetryButton : MonoBehaviour
{
    [SerializeField] Transform retryTransform = default;
    [SerializeField] GameMainManager mainManager = default;
    [SerializeField] ScoreManager scoreManager = default;
    Button button = default;
    bool retry = false;     // リトライ中かどうか


    void Start()
    {
        // リトライフレームは隠しておく
        retryTransform.localScale = Vector3.zero;
        button = GetComponent<Button>();
    }


    void Update()
    {
        if (Time.timeScale != 0 && !retry)
        {
            // リトライボタンはゲームプレイ中のみ有効
            if (mainManager.state == GameMainManager.STATE.MAIN)
            {
                button.enabled = true;
            }
            else
            {
                button.enabled = false;
            }
        }
    }

    /// <summary>
    /// リトライボタン押下時の処理
    /// </summary>
    public void PushRetryButton()
    {
        button.enabled = false;
        Time.timeScale = 0f;
        retryTransform.localScale = new Vector3(1f, 1f, 1f);
    }

    /// <summary>
    /// リトライキャンセルボタン押下時
    /// </summary>
    public void PushCancelButton()
    {
        if (retry) return;

        Time.timeScale = 1f;
        retryTransform.localScale = Vector3.zero;
        button.enabled = true;
    }

    /// <summary>
    /// リトライ続行ボタン押下時
    /// </summary>
    public void PushRetryContinue()
    {
        if (retry) return;

        Time.timeScale = 1f;
        scoreManager.Reset();
        SceneManager.LoadScene("GameMainScene");

        retry = true;
    }
}
