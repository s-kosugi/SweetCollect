using UnityEngine;
using TMPro;

/// <summary>
/// 拡大+フェードアウトテキストクラス
/// </summary>
public class UpScaleFadeOutText : MonoBehaviour
{
    [SerializeField] float EndScale = 3.0f;
    [SerializeField] float FadeOutTime = 2.0f;
    [SerializeField] TextMeshProUGUI sourceText = default;
    private float fadeOutCount = 0f;
    TextMeshProUGUI textMesh = default;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();

        textMesh.transform.localScale = new Vector3(1.0f, 1.0f);
        textMesh.text = sourceText.text;
    }


    void Update()
    {
        fadeOutCount += Time.deltaTime;
        if (fadeOutCount > FadeOutTime)
        {
            // フェードアウトが終わったら非表示にしておく
            this.gameObject.SetActive(false);
            return;
        }
        // 徐々に大きくする
        float scale = Easing.Linear(fadeOutCount, FadeOutTime, EndScale, 1.0f);
        textMesh.transform.localScale = new Vector3(scale, scale);

        // 徐々に薄くする
        Color color = textMesh.color;
        color.a = Easing.Linear(fadeOutCount, FadeOutTime, 0f, 1f);
        textMesh.color = color;
    }
}
