using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwipeMove_Shop : MonoBehaviour
{
    [SerializeField] private Clothing               clothing = null;                //服
    [SerializeField] private ClothingBuyAndWear     buyandwearbutton = null;        //着用購入ボタン
    [SerializeField] private CurtainAnime           curtainanime = null;            //カーテン

    float                                           HorizontalRate = 1.0f;          //水平方向レート
    [SerializeField] float                          HorizontalRateAndroid = 0.5f;   //アンドロイド版水平方向レート
    [SerializeField] float                          VerticalRate = 1.0f;            //垂直方向レート
    float                                           Friction = 0.9f;                //摩擦力
    [SerializeField] float                          FrictionAndroid = 0.88f;        //アンドロイド版摩擦力
    [SerializeField] float                          StopThreshold = 0.01f;          //停止速度
    [SerializeField] public Vector2                 MinmoveLimitRect = default;     //最小範囲
    [SerializeField] public Vector2                 MaxmoveLimitRect = default;     //最大範囲

    public bool                                     TouchFlag { get; private set; } //タッチフラグ
    private bool                                    PrevtouchFlag = false;          //一つ前フレームのタッチフラグ
    private Vector3                                 OldTouchPos = Vector3.zero;     //過去位置
    [SerializeField] private Vector3                InertiaMove = Vector3.zero;     //移動量

    [SerializeField] private float                  MaxWidth = 0;                   //最大移動値

    //移動距離
    [SerializeField] private Vector3                TouchPos = Vector3.zero;        //押された瞬間の位置
    [SerializeField] private float                  MinMoveDistance = 1.0f;         //最小移動距離
    //画面
    [SerializeField] private int                    HalfScreenSizeHeight = 0;       //高さの半分サイズ
    [SerializeField] private int                    ScreenSizeHeight = 0;           //高さのサイズ

    private void Start()
    {
        ScreenSizeHeight = Screen.height;
        HalfScreenSizeHeight = ScreenSizeHeight / 2;

#if UNITY_WEBGL    
        this.enabled = false;
#endif
#if UNITY_ANDROID
        this.enabled = true;
        HorizontalRate = HorizontalRateAndroid;
        Friction = FrictionAndroid;
#endif
    }

    void Update()
    {
        if (clothing.GetState() == Clothing.SHELFSTATE.PREVIEW
            && buyandwearbutton.GetState() == ClothingBuyAndWear.STATE.RECEPTION && curtainanime.state == CurtainAnime.STATE.WAIT)
        {
            if (Input.GetMouseButton(0))
            {  
                // 押された瞬間だった場合は旧座標を保存する
                if (!TouchFlag)
                {
                    OldTouchPos = Input.mousePosition;
                    TouchPos = OldTouchPos;
                    TouchFlag = true;
                }

                //移動量が一定値に達しているなら
                if (CheckDistance())
                {
                    Vector3 velocity = Input.mousePosition - OldTouchPos;

                    // 上下左右それぞれの移動倍率をかける
                    velocity = new Vector3(velocity.x * HorizontalRate, velocity.y * VerticalRate, velocity.z);
                    // 慣性移動用の値を保存
                    InertiaMove = velocity;

                    // オブジェクトを移動させる
                    MoveObject(velocity);
                }
                // 移動が終わったので旧座標を保存する
                OldTouchPos = Input.mousePosition;
            }
            else
            {
                TouchFlag = false;


                // 摩擦率をかける
                InertiaMove *= Friction;

                // スピードが遅くなったら完全停止させる
                if (Mathf.Abs(InertiaMove.x) <= StopThreshold && Mathf.Abs(InertiaMove.y) <= StopThreshold)
                {
                    InertiaMove = Vector3.zero;
                }

                // 摩擦計算込みの移動をする
                MoveObject(InertiaMove);

            }
        }

    }

    /// <summary>
    /// オブジェクトを移動させて移動限界にきたら強制的に座標をもどす。
    /// </summary>
    /// <param name="velocity"></param>
    private void MoveObject(Vector3 velocity)
    {
        Vector3 checkPos = gameObject.transform.localPosition + velocity;

        // 左側チェック
        if (checkPos.x < MinmoveLimitRect.x)
        {
            checkPos.x = MinmoveLimitRect.x;
            InertiaMove = Vector3.zero;
        }
        // 右側チェック
        if (checkPos.x > MaxmoveLimitRect.x)
        {
            checkPos.x = MaxmoveLimitRect.x;
            InertiaMove = Vector3.zero;
        }
        // 上側チェック
        if (checkPos.y < MinmoveLimitRect.y)
        {
            checkPos.y = MinmoveLimitRect.y;
        }
        // 下側チェック
        if (checkPos.y > MaxmoveLimitRect.y)
        {
            checkPos.y = MaxmoveLimitRect.y;
        }
        // 移動させる
        gameObject.transform.localPosition = checkPos;
    }


    //最大値の計算
    //size : 画像のサイズ
    //margin : 余白
    //maxspritenum : 最大スプライト数
    public void MaxMoveCalculation(float size, float margin, int maxspritenum)
    {
        MaxWidth = (size * (maxspritenum)) + (margin * maxspritenum);
        MinmoveLimitRect.x = -MaxWidth;
    }

    //離した瞬間
    public bool GetReleaseTap()
    {
        bool release = false;

        if (!TouchFlag && PrevtouchFlag)
            release = true;
        else
            release = false;

        //1フレーム前の状態を保存
        PrevtouchFlag = TouchFlag;

        return release;
    }

    //現在の移動速度
    public float GetInertiaMove()
    {
        return InertiaMove.magnitude;
    }
    //移動最低速度
    public float GetStopThreshold()
    {
        return StopThreshold;
    }

    //移動距離が一定以上か
    public bool CheckDistance()
    {
        float distance = Mathf.Abs(Vector3.Distance(OldTouchPos, TouchPos));

        if (distance > MinMoveDistance)
            return true;
        else
            return false;
    }
}
