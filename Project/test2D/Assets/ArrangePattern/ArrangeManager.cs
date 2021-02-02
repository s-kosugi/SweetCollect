using PlayFab.ClientModels;
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
    [SerializeField] ArrangeTable easyTable = default;
    [SerializeField] ArrangeTable normalTable = default;
    [SerializeField] ArrangeTable hardTable = default;
    [SerializeField] ArrangeTable veryhardTable = default;
    private float m_Timer = 0f;
    private CameraController m_Camera = null;
    GameMainManager m_GameMainManager = null;
    [SerializeField]private List<ArrangePattern> patternList = default;
    [SerializeField]private PlayFabPlayerData playerData = default;
    //固定配置位置
    private float AddCreatePorision = 590.0f;     //親からのローカル位置

    void Start()
    {
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>().GetComponent<CameraController>();
        m_Timer = OneDisplayTime;
        m_GameMainManager = transform.root.GetComponent<GameMainManager>();
        // とりあえずイージーをいれておく
        patternList = new List<ArrangePattern>(easyTable.ArrangeTableItemList);
    }

    /// <summary>
    /// 難易度での配置変更
    /// </summary>
    public void ChangeDifficultPattern()
    {
        if( playerData.m_isGet)
        {
            UserDataRecord record = default;
            if (playerData.m_Data.TryGetValue(PlayerDataName.SELECTED_DIFFICULT, out record))
            {
                switch (record.Value)
                {
                    case DifficultName.EASY: patternList = new List<ArrangePattern>(easyTable.ArrangeTableItemList); break;
                    case DifficultName.NORMAL: patternList = new List<ArrangePattern>(normalTable.ArrangeTableItemList); break;
                    case DifficultName.HARD: patternList = new List<ArrangePattern>(hardTable.ArrangeTableItemList); break;
                    case DifficultName.VERYHARD: patternList = new List<ArrangePattern>(veryhardTable.ArrangeTableItemList); break;
                }
            }
        }
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
                obj.transform.position = new Vector3(this.transform.position.x + AddCreatePorision, 0f, 0f);
                // 一度出たパターンは出ないようにする
                patternList.RemoveAt(index);

                // 全てのパターンが出てしまったらテーブルをリセット
                if (patternList.Count == 0)
                {
                    ChangeDifficultPattern();
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
