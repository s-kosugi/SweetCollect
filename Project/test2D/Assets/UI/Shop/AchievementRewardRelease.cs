using UnityEngine;

public class AchievementRewardRelease : MonoBehaviour
{
    [SerializeField] private PlayFabWaitConnect connect = null;                               //通信
    [SerializeField] private PlayFabInventory inventory = null;                               //インベントリ
    [SerializeField] private ReachAchievement reachachievement = null;                        //実績達成管理
    [SerializeField] private PlayFabStore playfabstore = null;                                //服ストア
    [SerializeField] private PlayFabStore storeachivement = null;                             //実績ストア
    [SerializeField] private ShopSceneManager shop = null;                                    //ショップマネージャー
    [SerializeField] private Clothing clothing = null;                                        //洋服
    [SerializeField] private ClothingBuyAndWear buyandwearbutton = null;                       //購入または着用
    [SerializeField] private ClotingNameText clotingNameText = null;                          //衣服名表示テキスト

    public bool AchievementFlag { get; private set; }                                       　 //実績達成
    public string AchievementClotingName { get; private set; }      //実績達成のアイテムID
    private string ClotingName = "??";                              //実績達成の服の名前

    public bool ClotingMoveEndFlag { get; private set; }                         //服の移動終了
    public bool BuyEndFlag { get; private set; }                                 //購入終了
    public bool PreviewFlag  { get; private set; }                               //表示終了

    private float PreviewTimer = 0.0f;                                           //テロップ表示までの時間
    private float PREVIEW_TIME = 1.0f;                                           //テロップ表示までの時間

    public enum REWARDRELEASE
    {
        NONE = -1,     //
        CHECK = 0,                      //確認
        CLOTHING_MOVE,                  //服の移動
        CLOTHING_BUY,                   //衣服の購入
        UPDATA,                         //購入情報を更新
        PREVIEW,                        //表示
        SEARCH,                         //検索
        WAIT,                           //待ち
    }
    [SerializeField] REWARDRELEASE State = REWARDRELEASE.NONE; //実勢達積イベント状態

    // Start is called before the first frame update
    void Start()
    {
        State = REWARDRELEASE.WAIT;

        AchievementFlag = false;
        AchievementClotingName = "009_GOTHIC";

        ClotingMoveEndFlag = false;
        BuyEndFlag = false;
        PreviewFlag = false;
        PreviewTimer = 0.0f;

        IsCheck();
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case REWARDRELEASE.CHECK: Check(); break;
            case REWARDRELEASE.CLOTHING_MOVE: ClothingMove(); break;
            case REWARDRELEASE.CLOTHING_BUY: ClothigBuy(); break;
            case REWARDRELEASE.UPDATA: DataUpdate(); break;
            case REWARDRELEASE.PREVIEW: Preview(); break;
            case REWARDRELEASE.SEARCH: Search(); break;
            case REWARDRELEASE.WAIT: Wait(); break;
        }
    }

    //状態関連
    //確認
    private void Check()
    {
        if(shop.IsFadeEnd())
        {
            if(!connect.IsWait())
            {
                //二つの状態が服開放になっているならイベントを実行
                if(clothing.GetState() == Clothing.SHELFSTATE.REWARDRELEASE 
                    && buyandwearbutton.GetState() == ClothingBuyAndWear.STATE.REWARDRELEASE)
                    State = REWARDRELEASE.CLOTHING_MOVE;
            }
        }

    }
    //服の移動
    private void ClothingMove()
    {
        if(ClotingMoveEndFlag)
            State = REWARDRELEASE.CLOTHING_BUY;
    }
    //服の購入
    private void ClothigBuy()
    {
        if(BuyEndFlag)
        {
            State = REWARDRELEASE.UPDATA;
            ClotingMoveEndFlag = false;
            BuyEndFlag = false;
            inventory.RequestUpdate();
        }

    }
    //更新
    private void DataUpdate()
    {
        if (!connect.IsWait())
        {
            //一定時間経過でテロップの表示
            PreviewTimer += Time.deltaTime;
            if(PreviewTimer > PREVIEW_TIME)
            {
                PreviewTimer = 0.0f;
                clotingNameText.GetClotingName(ClotingName);
                State = REWARDRELEASE.PREVIEW;
            }
        }
    }
    //表示
    private void Preview()
    {
        if(PreviewFlag)
        {
            AchievementFlag = false;
            PreviewFlag = false;
            IsCheck();
        }
    }
    //検索
    private void Search()
    {
        CheckAchievement();
    }
    //実行待ち
    private void Wait()
    {
    }

    //実績達成の服までの移動
    public void FinishClotingMove()
    {
        ClotingMoveEndFlag = true;
    }
    //服の購入終了
    public void FinishBuyEvent()
    {
        BuyEndFlag = true;
    }
    //実績解除確認開始
    public void IsCheck()
    {
        State = REWARDRELEASE.SEARCH;
    }

    //実績達成と服の所持
    private void CheckAchievement()
    {
        if(!connect.IsWait())
        {
            //取得完了
            if(playfabstore.isCatalogGet && storeachivement.isStoreGet && playfabstore.isStoreGet)
            {
                foreach (var value in playfabstore.CatalogItems)
                {
                    var StoreItem = playfabstore.StoreItems.Find(x => x.ItemId == value.ItemId);
                    //ストアアイテム内になく、Dummyでなければ
                    //検索処理を終了する
                    if(StoreItem == null && value.ItemId != "-1")
                    {
                        State = REWARDRELEASE.WAIT;
                        break;
                    }

                    if (value.CustomData != null)
                    {
                        //実績達成アイテムを所持しているか
                        var achievementItem = storeachivement.StoreItems.Find(x => x.ItemId == value.CustomData.ToString());
                        //実績達成
                        if(reachachievement.IsReachAchievement(achievementItem.ItemId.ToString()))
                        {
                            //実績により解放された服を所持していなければ服の解放
                            if (!inventory.IsHaveItem(StoreItem.ItemId))
                            {
                                AchievementClotingName = value.ItemId;
                                ClotingName = value.DisplayName;
                                AchievementFlag = true;
                                State = REWARDRELEASE.CHECK;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    //取得
    public REWARDRELEASE GetState()
    {
        return State;
    }

    //ボタン処理
    public void FinishPreview()
    {
        PreviewFlag = true;
    }
}
