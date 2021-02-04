using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class BuyAndWearButton : MonoBehaviour
{
    [SerializeField] private PlayFabStore store = null;            //ストア
    [SerializeField] private PlayFabInventory inventory = null;    //インベントリ
    [SerializeField] private PlayFabWaitConnect connect = null;    //通信
    [SerializeField] private PlayFabPlayerData playerData = null;  //プレイヤーデータ
    [SerializeField] private PlayFabStore PlayFabStoreAchivement = null; //達成ストア
    [SerializeField] private ReachAchievement reachachievement = null;         //実績達成管理
    [SerializeField]private AchievementRewardRelease RewardRelease = null; //達成服解放

    private ShopCanvasController ShopCanvas = null;
    [SerializeField] private Money_Text playermoney = null;
    private Clothing clothing = null;
    [SerializeField] private AchievementName achievementtext = null;

    private Button button;     //ボタン
    private bool IsPush;       //ボタンを押したかどうか
    private bool IsAction;     //行動できるかどうか
    private bool IsSelect;     //選択中(購入または着用)
    private bool IsUpdate;     //更新中
    [SerializeField] private bool IsPreviewHint; //ヒント表示
    private bool IsAchievementsClothingRelease; //実績服の解放

    [SerializeField] private PlayerAvatar playerAvatar = default;
    [SerializeField] private CurtainAnime curtainAnime = default;
    [SerializeField] private string PriceName = default;
    [SerializeField] ShopAchievement achievement = default;

    //状態分け
    public enum STATE
    {
        NONE = -1,

        RECEPTION,        //受付
        PUSH,             //押された
        BUYorWEAR,        //購入または着る
        UPDATE,           //更新
        ACHIEVEMENTREWARDRELEASE,      //実績報酬解放
        PREVIEWHINT,          //表示
    }
    [SerializeField] private STATE State = STATE.NONE;      //ボタンの状態


    // Start is called before the first frame update
    void Start()
    {
        store = GameObject.Find("PlayFabStore").GetComponent<PlayFabStore>();
        inventory = GameObject.Find("PlayFabInventory").GetComponent<PlayFabInventory>();
        connect = GameObject.Find("PlayFabManager").GetComponent<PlayFabWaitConnect>();
        playerData = GameObject.Find("PlayFabPlayerData").GetComponent<PlayFabPlayerData>();
        PlayFabStoreAchivement = GameObject.Find("PlayFabStoreAchivement").GetComponent<PlayFabStore>();

        ShopCanvas = this.transform.root.GetComponent<ShopCanvasController>();
        // ハードコーディングで可変に対応できない為コメントアウト
        //playermoney = this.transform.root.transform.Find("Player_Money/Money_Buck/Money_Text").GetComponent<Money_Text>();
        clothing = GameObject.Find("Clothing_Parent/Clothing").GetComponent<Clothing>();
        button = this.GetComponent<Button>();

        IsPush = false;
        IsAction = false;
        IsSelect = false;
        IsUpdate = false;
        IsPreviewHint = false;
        IsAchievementsClothingRelease = false;

        PriceName = "HA";

        State = STATE.UPDATE;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case STATE.RECEPTION: Reception(); break;
            case STATE.PUSH: Push(); break;
            case STATE.BUYorWEAR: BuyorWear(); break;
            case STATE.UPDATE: Button_Update(); break;
            case STATE.ACHIEVEMENTREWARDRELEASE: AchievementRewardRelease(); break;
            case STATE.PREVIEWHINT: PreviewHint(); break;
        }

        EnableButton();

    }
    //===========================================================================================================
    //状態関連
    //受付
    private void Reception()
    {
        //押された際にアクション・通信中でなければ押された状態へ
        if (IsPush && !IsAction)
        {
            if (inventory.IsHaveItem(ShopCanvas.GetItemInfo().storeItem.ItemId))
            {
                // 押されてボタンが有効化された時にカーテンを閉じるアニメーションに変更
                curtainAnime.ChangeClose();
            }
            State = STATE.PUSH;
            IsPush = false;
            IsAction = true;
        }

        //服開放実行
        if (RewardRelease.AchievementFlag && RewardRelease.GetState() == global::AchievementRewardRelease.ACHIEVEMENTREWARDRELEASE.CHECK_CLOTHING_RELEASE)
        {
            State = STATE.ACHIEVEMENTREWARDRELEASE;
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
                if (!IsPreviewHint)
                {
                    State = STATE.BUYorWEAR;
                    IsAction = false;
                }
                else
                {
                    State = STATE.PREVIEWHINT;
                    IsAction = false;

                    // アイテムがカタログ内にあるのかを探し、それに対応する説明を設定
                    var catalogItem = store.CatalogItems.Find(x => x.ItemId == ShopCanvas.GetItemInfo().catalogItem.ItemId);
                    if (catalogItem.CustomData != null)
                    {
                        //データがあれば、そのデータを表示
                        var achievementItem = PlayFabStoreAchivement.CatalogItems.Find(x => x.ItemId == catalogItem.CustomData);
                        if (achievementItem != null)
                            achievementtext.GetAchievementName(achievementItem.Description);
                        else
                            achievementtext.GetAchievementName("設定されていません");
                    }
                }
            }
            else
            {
                State = STATE.RECEPTION;
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
                if (!inventory.IsHaveItem(ShopCanvas.GetItemInfo().storeItem.ItemId))
                {
                    store.BuyItem(ShopCanvas.GetItemInfo().storeItem.ItemId, PriceName);
                    Debug.Log(ShopCanvas.GetItemInfo().storeItem.ItemId + "を購入しました");
                }
                //持っていれば着用
                else
                {
                    playerData.SetPlayerData(PlayerDataName.ECLOTHES, ShopCanvas.GetItemInfo().catalogItem.ItemId);
                    // ユーザーデータの更新
                    playerData.RequestGetUserData();
                    Debug.Log(ShopCanvas.GetItemInfo().catalogItem.ItemId + "を着用しました");

                    // プレイヤーの見た目更新
                    playerAvatar.UpdateAvatar(ShopCanvas.GetItemInfo().catalogItem.ItemId);

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
                State = STATE.UPDATE;
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
            //通信中でなければ待機へ
            if (!connect.IsWait())
            {
                State = STATE.RECEPTION;
                IsUpdate = false;
                clothing.CheckHavingCloting();
            }
        }

    }

    //実績達成
    private void AchievementRewardRelease()
    {
        if (RewardRelease.GetState() == global::AchievementRewardRelease.ACHIEVEMENTREWARDRELEASE.CLOTHING_BUY)
        {
            if (!connect.IsWait())
            {
                if (!RewardRelease.BuyEndFlag)
                {
                    if (!IsAchievementsClothingRelease)
                    {

                        //インベントリ内にアイテムがなければ購入
                        if (!inventory.IsHaveItem(ShopCanvas.GetItemInfo().storeItem.ItemId))
                        {
                            store.BuyItem(ShopCanvas.GetItemInfo().storeItem.ItemId, PriceName);
                            Debug.Log(ShopCanvas.GetItemInfo().storeItem.ItemId + "を購入しました");
                        }
                        playermoney.RequestMoney();
                        IsAchievementsClothingRelease = true;
                        clothing.BuyButtonPush();
                    }
                    else
                    {
                        if (!connect.IsWait())
                            RewardRelease.FinishBuyEvent();
                    }
                }

            }
        }

        if (!RewardRelease.AchievementFlag)
        {
            State = STATE.RECEPTION;
            IsAchievementsClothingRelease = false;
        }
    }
    //ヒント表示
    public void PreviewHint()
    {

    }

    //===========================================================================================================
    //===========================================================================================================
    //ボタン
    public void Push_Button()
    {
        // 受付状態且つ表示状態ならボタンを押せる
        if (State == STATE.RECEPTION && clothing.GetState() == Clothing.SHELFSTATE.PREVIEW)
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
            State != STATE.RECEPTION
            || curtainAnime.state != CurtainAnime.STATE.WAIT
            )
        {
            button.enabled = false;
            return;
        }

        // (アイテムを持っているorお金が足りている)ならボタンを有効化
        if ((ShopCanvas.GetItemInfo().storeItem.VirtualCurrencyPrices[PriceName] <= playermoney.GetPossessionMoney() ||
            inventory.IsHaveItem(ShopCanvas.GetItemInfo().catalogItem.ItemId)))
            button.enabled = true;
        else
            button.enabled = false;


        //取得完了
        if (!connect.IsWait() && store.m_isCatalogGet && PlayFabStoreAchivement.m_isStoreGet && store.m_isStoreGet)
        {
            if(ShopCanvas.GetItemInfo().catalogItem.CustomData != null)
            {
                //実績達成アイテムを所持しているか
                var achievementItem = PlayFabStoreAchivement.StoreItems.Find(x => x.ItemId == ShopCanvas.GetItemInfo().catalogItem.CustomData.ToString());
                //条件達成服を持っておらず、実績を達成していなければ、ヒントを表示
                if (!inventory.IsHaveItem(ShopCanvas.GetItemInfo().catalogItem.ItemId) && !reachachievement.IsReachAchievement(achievementItem.ItemId.ToString()))
                {
                    button.enabled = true;
                    IsPreviewHint = true;
                }
                else
                {
                    button.enabled = true;
                    IsPreviewHint = false;
                }
            }
            else
                IsPreviewHint = false;
        }
        else
        {
            button.enabled = false;
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //取得
    public STATE GetState()
    {
        return State;
    }

    //表示関連
    public void PreviewEnd()
    {
        State = STATE.RECEPTION;
    }
}
