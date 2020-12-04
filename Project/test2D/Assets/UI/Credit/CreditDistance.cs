using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditDistance : MonoBehaviour
{
    [SerializeField] GameObject StartPosition;  //スタートオブジェクト
    [SerializeField] GameObject EndPosition;    //ゴールオブジェクト


    [SerializeField] private float Distance = 0.0f;                   //スタートからゴールまでの距離
    [SerializeField] public bool IsConfirmed { get; private set; }    //確認済み

    private void Awake()
    {
        StartPosition = this.transform.Find("StartPosition").gameObject;
        EndPosition = this.transform.Find("EndPosition").gameObject;

    }

    // Start is called before the first frame update
    void Start()
    {
        Distance = 0.0f;
        IsConfirmed = false;

        CheckDistance();
    }

    //距離の確認
    private void CheckDistance()
    {
        Distance = EndPosition.transform.localPosition.x - StartPosition.transform.localPosition.x;
        IsConfirmed = true;
    }

    //距離の取得
    public float GetDistance()
    {
        if (IsConfirmed)
            return Distance;
        else
            return 0.0f;
    }
}
