using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButton : MonoBehaviour
{
    [SerializeField] PlayFabStore store = null;
    [SerializeField] PlayFabInventory inventory = null;
    [SerializeField] PlayFabWaitConnect connect = null; 
    ShopCanvasController shop = null;

    [SerializeField] bool IsBuy;                //購入フラグ
    // Start is called before the first frame update
    void Start()
    {
        store = GameObject.Find("PlayFabStore").GetComponent<PlayFabStore>();
        inventory = GameObject.Find("PlayFabInventory").GetComponent<PlayFabInventory>();
        connect = GameObject.Find("PlayFabManager").GetComponent<PlayFabWaitConnect>();
        shop = this.transform.root.GetComponent<ShopCanvasController>();

        IsBuy = false;
    }

    // Update is called once per frame
    void Update()
    {
        RequestInventory();
    }

    public void Push_Button()
    {
        if (shop.GetItemInfo().storeItem.ItemId == "") return;

        IsBuy = true;

        inventory.RequestUpdate();    
    }

    //インベントリの中身を参照
    private void RequestInventory()
    {
        if(IsBuy)
        {
            if(!connect.IsWait())
            {
                IsBuy = false;

                if(!inventory.IsHaveItem(shop.GetItemInfo().storeItem.ItemId))
                {
                    store.BuyItem(shop.GetItemInfo().storeItem.ItemId, "HA");
                    Debug.Log(shop.GetItemInfo().storeItem.ItemId + "を購入しました");
                }
                else
                {
                    Debug.Log(shop.GetItemInfo().storeItem.ItemId + "は所持しています");
                }
            }
        }

    }
}
