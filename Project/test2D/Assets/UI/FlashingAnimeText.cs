using TMPro;
using UnityEngine;


public class FlashingAnimeText : MonoBehaviour
{
    [SerializeField] float AnimeSpeed = 2.0f;
    [SerializeField] float MinAlpha = 0.0f;
    private TextMeshProUGUI text = default;
    private float Angle = 0.0f;

    private void Awake()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }


    void FixedUpdate()
    {
        // サインカーブでα値を変える(0.0～1.0)
        Angle += AnimeSpeed;
        float alpha = (Mathf.Cos(Angle * Mathf.Deg2Rad) + 1.0f) / 2.0f;
        alpha += MinAlpha;
        if (alpha >= 1.0f) alpha = 1.0f;

        Color c = text.color;
        c = new Color(c.r, c.g, c.b, alpha);
        text.color = c;
    }

    public void ResetAlpha()
    {
        Color c = text.color;
        c = new Color(c.r, c.g, c.b, 1.0f);
        text.color = c;
    }
}
