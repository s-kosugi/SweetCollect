using PlayFab.ClientModels;
using UnityEngine;

public class SelectClothing : MonoBehaviour
{
    [SerializeField] PlayFabStore playfabstore;                     //プレイハブのストア系統の処理

    [System.Serializable]
    public struct SelectItem_Info
    {
        public StoreItem storeItem;                                //ストアアイテム
        public CatalogItem catalogItem;                            //カタログアイテム
    }
    [SerializeField] public SelectItem_Info SelectItem = default;  //選択アイテム

    // Start is called before the first frame update
    void Start()
    {
        playfabstore = GameObject.Find("PlayFabStore").GetComponent<PlayFabStore>();
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
    //info : 確認したストアアイテムの情報
    public void SetSelectItem(StoreItem info)
    {
        //SelectItem.StoreItem
        SelectItem.storeItem = info;
        
        SelectItem.catalogItem = playfabstore.CatalogItems.Find(x => x.ItemId == SelectItem.storeItem.ItemId);
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
