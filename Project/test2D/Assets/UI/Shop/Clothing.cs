using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Clothing : MonoBehaviour
{
    [SerializeField] PlayFabStore PalyFabStore;
    [SerializeField] ShopCanvasController shopcanvas = null;
    [SerializeField] SelectButton selectbutton = null;
    [SerializeField] List<Ui_Clothing> ClothingChild = new List<Ui_Clothing>();
    [SerializeField] private int SpriteDictionaryNumber; //画像の最大数
    [SerializeField] private int SelectNumber;          //選択されている画像の番号
    [SerializeField] private Vector2 ChildSize = new Vector2(144.0f, 144.0f);
    [SerializeField] private float Margin = 0;                              //余白
    Dictionary<string, Sprite> SpriteDictionary = new Dictionary<string, Sprite>();
    [SerializeField] private string TestName;

    [SerializeField] private float DIRECTION_TIME = 0.3f;                //演出時間

    enum SHELFSTATE
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
        PalyFabStore = GameObject.Find("PlayFabStore").GetComponent<PlayFabStore>();
        shopcanvas = this.GetComponentInParent<ShopCanvasController>();
        selectbutton = this.transform.root.Find("ItemState_Parent").GetComponentInChildren<SelectButton>();
        State = SHELFSTATE.WAIT;
        SelectNumber = 0;
        Margin = ChildSize.x / 4;
        FindChild();
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
        if(PalyFabStore.m_isCatalogGet)
        {
            if (PalyFabStore.m_isStoreGet)
            {
                State = SHELFSTATE.LOAD;
            }
        }
    }

    private void Load()
    {
        //ストアのアイテムカウント分リストを追加
        for (int i = 0; i < PalyFabStore.StoreItems.Count; i++)
        {
            SpriteDictionary.Add(PalyFabStore.StoreItems[i].ItemId, Resources.Load<Sprite>("Player\\" + PalyFabStore.StoreItems[i].ItemId));
            SpriteDictionaryNumber = SpriteDictionary.Count;
        }

        for (int i = 0; i < ClothingChild.Count; i++)
        {

            if (i < PalyFabStore.StoreItems.Count)
            {
                ClothingChild[i].SetPreviewImage(SpriteDictionary[PalyFabStore.StoreItems[i].ItemId]);
            }
            else
            {
                ClothingChild[i].SetPreviewImage(SpriteDictionary[PalyFabStore.StoreItems[0].ItemId]);
            }
            ClothingChild[i].SetPreviewOrder(i);
            ClothingChild[i].WhatFromPreview(SelectNumber);
        }
        TestName = SpriteDictionary[PalyFabStore.StoreItems[SelectNumber].ItemId].name;
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
            shopcanvas.SetSelectItem(PalyFabStore.StoreItems[SelectNumber]);
        }

        selectbutton.CheckHaving();

        State = SHELFSTATE.PREVIEW;
    }

    private void Preview()
    {
        
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
            TestName = SpriteDictionary[PalyFabStore.StoreItems[SelectNumber].ItemId].name;
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
            TestName = SpriteDictionary[PalyFabStore.StoreItems[SelectNumber].ItemId].name;
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
    private void FindChild()
    {
        foreach (Transform Child in this.transform)
        {
            //情報があった場合
            Ui_Clothing ItemInfo = Child.GetComponent<Ui_Clothing>();
            if (!ItemInfo)
            {
                ItemInfo = Child.gameObject.AddComponent<Ui_Clothing>();
            }
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
    //取得(Getter)
    public float GetDirectionTime()
    {
        return DIRECTION_TIME;
    }
    //===========================================================================================================
}
