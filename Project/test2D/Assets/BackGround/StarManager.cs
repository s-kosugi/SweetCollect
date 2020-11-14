using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class StarManager : MonoBehaviour
{
    private CameraController m_Camera = null;
    EffekseerEffectAsset m_StarEffect = null;
    [SerializeField] float CreateInterval = 1.0f;
    float m_CreateTimer = 0f;

    void Start()
    {
        m_StarEffect = Resources.Load<EffekseerEffectAsset>("Effect\\star");
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>().GetComponent<CameraController>();
        m_CreateTimer = 0f;
    }


    void Update()
    {
        m_CreateTimer += Time.deltaTime;
        if (m_CreateTimer >= CreateInterval)
        {
            m_CreateTimer = 0f;
            float PosX = Random.Range(m_Camera.GetScreenLeft(), m_Camera.GetScreenRight());
            float PosY = Random.Range(m_Camera.GetScreenTop(), m_Camera.GetScreenBottom() / 4.0f);
            // エフェクトの取得
            EffekseerSystem.PlayEffect(m_StarEffect, new Vector3(PosX, PosY));
        }
    }
}
