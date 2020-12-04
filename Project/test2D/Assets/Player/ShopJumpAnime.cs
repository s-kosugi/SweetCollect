using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopJumpAnime : MonoBehaviour
{
    Rigidbody2D rigidbody2 = default;
    [SerializeField] float JumpPower = 5000;
    [SerializeField] float JumpInterval = 1.0f;
    float JumpCounter = 0f;
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        JumpCounter += Time.deltaTime;
        if ( JumpCounter >= JumpInterval)
        {
            Vector2 v = new Vector2(0.0f, JumpPower);
            rigidbody2.AddForce(v);

            JumpCounter = 0f;
        }
    }


}
