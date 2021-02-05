﻿using PlayFab.InsightsModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Clothing : MonoBehaviour
{
    [SerializeField] private SpriteRenderer PreviewImage = null;    //表示する画像

    private SpriteRenderer spriteRenderer = default;               //画像

    private void Awake()
    {
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
    }

    //自分の表示する画像の設定
    public void SetPreviewImage(Sprite preveiw)
    {
        PreviewImage = this.GetComponent<SpriteRenderer>();
        PreviewImage.sprite = preveiw;
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
