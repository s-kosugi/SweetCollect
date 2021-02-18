using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 点滅アニメーションイメージクラス
/// </summary>
public class FlashingAnimeImage : MonoBehaviour
{
    [SerializeField] float AnimeSpeed = 2.0f;
    [SerializeField] float MinAlpha = 0.0f;
    private float angle = 0.0f;
    private Image image = default;

    private void Start()
    {
        image = GetComponent<Image>();
    }


    void FixedUpdate()
    {
        // サインカーブでα値を変える(0.0～1.0)
        angle += AnimeSpeed;
        float alpha = (Mathf.Cos(angle * Mathf.Deg2Rad) + 1.0f) / 2.0f;
        alpha += MinAlpha;
        if (alpha >= 1.0f) alpha = 1.0f;
        Color c = image.color;
        c = new Color(c.r, c.g, c.b, alpha);
        image.color = c;
    }

    /// <summary>
    /// 透過度のリセット
    /// </summary>
    public void ResetAlpha()
    {
        Color c = image.color;
        c = new Color(c.r, c.g, c.b, 1.0f);
        image.color = c;
    }
}
