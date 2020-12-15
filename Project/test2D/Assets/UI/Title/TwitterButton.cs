using UnityEngine;
using UnityEngine.Networking;

public class TwitterButton : MonoBehaviour
{
    private ScoreManager scoreManager = default;
    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PushButton()
    {
        //urlの作成
        string esctext = UnityWebRequest.EscapeURL("今回のスコアは\n"+scoreManager.GetScore()+" だよ\nhttps://twitter.com/SweetCollectDev");
        string esctag = UnityWebRequest.EscapeURL("スイートコレクト");
        string url = "https://twitter.com/intent/tweet?text=" + esctext + "&hashtags=" + esctag;

        //Twitter投稿画面の起動
        Application.OpenURL(url);
    }
}
