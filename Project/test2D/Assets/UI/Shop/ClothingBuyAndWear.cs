using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class ClothingBuyAndWear : MonoBehaviour
{
    [SerializeField] private PlayFabStore store = null;                     //ストア
    [SerializeField] private PlayFabInventory inventory = null;             //インベントリ
    [SerializeField] private PlayFabWaitConnect connect = null;             //通信
    [SerializeField] private PlayFabPlayerData playerData = null;           //プレイヤーデータ
    [SerializeField] private PlayFabStore storeachivement = null;           //達成ストア
    [SerializeField] private ReachAchievement reachachievement = null;      //実績達成管理
    [SerializeField]private AchievementRewardRelease rewardrelease = null;  //達成服解放
    [SerializeField]private PreviewParent previewparent = null;             //表示衣服の親

    [SerializeField] private SelectClothing selectclothing = null;          //選択されている服
    [SerializeField] private Money_Text playermoney = null;                 //プレイヤー所持金
    [SerializeField] private Clothing clothing = null;                      //服
    [SerializeField] private AchievementName achievementtext = null;        //実績名

    private bool IsPush;                                                    //ボタンを押したかどうか
    private bool IsAction;                                                  //行動できるかどうか
    private bool IsSelect;                                                  //選択中(購入または着用)
    private bool IsUpdate;                                                  //更新中
    [SerializeField] private bool IsPreviewHint;                            //ヒント表示
    private bool IsAchievementsClothingRelease;                             //実績服の解放

    [SerializeField] private PlayerAvatar playerAvatar = default;           //プレイヤーアバター
    [SerializeField] private CurtainAnime curtainAnime = default;           //カーテン
    [SerializeField] private string PriceName = default;                    //価格名
    [SerializeField] ShopAchievement achievement = default;                 //実績

    //サウンド
    [SerializeField] private string sefilename = "Tap";

    //状態分け
    public enum STATE
    {
        NONE = -1,

        RECEPTION,        //受付
        PUSH,             //押された
        BUYorWEAR,        //購入または着る
        UPDATE,           //更新
        REWARDRELEASE,    //服解放
        PREVIEWHINT,      //表示
    }
    [SerializeField] private STATE State = STATE.NONE;      //ボタンの状態


    // Start is called before the first frame update
    void Start()
    {
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
            case STATE.REWARDRELEASE: AchievementRewardRelease(); break;
            case STATE.PREVIEWHINT: ; break;
        }

        PreviewHint();
    }
    //===========================================================================================================
    //状態関連
    //受付
    private void Reception()
    {
        //押された際にアクション・通信中でなければ押された状態へ
        if (IsPush && !IsAction)
        {
            if (inventory.IsHaveItem(selectclothing.GetItemInfo().storeItem.ItemId))
            {
                // 押されてボタンが有効化された時にカーテンを閉じるアニメーションに変更
                curtainAnime.ChangeClose();
            }
            State = STATE.PUSH;
            IsPush = false;
            IsAction = true;
        }

        //服開放実行
        if (rewardrelease.AchievementFlag && rewardrelease.GetState() == global::AchievementRewardRelease.REWARDRELEASE.CHECK_CLOTHING_RELEASE)
        {
            State = STATE.REWARDRELEASE;
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
       
                    SoundManager.Instance.PlaySE(sefilename);

                    // アイテムがカタログ内にあるのかを探し、それに対応する説明を設定
                    var catalogItem = store.CatalogItems.Find(x => x.ItemId == selectclothing.GetItemInfo().catalogItem.ItemId);
                    if (catalogItem.CustomData != null)
                    {
                        //データがあれば、そのデータを表示
                        var achievementItem = storeachivement.CatalogItems.Find(x => x.ItemId == catalogItem.CustomData);
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
                if (!inventory.IsHaveItem(selectclothing.GetItemInfo().storeItem.ItemId))
                {
                    store.BuyItem(selectclothing.GetItemInfo().storeItem.ItemId, PriceName);
                    Debug.Log(selectclothing.GetItemInfo().storeItem.ItemId + "を購入しました");
                }
                //持っていれば着用
                else
                {
                    playerData.SetPlayerData(PlayerDataName.ECLOTHES, selectclothing.GetItemInfo().catalogItem.ItemId);
                    // ユーザーデータの更新
                    playerData.RequestGetUserData();
                    Debug.Log(selectclothing.GetItemInfo().catalogItem.ItemId + "を着用しました");

                    // プレイヤーの見た目更新
                    playerAvatar.UpdateAvatar(selectclothing.GetItemInfo().catalogItem.ItemId);

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
        //服開放オブジェクトが服購入
        if (rewardrelease.GetState() == global::AchievementRewardRelease.REWARDRELEASE.CLOTHING_BUY)
        {
            if (!connect.IsWait())
            {
                if (!rewardrelease.BuyEndFlag)
                {
                    //解放服が解放されていなければ解放
                    if (!IsAchievementsClothingRelease)
                    {

                        //インベントリ内にアイテムがなければ購入
                        if (!inventory.IsHaveItem(selectclothing.GetItemInfo().storeItem.ItemId))
                        {
                            store.BuyItem(selectclothing.GetItemInfo().storeItem.ItemId, PriceName);
                            Debug.Log(selectclothing.GetItemInfo().storeItem.ItemId + "を購入しました");
                        }
                        //プレイヤー所持金を確認
                        playermoney.RequestMoney();
                        //実績服解放済み
                        IsAchievementsClothingRelease = true;
                        clothing.BuyButtonPush();
                    }
                    else
                    {
                        if (!connect.IsWait())
                            rewardrelease.FinishBuyEvent();
                    }
                }

            }
        }
        //服を解放終了
        if (!rewardrelease.AchievementFlag)
        {
            State = STATE.RECEPTION;
            IsAchievementsClothingRelease = false;
        }
    }
    //ヒント表示
    private void PreviewHint()
    {
        // 通信中または下記状態なら不可
        if (clothing.GetState() != Clothing.SHELFSTATE.PREVIEW ||
            GetState() != ClothingBuyAndWear.STATE.RECEPTION
            || curtainAnime.state != CurtainAnime.STATE.WAIT
            || previewparent.State != PreviewParent.STATE.WAIT
            )
        {
            return;
        }

        //取得完了
        if (!connect.IsWait() && store.m_isCatalogGet && storeachivement.m_isStoreGet && store.m_isStoreGet)
        {
            if (selectclothing.GetItemInfo().catalogItem.CustomData != null)
            {
                //実績達成アイテムを所持しているか
                var achievementItem = storeachivement.StoreItems.Find(x => x.ItemId == selectclothing.GetItemInfo().catalogItem.CustomData.ToString());
                //条件達成服を持っておらず、実績を達成していなければ、ヒントを表示
                if (!inventory.IsHaveItem(selectclothing.GetItemInfo().catalogItem.ItemId) && !reachachievement.IsReachAchievement(achievementItem.ItemId.ToString()))
                {
                    IsPreviewHint = true;
                }
                else
                {
                    IsPreviewHint = false;
                }
            }
            else
                IsPreviewHint = false;
        }
    }
    //ボタンが押された
    public void PushButton()
    {
        IsPush = true;
    }
    //===========================================================================================================

    //取得
    public STATE GetState()
    {
        return State;
    }

    //表示関連
    public void PreviewHintEnd()
    {
        State = STATE.RECEPTION;
    }
    //表示金額の名前
    public string GetPriceName()
    {
        return PriceName;
    }
}
