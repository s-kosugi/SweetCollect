using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCredit : MonoBehaviour
{
    [SerializeField] private CreditSceneManager creditscene;        //クレジット
    [SerializeField] private RectTransform recttransform;
    [SerializeField] private CreditDistance creaditdistance;
    [SerializeField] private GameObject EndPoint;                //停止位置オブジェクト

    [SerializeField] private Vector3 StartPosition = Vector3.zero; //スタート地点
    [SerializeField] private float MoveSpee = 1.0f;               //移動速度
    [SerializeField] private bool IsFinishMove;                         //移動終了
    [SerializeField] private bool IsEsing;                         //イージング
    [SerializeField] private float MoveTimer = 0.0f;              //移動時間
    [SerializeField] private float MOVE_TIME = 5.0f;              //移動時間 

    [SerializeField] private float Test;

    private void Awake()
    {
        recttransform = this.GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        creditscene = GameObject.Find("CreditSceneManager").GetComponent<CreditSceneManager>();
        creaditdistance = this.transform.parent.Find("Size").GetComponent<CreditDistance>();
        EndPoint = this.transform.Find("EndPoint").gameObject;
        SetStartPosition();
        IsFinishMove = false;
        IsEsing = false;
        MoveTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Test =  Screen.width;
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
                    if (MoveTimer < MOVE_TIME)
                    {
                        recttransform.localPosition = new Vector3(Easing.Linear(MoveTimer, MOVE_TIME, -(creaditdistance.GetDistance() + Screen.width), StartPosition.x), 0.0f, 0.0f);
                    }
                    else
                    {
                        IsFinishMove = true;
                    }
                }
            }
            else
            {
                recttransform.localPosition += new Vector3(-MoveSpee, 0.0f, 0.0f) * Time.deltaTime;
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
