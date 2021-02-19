using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スコア減算オブジェクトクラス
/// </summary>
public class MinusUIObject : MonoBehaviour
{
    [SerializeField] float UpSpeed = 300.0f;
    [SerializeField] float LifeTime = 1.5f;
    [SerializeField] Image image = default;
    private float lifeCount = 0;
    TextMeshProUGUI textMesh = default;
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        Vector3 temp = transform.position;
        temp.y += UpSpeed * Time.deltaTime;
        transform.position = temp;

        lifeCount += Time.deltaTime;

        // だんだん色を薄くする
        Color tempColor = textMesh.color;
        tempColor.a = Easing.OutSine(lifeCount, LifeTime, 0.0f, 1.0f);
        textMesh.color = tempColor;

        tempColor = image.color;
        tempColor.a = Easing.OutSine(lifeCount, LifeTime, 0.0f, 1.0f);
        image.color = tempColor;

        if (lifeCount >= LifeTime)
        {
            Destroy(this.gameObject);
        }
    }
}
