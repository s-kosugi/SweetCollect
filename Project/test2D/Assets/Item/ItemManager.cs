using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private CameraController m_Camera = null;
    private GameMainManager m_GameMain = null;
    static float SPRITE_SIZE = 32.0f;

    private SweetsPopTable m_Table = null;

    [SerializeField] GameObject ItemCandy = null;
    [SerializeField] GameObject ItemPudding = null;

    void Start()
    {
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>().GetComponent<CameraController>();
        m_GameMain = GameObject.Find("GameManager").GetComponent<GameMainManager>();

        // 出現テーブルの0を読み込む
        LoadTable(0);
    }

    // Update is called once per frame
    void Update()
    {
        // ゲームプレイ中のみ敵を出現させる
        if (m_GameMain.state == GameMainManager.STATE.MAIN)
        {
            // 出現テーブルからアイテムを出現させる
            foreach (SweetsTableItem item in m_Table.SweetsTableItemList)
            {
                item.Elapsed += Time.deltaTime;
                if (item.INTERVAL <= item.Elapsed)
                {
                    item.Elapsed = 0;
                    // 座標をランダムに決める
                    Vector3 pos;
                    pos.x = m_Camera.GetScreenRight() + SPRITE_SIZE / 2.0f;
                    pos.y = Random.Range(m_Camera.GetScreenTop(), m_Camera.GetScreenBottom());
                    pos.z = 0.0f;
                    GameObject popItem = null;
                    switch (item.ID)
                    {
                        case SweetsTableItem.SweetsID.CANDY: popItem = ItemCandy; break;
                        case SweetsTableItem.SweetsID.PUDDING: popItem = ItemPudding; break;
                        default: return;
                    }
                    // リソースからロード
                    Instantiate(popItem, pos, Quaternion.identity, transform);
                }
            }
        }
    }

    private void LoadTable(int TableNo)
    {
        string fileName = "Item\\SweetsPopTable_" + TableNo;
        m_Table = Resources.Load<SweetsPopTable>(fileName);

    }
}
