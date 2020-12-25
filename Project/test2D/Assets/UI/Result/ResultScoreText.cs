using TMPro;
using UnityEngine;

public class ResultScoreText : MonoBehaviour
{
    private TextMeshProUGUI text = null;
    private ScoreManager scoreManager = null;
    private bool isSet = false;
    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // ゲームスコアを取得して表示
        if (scoreManager != null && !isSet)
        {
            text.text = string.Format("{0:0000}", scoreManager.GameScore);
            isSet = true;
        }
    }
}
