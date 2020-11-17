using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideInText : MonoBehaviour
{
    [SerializeField] float StartPositionX = -1000.0f;
    [SerializeField] float StartPositionY = 0;
    [SerializeField] float EndPositionX = 0;
    [SerializeField] float EndPositionY = 0;
    [SerializeField] float SlideTime = 2.0f;
    [SerializeField] bool EndDestroy = true;
    private RectTransform m_Rect;
    private float m_Timer = 0f;

    // スライドインが終わったかどうか
    public bool isEnd { get; private set; }

    void Start()
    {
        m_Rect = GetComponent<RectTransform>();
        m_Rect.localPosition = new Vector3(StartPositionX, StartPositionY);
        m_Timer = 0;
        isEnd = false;
    }


    void Update()
    {
        m_Timer += Time.deltaTime;

        if( m_Timer >= SlideTime)
        {
            m_Rect.localPosition = new Vector3(EndPositionX, EndPositionY);
            isEnd = true;

            // 削除フラグが立っていたら削除する
            if (EndDestroy) Destroy(gameObject);
        }
        else
        {
            float positionX = Easing.OutExp(m_Timer, SlideTime, EndPositionX, StartPositionX);
            float positionY = Easing.OutExp(m_Timer, SlideTime, EndPositionY, StartPositionY);
            m_Rect.localPosition = new Vector3(positionX, positionY);
        }
    }
}
