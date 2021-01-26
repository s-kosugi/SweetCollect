using PlayFab.InsightsModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Clothing : MonoBehaviour
{
    [SerializeField] private Clothing clothing = null;              //棚オブジェクト
    [SerializeField] private SpriteRenderer PreviewImage = null;    //表示する画像

    private int PreviewOrder = 0;   //リストの何番目か
    private int OrderFromTheCenter;  //中心から見て何番目か

    private bool    IsMoveStart = false;           //移動開始
    private Vector3 MoveStartPosition;             //移動開始場所
    private Vector3 EndPosition;                 //最終的な最終位置
    private float DirectionTimer;                //演出時間

    private SpriteRenderer spriteRenderer = default;               //画像

    private void Awake()
    {
        clothing = this.transform.parent.GetComponent<Clothing>();
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        //移動開始
        if (IsMoveStart)
        {
            DirectionTimer += Time.deltaTime;

            //時間内は移動
            if(DirectionTimer < clothing.GetDirectionTime())
            {
                    this.transform.localPosition = new Vector3(Easing.OutSine(DirectionTimer, clothing.GetDirectionTime(), EndPosition.x , MoveStartPosition.x)
                        , 0.0f, 0.0f);
            }
            else
            {
                this.transform.localPosition = new Vector3(EndPosition.x, 0.0f, 0.0f);
                IsMoveStart = false;
                DirectionTimer = 0.0f;
            }
        }
    }

    //自分の表示する画像の設定
    public void SetPreviewImage(Sprite preveiw)
    {
        PreviewImage = this.GetComponent<SpriteRenderer>();
        PreviewImage.sprite = preveiw;
    }

    //自分の配列内番号
    public void SetPreviewOrder(int order)
    {
        PreviewOrder = order;
    }
    //選択されたPreviewから何番目か
    //selectnum : 選択されている番号
    public void WhatFromPreview(int selectnum)
    {
        //自分の番号から選択されている番号への差を求める
        OrderFromTheCenter = PreviewOrder - selectnum;

        //移動中なら再度やり直す
        if(IsMoveStart)
         DirectionTimer = 0.0f;
        else
         IsMoveStart = true;
        
        MoveStartPosition = this.transform.localPosition;
        EndPosition = clothing.SortChildPosition(OrderFromTheCenter);
    }

    /// <summary>
    /// 外部からの色のセット
    /// </summary>
    /// <param name="color"></param>
    public void SetColor( Color color)
    {
        spriteRenderer.color = color;
    }
}
