using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHorizontal : MonoBehaviour
{
    GameMainManager m_GameMainManager = null;
    [SerializeField] protected float MoveSpeed = 80.0f;


    void Start()
    {
        m_GameMainManager = transform.root.GetComponent<GameMainManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GameMainManager.state == GameMainManager.STATE.MAIN)
        {
            // 移動
            Vector3 pos = transform.position;
            pos.x -= MoveSpeed * Time.deltaTime;
            transform.position = pos;
        }
    }
}
