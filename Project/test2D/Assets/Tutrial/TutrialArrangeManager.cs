using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 配置管理クラス
/// </summary>
public class TutrialArrangeManager : MonoBehaviour
{
    /// <summary>
    /// 1フレームあたりの時間
    /// </summary>
    [SerializeField] float OneDisplayTime = 10.0f;
    /// <summary>
    /// 配置テーブル
    /// </summary>
    private float m_Timer = 0f;
    private CameraController m_Camera = null;
    TutrialSceneManager m_TutrialManager = null;
    [SerializeField] GameObject Tutrial_01 = null;
    [SerializeField] GameObject Tutrial_02 = null;
    [SerializeField] GameObject Tutrial_03 = null;
    [SerializeField] GameObject Tutrial_04 = null;
    void Start()
    {
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>().GetComponent<CameraController>();
        m_Timer = OneDisplayTime;
        m_TutrialManager = transform.root.GetComponent<TutrialSceneManager>();
    }


    void Update()
    {

        if (m_TutrialManager.state == TutrialSceneManager.STATE.MAIN && m_TutrialManager.tutrial != TutrialSceneManager.TUTRIAL.TUTRIAL_DESCRIPTION)
        {
            m_Timer += Time.deltaTime;
            if (OneDisplayTime <= m_Timer)
            {
                m_Timer = 0;
                GameObject obj = null;
                // ランダムでテーブルからどのパターンから出るかを決める
                if (m_TutrialManager.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_JUMP)
                {
                    obj = Instantiate(Tutrial_01, this.transform);
                }
                else if (m_TutrialManager.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_DOUBLEJUMP)
                {
                    obj = Instantiate(Tutrial_02, this.transform);
                }
                else if (m_TutrialManager.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_SYOKUDAI)
                {
                    obj = Instantiate(Tutrial_03, this.transform);
                }
                else if (m_TutrialManager.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_CHEF)
                {
                    obj = Instantiate(Tutrial_04, this.transform);
                }

                if(obj)
                    obj.transform.position = new Vector3(m_Camera.GetScreenRight() * 2.0f, 0f, 0f);
            }
        }
        if (m_TutrialManager.state == TutrialSceneManager.STATE.FADEOUT)
        {
            // すべての子オブジェクトを削除
            foreach (Transform n in gameObject.transform)
            {
                GameObject.Destroy(n.gameObject);
            }

            // 次にすぐ出現するためにタイマーを満たしておく
            m_Timer = OneDisplayTime;
        }
    }
}
