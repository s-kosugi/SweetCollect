using TMPro;
using UnityEngine;

/// <summary>
/// ハイスコアの点滅クラス
/// </summary>
public class HiScoreFlashing : MonoBehaviour
{
    [SerializeField] FlashingAnimeText textAnime = default;
    [SerializeField] ResultSceneManager sceneManager = default;
    [SerializeField] ScoreManager scoreManager = default;
    [SerializeField] HiScoreText hiScore = default;
    bool isSet = false;

    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        textAnime.enabled = false;
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
                        // メイン状態になったらテキスト点滅させる
                        textAnime.enabled = true;
                    }
                    isSet = true;
                }
            }
        }
    }
}
