using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePudding : MonoBehaviour
{
    GameMainManager m_GameMainManager = null;
    private Rigidbody2D rb = null;

    void Start()
    {
        m_GameMainManager = transform.root.GetComponent<GameMainManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (m_GameMainManager.state != GameMainManager.STATE.MAIN)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }
}
