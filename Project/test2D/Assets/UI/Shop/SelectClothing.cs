using PlayFab.ClientModels;
using UnityEngine;

public class SelectClothing : MonoBehaviour
{
    //[SerializeField] StoreItem SelectItem;                          //選択しているアイテム
    [SerializeField] PlayFabStore PalyFabStore;                     //プレイハブのストア系統の処理

    [System.Serializable]
    public struct SelectItem_Info
    {
        public StoreItem storeItem;
        public CatalogItem catalogItem;
    }
    [SerializeField] public SelectItem_Info SelectItem = default;

    // Start is called before the first frame update
    void Start()
    {
        PalyFabStore = GameObject.Find("PlayFabStore").GetComponent<PlayFabStore>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    //===========================================================================================================
    //選択関連

    //===========================================================================================================

    //===========================================================================================================
    //設定(Setter)
    //現在選択しているUI
    public void SetSelectItem(StoreItem info)
    {
        //SelectItem.StoreItem
        SelectItem.storeItem = info;
        
        SelectItem.catalogItem = PalyFabStore.CatalogItems.Find(x => x.ItemId == SelectItem.storeItem.ItemId);
    }
    //===========================================================================================================

    //===========================================================================================================
    //取得(Getter)
    //現在選択しているアイテム情報
    public SelectItem_Info GetItemInfo()
    {
        return SelectItem;
    }


    //===========================================================================================================

}
