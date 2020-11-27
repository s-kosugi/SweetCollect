using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private CameraController m_Camera = null;
    private GameMainManager m_GameMain = null;
    static float SPRITE_SIZE = 32.0f;

    private CoinPopTable m_Table = null;

    [SerializeField] GameObject CoinNormal = null;
    [SerializeField] GameObject CoinRed = null;


    void Start()
    {
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>().GetComponent<CameraController>();
        m_GameMain = GameObject.Find("GameManager").GetComponent<GameMainManager>();

        // コイン出現テーブルの0番を読み込む
        LoadTable(0);
    }


    void Update()
    {
        // ゲームプレイ中のみコインを出現させる
        if (m_GameMain.state == GameMainManager.STATE.MAIN)
        {
            // 出現テーブルからコインを出現させる
            foreach (CoinTableItem item in m_Table.CoinTableItemList)
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
                    GameObject popCoin = null;

                    switch (item.ID)
                    {
                        case CoinTableItem.CoinID.COIN_NORMAL: popCoin = CoinNormal; break;
                        case CoinTableItem.CoinID.COIN_RED: popCoin = CoinRed; break;
                        default: return;
                    }

                    Instantiate(popCoin, pos, Quaternion.identity, transform);
                }
            }
        }
    }

    private void LoadTable(int TableNo)
    {
        string fileName = "Coin\\CoinPopTable_" + TableNo;
        m_Table = Resources.Load<CoinPopTable>(fileName);

    }
}
