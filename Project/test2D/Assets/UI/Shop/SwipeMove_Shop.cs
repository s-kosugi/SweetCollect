using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwipeMove_Shop : MonoBehaviour
{
    [SerializeField] private Clothing clothing = null;
    [SerializeField] private BuyAndWearButton BuyAndWearButton = null;          //着用購入ボタン
    [SerializeField] private CurtainAnime curtainAnime = null;                  //カーテン

    [SerializeField] float horizontalRate = 1.0f;
    [SerializeField] float verticalRate = 1.0f;
    [SerializeField] float friction = 0.9f;
    [SerializeField] float stopThreshold = 0.01f;
    [SerializeField] public Vector2 MinmoveLimitRect = default;
    [SerializeField] public Vector2 MaxmoveLimitRect = default;

    public bool touchFlag { get; private set; }
    private bool PrevtouchFlag = false;
    private Vector3 oldTouchPos = Vector3.zero;
    [SerializeField] private Vector3 inertiaMove = Vector3.zero;

    //移動したい値の最大値関連
    [SerializeField] private float Max_Wight = 0;

    //移動距離
    [SerializeField] private Vector3 TouchPos = Vector3.zero; //押された瞬間の位置
    [SerializeField] private float MinMoveDistance = 1.0f;   　//最小移動距離
    //画面
    [SerializeField] private int HalfScreenSizeHeight = 0;  //高さのサイズ()
    [SerializeField] private int ScreenSizeHeight = 0;  //高さのサイズ
    [SerializeField] private float Percentage = 0.0f;       //割合

    private void Start()
    {
        ScreenSizeHeight = Screen.height;
        HalfScreenSizeHeight = ScreenSizeHeight / 2;
    }

    void Update()
    {
        if (clothing.GetState() == Clothing.SHELFSTATE.PREVIEW && BuyAndWearButton.GetState() == BuyAndWearButton.STATE.RECEPTION && curtainAnime.state == CurtainAnime.STATE.WAIT)
        {
            if (Input.GetMouseButton(0))
            {  
                // 押された瞬間だった場合は旧座標を保存する
                if (!touchFlag)
                {
                    oldTouchPos = Input.mousePosition;
                    TouchPos = oldTouchPos;
                    touchFlag = true;
                }

                if (CheckDistance())
                {
                    Vector3 velocity = Input.mousePosition - oldTouchPos;

                    // 上下左右それぞれの移動倍率をかける
                    velocity = new Vector3(velocity.x * horizontalRate, velocity.y * verticalRate, velocity.z);
                    // 慣性移動用の値を保存
                    inertiaMove = velocity;

                    // オブジェクトを移動させる
                    MoveObject(velocity);
                }
                // 移動が終わったので旧座標を保存する
                oldTouchPos = Input.mousePosition;
            }
            else
            {
                touchFlag = false;


                // 摩擦率をかける
                inertiaMove *= friction;

                // スピードが遅くなったら完全停止させる
                if (Mathf.Abs(inertiaMove.x) <= stopThreshold && Mathf.Abs(inertiaMove.y) <= stopThreshold)
                {
                    inertiaMove = Vector3.zero;
                }

                // 摩擦計算込みの移動をする
                MoveObject(inertiaMove);

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
            inertiaMove = Vector3.zero;
        }
        // 右側チェック
        if (checkPos.x > MaxmoveLimitRect.x)
        {
            checkPos.x = MaxmoveLimitRect.x;
            inertiaMove = Vector3.zero;
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
        Max_Wight = (size * (maxspritenum)) + (margin * maxspritenum);
        MinmoveLimitRect.x = -Max_Wight;
    }

    //離した瞬間
    public bool GetReleaseTap()
    {
        bool release = false;

        if (!touchFlag && PrevtouchFlag)
            release = true;
        else
            release = false;

        //1フレーム前の状態を保存
        PrevtouchFlag = touchFlag;

        return release;
    }

    //現在の移動速度
    public float GetInertiaMove()
    {
        return inertiaMove.magnitude;
    }
    //移動最低速度
    public float GetStopThreshold()
    {
        return stopThreshold;
    }

    //移動距離が一定以上か
    public bool CheckDistance()
    {
        float distance = Mathf.Abs(Vector3.Distance(oldTouchPos, TouchPos));

        if (distance > MinMoveDistance)
            return true;
        else
            return false;
    }
}
