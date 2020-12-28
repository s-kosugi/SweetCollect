using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchivementButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI targetText = default;
    [SerializeField] string descriptText = default;
    [SerializeField] RewordImage rewordImage = default;

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
    /// <param name="image">報酬画像オブジェクト</param>
    public void Setup(TextMeshProUGUI target,string description, RewordImage image)
    {
        targetText = target;
        descriptText = description;
        rewordImage = image;
    }

    /// <summary>
    /// ボタンクリックされた時にDescript用テキストを変更する
    /// </summary>
    public void ChangeDescript()
    {
        // 親にセットされた文章をDescript用オブジェクトにセットする
        targetText.text = descriptText;
    }

    /// <summary>
    /// ボタンがクリックされた時に報酬画像を変更する
    /// </summary>
    public void ChangeRewordImage()
    {
        // ゲームオブジェクト名が実績名なのでそれを渡す
        rewordImage.ShowImage(gameObject.name);
    }

}
