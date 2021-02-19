using UnityEngine;


/// <summary>
/// 開始UIクラス
/// </summary>
public class StartUI : MonoBehaviour
{
    [SerializeField] private GameObject LetsTextObject = null;
    [SerializeField] private GameObject StartTextObject = null;

    // 演出終了フラグ
    public bool isEnd { get; private set; }


    void Start()
    {
        isEnd = false;

        // オブジェクト起動時に最初の開始演出を有効化する。
        LetsTextObject.SetActive(true);
    }


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
