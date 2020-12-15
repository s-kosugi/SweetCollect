using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] int Score = 0;
    [SerializeField] PlayFabVirtualCurrency m_PlayFabVirtualCurrency = null;

    // Start is called before the first frame update
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
        Score += value;
        if (Score < 0) Score = 0;
    }
    public int GetScore()
    {
        return Score;
    }
    public void ResetScore()
    {
        Score = 0;
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
            if (Score > 0)
            {
                // 仮想通貨を加算する
                m_PlayFabVirtualCurrency.AddUserVirtualCurrency("HA", Score);
            }
        }
    }
}
