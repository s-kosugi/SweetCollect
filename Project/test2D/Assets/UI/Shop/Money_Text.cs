using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money_Text : MonoBehaviour
{
    [SerializeField] private PlayFabVirtualCurrency playFabVirtualCurrency = null;  //仮想通貨
    [SerializeField] PlayFabWaitConnect connect = null;                             //通信関連
    uint MyMoney;                                                  //所持金
    [SerializeField] TextMeshProUGUI Text_Money;                                    //表示テキスト
    bool IsCheck;                                                  //確認中
    bool IsRequest;                                                //リクエスト中
    // Start is called before the first frame update
    private void Awake()
    {
        IsRequest = true;
        Text_Money.text = "??????";
        RequestMoney();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckMoney();
    }
    //===========================================================================================================
    //描画関連 
    private void PreviewMoney()
    {
        Text_Money.text = MyMoney.ToString();
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
                    playFabVirtualCurrency.RequestUpdate();
                    IsRequest = true;
                }

                // 仮想通貨情報が取得済みかどうか
                if (playFabVirtualCurrency.isGet)
                {
                    if (playFabVirtualCurrency.VirtualCurrency.ContainsKey("HA"))
                    {
                        MyMoney = (uint)playFabVirtualCurrency.VirtualCurrency["HA"];
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
