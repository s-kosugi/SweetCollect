using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeRotate : MonoBehaviour
{
    GameMainManager m_GameMainManager = null;
    [SerializeField] protected float RotateSpeed = 4.0f;
    [SerializeField] protected float MaxRotateAngle = 10.0f;
    protected float SinAngle = 0f;

    void Start()
    {
        m_GameMainManager = transform.root.GetComponent<GameMainManager>();

        SinAngle = 0f;
    }


    void Update()
    {
        if (m_GameMainManager.state == GameMainManager.STATE.MAIN)
        {
            // 回転値を指定して回転させる
            SinAngle += RotateSpeed;
            transform.eulerAngles = new Vector3(0.0f, 0.0f, Mathf.Sin(SinAngle * Mathf.Deg2Rad) * MaxRotateAngle);
        }
    }
}
