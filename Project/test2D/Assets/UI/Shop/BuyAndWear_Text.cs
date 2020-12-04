using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyAndWear_Text : MonoBehaviour
{
    [SerializeField] string BuyText = "-2000";
    [SerializeField] string WearText = "";

    [SerializeField] TextMeshProUGUI Text;          //文字
    [SerializeField] private bool IsHaving;     //取得済み

    struct Text_BuyAndWear
    {
        public string Buy;
        public string Wear;
    }
    Text_BuyAndWear text_BuyAndWear;

    // Start is called before the first frame update
    private void Awake()
    {
        Text = this.GetComponent<TextMeshProUGUI>();
        text_BuyAndWear.Buy = BuyText;
        text_BuyAndWear.Wear = WearText;
        Text.text = "";
    }

    //===========================================================================================================
    //描画関連
    private void PreviewText()
    {
        if(IsHaving)
        {
            Text.text = text_BuyAndWear.Wear;
        }
        else
        {
            Text.text = text_BuyAndWear.Buy;
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
}
