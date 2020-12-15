using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 配置管理クラス
/// </summary>
public class ArrangeManager : MonoBehaviour
{
    /// <summary>
    /// 1フレームあたりの時間
    /// </summary>
    [SerializeField] float OneDisplayTime = 10.0f;
    /// <summary>
    /// 配置テーブル
    /// </summary>
    [SerializeField] ArrangeTable sorceTable = default;
    private float m_Timer = 0f;
    private CameraController m_Camera = null;
    GameMainManager m_GameMainManager = null;
    [SerializeField]private List<ArrangePattern> patternList = default;

    void Start()
    {
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>().GetComponent<CameraController>();
        m_Timer = OneDisplayTime;
        m_GameMainManager = transform.root.GetComponent<GameMainManager>();
        patternList = new List<ArrangePattern>(sorceTable.ArrangeTableItemList);
    }


    void Update()
    {

        if (m_GameMainManager.state == GameMainManager.STATE.MAIN)
        {
            m_Timer += Time.deltaTime;
            if (OneDisplayTime <= m_Timer)
            {
                m_Timer = 0;
                // ランダムでテーブルからどのパターンから出るかを決める
                int index = Random.Range(0, patternList.Count);
                GameObject obj = Instantiate(patternList[index].PatternPrefab, this.transform);
                obj.transform.position = new Vector3(m_Camera.GetScreenRight() * 2.0f, 0f, 0f);
                // 一度出たパターンは出ないようにする
                patternList.RemoveAt(index);

                // 全てのパターンが出てしまったらテーブルをリセット
                if (patternList.Count == 0)
                {
                    patternList = new List<ArrangePattern>(sorceTable.ArrangeTableItemList);
                }
            }
        }
        if (m_GameMainManager.state == GameMainManager.STATE.OVER)
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
