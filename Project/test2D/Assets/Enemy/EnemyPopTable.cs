using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyPopTable : ScriptableObject
{
    public List<EnemyTableItem> EnemyTableItemList = new List<EnemyTableItem>();
}

[System.Serializable]
public class EnemyTableItem
{
    public enum EnemyID
    {
        ENEMY_RED,
        ENEMY_GREEN,
        ENEMY_BLUE,
        ENEMY_SYOKUDAI,
        ENEMY_CHEF,
    }
    /// <summary>
    /// 出現間隔
    /// </summary>
    public float INTERVAL = 2.0f;
    public EnemyID ID = 0;
    /// <summary>
    /// 出現経過時間(出現制御用変数)
    /// </summary>
    public float Elapsed = 0;

}