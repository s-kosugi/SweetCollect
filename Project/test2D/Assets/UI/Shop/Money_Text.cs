using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money_Text : MonoBehaviour
{
    private PlayFabVirtualCurrency playFabVirtualCurrency = null;
    [SerializeField] PlayFabWaitConnect connect = null;
    [SerializeField] uint MyMoney;  //所持金
    [SerializeField] TextMeshProUGUI Text_Money; //表示テキスト
    [SerializeField] bool IsCheck;
    [SerializeField] bool IsRequest;
    // Start is called before the first frame update
    void Start()
    {
        playFabVirtualCurrency = GameObject.Find("PlayFabVirtualCurrency").GetComponent<PlayFabVirtualCurrency>();
        connect = GameObject.Find("PlayFabManager").GetComponent<PlayFabWaitConnect>();
        Text_Money = this.GetComponent<TextMeshProUGUI>();
        IsRequest = false;
        RequestMoney();
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
        if (IsCheck)
        {
            if(!connect.IsWait())
            {
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

}
