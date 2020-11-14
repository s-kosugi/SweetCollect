using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRed : EnemyBase
{
    private float Angle = 0f;
    [SerializeField] float SinSpeed = 4.0f;
    [SerializeField] float SinWidth = 3.0f;
    // Start is called before the first frame update
    protected override void Start()
    {
        Angle = 0f;

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        Angle += SinSpeed;
        // とりあえずサインカーブで動かす
        float MoveY = Mathf.Sin(Mathf.Deg2Rad * Angle) * SinWidth;
        Vector3 pos = transform.position;
        pos.y += MoveY;
        transform.position = pos;

        base.Update();
    }
}
