﻿using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class Clothing : MonoBehaviour
{
    PlayFabStore playFabStore;                                               //プレイハブ
    private PlayFabInventory inventory = null;                               //インベントリ
    private PlayFabWaitConnect connect = null;                               //通信
    private AchievementRewardRelease AchievementEvents = null;               //達成イベント
    ShopCanvasController shopcanvas = null;                                  //ショップキャンバス
    [SerializeField] GameObject PreviewSprite = default;                     //服表示オブジェクト

    [SerializeField] List<Ui_Clothing> ClothingChild = new List<Ui_Clothing>(); //表示する子供の数
    [SerializeField] private int SpriteDictionaryNumber;                      //画像の最大数
    [SerializeField] private int SelectNumber;                                //選択されている画像の番号
    [SerializeField] private Vector2 ChildSize = new Vector2(144.0f, 144.0f); //表示画像のサイズ（子）
    [SerializeField] private float Margin = 0;                                 //余白
    Dictionary<string, Sprite> SpriteDictionary = new Dictionary<string, Sprite>(); //画像
    Dictionary<string, int> SpriteNumber = new Dictionary<string, int>(); //画像の番号

    private bool IsHaveCheck;                                                 //取得確認中
    [SerializeField] private float DIRECTION_TIME = 0.3f;                     //演出時間
    [SerializeField] private float EventDirectionTime = 0.0f;                 //実績達成イベント時間
    [SerializeField] private bool EventMoveFlag;                                //実績達成イベント移動

    [SerializeField] private BuyButtonPicture buyButtonPicture = default;       //
    private List<bool> oldClothingChild = new List<bool>();                     //
    private EffekseerEffectAsset buyEffect = null;                              //
    private bool isBuyButtonPush = false;                                       // 購入ボタンの押下チェック
    public enum SHELFSTATE
    {
        NONE = -1,
        WAIT = 0,
        LOAD,
        CHANGE,
        PREVIEW,
        ACHIEVEMENTREWARDRELEASE,
        MAX,
    }
    [SerializeField] SHELFSTATE State;      //状態
    // Start is called before the first frame update
    void Start()
    {
        playFabStore = GameObject.Find("PlayFabStore").GetComponent<PlayFabStore>();
        inventory = GameObject.Find("PlayFabInventory").GetComponent<PlayFabInventory>();
        connect = GameObject.Find("PlayFabManager").GetComponent<PlayFabWaitConnect>();
        AchievementEvents = GameObject.Find("AchievementEventController").GetComponent<AchievementRewardRelease>();

        shopcanvas = GameObject.Find("ShopCanvas").GetComponentInParent<ShopCanvasController>();
        // ハードコーディングで可変に対応できない為コメントアウト
        //buyButtonPicture = GameObject.Find("ShopCanvas/RectScaleParent/ShopButton/BuyAndWearButton").GetComponent<BuyButtonPicture>();
        buyEffect = Resources.Load<EffekseerEffectAsset>("Effect\\buy");

        State = SHELFSTATE.WAIT;
        SelectNumber = 0;
        IsHaveCheck = false;
        EventMoveFlag = false;

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
            case SHELFSTATE.ACHIEVEMENTREWARDRELEASE: AchievementRewardRelease(); break;
        }
    }

    //===========================================================================================================
    //状態関連
    private void Wait()
    {
        //プレイファブのストアとカタログ情報が入手できればロード状態
        if(playFabStore.m_isCatalogGet)
        {
            if (playFabStore.m_isStoreGet)
            {
                State = SHELFSTATE.LOAD;
            }
        }
    }

    private void Load()
    {
        //ストアのアイテムカウント分リストを追加
        for (int i = 0; i < playFabStore.StoreItems.Count; i++)
        {
            Sprite sprite = Resources.Load<Sprite>("Player\\" + playFabStore.StoreItems[i].ItemId);
            if (sprite == null)
            {
                continue;
            }
            SpriteDictionary.Add(playFabStore.StoreItems[i].ItemId, sprite);
            SpriteNumber.Add(playFabStore.StoreItems[i].ItemId, i);
            SpriteDictionaryNumber = SpriteDictionary.Count;
        }

        //画像表示オブジェクトの追加
        CreateChild();

        //画像表示オブジェクトにスプライトを設定
        for (int i = 0; i < ClothingChild.Count; i++)
        {
            if (i < playFabStore.StoreItems.Count)
            {
                ClothingChild[i].SetPreviewImage(SpriteDictionary[playFabStore.StoreItems[i].ItemId]);
            }
            else
            {
                ClothingChild[i].SetPreviewImage(SpriteDictionary[playFabStore.StoreItems[0].ItemId]);
            }
            ClothingChild[i].SetPreviewOrder(i);
            ClothingChild[i].WhatFromPreview(SelectNumber);
        }

        State = SHELFSTATE.CHANGE;
        
    }
    private void Change()
    {
        //現在選択されている配列から何番目かを設定
        for(int i = 0; i < ClothingChild.Count; i++)
        {
            ClothingChild[i].WhatFromPreview(SelectNumber);
        }

        //ショップキャンバスに選択されているアイテムを設定
        if(shopcanvas)
        {
            shopcanvas.SetSelectItem(playFabStore.StoreItems[SelectNumber]);
        }

        CheckHavingCloting();
        State = SHELFSTATE.PREVIEW;

        // 持っていない服の黒塗り処理
        FillBlack();
    }

    private void Preview()
    {
        //服開放実行
        if(AchievementEvents.AchievementFlag)
        {
            State = SHELFSTATE.ACHIEVEMENTREWARDRELEASE;
        }
        
        //所持しているかを確認
        ChangeButtonAppearance();
        // 持っていない服の黒塗り処理
        FillBlack();

    }

    private void AchievementRewardRelease()
    {
        if (!AchievementEvents.ClotingMoveEventFlag)
        {
            if(!EventMoveFlag)
            {
                int value;
                if (SpriteNumber.TryGetValue(AchievementEvents.AchievementClotingName, out value))
                {
                    SelectNumber = value;

                    //ショップキャンバスに選択されているアイテムを設定
                    if (shopcanvas)
                    {
                        shopcanvas.SetSelectItem(playFabStore.StoreItems[SelectNumber]);
                    }

                    CheckSelectNum();

                    for (int i = 0; i < ClothingChild.Count; i++)
                    {
                        ClothingChild[i].WhatFromPreview(SelectNumber);
                    }
                    EventMoveFlag = true;
                }
            }

            if (EventDirectionTime > DIRECTION_TIME)
                AchievementEvents.FinishClotingMoveEvent();
            else
                EventDirectionTime += Time.deltaTime;
        }

        if(!AchievementEvents.AchievementFlag)
        {
            State = SHELFSTATE.CHANGE;
            EventMoveFlag = false;
            EventDirectionTime = 0.0f;
        }
        // 持っていない服の黒塗り処理
        FillBlack();

    }

    //===========================================================================================================
    //ボタン関連
    //一つ進める
    public void PushButton_Next()
    {
        if (State == SHELFSTATE.PREVIEW)
        {
            SelectNumber += 1;
            CheckSelectNum();
            State = SHELFSTATE.CHANGE;
        }
    }
    //一つ戻す
    public void PushButton_Back()
    {
        if (State == SHELFSTATE.PREVIEW)
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
        foreach( var item in playFabStore.StoreItems)
        {
            // スプライトが存在しない場合は作らない
            Sprite sprite;
            if(!SpriteDictionary.TryGetValue(item.ItemId,out sprite))
            {
                continue;
            }
            GameObject Preview = Instantiate(PreviewSprite, this.transform);
            Ui_Clothing ItemInfo = Preview.GetComponent<Ui_Clothing>();
            ClothingChild.Add(ItemInfo);
        }
        SortChild();
    }

    //生成されたオブジェクトを並べる
    private void SortChild()
    {
        for (int x = 0; x < ClothingChild.Count; x++)
        {
            ClothingChild[x].transform.localPosition = new Vector3((ChildSize.x + Margin) * x, 0.0f, 0.0f);
        }
    }
    //オブジェクトソート
    // Num : 中心から見て何番目
    public Vector3 SortChildPosition(int Num)
    {
        return new Vector3((ChildSize.x + Margin) * Num, 0.0f, 0.0f);
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
                string ItemID = shopcanvas.GetItemInfo().storeItem.ItemId;
                //インベントリで所持状態を確認
                if (inventory.IsHaveItem(ItemID))
                {
                    buyButtonPicture.ChangeWearState();
                }
                // 条件付きアイテムの場合
                else if (playFabStore.CatalogItems.Find(x => x.ItemId == ItemID).CustomData != null)
                {
                    buyButtonPicture.ChangeQuestionState();
                }
                // 購入ボタンの場合
                else
                {
                    string price = playFabStore.StoreItems.Find((x => x.ItemId == ItemID)).VirtualCurrencyPrices["HA"].ToString();
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
    public float GetDirectionTime()
    {
        return DIRECTION_TIME;
    }

    public SHELFSTATE GetState()
    {
        return State;
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
                oldClothingChild.Add(inventory.IsHaveItem(playFabStore.StoreItems[i].ItemId));
            }
        }

        for(int i = 0; i < ClothingChild.Count;i++)
        {
            bool have = inventory.IsHaveItem(playFabStore.StoreItems[i].ItemId);
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
}
