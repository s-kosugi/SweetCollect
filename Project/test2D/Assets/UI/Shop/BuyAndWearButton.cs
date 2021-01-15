using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class BuyAndWearButton : MonoBehaviour
{
    private PlayFabStore store = null;            //ストア
    private PlayFabInventory inventory = null;    //インベントリ
    private PlayFabWaitConnect connect = null;    //通信
    private PlayFabPlayerData playerData = null;  //プレイヤーデータ

    private ShopCanvasController shop = null;
    [SerializeField] private Money_Text playermoney = null;
     private Clothing clothing = null;

    private Button button;     //ボタン
    private bool IsConnect;    //通信中
    private bool IsPush;       //ボタンを押したかどうか
    private bool IsAction;     //行動できるかどうか
    private bool IsSelect;     //選択中(購入または着用)
    private bool IsUpdate;     //更新中

    [SerializeField] private PlayerAvatar playerAvatar = default;
    [SerializeField] private CurtainAnime curtainAnime = default;
    [SerializeField] private string PriceName = default;
    [SerializeField] ShopAchievement achievement = default;

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
        playerData = GameObject.Find("PlayFabPlayerData").GetComponent<PlayFabPlayerData>();
        
        shop = this.transform.root.GetComponent<ShopCanvasController>();
        // ハードコーディングで可変に対応できない為コメントアウト
        //playermoney = this.transform.root.transform.Find("Player_Money/Money_Buck/Money_Text").GetComponent<Money_Text>();
        clothing = GameObject.Find("Clothing_Parent/Clothing").GetComponent<Clothing>();
        button = this.GetComponent<Button>();

        IsConnect = false;
        IsPush = false;
        IsAction = false;
        IsSelect = false;
        IsUpdate = false;

        PriceName = "HA";

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
        //押された際にアクション・通信中でなければ押された状態へ
        if(IsPush && !IsAction && !IsConnect)
        {
            if (inventory.IsHaveItem(shop.GetItemInfo().storeItem.ItemId))
            {
                // 押されてボタンが有効化された時にカーテンを閉じるアニメーションに変更
                curtainAnime.ChangeClose();
            }
            State_Button = STATE.PUSH;
            IsPush = false;
            IsAction = true;
        }
    }
    //押された
    private void Push()
    {
        //アクション中
        if (IsAction)
        {
            //通信中でなければ購入・着用処理へ
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
                //インベントリ内にアイテムがなければ購入
                if (!inventory.IsHaveItem(shop.GetItemInfo().storeItem.ItemId))
                {
                    store.BuyItem(shop.GetItemInfo().storeItem.ItemId, PriceName);
                    Debug.Log(shop.GetItemInfo().storeItem.ItemId + "を購入しました");
                }
                //持っていれば着用
                else
                {
                    playerData.SetPlayerData(PlayerDataName.ECLOTHES, shop.GetItemInfo().catalogItem.ItemId);
                    // ユーザーデータの更新
                    playerData.RequestGetUserData();
                    Debug.Log(shop.GetItemInfo().catalogItem.ItemId + "を着用しました");

                    // プレイヤーの見た目更新
                    playerAvatar.UpdateAvatar();

                    // カーテンを開くアニメーション
                    curtainAnime.ChangeOpen();
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
            //通信中でなければリクエストにする
            if (!connect.IsWait())
            {
                inventory.RequestUpdate();
                // 服を未カウント状態へ戻す（購入で服の所持数が変わる）
                achievement.isHaveClothesCount = false;
                IsUpdate = true;
            }
        }
        else
        {
            if (IsConnect)
            {
                //通信中になるまで待機
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
        // 受付状態且つ表示状態ならボタンを押せる
        if (State_Button ==  STATE.RECEPTION && clothing.GetState() == Clothing.SHELFSTATE.PREVIEW)
        {
            IsPush = true;
        }
    }
    //ボタンの有効化
    private void EnableButton()
    {
        // 通信中または下記状態ならボタン選択不可
        if (connect.IsWait() ||
            !store.m_isStoreGet ||
            clothing.GetState() != Clothing.SHELFSTATE.PREVIEW ||
            State_Button != STATE.RECEPTION
            )
        {
            button.enabled = false;
            return;
        }

        // (アイテムを持っているorお金が足りている)ならボタンを有効化
        if ((shop.GetItemInfo().storeItem.VirtualCurrencyPrices[PriceName] <= playermoney.GetPossessionMoney() ||
            inventory.IsHaveItem(shop.GetItemInfo().catalogItem.ItemId)))
        {
            button.enabled = true;
        }
        // アイテムを持っておらず、条件付きの場合はボタンを無効化
        var catalogItem = store.CatalogItems.Find(x => x.ItemId == shop.GetItemInfo().catalogItem.ItemId);
        if (!inventory.IsHaveItem(shop.GetItemInfo().catalogItem.ItemId) && catalogItem.CustomData != null)
            button.enabled = false;
    }

}
