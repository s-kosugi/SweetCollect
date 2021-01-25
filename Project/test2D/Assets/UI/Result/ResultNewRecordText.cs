using TMPro;
using UnityEngine;

public class ResultNewRecordText : MonoBehaviour
{
    [SerializeField] ResultSceneManager sceneManager = default;
    [SerializeField] ScoreManager scoreManager = default;
    [SerializeField] HiScoreText hiScore = default;
    private TextMeshProUGUI textMesh = default;
    bool isSet = false;


    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        transform.localScale = Vector3.zero;
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }


    void Update()
    {
        if (!isSet)
        {
            if (sceneManager.state == ResultSceneManager.STATE.MAIN)
            {
                if (hiScore.isSet)
                {
                    if (scoreManager.GameScore > hiScore.hiScore)
                    {
                        // メイン状態になったらテキストを表示する
                        transform.localScale = new Vector3(1.0f, 1.0f);
                    }
                    isSet = true;
                }
            }
        }
    }
}
