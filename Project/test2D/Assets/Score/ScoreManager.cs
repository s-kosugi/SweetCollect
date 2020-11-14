using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] int Score = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
    }
    public int GetScore()
    {
        return Score;
    }
    public void ResetScore()
    {
        Score = 0;
    }
}
