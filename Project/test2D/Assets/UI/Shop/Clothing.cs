﻿using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class Clothing : MonoBehaviour
{
    [SerializeField] PlayFabStore playFabStore;
    [SerializeField] private PlayFabInventory inventory = null;    //インベントリ
    [SerializeField] private PlayFabWaitConnect connect = null;    //通信
    [SerializeField] ShopCanvasController shopcanvas = null;
    [SerializeField] GameObject PreviewSprite;                     //服表示オブジェクト

    [SerializeField] List<Ui_Clothing> ClothingChild = new List<Ui_Clothing>();
    [SerializeField] private int SpriteDictionaryNumber; //画像の最大数
    [SerializeField] private int SelectNumber;          //選択されている画像の番号
    [SerializeField] private Vector2 ChildSize = new Vector2(144.0f, 144.0f);
    [SerializeField] private float Margin = 0;                              //余白
    Dictionary<string, Sprite> SpriteDictionary = new Dictionary<string, Sprite>();

    [SerializeField] private string TestName;
    [SerializeField] private bool IsHaving;                             //取得済み
    [SerializeField] private bool IsHaveCheck;                          //取得確認中
    [SerializeField] private float DIRECTION_TIME = 0.3f;                //演出時間

    private BuyButtonPicture buyButtonPicture = default;
    private List<bool> oldClothingChild = new List<bool>();
    private EffekseerEffectAsset buyEffect = null;
    public enum SHELFSTATE
    {
        NONE = -1,
        WAIT = 0,
        LOAD,
        CHANGE,
        PREVIEW,
        MAX,
    }
    [SerializeField] SHELFSTATE State;      //状態
    // Start is called before the first frame update
    void Start()
    {
        playFabStore = GameObject.Find("PlayFabStore").GetComponent<PlayFabStore>();
        inventory = GameObject.Find("PlayFabInventory").GetComponent<PlayFabInventory>();
        connect = GameObject.Find("PlayFabManager").GetComponent<PlayFabWaitConnect>();

        shopcanvas =GameObject.Find("ShopCanvas").GetComponentInParent<ShopCanvasController>();
        buyButtonPicture = GameObject.Find("ShopCanvas/ShopButton/BuyAndWearButton").GetComponent<BuyButtonPicture>();
        buyEffect = Resources.Load<EffekseerEffectAsset>("Effect\\buy");

        State = SHELFSTATE.WAIT;
        SelectNumber = 0;
        //Margin = ChildSize.x / 4;
        IsHaving = false;
        IsHaveCheck = false;

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
        }
    }

    //===========================================================================================================
    //状態関連
    private void Wait()
    {
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
            SpriteDictionary.Add(playFabStore.StoreItems[i].ItemId, Resources.Load<Sprite>("Player\\" + playFabStore.StoreItems[i].ItemId));
            SpriteDictionaryNumber = SpriteDictionary.Count;
        }

        CreateChild();

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
        TestName = SpriteDictionary[playFabStore.StoreItems[SelectNumber].ItemId].name;
        State = SHELFSTATE.CHANGE;
        
    }
    private void Change()
    {
        for(int i = 0; i < ClothingChild.Count; i++)
        {
            ClothingChild[i].WhatFromPreview(SelectNumber);
        }

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
        HavingItem();


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
            TestName = SpriteDictionary[playFabStore.StoreItems[SelectNumber].ItemId].name;
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
            TestName = SpriteDictionary[playFabStore.StoreItems[SelectNumber].ItemId].name;
            State = SHELFSTATE.CHANGE;
        }
    }

    //配列外参照をしないように確認
    private void CheckSelectNum()
    {
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
        //foreach (Transform Child in this.transform)
        //{
        //    //情報があった場合
        //    Ui_Clothing ItemInfo = Child.GetComponent<Ui_Clothing>();
        //    if (!ItemInfo)
        //    {
        //        ItemInfo = Child.gameObject.AddComponent<Ui_Clothing>();
        //    }
        //    ClothingChild.Add(ItemInfo);
        //}
        //SortChild();

        for(int i = 0; i < playFabStore.StoreItems.Count; i++)
        {
            GameObject Preview = Instantiate(PreviewSprite, this.transform);
            Ui_Clothing ItemInfo = Preview.GetComponent<Ui_Clothing>();
            ClothingChild.Add(ItemInfo);
        }
        SortChild();
    }

    //並べる
    private void SortChild()
    {
        for (int x = 0; x < ClothingChild.Count; x++)
        {
            ClothingChild[x].transform.localPosition = new Vector3((ChildSize.x + Margin) * x, 0.0f, 0.0f);
        }
    }
    //並べる
    public Vector3 SortChildPosition(int Num)
    {
        return new Vector3((ChildSize.x + Margin) * Num, 0.0f, 0.0f);
    }
    //===========================================================================================================
    //===========================================================================================================
    //アイテム
    //選択されている服を持っているか
    private void HavingItem()
    {
        if(IsHaveCheck)
        {
            if (!connect.IsWait())
            {
                if (inventory.IsHaveItem(shopcanvas.GetItemInfo().storeItem.ItemId))
                {
                    IsHaving = true;
                }
                else
                {
                    IsHaving = false;
                }
                buyButtonPicture.SetWear(IsHaving);
                IsHaveCheck = false;
            }
        }
        
    }

    //所持フラグの取得
    public bool GetHavingFlag()
    {
        return IsHaving;
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
                ClothingChild[i].SetColor(new Color(0f,0f,0f));
            }
            if (have == true && oldClothingChild[i] == false)
            {
                // 買った瞬間なのでエフェクトを再生する
                EffekseerSystem.PlayEffect(buyEffect,this.transform.position);
                SoundManager.Instance.PlaySE("Buy");

                oldClothingChild[i] = true;
            }
        }
    }
}
