using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SweetsPopTable : ScriptableObject
{
    public List<SweetsTableItem> SweetsTableItemList = new List<SweetsTableItem>();
}

[System.Serializable]
public class SweetsTableItem
{
    public enum SweetsID
    {
        CANDY,
        PUDDING,
    }
    /// <summary>
    /// 出現間隔
    /// </summary>
    public float INTERVAL = 1.0f;
    public SweetsID ID = 0;
    /// <summary>
    /// 出現経過時間(出現制御用変数)
    /// </summary>
    public float Elapsed = 0;

}