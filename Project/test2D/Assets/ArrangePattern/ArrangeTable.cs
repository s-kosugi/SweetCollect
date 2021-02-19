using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 配置パターンリストクラス
/// </summary>
[CreateAssetMenu]
public class ArrangeTable : ScriptableObject
{
    public List<ArrangePattern> ArrangeTableItemList = new List<ArrangePattern>();
}

/// <summary>
/// 配置パターン用クラス
/// </summary>
[System.Serializable]
public class ArrangePattern
{
    [SerializeField] public GameObject PatternPrefab;
}