using TMPro;
using UnityEngine;

/// <summary>
/// スコア数字クラス
/// </summary>
public class MainGameScoreNumber : MonoBehaviour
{
    private ScoreManager scoreManager = default;
    private TextMeshProUGUI textMesh = default;
    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        textMesh = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if( scoreManager ) textMesh.text = string.Format("× {0:0000}", scoreManager.GetCoinScore());
    }
}
