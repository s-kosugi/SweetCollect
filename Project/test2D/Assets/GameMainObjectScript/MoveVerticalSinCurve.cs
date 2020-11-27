using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVerticalSinCurve : MonoBehaviour
{
    private float Angle = 0f;
    GameMainManager m_GameMainManager = null;
    [SerializeField] float SinSpeed = 4.0f;
    [SerializeField] float SinWidth = 3.0f;

    void Start()
    {
        m_GameMainManager = transform.root.GetComponent<GameMainManager>();
        Angle = 0f;
    }

   void Update()
    {
        if (m_GameMainManager.state == GameMainManager.STATE.MAIN)
        {
            Angle += SinSpeed;
            // サインカーブで上下移動
            float MoveY = Mathf.Sin(Mathf.Deg2Rad * Angle) * SinWidth;
            Vector3 pos = transform.position;
            pos.y += MoveY;
            transform.position = pos;
        }
    }
}
