using TMPro;
using UnityEngine;

public class ResultScoreText : MonoBehaviour
{
    private TextMeshProUGUI text = null;
    private ScoreManager scoreManager = null;
    private bool isSet = false;

    public float animationTime = 3.0f;
    private float animationCount = 0f;

    public STATE state = STATE.WAIT;
    
    public enum STATE
    {
        APPEAR,
        WAIT,
    }

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    void Update()
    {
        switch (state)
        {
            case STATE.APPEAR: Appear(); break;
            case STATE.WAIT: Wait(); break;
        }
    }

    /// <summary>
    /// 出現時の演出
    /// </summary>
    private void Appear()
    {
        animationCount += Time.deltaTime;
        if (animationCount >= animationTime)
        {
            state = STATE.WAIT;
            animationCount = 0f;
            text.text = string.Format("{0:0000}", 0);
            isSet = false;
        }
        else
        {
            // 数字をランダムに入れて表示する演出
            text.text = string.Format("{0:0000}",Random.Range(0,9999));
        }
    }

    /// <summary>
    /// 待機時の処理
    /// </summary>
    private void Wait()
    {
        // ゲームスコアを取得して表示
        if (scoreManager != null && !isSet)
        {
            text.text = string.Format("{0:0000}", scoreManager.GameScore);
            animationCount = 0f;
            isSet = true;
        }
        else
        {
            text.text = string.Format("{0:0000}", 0);
        }
    }

}
