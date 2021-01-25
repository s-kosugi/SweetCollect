using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementRewardRelease : MonoBehaviour
{
    [SerializeField] private PlayFabWaitConnect connect = null;                               //通信
    [SerializeField] private PlayFabInventory inventory = null;                               //インベントリ
    [SerializeField] private ReachAchievement reachachievement = null;                        //実績達成管理
    [SerializeField] private PlayFabStore PlayFabStore = null;                                //服ストア
    [SerializeField] private PlayFabStore PlayFabStoreAchivement = null;                      //実績ストア
    [SerializeField] private ShopSceneManager shop = null;                                    //ショップマネージャー
    [SerializeField] private Clothing clothing = null;                                        //洋服
    [SerializeField] private BuyAndWearButton buyandwearbutton = null;                        //購入または着用
    [SerializeField] private ClotingNameText clotingNameText = null;                          //衣服名表示テキスト

    public bool AchievementFlag { get; private set; }        //実績達成
    public string AchievementClotingName { get; private set; }      //実績達成のアイテムID
    private string ClotingName = "??";                              //実績達成の服の名前

    public bool ClotingMoveEndFlag { get; private set; }                       //服の移動終了
    public bool BuyEndFlag { get; private set; }                               //購入終了
    public bool PreviewFlag  { get; private set; }                               //表示終了

    private float PreviewTimer = 0.0f;                                           //テロップ表示までの時間
    private float PREVIEW_TIME = 1.0f;                                           //テロップ表示までの時間

    public enum ACHIEVEMENTREWARDRELEASE
    {
        NONE = -1,     //
        CHECK_CLOTHING_RELEASE = 0,     //服開放
        CLOTHING_MOVE, //服の移動
        CLOTHING_BUY,  //衣服の購入
        UPDATA,        //購入情報を更新
        PREVIEW,       //表示
        SEARCH,         //検索
        WAIT,          //実行待ち
    }
    [SerializeField] ACHIEVEMENTREWARDRELEASE EventState = ACHIEVEMENTREWARDRELEASE.NONE; //実勢達積イベント状態

    // Start is called before the first frame update
    void Start()
    {
        EventState = ACHIEVEMENTREWARDRELEASE.WAIT;

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
        switch (EventState)
        {
            case ACHIEVEMENTREWARDRELEASE.CHECK_CLOTHING_RELEASE: Check(); break;
            case ACHIEVEMENTREWARDRELEASE.CLOTHING_MOVE: Clothing_Move(); break;
            case ACHIEVEMENTREWARDRELEASE.CLOTHING_BUY: Clothig_Buy(); break;
            case ACHIEVEMENTREWARDRELEASE.UPDATA: Data_Update(); break;
            case ACHIEVEMENTREWARDRELEASE.PREVIEW: Preview(); break;
            case ACHIEVEMENTREWARDRELEASE.SEARCH: Search(); break;
            case ACHIEVEMENTREWARDRELEASE.WAIT: Wait(); break;
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
                //二つの状態が実績になっているならイベントを実行
                if(clothing.GetState() == Clothing.SHELFSTATE.ACHIEVEMENTREWARDRELEASE 
                    && buyandwearbutton.GetState() == BuyAndWearButton.STATE.ACHIEVEMENTREWARDRELEASE)
                    EventState = ACHIEVEMENTREWARDRELEASE.CLOTHING_MOVE;
            }
        }

    }
    //服の移動
    private void Clothing_Move()
    {
        if(ClotingMoveEndFlag)
            EventState = ACHIEVEMENTREWARDRELEASE.CLOTHING_BUY;
    }
    //服の購入
    private void Clothig_Buy()
    {
        if(ClotingMoveEndFlag)
        {
            EventState = ACHIEVEMENTREWARDRELEASE.UPDATA;
            ClotingMoveEndFlag = false;
            BuyEndFlag = false;
            inventory.RequestUpdate();
        }

    }
    //更新
    private void Data_Update()
    {
        if (!connect.IsWait())
        {
            //一定時間経過でテロップの表示
            PreviewTimer += Time.deltaTime;
            if(PreviewTimer > PREVIEW_TIME)
            {
                PreviewTimer = 0.0f;
                clotingNameText.GetClotingName(ClotingName);
                EventState = ACHIEVEMENTREWARDRELEASE.PREVIEW;
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
        EventState = ACHIEVEMENTREWARDRELEASE.SEARCH;
    }

    //実績達成と服の所持
    private void CheckAchievement()
    {
        if(!connect.IsWait())
        {
            //取得完了
            if(PlayFabStore.m_isCatalogGet && PlayFabStoreAchivement.m_isStoreGet && PlayFabStore.m_isStoreGet)
            {
                foreach (var value in PlayFabStore.CatalogItems)
                {
                    var StoreItem = PlayFabStore.StoreItems.Find(x => x.ItemId == value.ItemId);
                    //ストアアイテム内になく、Dummyでなければ
                    //検索処理を終了する
                    if(StoreItem == null && value.ItemId != "-1")
                    {
                        EventState = ACHIEVEMENTREWARDRELEASE.WAIT;
                        break;
                    }

                    if (value.CustomData != null)
                    {
                        //実績達成アイテムを所持しているか
                        var achievementItem = PlayFabStoreAchivement.StoreItems.Find(x => x.ItemId == value.CustomData.ToString());
                        //実績達成
                        if(reachachievement.IsReachAchievement(achievementItem.ItemId.ToString()))
                        {
                            //実績により解放された服を所持していなければ服の解放
                            if (!inventory.IsHaveItem(StoreItem.ItemId))
                            {
                                AchievementClotingName = value.ItemId;
                                ClotingName = value.DisplayName;
                                AchievementFlag = true;
                                EventState = ACHIEVEMENTREWARDRELEASE.CHECK_CLOTHING_RELEASE;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    //取得
    public ACHIEVEMENTREWARDRELEASE GetState()
    {
        return EventState;
    }

    //ボタン処理
    public void FinishPreview()
    {
        PreviewFlag = true;
    }
}
