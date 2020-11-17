using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private CameraController m_Camera = null;
    private GameMainManager m_GameMain = null;
    static float SPRITE_SIZE = 32.0f;

    private EnemyPopTable m_Table = null;

    [SerializeField] GameObject EnemyRed = null;
    [SerializeField] GameObject EnemyGreen = null;
    [SerializeField] GameObject EnemyBlue = null;


    void Start()
    {
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>().GetComponent<CameraController>();
        m_GameMain = GameObject.Find("GameManager").GetComponent<GameMainManager>();

        // 敵出現テーブルの0を読み込む
        LoadTable(0);
    }


    void Update()
    {
        // ゲームプレイ中のみ敵を出現させる
        if (m_GameMain.state == GameMainManager.STATE.MAIN)
        {
            // 出現テーブルから敵を出現させる
            foreach (EnemyTableItem item in m_Table.EnemyTableItemList)
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
                    GameObject popEnemy = null;

                    switch (item.ID)
                    {
                        case EnemyTableItem.EnemyID.ENEMY_RED: popEnemy = EnemyRed; break;
                        case EnemyTableItem.EnemyID.ENEMY_GREEN: popEnemy = EnemyGreen; break;
                        case EnemyTableItem.EnemyID.ENEMY_BLUE: popEnemy = EnemyBlue; break;
                        default: return;
                    }
                    // ゲームメインを渡す
                    popEnemy.GetComponent<EnemyBase>().m_GameMainManager = m_GameMain;
                    Instantiate(popEnemy, pos, Quaternion.identity, transform);
                }
            }
        }
    }

    private void LoadTable(int TableNo)
    {
        string fileName = "Enemy\\EnemyPopTable_" + TableNo;
        m_Table = Resources.Load<EnemyPopTable>(fileName);

    }
}
