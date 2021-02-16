using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] int BackGroundHeight = 882;
    [SerializeField] int BackGroundWidth = 1568;
    [SerializeField] float BackGroundScale = 0.3f;
    private Camera m_Camera = null;
    // Start is called before the first frame update
    void Start()
    {
        // アスペクト比(1未満=LANDSCAPE、1=正方形、1より大きい=PORTRAIT
        float gameAspect = BackGroundHeight * BackGroundScale / (BackGroundWidth * BackGroundScale);
        float screenAspect = (float)Screen.height / (float)Screen.width;
        float fixRatio;
        m_Camera = GetComponent<Camera>();

        if (gameAspect > screenAspect)
        {
            // スクリーンの左右が黒帯
            fixRatio = screenAspect / gameAspect;
            m_Camera.rect = new Rect(0.5f - fixRatio / 2f, 0f, fixRatio, 1f);
        }
        else
        {
            // スクリーンの上下が黒帯
            fixRatio = gameAspect / screenAspect;
            m_Camera.rect = new Rect(0f, 0.5f - fixRatio / 2f, 1f, fixRatio);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetScreenTop( )
    {
        // 画面の左上を取得
        Vector3 topLeft = m_Camera.ScreenToWorldPoint(Vector3.zero);
        // 上下反転させる
        topLeft.Scale(new Vector3(1f, -1f, 1f));
        return topLeft.y;
    }
    public float GetScreenLeft()
    {
        // 画面の左上を取得
        Vector3 topLeft = m_Camera.ScreenToWorldPoint(Vector3.zero);
        return topLeft.x;
    }
    public float GetScreenRight()
    {
        // 画面の右下を取得
        Vector3 bottomRight = m_Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        return bottomRight.x;
    }
    public float GetScreenBottom()
    {
        // 画面の右下を取得
        Vector3 bottomRight = m_Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        // 上下反転させる
        bottomRight.Scale(new Vector3(1f, -1f, 1f));
        return bottomRight.y;
    }
}
