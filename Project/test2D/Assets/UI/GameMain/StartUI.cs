using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    [SerializeField] private GameObject LetsTextObject = null;
    [SerializeField] private GameObject StartTextObject = null;

    // 演出終了フラグ
    public bool isEnd { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        isEnd = false;

        // オブジェクト起動時に最初の開始演出を有効化する。
        LetsTextObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnd)
        {
            // 最初の演出オブジェクトが破棄されたら次の演出オブジェクトを呼び出す
            if (LetsTextObject == null && StartTextObject != null)
            {
                StartTextObject.SetActive(true);
            }
            // 最後の演出オブジェクトが破棄されていたら演出終了フラグをONにする
            if (StartTextObject == null)
            {
                isEnd = true;
            }
        }
    }
}
