using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルトシーンボタンの有効化クラス
/// </summary>
public class EnableResultButton : MonoBehaviour
{
    private ResultSceneManager resultScene = default;
    private Button button = default;


    void Start()
    {
        resultScene = GameObject.Find("ResultSceneManager").GetComponent<ResultSceneManager>();
        button = gameObject.GetComponent<Button>();
    }


    void Update()
    {
        // リザルトが操作可能になったらボタンを有効化する
        if (resultScene.state == ResultSceneManager.STATE.MAIN)
        {
            button.enabled = true;
        }
    }
}
