using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingAnimeSprite : MonoBehaviour
{
    [SerializeField] float AnimeSpeed = 2.0f;
    private float Angle = 0.0f;
    private SpriteRenderer spriteRenderer = default;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        // サインカーブでα値を変える(0.0～1.0)
        Angle += AnimeSpeed;
        float alpla = (Mathf.Cos(Angle*Mathf.Deg2Rad)+ 1.0f) / 2.0f;
        Color c = spriteRenderer.color;
        c = new Color(c.r, c.g, c.b, alpla);
        spriteRenderer.color = c;
    }
}
