using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpScaleFadeOutText : MonoBehaviour
{
    [SerializeField] float EndScale = 3.0f;
    [SerializeField] float FadeOutTime = 2.0f;
    [SerializeField] TextMeshProUGUI sourceText = default;
    private float FadeOutCounter = 0f;
    TextMeshProUGUI text = default;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        text.transform.localScale = new Vector3(1.0f, 1.0f);
        text.text = sourceText.text;
    }


    void Update()
    {
        FadeOutCounter += Time.deltaTime;
        if (FadeOutCounter > FadeOutTime)
        {
            this.gameObject.SetActive(false);
            return;
        }
        // 徐々に大きくする
        float scale = Easing.Linear(FadeOutCounter, FadeOutTime, EndScale, 1.0f);
        text.transform.localScale = new Vector3(scale, scale);

        // 徐々に薄くする
        Color color = text.color;
        color.a = Easing.Linear(FadeOutCounter, FadeOutTime, 0f, 1f);
        text.color = color;
    }
}
