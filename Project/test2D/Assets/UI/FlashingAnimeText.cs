using TMPro;
using UnityEngine;


public class FlashingAnimeText : MonoBehaviour
{
    [SerializeField] float AnimeSpeed = 2.0f;
    private TextMeshProUGUI text = default;
    private float Angle = 0.0f;

    private void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        // サインカーブでα値を変える(0.0～1.0)
        Angle += AnimeSpeed;
        float alpla = (Mathf.Cos(Angle * Mathf.Deg2Rad) + 1.0f) / 2.0f;
        Color c = text.color;
        c = new Color(c.r, c.g, c.b, alpla);
        text.color = c;
    }
}
