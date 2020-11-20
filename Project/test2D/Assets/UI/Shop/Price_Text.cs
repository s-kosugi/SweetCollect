using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Price_Text : MonoBehaviour
{
    [SerializeField] uint Price;  //金額
    [SerializeField] TextMeshProUGUI Text_Price; //表示テキスト

    // Start is called before the first frame update
    void Start()
    {
        Text_Price = this.GetComponent<TextMeshProUGUI>();
        Price = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }
    //===========================================================================================================
    //描画関連
    private void PreviewPrice()
    {
        Text_Price.text = Price.ToString();
    }
    //===========================================================================================================
    //設定(Setter)
    //対応する金額
    public void SetPrice(uint price )
    {
        Price = price;
        PreviewPrice();
    }
    //===========================================================================================================
    //===========================================================================================================
    //取得(Getter)
    //===========================================================================================================
}
