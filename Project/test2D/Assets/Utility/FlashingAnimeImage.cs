using UnityEngine;
using UnityEngine.UI;

public class FlashingAnimeImage : MonoBehaviour
{
    [SerializeField] float AnimeSpeed = 2.0f;
    private float Angle = 0.0f;
    private Image image = default;

    private void Start()
    {
        image = GetComponent<Image>();
    }


    void Update()
    {
        // サインカーブでα値を変える(0.0～1.0)
        Angle += AnimeSpeed;
        float alpla = (Mathf.Cos(Angle * Mathf.Deg2Rad) + 1.0f) / 2.0f;
        Color c = image.color;
        c = new Color(c.r, c.g, c.b, alpla);
        image.color = c;
    }
}
