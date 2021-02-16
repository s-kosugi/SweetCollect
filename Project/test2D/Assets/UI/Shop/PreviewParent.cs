using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewParent : MonoBehaviour
{
    [SerializeField] private Clothing clothing = null;                          //棚オブジェクト
    [SerializeField] private SwipeMove_Shop swipeMove_shop = null;              //スワイプ

    private int PreviewNumber = 0;                                  //選択されている服の番号

    private bool    IsMoveStart = false;                            //移動開始
    private Vector3 MoveStartPosition;                              //移動開始場所
    private Vector3 EndPosition;                                    //最終的な最終位置
    private float DirectionTimer;                                   //演出時間

    private SpriteRenderer spriteRenderer = default;                //画像

    [SerializeField] private float SpriteSize = 0.0f;           //画像サイズ
    [SerializeField] private float MarginSize = 0.0f;           //余白サイズ
    [SerializeField] private float StartPosWight = 0.0f;        //開始位置ズ

    public enum STATE
    {
        NONE = -1,
        WAIT = 0,
        SWIP,
        FRICTION,
        MOVE
    }
    public STATE State { get; private set; } //状態


    private void Awake()
    {
    }

    private void Start()
    {
        SpriteSize = clothing.GetSpriteSize();
        MarginSize = clothing.GetMarginSize();
        StartPosWight = -(SpriteSize / 2 + MarginSize / 2);

        this.transform.localPosition = new Vector3(StartPosWight, 0.0f, 0.0f);

        State = STATE.MOVE;
    }


    private void Update()
    {
        switch (State)
        {
            case STATE.WAIT:     Wait();        break;
            case STATE.SWIP:     Swip();        break;
            case STATE.FRICTION: Friction();    break;
            case STATE.MOVE:     Move();        break;

        }

    }

    //状態関連
    private void Wait()
    {
        //スワイプ開始
        if(swipeMove_shop.TouchFlag && swipeMove_shop.CheckDistance())
        {
            State = STATE.SWIP;
        }
    }
    private void Swip()
    {
        SelectClothingNumCheck();
        FinishSwipe();
    }
    private void Friction()
    {
        SelectClothingNumCheck();
        //スワイプ開始
        if (swipeMove_shop.TouchFlag)
        {
            State = STATE.SWIP;
            return;
        }

        //一定速度以下でフリクション終了
        if(Mathf.Abs(swipeMove_shop.GetInertiaMove()) < swipeMove_shop.GetStopThreshold())
        {
            ChangePosition(clothing.GetSelectNumber());
            State = STATE.MOVE;
        }

    }
    private void Move()
    {
        MoveMent();
    }

    //位置変更
    //selectnum : 選択されている番号
    public void ChangePosition(int selectnum)
    {
        PreviewNumber = selectnum;

        //移動中なら再度やり直す
        if (IsMoveStart)
            DirectionTimer = 0.0f;
        else
            IsMoveStart = true;

        MoveStartPosition = this.transform.localPosition;
        EndPosition = clothing.Sort_ParentPos(PreviewNumber);

        State = STATE.MOVE;
    }

    //移動
    private void  MoveMent()
    {
        //移動開始
        if (IsMoveStart)
        {
            DirectionTimer += Time.deltaTime;

            //時間内は移動
            if (DirectionTimer < clothing.GetMoveTime())
            {
                this.transform.localPosition = new Vector3(Easing.OutSine(DirectionTimer, clothing.GetMoveTime(), EndPosition.x, MoveStartPosition.x)
                    , 0.0f, 0.0f);
            }
            else
            {
                this.transform.localPosition = new Vector3(EndPosition.x, 0.0f, 0.0f);
                IsMoveStart = false;
                DirectionTimer = 0.0f;
                State = STATE.WAIT;
            }
        }

    }

    //自分が選択されるべき番号
    private void SelectClothingNumCheck()
    {
        //現在の自分の位置から何番目を表示するのかを求める
        int previewdisplaynum = (int)(this.transform.localPosition.x / (SpriteSize + MarginSize)) * -1;

        if (clothing.GetSelectNumber() != previewdisplaynum)
        {
            clothing.ChangeClothing_Swipe(previewdisplaynum);
        }
    }

    /// <summary>
    /// 外部からの色のセット
    /// </summary>
    /// <param name="color"></param>
    public void SetColor( Color color)
    {
        spriteRenderer.color = color;
    }

    //スワイプ終了
    private void FinishSwipe()
    {
        if (swipeMove_shop.GetReleaseTap())
        {
            State = STATE.FRICTION;
        }
    }

}
