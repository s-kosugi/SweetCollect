using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchivementButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI targetText = default;
    [SerializeField] string descriptText = default;
    void Start()
    {
        
    }


    void Update()
    {
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="targetText">変更対象のテキストオブジェクト</param>
    /// <param name="description">変更するテキスト内容</param>
    public void Setup(TextMeshProUGUI target,string description)
    {
        targetText = target;
        descriptText = description;
    }

    /// <summary>
    /// ボタンクリックされた時にDescript用テキストを変更する
    /// </summary>
    public void ChangeDescript()
    {
        // 親にセットされた文章をDescript用オブジェクトにセットする
        targetText.text = descriptText;
    }

}
