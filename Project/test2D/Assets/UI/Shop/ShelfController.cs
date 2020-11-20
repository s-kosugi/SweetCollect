using PlayFab.MultiplayerModels;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShelfController : MonoBehaviour
{
    [SerializeField] PlayFabStore PalyFabStore;                     //プレイハブのストア系統の処理

    [SerializeField] private List<Image> ShelfChild = new List<Image>();    //自分の子供リスト
    [SerializeField] private int Column = 4;                                //列
    [SerializeField] private int row = 4;                                   //行
    [SerializeField] private Sprite Sprite = null;                          //画像
    [SerializeField] private Vector2 Size = new Vector2(50.0f, 50.0f);      //画像サイズ

    [SerializeField] private string ItemID = default;
    [SerializeField] private int Number = 0;

    Dictionary<string, Sprite> SpriteDictionary = new Dictionary<string, Sprite>();

    [SerializeField] private string TestName;

    enum PAGE
    {
        NONE = -1,
        ONE = 0,        //1
        MAX
    }

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

        State = SHELFSTATE.WAIT;
        FindChild();
        SortImage();
        SetImage();
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case SHELFSTATE.WAIT: Wait(); break;
            case SHELFSTATE.LOAD: Load(); break;
            case SHELFSTATE.CHANGE: Change(); break;
            case SHELFSTATE.PREVIEW:Preview(); break;
        }    
    }

    //===========================================================================================================
    //状態関連
    private void Wait()
    {
        if (PalyFabStore.m_isStoreGet)
        {  
            State = SHELFSTATE.LOAD;
        }
    }

    private void Load()
    {
        for (int i = 0; i < PalyFabStore.StoreItems.Count; i++)
        {
            SpriteDictionary.Add(PalyFabStore.StoreItems[i].ItemId, Resources.Load<Sprite>("Player\\" + PalyFabStore.StoreItems[i].ItemId));
        }
        ItemID = PalyFabStore.StoreItems[Number].ItemId; //今の自分の服のストリング

        State = SHELFSTATE.CHANGE;
    }
    private void Change()
    {
        for(int i = 0; i < ShelfChild.Count; i++)
        {
            if(i < PalyFabStore.StoreItems.Count)
            {
                //情報があった場合
                ShelfInfo ItemInfo = ShelfChild[i].transform.gameObject.GetComponent<ShelfInfo>();
                if (!ItemInfo)
                {
                    ItemInfo = ShelfChild[i].transform.gameObject.AddComponent<ShelfInfo>();
                }
                ItemInfo.SetItemInfo(PalyFabStore.StoreItems[i]);

                ShelfChild[i].sprite = SpriteDictionary[PalyFabStore.StoreItems[i].ItemId];        
            }
            else
            {
                //情報がなかった場合
                ShelfChild[i].sprite = Sprite;
            }
        }

        State = SHELFSTATE.PREVIEW;
    } 
    private void Preview()
    {
        TestName = SpriteDictionary[PalyFabStore.StoreItems[Number].ItemId].name;
    }


    //===========================================================================================================
    //===========================================================================================================
    //子供関連
    //自分の子供を検索
    private void FindChild()
    {
        foreach(Transform Child in this.transform)
        {
            ShelfChild.Add(Child.transform.GetComponent<Image>());
        }
    }
    //===========================================================================================================
    //並び替え
    private void SortImage()
    {
        int Number = 0;

        for(int y = 0; y < row; y++)
        {
            for (int x = 0; x < Column; x++)
            {
                ShelfChild[Number].transform.localPosition = new Vector3(Size.x * x, -Size.y * y, 0.0f);

                Number++;
            }
        }
    }
    //===========================================================================================================
    //画像関連
    //画像の指定
    private void SetImage()
    {
        foreach( var Item in ShelfChild)
        {
            Item.sprite = Sprite;
        }
    }
}
