using Effekseer;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

public class Clothing : MonoBehaviour
{
    [SerializeField] private PlayFabStore                playfabstore = null;                     //プレイハブ
    [SerializeField] private PlayFabInventory            inventory = null;                        //インベントリ
    [SerializeField] private PlayFabWaitConnect          connect = null;                          //通信
    [SerializeField] private AchievementRewardRelease    rewardrelease = null;                    //達成イベント
    [SerializeField] private SelectClothing              selectclothing = null;                   //選択されている服
    [SerializeField] private GameObject                  previewsprite = default;                 //服表示オブジェクト
    [SerializeField] private CurtainAnime                curtain = null;                          //カーテンアニメ
    [SerializeField] private PlayFabPlayerData           playerdata = null;                       //プレイヤーデータ
    [SerializeField] private PreviewParent               previewparent = null;                    //服表示オブジェクトの親
    [SerializeField] private SwipeMove_Shop              swipemoveshop = null;                    //スワイプ

    [SerializeField] private GameObject                  PreviewParent = null;                    //画像表示オブジェクトの親
    List<Ui_Clothing>                                    ClothingChild = new List<Ui_Clothing>(); //表示する子供の数
    private int                                          SpriteDictionaryNumber;                  //画像の最大数
    private int                                          SelectNumber;                            //選択されている画像の番号
    [SerializeField] private float                       SpriteSizeWidth = 144.0f;                //表示画像のサイズ（子）
    [SerializeField] private float                       Margin = 0;                              //余白
    public float                                         HalfSize{ get; private set; }            //画像と余白を合わせた半分サイズ

    Dictionary<string, Sprite>                           SpriteDictionary = new Dictionary<string, Sprite>();  //画像
    Dictionary<string, int>                              SpriteNumber = new Dictionary<string, int>();         //画像の番号

    private bool                                         IsHaveCheck;                             //取得確認中
    [SerializeField] private float                       MOVETIMENORMAL      = 0.15f;             //移動時間
    [SerializeField] private float                       MOVETIMEACHIEVEMENT = 0.1f;              //移動時間
    private float                                        DirectionTime = 0.0f;                    //実績達成演出時間
    private bool                                         MoveEndFlag;                             //実績達成イベント移動(服の移動)
    private bool                                         InfoChild;                               //子に情報を与えた

    [SerializeField] private BuyButtonPicture buyButtonPicture = default;                         //ボタンの画像
    private List<bool> oldClothingChild = new List<bool>();                                       //黒塗をする際に必要な情報
    [SerializeField] EffekseerEffectAsset buyEffect = default;                                    //購入時のエフェクト
    private bool isBuyButtonPush = false;                                                         // 購入ボタンの押下チェック
    public enum SHELFSTATE
    {
        NONE = -1,
        WAIT = 0,                       //待機
        LOAD,                           //取得
        CHANGE,                         //変更
        PREVIEW,                        //表示
        REWARDRELEASE,                  //服開放
        MAX,
    }
    [SerializeField] SHELFSTATE State;      //状態
    // Start is called before the first frame update
    void Start()
    {
        State = SHELFSTATE.WAIT;
        SelectNumber = 0;
        HalfSize = SpriteSizeWidth / 2 + Margin / 2;
        IsHaveCheck = false;
        MoveEndFlag = false;
        InfoChild = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case SHELFSTATE.WAIT: Wait(); break;
            case SHELFSTATE.LOAD: Load(); break;
            case SHELFSTATE.CHANGE: Change(); break;
            case SHELFSTATE.PREVIEW: Preview(); break;
            case SHELFSTATE.REWARDRELEASE: RewardRelease(); break;
        }
    }

    //===========================================================================================================
    //状態関連
    private void Wait()
    {
        //PlayFabのストアとカタログ情報が入手できればロード状態
        if (!connect.IsWait())
        {
            if (playfabstore.isCatalogGet && playfabstore.isStoreGet)
            {
                State = SHELFSTATE.LOAD;
            }
        }
    }

    private void Load()
    {
        //子供の情報を設定
        if(!InfoChild)
        {
            //複数回通してはいけない

            //ストアのアイテムカウント分リストを追加
            for (int i = 0; i < playfabstore.StoreItems.Count; i++)
            {
                Sprite sprite = Resources.Load<Sprite>("Player\\" + playfabstore.StoreItems[i].ItemId);
                if (sprite == null)
                {
                    continue;
                }
                SpriteDictionary.Add(playfabstore.StoreItems[i].ItemId, sprite);
                SpriteNumber.Add(playfabstore.StoreItems[i].ItemId, i);
                SpriteDictionaryNumber = SpriteDictionary.Count;
            }

            //最大スワイプ位置を設定
            swipemoveshop.MaxMoveCalculation(SpriteSizeWidth, Margin, SpriteDictionaryNumber);

            //画像表示オブジェクトの追加
            CreateChild();

            //画像表示オブジェクトにスプライトを設定
            for (int i = 0; i < ClothingChild.Count; i++)
            {
                if (i < playfabstore.StoreItems.Count)
                {
                    ClothingChild[i].SetPreviewImage(SpriteDictionary[playfabstore.StoreItems[i].ItemId]);
                }
                else
                {
                    ClothingChild[i].SetPreviewImage(SpriteDictionary[playfabstore.StoreItems[0].ItemId]);
                }
            }
            //全ての処理が通ったから情報を与えられたと仮定
            InfoChild = true;
        }

        //プレイヤーが着ている服
        PlayerWearing();

    }
    private void Change()
    {
        //配列外参照防止
        CheckSelectNum();

        //現在の選択番号
        previewparent.ChangePosition(SelectNumber);

        //選択されている服を設定
        if (selectclothing)
        {
            selectclothing.SetSelectItem(playfabstore.StoreItems[SelectNumber]);
        }

        //その服を持っているかどうかを確認
        CheckHavingCloting();

        State = SHELFSTATE.PREVIEW;

        // 持っていない服の黒塗り処理
        FillBlack();
    }

    private void Preview()
    {
        //服開放実行
        if(rewardrelease.AchievementFlag && rewardrelease.GetState() == global::AchievementRewardRelease.REWARDRELEASE.CHECK)
        {
            State = SHELFSTATE.REWARDRELEASE;
        }
        
        //所持しているかを確認
        ChangeButtonAppearance();
        // 持っていない服の黒塗り処理
        FillBlack();

    }
    //実績達成演出
    private void RewardRelease()
    {
        if(rewardrelease.GetState() == global::AchievementRewardRelease.REWARDRELEASE.CLOTHING_MOVE)
        {
            //服の移動が終了していなければ
            if (!rewardrelease.ClotingMoveEndFlag)
            {
                //移動が終了していなければ移動継続
                if(!MoveEndFlag)
                {
                    int value;
                    if (SpriteNumber.TryGetValue(rewardrelease.AchievementClotingName, out value))
                    {
                        SelectNumber = value;

                        //選択されている服を設定
                        if (selectclothing)
                        {
                            selectclothing.SetSelectItem(playfabstore.StoreItems[SelectNumber]);
                        }

                        //選択されている番号が配列外参照を起こさないかをチェック
                        CheckSelectNum();

                        //選択番号まで位置を変更
                        previewparent.ChangePosition(SelectNumber);

                        MoveEndFlag = true;
                    }
                }

                //一定時間経過で移動を終了
                if (DirectionTime > GetMoveTime())
                    rewardrelease.FinishClotingMove();
                else
                    DirectionTime += Time.deltaTime;
            }
        }

        //実績を達成した服の購入が終わったら変更状態に戻す
        if(!rewardrelease.AchievementFlag)
        {
            State = SHELFSTATE.CHANGE;
            MoveEndFlag = false;
            DirectionTime = 0.0f;
        }
        // 持っていない服の黒塗り処理
        FillBlack();

    }

    //===========================================================================================================
    //ボタン関連
    //一つ進める
    public void PushButton_Next()
    {
        //表示状態でカーテンが待機状態なら洗濯している服を変更
        if (State == SHELFSTATE.PREVIEW && curtain.state == CurtainAnime.STATE.WAIT)
        {
            SelectNumber += 1;
            CheckSelectNum();
            State = SHELFSTATE.CHANGE;
        }
    }
    //一つ戻す
    public void PushButton_Back()
    {
        //表示状態でカーテンが待機状態なら洗濯している服を変更
        if (State == SHELFSTATE.PREVIEW && curtain.state == CurtainAnime.STATE.WAIT)
        {
            SelectNumber -= 1;
            CheckSelectNum();
            State = SHELFSTATE.CHANGE;
        }
    }

    //配列外参照をしないように確認
    private void CheckSelectNum()
    {
        //中心に表示されている画像が配列外かどうかを確認
        //マイナスにはならない
        if(SelectNumber < 0)
        {
            SelectNumber = 0;
        }
        else if(SpriteDictionary.Count - 1 < SelectNumber)
        {
            SelectNumber = SpriteDictionary.Count - 1;
        }
    }

    //===========================================================================================================
    //子供関連
    private void CreateChild()
    {
        //プレイファブストアのストアアイテム分画像表示用オブジェクトを生成
        foreach( var item in playfabstore.StoreItems)
        {
            // スプライトが存在しない場合は作らない
            Sprite sprite;
            if(!SpriteDictionary.TryGetValue(item.ItemId,out sprite))
            {
                continue;
            }
            GameObject Preview = Instantiate(previewsprite, PreviewParent.transform);
            Ui_Clothing ItemInfo = Preview.GetComponent<Ui_Clothing>();
            ClothingChild.Add(ItemInfo);
        }

        //子供を並び替え
        SortCreateObject();
    }

    //生成されたオブジェクトを並べる
    private void SortCreateObject()
    {
        for (int x = 0; x < ClothingChild.Count; x++)
        {
            ClothingChild[x].transform.localPosition = new Vector3((SpriteSizeWidth + Margin) * x + HalfSize, 0.0f, 0.0f);
        }
    }
    //オブジェクトソート
    // Num : 中心から見て何番目
    public Vector3 Sort_ParentPos(int Num)
    {
        return new Vector3((-(SpriteSizeWidth + Margin) * Num) - HalfSize, 0.0f, 0.0f);
    }
    //===========================================================================================================
    //===========================================================================================================
    // アイテム
    // ボタンの見た目を変更する
    private void ChangeButtonAppearance()
    {
        //所持確認
        if(IsHaveCheck)
        {
            if (!connect.IsWait())
            {
                string ItemID = selectclothing.GetItemInfo().storeItem.ItemId;
                //インベントリで所持状態を確認
                if (inventory.IsHaveItem(ItemID))
                {
                    buyButtonPicture.ChangeWearState();
                }
                // 条件付きアイテムの場合
                else if (playfabstore.CatalogItems.Find(x => x.ItemId == ItemID).CustomData != null)
                {
                    buyButtonPicture.ChangeQuestionState();
                }
                // 購入ボタンの場合
                else
                {
                    string price = playfabstore.StoreItems.Find((x => x.ItemId == ItemID)).VirtualCurrencyPrices["HA"].ToString();
                    buyButtonPicture.ChangeBuyState(price);
                }

                IsHaveCheck = false;
            }
        }
        
    }

    //取得状態の確認
    public void CheckHavingCloting()
    {
        IsHaveCheck = true;
    }

    //===========================================================================================================

    //===========================================================================================================
    //取得(Getter)
    //移動時間
    public float GetMoveTime()
    {
        if(State == SHELFSTATE.REWARDRELEASE)
            return MOVETIMEACHIEVEMENT;
        else
           return MOVETIMENORMAL;
    }
    //状態取得
    public SHELFSTATE GetState()
    {
        return State;
    }
    //画像サイズ
    public float GetSpriteSize()
    {
        return SpriteSizeWidth;
    }

    //余白サイズ
    public float GetMarginSize()
    {
        return Margin;
    }
    //選択中の番号
    public int GetSelectNumber()
    {
        return SelectNumber;
    }

    //===========================================================================================================

    /// <summary>
    /// 服の黒塗処理
    /// </summary>
    private void FillBlack( )
    {
        // old未作成なら作成する
        if (ClothingChild.Count > 0 && oldClothingChild.Count == 0)
        {
            for( int i = 0; i < ClothingChild.Count; i++)
            {
                oldClothingChild.Add(inventory.IsHaveItem(playfabstore.StoreItems[i].ItemId));
            }
        }

        for(int i = 0; i < ClothingChild.Count;i++)
        {
            bool have = inventory.IsHaveItem(playfabstore.StoreItems[i].ItemId);
            if (have)
            {
                ClothingChild[i].SetColor(new Color(1f,1f,1f));
            }
            else
            {
                ClothingChild[i].SetColor(new Color(0.2f,0.2f,0.2f));
            }
            if (have == true && oldClothingChild[i] == false)
            {
                // 0番服はデフォルト服なのでエフェクト再生などは無し
                if (i != 0)
                {
                    // 購入ボタンが押されていた
                    if (isBuyButtonPush)
                    {
                        // 買った瞬間なのでエフェクトを再生する
                        EffekseerSystem.PlayEffect(buyEffect, this.transform.position);
                        SoundManager.Instance.PlaySE("Buy");

                        isBuyButtonPush = false;
                    }
                }

                oldClothingChild[i] = true;
            }
        }
    }
    /// <summary>
    /// 購入ボタン押下時処理
    /// </summary>
    public void BuyButtonPush()
    {
        isBuyButtonPush = true;
    }

    //プレイヤーがどの服を着ているか
    private void PlayerWearing()
    {
        if(!connect.IsWait())
        {
            if(playerdata.isGet)
            {
                UserDataRecord playerclothingdata = null;
                if(playerdata.data.TryGetValue(PlayerDataName.ECLOTHES, out playerclothingdata))
                {
                    SelectNumber = SpriteNumber[playerclothingdata.Value];
                    State = SHELFSTATE.CHANGE;
                }
            }
        }
    }

    //スワイプ移動による選択衣服を変更
    public void ChangeClothing_Swipe(int select)
    {
        SelectNumber = select;

        //配列外参照
        CheckSelectNum();

        //選択されている服を設定
        if (selectclothing)
        {
            selectclothing.SetSelectItem(playfabstore.StoreItems[SelectNumber]);
        }

        CheckHavingCloting();

    }

}
