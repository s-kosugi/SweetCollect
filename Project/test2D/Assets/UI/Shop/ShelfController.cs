using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShelfController : MonoBehaviour
{
    [SerializeField] PlayFabStore PalyFabStore;                     //プレイハブのストア系統の処理

    [SerializeField] private List<Image> ShelfChild = new List<Image>();    //自分の子供リスト
    [SerializeField] private int Column = 4;                                //列
    [SerializeField] private int row = 4;                                   //行
    [SerializeField] private Sprite Sprite;                                 //画像
    [SerializeField] private Vector2 Size = new Vector2(50.0f, 50.0f);      //画像サイズ

    [SerializeField] private string ItemID;
    [SerializeField] private int Number = 0;

    enum PAGE
    {
        NONE = -1,
        ONE = 0,        //1
        MAX
    }


    // Start is called before the first frame update
    void Start()
    {
        PalyFabStore = GameObject.Find("PlayFabManager").GetComponent<PlayFabStore>();

        FindChild();
        SortImage();
        SetImage();
    }

    // Update is called once per frame
    void Update()
    {
        if(PalyFabStore.m_isStoreGet)
            ItemID = PalyFabStore.StoreItems[Number].ItemId;
    }
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
            for (int x = 0; x < row; x++)
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
