using UnityEngine;
using UnityEngine.UI;

public class NonAlphaFlashImage : MonoBehaviour
{
    Image image = default;
    [SerializeField] float fadeSpeed = 20.0f;
    [SerializeField] float minColor = 0.5f;
    private float Angle = 0.0f;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void FixedUpdate()
    {
        // サインカーブでカラー値を変える(minColor～1.0)
        Angle += fadeSpeed * Time.deltaTime * 60.0f;
        float value = (Mathf.Cos(Angle * Mathf.Deg2Rad) + 1.0f) / 2.0f;
        value *= (1.0f - minColor);
        value = value + minColor;

        Color c = image.color;
        c = new Color(value, value, value, c.a);
        image.color = c;
    }
}
