using UnityEngine;

/// <summary>
/// ショップ中のジャンプアニメーション
/// </summary>
public class ShopJumpAnime : MonoBehaviour
{
    Rigidbody2D rigidBody2D = default;
    [SerializeField] float jumpPower = 7000;
    [SerializeField] float jumpInterval = 1.3f;
    float JumpCounter = 0f;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        JumpCounter += Time.deltaTime;
        if ( JumpCounter >= jumpInterval)
        {
            Vector2 v = new Vector2(0.0f, jumpPower);
            rigidBody2D.AddForce(v);

            JumpCounter = 0f;
        }
    }
}
