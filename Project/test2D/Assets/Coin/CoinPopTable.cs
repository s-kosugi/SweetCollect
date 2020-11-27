using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CoinPopTable : ScriptableObject
{
    public List<CoinTableItem> CoinTableItemList = new List<CoinTableItem>();
}

[System.Serializable]
public class CoinTableItem
{
    public enum CoinID
    {
        COIN_NORMAL,
        COIN_RED,
    }
    /// <summary>
    /// 出現間隔
    /// </summary>
    public float INTERVAL = 2.0f;
    public CoinID ID = 0;
    /// <summary>
    /// 出現経過時間(出現制御用変数)
    /// </summary>
    public float Elapsed = 0;

}