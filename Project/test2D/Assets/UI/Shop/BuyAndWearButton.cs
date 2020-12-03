using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyAndWearButton : MonoBehaviour
{
    [SerializeField] private PlayFabStore store = null;            //ストア
    [SerializeField] private PlayFabInventory inventory = null;    //インベントリ
    [SerializeField] private PlayFabWaitConnect connect = null;    //通信
    [SerializeField] private PlayFabPlayerData AvatarData = null;  //プレイヤーデータ

    [SerializeField] private ShopCanvasController shop = null;
    [SerializeField] private Money_Text playermoney = null;
    [SerializeField] private Clothing clothing = null;

    [SerializeField] private Button button;

    [SerializeField] private bool IsPush;       //ボタンを押したかどうか
    [SerializeField] private bool IsAction;     //購入中または着用中
    [SerializeField] private bool IsUpdate;     //更新中

    //状態分け
    enum STATE
    {
        NONE = -1,
        RECEPTION = 0,    //受付
        PUSH,             //押された
        BUYorWEAR,        //購入または着る
        UPDATE,           //更新
    }
    [SerializeField] private STATE State_Button = STATE.NONE;      //ボタンの状態


    // Start is called before the first frame update
    void Start()
    {
        store = GameObject.Find("PlayFabStore").GetComponent<PlayFabStore>();
        inventory = GameObject.Find("PlayFabInventory").GetComponent<PlayFabInventory>();
        connect = GameObject.Find("PlayFabManager").GetComponent<PlayFabWaitConnect>();
        AvatarData = GameObject.Find("PlayFabEclothesData").GetComponent<PlayFabPlayerData>();
        
        shop = this.transform.root.GetComponent<ShopCanvasController>();
        playermoney = this.transform.root.transform.Find("Player_Money/Money_Buck/Money_Text").GetComponent<Money_Text>();
        clothing = this.transform.root.transform.Find("Clothing_Parent/Clothing").GetComponent<Clothing>();

        IsPush = false;
        IsAction = false;
        IsUpdate = false;

        State_Button = STATE.UPDATE;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State_Button)
        {
            case STATE.RECEPTION: Reception(); break;
            case STATE.PUSH: Push(); break;
            case STATE.BUYorWEAR: BuyorWear(); break;
            case STATE.UPDATE: Button_Update(); break;
        }
    }
    //===========================================================================================================
    //状態関連
    //受付
    private void Reception()
    {
        if(IsPush)
        {
            State_Button = STATE.PUSH;
            IsPush = false;
        }
    }
    //押された
    private void Push()
    {
        if(!connect.IsWait())
        {
            State_Button = STATE.BUYorWEAR;
        }
        else
        {
            State_Button = STATE.RECEPTION;
        }
    }
    //購入または着る
    private void BuyorWear()
    {
        if (IsAction == false)
        {
            if (!connect.IsWait())
            {
                if (!inventory.IsHaveItem(shop.GetItemInfo().storeItem.ItemId))
                {
                    store.BuyItem(shop.GetItemInfo().storeItem.ItemId, "HA");
                    Debug.Log(shop.GetItemInfo().storeItem.ItemId + "を購入しました");
                }
                else
                {
                    Debug.Log(shop.GetItemInfo().storeItem.ItemId + "は購入済みです");
                    AvatarData.SetPlayerData(shop.GetItemInfo().catalogItem.ItemId);
                    Debug.Log(shop.GetItemInfo().catalogItem.ItemId + "を着用しました");
                }
                playermoney.RequestMoney();
                IsAction = true;
            }
        }
        else
        {
            if (!connect.IsWait())
            {
                State_Button = STATE.UPDATE;
                IsAction = false;
            }
        }
    }
    //更新
    private void Button_Update()
    {
        if(!IsUpdate)
        {
            if (!connect.IsWait())
            {
                inventory.RequestUpdate();
                IsUpdate = true;
            }
        }
        else
        {
            if (!connect.IsWait())
            {
                State_Button = STATE.RECEPTION;
                IsUpdate = false;
                clothing.CheckHavingCloting();
            }
        }
    }

    //===========================================================================================================
    //===========================================================================================================
    //ボタン選択
    public void Push_Button()
    {
        if (State_Button ==  STATE.RECEPTION && clothing.GetState() == Clothing.SHELFSTATE.PREVIEW)
        {
            IsPush = true;
        }
    }

    //===========================================================================================================
    //===========================================================================================================
    //所持アイテムの確認

    //===========================================================================================================
    //===========================================================================================================
    //

    //===========================================================================================================

}
