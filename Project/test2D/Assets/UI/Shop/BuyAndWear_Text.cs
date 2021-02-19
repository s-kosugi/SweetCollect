using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyAndWear_Text : MonoBehaviour
{
    [SerializeField] string             BuyText = "-2000";

    [SerializeField] TextMeshProUGUI    Text;          //文字
    [SerializeField] private bool       IsHaving;     //取得済み


    private void Start()
    {
        Text = this.GetComponent<TextMeshProUGUI>();
        Text.text = "";
    }

    //===========================================================================================================
    //描画関連
    private void PreviewText()
    {
        if(IsHaving)
        {
            Text.text = BuyText;
        }
        else
        {
            Text.text = "";
        }
    }
    //===========================================================================================================
    //設定(Setter)
    //取得フラグの設定
    public void SetTextFlag(bool have)
    {
        IsHaving = have;
        PreviewText();
    }
    /// <summary>
    /// 購入テキストの変更
    /// </summary>
    /// <param name="text">変更内容</param>
    public void SetBuyText(string text)
    {
        BuyText = text;
        PreviewText();
    }
}
