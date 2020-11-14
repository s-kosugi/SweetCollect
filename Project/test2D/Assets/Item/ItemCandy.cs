using UnityEngine;

public class ItemCandy : ItemBase
{
    [SerializeField] int SCORE = 2;
    // Start is called before the first frame update
    protected override void Start()
    {
        Score = SCORE;
    }
}
