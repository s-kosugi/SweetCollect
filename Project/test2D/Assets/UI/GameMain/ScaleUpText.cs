using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpText : MonoBehaviour
{
    [SerializeField] float StartScale = 0.0f;
    [SerializeField] float EndScale = 1.0f;
    [SerializeField] float AnimeTime = 2.0f;
    [SerializeField] float BackScale = 3.0f;
    [SerializeField] bool EndDestroy = true;
    private RectTransform m_Rect;
    private float m_Timer = 0f;

    // スライドインが終わったかどうか
    public bool isEnd { get; private set; }

    void Start()
    {
        m_Rect = GetComponent<RectTransform>();
        m_Rect.localScale = new Vector3(StartScale,StartScale);
        m_Timer = 0;
        isEnd = false;
    }


    void Update()
    {
        m_Timer += Time.deltaTime;

        if (m_Timer >= AnimeTime)
        {
            m_Rect.localScale = new Vector3(EndScale, EndScale);
            isEnd = true;

            // 削除フラグが立っていたら削除する
            if (EndDestroy) Destroy(gameObject);
        }
        else
        {
            float scale = Easing.OutBack(m_Timer, AnimeTime, EndScale, StartScale, BackScale);
            m_Rect.localScale = new Vector3(scale, scale);
        }
    }
}
