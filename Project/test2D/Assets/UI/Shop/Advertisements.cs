using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 広告
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class Advertisements : MonoBehaviour
{
    [SerializeField] private int PlusMoney = 1000;
    public int plusmoney { get { return this.PlusMoney; } }

    private bool IsShow = false;
    public bool isShow
    {  get { return this.IsShow; }}

    // Start is called before the first frame update
    void Start()
    {
        IsShow = false;
#if UNITY_ANDROID
        // 広告の初期化
        Advertisement.Initialize("3890947", true);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        // ゲームメインシーンでは削除しない
        if (SceneManager.GetActiveScene().name == "ShopScene")
            ScoreManager.DontDestroyOnLoad(this.gameObject);
        else
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }


    // 広告の表示
    public void ShowAds()
    {
#if UNITY_ANDROID
        if (Advertisement.IsReady())
        {
            //広告の表示
            Advertisement.Show();

            // 広告表示済みフラグ
            IsShow = true;
        }
        else
        {
            Debug.Log("Ads shows Failed");
        }
#endif
    }
}
