using UnityEngine;

/// <summary>
/// アイテム効果格納用
/// </summary>
public class ItemEffect : MonoBehaviour
{
    [SerializeField] protected int Score = 1;
    //[SerializeField] protected float RecoverValue = 10f;

    public int score
    { get { return Score; } }
    //public float recoverValue { get { return this.RecoverValue; } }

}
