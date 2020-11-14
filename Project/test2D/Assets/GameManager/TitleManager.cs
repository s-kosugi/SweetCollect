using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TitleManager : BaseScene
{
    [SerializeField] GameObject textBox = null;
    private GameObject playFabManager;

    override protected void Start()
    {
        SoundManager.Instance.PlayBGM("MainGame");

        playFabManager = GameObject.Find("PlayFabManager");

        NextSceneName = "GameMainScene";

        base.Start();


        // フェードイン状態にする
        fadeState = FADE_STATE.FADEIN;
    }
    // Update is called once per frame
    override protected void Update()
    {
        // テストコード：仮想通貨取得
        //PlayFabVirtualCurrency vc = playFabManager.GetComponent<PlayFabVirtualCurrency>();
        //if( vc && vc.VirtualCurrency.ContainsKey("HA"))
        //    Debug.Log(playFabManager.GetComponent<PlayFabVirtualCurrency>().VirtualCurrency["HA"] + "Happyだよ");
        // テストコード：仮想通貨の追加
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    PlayFabVirtualCurrency vc = playFabManager.GetComponent<PlayFabVirtualCurrency>();
        //    vc.AddUserVirtualCurrency("HA", 2);
        //}
        // テストコード：仮想通貨の消費
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    PlayFabVirtualCurrency vc = playFabManager.GetComponent<PlayFabVirtualCurrency>();
        //    vc.SubtractUserVirtualCurrency("HA", 1);
        //}


        base.Update();
    }

    // 次のシーンへ
    public void NextScene()
    {
        // テキストボックスが空白の場合は次のシーンへいかない
        if (textBox.GetComponent<InputField>().text != ""  && fadeState!=FADE_STATE.FADEOUT)
        {
            // ユーザー名の更新
            playFabManager.GetComponent<PlayFabUserProfiel>().SetUserName(textBox.GetComponent<InputField>().text);

            // フェードアウト状態にする
            fadeState = FADE_STATE.FADEOUT;
        }
    }
}
