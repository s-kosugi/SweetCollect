using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinusUIObject : MonoBehaviour
{
    [SerializeField] float UpSpeed = 5.0f;
    [SerializeField] float LifeTime = 1.5f;
    [SerializeField] Image image = default;
    private float LifeCount = 0;
    TextMeshProUGUI Text = default;
    void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        Vector3 temp = transform.position;
        temp.y += UpSpeed;
        transform.position = temp;

        LifeCount += Time.deltaTime;

        // だんだん色を薄くする
        Color tempColor = Text.color;
        tempColor.a = Easing.OutSine(LifeCount, LifeTime, 0.0f, 1.0f);
        Text.color = tempColor;

        tempColor = image.color;
        tempColor.a = Easing.OutSine(LifeCount, LifeTime, 0.0f, 1.0f);
        image.color = tempColor;

        if (LifeCount >= LifeTime)
        {
            Destroy(this.gameObject);
        }
    }
}
