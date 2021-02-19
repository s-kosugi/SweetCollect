using UnityEngine;
using UnityEngine.UI;

public class BuyAndWearButton : MonoBehaviour
{
    [SerializeField]ClothingBuyAndWear clothingbuyandwear = null;           //服の着用購入
    private Button button;                                                  //ボタン

    [SerializeField] private PlayFabStore playfabstore = null;              //ストア
    [SerializeField] private PlayFabInventory inventory = null;             //インベントリ
    [SerializeField] private PlayFabWaitConnect connect = null;             //通信
    [SerializeField] private PlayFabStore storeachivement = null;           //達成ストア
    [SerializeField] private ReachAchievement reachachievement = null;      //実績達成管理
    [SerializeField] private PreviewParent previewparent = null;            //表示衣服の親

    [SerializeField] private SelectClothing selectclothing = null;          //選択衣服
    [SerializeField] private Money_Text playermoney = null;                 //プレイヤー所持金
    [SerializeField] private Clothing clothing = null;                      //衣服
    [SerializeField] private CurtainAnime curtainanime = default;           //カーテン

    // Start is called before the first frame update
    void Start()
    {
        button = this.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        EnableButton();
    }

    //===========================================================================================================
    //ボタン
    public void PushButton()
    {
        // 受付状態且つ表示状態ならボタンを押せる
        if (clothingbuyandwear.GetState() == ClothingBuyAndWear.STATE.RECEPTION
            && clothing.GetState() == Clothing.SHELFSTATE.PREVIEW)
        {
            clothingbuyandwear.PushButton();
        }
    }
    //ボタンの有効化
    private void EnableButton()
    {
        // 通信中または下記状態ならボタン選択不可
        if (connect.IsWait() ||
            !playfabstore.m_isStoreGet ||
            clothing.GetState() != Clothing.SHELFSTATE.PREVIEW ||
            clothingbuyandwear.GetState() != ClothingBuyAndWear.STATE.RECEPTION
            || curtainanime.state != CurtainAnime.STATE.WAIT
            || previewparent.State != PreviewParent.STATE.WAIT
            )
        {
            button.enabled = false;
            return;
        }

        // (アイテムを持っているorお金が足りている)ならボタンを有効化
        if ((selectclothing.GetItemInfo().storeItem.VirtualCurrencyPrices[clothingbuyandwear.GetPriceName()] <= playermoney.GetPossessionMoney() ||
            inventory.IsHaveItem(selectclothing.GetItemInfo().catalogItem.ItemId)))
            button.enabled = true;
        else
            button.enabled = false;


        //取得完了
        if (!connect.IsWait() && playfabstore.m_isCatalogGet 
            && storeachivement.m_isStoreGet && playfabstore.m_isStoreGet)
        {
            if (selectclothing.GetItemInfo().catalogItem.CustomData != null)
            {
                //実績達成アイテムを所持しているか
                var achievementItem = storeachivement.StoreItems.Find(x => x.ItemId == selectclothing.GetItemInfo().catalogItem.CustomData.ToString());
                //条件達成服を持っておらず、実績を達成していなければ、ヒントを表示
                if (!inventory.IsHaveItem(selectclothing.GetItemInfo().catalogItem.ItemId) 
                    && !reachachievement.IsReachAchievement(achievementItem.ItemId.ToString()))
                {
                    button.enabled = true;
                }
                else
                {
                    button.enabled = true;
                }
            }
        }
        else
        {
            button.enabled = false;
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
