using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] int CoinScore = 0;
    [SerializeField] public int GameScore { get; private set; } = 0;
    [SerializeField] PlayFabVirtualCurrency m_PlayFabVirtualCurrency = null;


    void Start()
    {
        
    }

    void Update()
    {
        // ゲームメインシーンでは削除しない
        if (SceneManager.GetActiveScene().name == "GameMainScene")
            ScoreManager.DontDestroyOnLoad(this.gameObject);
        else
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }

    public void AddScore(int value)
    {
        CoinScore += value;
        if (CoinScore < 0) CoinScore = 0;
    }
    public int GetCoinScore()
    {
        return CoinScore;
    }
    public void Reset()
    {
        CoinScore = 0;
        GameScore = 0;
        m_PlayFabVirtualCurrency = GameObject.Find("PlayFabVirtualCurrency").GetComponent<PlayFabVirtualCurrency>();
    }
    /// <summary>
    /// スコアの確定
    /// </summary>
    public void ConfirmScore()
    {
        GameScore = CoinScore;
    }

    /// <summary>
    /// 現在のスコアを仮想通貨に追加
    /// </summary>
    public void AddVirtualCurrency()
    {
        // 仮想通貨の加算
        if (m_PlayFabVirtualCurrency)
        {
            Debug.Log("AddVirtualCurrency");
            if (CoinScore > 0)
            {
                // 仮想通貨を加算する
                m_PlayFabVirtualCurrency.AddUserVirtualCurrency("HA", CoinScore);
            }
        }
    }
}
