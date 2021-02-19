using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money_Text : MonoBehaviour
{
    [SerializeField] private PlayFabVirtualCurrency virtualcurrency = null;         //仮想通貨
    [SerializeField] PlayFabWaitConnect             connect = null;                 //通信関連
    [SerializeField] TextMeshProUGUI                TextMoney = default;            //表示テキスト
    uint                                            MyMoney;                        //所持金
    bool                                            IsCheck;                        //確認中
    bool                                            IsRequest;                      //リクエスト中
    private void Awake()
    {
        IsRequest = true;
        TextMoney.text = "??????";
        RequestMoney();
    }

    void Update()
    {
        CheckMoney();
    }
    //===========================================================================================================
    //描画関連 
    private void PreviewMoney()
    {
        TextMoney.text = MyMoney.ToString();
    }
    //===========================================================================================================

    //所持金の確認
    public void CheckMoney()
    {
        //確認中
        if (IsCheck)
        {
            //通信終了
            if(!connect.IsWait())
            {
                //リクエストしていなければリクエスト
                if(!IsRequest)
                {
                    virtualcurrency.RequestUpdate();
                    IsRequest = true;
                }

                // 仮想通貨情報が取得済みかどうか
                if (virtualcurrency.isGet)
                {
                    if (virtualcurrency.VirtualCurrency.ContainsKey("HA"))
                    {
                        MyMoney = (uint)virtualcurrency.VirtualCurrency["HA"];
                        IsCheck = false;
                        IsRequest = false;
                        Debug.Log("所持金" + MyMoney);
                    }
                    PreviewMoney();
                }
            }
        }
    } 

    //所持金の確認
    public void RequestMoney()
    {
        IsCheck = true;
    }
    //===========================================================================================================
    //===========================================================================================================
    //描画関連 
    public uint GetPossessionMoney()
    {
        return MyMoney;
    }
    //===========================================================================================================
    
}
