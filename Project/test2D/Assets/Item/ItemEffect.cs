using UnityEngine;

/// <summary>
/// アイテム効果格納用
/// </summary>
public class ItemEffect : MonoBehaviour
{
    [SerializeField] protected int Score = 1;
    private GameMainManager gameMainManager = default;

    private void Start()
    {
        gameMainManager = GameObject.Find("GameManager").GetComponent<GameMainManager>();
    }

    public int score
    { get { return (int)(Score * gameMainManager.CoinGetRate); } }

}
