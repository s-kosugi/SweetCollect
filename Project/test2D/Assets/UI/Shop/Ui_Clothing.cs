using PlayFab.InsightsModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Clothing : MonoBehaviour
{
    [SerializeField] private Clothing clothing = null;   
    [SerializeField] private SpriteRenderer PreviewImage = null;

    [SerializeField] private int PreviewOrder = 0;   //リストの何番目か
    [SerializeField] private int OrderFromTheCenter;  //中心から見て何番目か

    [SerializeField] private bool IsDirection;       //演出
    [SerializeField] private Vector3 DirectionStartPosition;       //演出開始場所
    [SerializeField] private Vector3 EndPosition;                 //最終的な最終位置
    [SerializeField] private float DirectionTimer;                //演出時間

    private void Awake()
    {
        clothing = this.transform.parent.GetComponent<Clothing>();
    }

    private void Start()
    {
    }


    private void Update()
    {
        if (IsDirection)
        {
            DirectionTimer += Time.deltaTime;
            if(DirectionTimer < clothing.GetDirectionTime())
            {
                    this.transform.localPosition = new Vector3(Easing.Linear(DirectionTimer, clothing.GetDirectionTime(), EndPosition.x , DirectionStartPosition.x)
                        , 0.0f, 0.0f);
            }
            else
            {
                this.transform.localPosition = new Vector3(EndPosition.x, 0.0f, 0.0f);
                IsDirection = false;
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
    public void WhatFromPreview(int selectnum)
    {
        OrderFromTheCenter = PreviewOrder - selectnum;

        if(IsDirection)
         DirectionTimer = 0.0f;
        else
         IsDirection = true;
        
        DirectionStartPosition = this.transform.localPosition;
        EndPosition = clothing.SortChildPosition(OrderFromTheCenter);
    }


}
