using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ArrangeTable : ScriptableObject
{
    public List<ArrangePattern> ArrangeTableItemList = new List<ArrangePattern>();
}

[System.Serializable]
public class ArrangePattern
{
    [SerializeField] public GameObject PatternPrefab;
}