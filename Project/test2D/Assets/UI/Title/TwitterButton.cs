using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TwitterButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PushButton()
    {
        //urlの作成
        string esctext = UnityWebRequest.EscapeURL("テストツイート\nhttps://twitter.com/SweetCollectDev");
        string esctag = UnityWebRequest.EscapeURL("スイートコレクト");
        string url = "https://twitter.com/intent/tweet?text=" + esctext + "&hashtags=" + esctag;

        //Twitter投稿画面の起動
        Application.OpenURL(url);
    }
}
