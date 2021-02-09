using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingAnimeSprite : MonoBehaviour
{
    [SerializeField] float AnimeSpeed = 2.0f;
    [SerializeField] float MinAlpha = 0.0f;
    private float Angle = 0.0f;
    private SpriteRenderer spriteRenderer = default;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void FixedUpdate()
    {
        // サインカーブでα値を変える(0.0～1.0)
        Angle += AnimeSpeed;
        float alpha = (Mathf.Cos(Angle * Mathf.Deg2Rad) + 1.0f) / 2.0f;
        alpha += MinAlpha;
        if (alpha >= 1.0f) alpha = 1.0f;
        Color c = spriteRenderer.color;
        c = new Color(c.r, c.g, c.b, alpha);
        spriteRenderer.color = c;
    }

    public void ResetAlpha()
    {
        Color c = spriteRenderer.color;
        c = new Color(c.r, c.g, c.b, 1.0f);
        spriteRenderer.color = c;
    }
}
