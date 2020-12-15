using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutrialMoveHorizontal : MonoBehaviour
{
    TutrialSceneManager m_TutrialManager = null;
    [SerializeField] protected float MoveSpeed = 80.0f;
    void Start()
    {
        m_TutrialManager = transform.root.GetComponent<TutrialSceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_TutrialManager.state == TutrialSceneManager.STATE.MAIN)
        {
            // 移動
            Vector3 pos = transform.position;
            pos.x -= MoveSpeed * Time.deltaTime;
            transform.position = pos;
        }
    }
}
