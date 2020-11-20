using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShelfInfo : MonoBehaviour , IPointerClickHandler
{

    [SerializeField] public StoreItem ItemImfo; //ストアのアイテム情報
    [SerializeField] ShopCanvasController Shop; //ショップキャンバスコントロールクラス

    // Start is called before the first frame update
    void Start()
    {
        Shop = this.transform.root.GetComponent<ShopCanvasController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //===========================================================================================================
    //情報の設定
    public void SetItemInfo(StoreItem item)
    {
        ItemImfo = item;
    }
    //===========================================================================================================
    //選択
    //Uiオブジェクトを選択
    public void OnPointerClick(PointerEventData pointerData)
    {
        Debug.Log(ItemImfo.ItemId + " がクリックされた!");
        Shop.SetSelectItem(ItemImfo);

        this.transform.root.Find("Preview_Costume/Costume_Parent/Costume_Description")
            .GetComponent<Costume_Description>().SetDescription(Shop.GetItemInfo().catalogItem.Description);
        this.transform.root.Find("Item_Price/Price_Text")
            .GetComponent<Price_Text>().SetPrice(Shop.GetItemInfo().storeItem.VirtualCurrencyPrices["HA"]);
    }
    //===========================================================================================================
    // 
    //===========================================================================================================
    //
    //===========================================================================================================

}
