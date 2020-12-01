using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton : MonoBehaviour
{
    [SerializeField] private PlayFabStore store = null;
    [SerializeField] private PlayFabInventory inventory = null;
    [SerializeField] private PlayFabWaitConnect connect = null;
    [SerializeField] private PlayFabPlayerData AvatarData = null;

    [SerializeField] private Money_Text playermoney = null;

    ShopCanvasController shop = null;
    [SerializeField] bool IsCheck_Having;     //確認フラグ
    [SerializeField] bool IsSelect;    //選択フラグ
    [SerializeField] bool IsHaving;    //所持フラグ
    // Start is called before the first frame update
    void Start()
    {
        store = GameObject.Find("PlayFabStore").GetComponent<PlayFabStore>();
        inventory = GameObject.Find("PlayFabInventory").GetComponent<PlayFabInventory>();
        connect = GameObject.Find("PlayFabManager").GetComponent<PlayFabWaitConnect>();
        AvatarData = GameObject.Find("PlayFabEclothesData").GetComponent<PlayFabPlayerData>();

        shop = this.transform.root.GetComponent<ShopCanvasController>();
        playermoney = this.transform.root.transform.Find("Player_Money/Money_Text").GetComponent<Money_Text>();

        IsCheck_Having = false;
        IsSelect = false;
        IsHaving = false;
    }

    // Update is called once per frame
    void Update()
    {
        HavingItem();
        RequestInventory();
    }

    public void Push_Button()
    {
        if (shop.GetItemInfo().storeItem.ItemId == "") return;

        IsSelect = true;

        inventory.RequestUpdate();
    }

    //インベントリの中身を参照
    private void RequestInventory()
    {
        if (IsSelect && !IsCheck_Having)
        {
            if (!connect.IsWait())
            {
                IsSelect = false;

                if (!inventory.IsHaveItem(shop.GetItemInfo().storeItem.ItemId))
                {
                    store.BuyItem(shop.GetItemInfo().storeItem.ItemId, "HA");
                    Debug.Log(shop.GetItemInfo().storeItem.ItemId + "を購入しました");
                }
                else
                {
                    Debug.Log(shop.GetItemInfo().storeItem.ItemId + "は購入済みです");
                }

                playermoney.RequestMoney();
            }
        }
    }
    //選択アイテムのチェック
    public void CheckHaving()
    {
        inventory.RequestUpdate();
        IsCheck_Having = true;
    }
    //選択されている服を持っているか
    private void HavingItem()
    {
        if(IsCheck_Having)
        {
            if (!connect.IsWait())
            {
                IsCheck_Having = false;
                if (inventory.IsHaveItem(shop.GetItemInfo().storeItem.ItemId))
                {
                    Debug.Log(shop.GetItemInfo().storeItem.ItemId + "を所持");
                    IsHaving = true;
                }
                else
                {
                    Debug.Log(shop.GetItemInfo().storeItem.ItemId + "を未所持");
                    IsHaving = false;
                }
            }
        }
    }

    //所持フラグの取得
    public bool GetHavingFlag()
    {
        return IsHaving;
    }

}
