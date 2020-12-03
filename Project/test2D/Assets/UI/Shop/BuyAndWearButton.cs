using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    [SerializeField] private bool IsConnect;    //通信中
    [SerializeField] private bool IsPush;       //ボタンを押したかどうか
    [SerializeField] private bool IsAction;     //行動できるかどうか
    [SerializeField] private bool IsSelect;     //選択中(購入または着用)
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
        button = this.GetComponent<Button>();

        IsConnect = false;
        IsPush = false;
        IsAction = false;
        IsSelect = false;
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

        EnableButton();

    }
    //===========================================================================================================
    //状態関連
    //受付
    private void Reception()
    {
        if(IsPush && !IsAction && !IsConnect)
        {
            State_Button = STATE.PUSH;
            IsPush = false;
            IsAction = true;
        }
    }
    //押された
    private void Push()
    {
        if (IsAction)
        {
            if (!connect.IsWait())
            {
                State_Button = STATE.BUYorWEAR;
                IsAction = false;
            }
            else
            {
                State_Button = STATE.RECEPTION;
                IsAction = false;
            }
        }
    }
    //購入または着る
    private void BuyorWear()
    {
        if (IsSelect == false)
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
                IsSelect = true;
            }
        }
        else
        {
            if (!connect.IsWait())
            {
                State_Button = STATE.UPDATE;
                IsSelect = false;
            }
        }
    }
    //更新
    private void Button_Update()
    {
        if (!IsUpdate)
        {
            if (!connect.IsWait())
            {
                inventory.RequestUpdate();
                IsUpdate = true;
            }
        }
        else
        {
            if (IsConnect)
            {
                if (!connect.IsWait())
                {
                    State_Button = STATE.RECEPTION;
                    IsUpdate = false;
                    IsConnect = false;
                    clothing.CheckHavingCloting();
                }
            }
            else
            {
                if (connect.IsWait())
                {
                    IsConnect = true;
                }
            }
        }
        
    }

    //===========================================================================================================
    //===========================================================================================================
    //ボタン
    public void Push_Button()
    {
        if (State_Button ==  STATE.RECEPTION && clothing.GetState() == Clothing.SHELFSTATE.PREVIEW)
        {
            IsPush = true;
        }
    }
    //ボタンの有効化
    private void EnableButton()
    {
        if (State_Button == STATE.RECEPTION)
        {
            button.enabled = true;
        }
        else
        {
            button.enabled = false;
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
