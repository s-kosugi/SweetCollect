using UnityEngine;

public class ItemPudding : ItemBase
{
    [SerializeField] int SCORE = 3;
    [SerializeField] float MOVESPEED = 2.0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        Score = SCORE;
        MoveSpeed = MOVESPEED;
    }

    protected override void Update()
    {

        base.Update();
    }

}