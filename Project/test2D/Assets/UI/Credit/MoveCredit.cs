using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCredit : MonoBehaviour
{
    [SerializeField] private CreditSceneManager creditscene = default;            //クレジットシーンマネージャー
    [SerializeField] private RectTransform recttransform = default;               //描画範囲トランスフォーム
    [SerializeField] private CreditDistance creaditdistance = default;            //距離関係
    [SerializeField] private GameObject EndPoint = default;                       //停止位置オブジェクト

    private Vector3 StartPosition = Vector3.zero;      //スタート地点
    [SerializeField] private float MoveSpeed = 150.0f;                  //移動速度
    private bool IsFinishMove;                         //移動終了
    [SerializeField] private bool IsEsing;                              //イージング
    private float MoveTimer = 0.0f;                    //移動時間
    [SerializeField] private float MOVETIME = 5.0f;                     //移動時間 

    private void Awake()
    {
        recttransform = this.GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SetStartPosition();
        IsFinishMove = false;
        IsEsing = false;
        MoveTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    //自分の開始位置の設定
    private void SetStartPosition()
    {
        recttransform.localPosition = new Vector3(400f, 0.0f, 0.0f);
        StartPosition = recttransform.localPosition;
    }

    //移動
    private void Movement()
    {
        if (CheckMove())
        {
            if(IsEsing)
            {
                if (creaditdistance.IsConfirmed && !IsFinishMove)
                {
                    MoveTimer += Time.deltaTime;
                    if (MoveTimer < MOVETIME)
                    {
                        recttransform.localPosition = new Vector3(Easing.Linear(MoveTimer, MOVETIME, -(creaditdistance.GetDistance() + Screen.width), StartPosition.x), 0.0f, 0.0f);
                    }
                    else
                    {
                        IsFinishMove = true;
                    }
                }
            }
            else
            {
                recttransform.localPosition += new Vector3(-MoveSpeed, 0.0f, 0.0f) * Time.deltaTime;
            }
        }
        if (EndPoint.transform.position.x < 0.0f)
        {
            creditscene.Push_CreditButton();
        }
    }

    //移動開始
    private bool CheckMove()
    {
        if(creditscene.GetState() == CreditSceneManager.STATE.MAIN )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
