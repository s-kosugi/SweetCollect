using TMPro;
using UnityEngine;

/// <summary>
/// 入手コイン数字クラス
/// </summary>
public class GetPointNumber : MonoBehaviour
{
    private ScoreManager scoreManager = default;
    private TextMeshProUGUI textMesh = default;

    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        textMesh = gameObject.GetComponent<TextMeshProUGUI>();
        textMesh.text = "    = " + string.Format("{0:00000}", scoreManager.GetCoinScore());
        
    }

}
