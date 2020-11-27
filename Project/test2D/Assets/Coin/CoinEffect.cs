using UnityEngine;


/// <summary>
/// コイン効果格納用
/// </summary>
public class CoinEffect : MonoBehaviour
{
    [SerializeField] protected int Score = 50;

    public int score
    { get { return Score; } }

}
