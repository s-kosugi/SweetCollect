using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinusTime : MonoBehaviour
{
    [SerializeField] float UpSpeed = 5.0f;
    [SerializeField] float LifeTime = 1.5f;
    private float LifeCount = 0;
    TextMeshProUGUI Text = default;
    void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
        Text.text = "-5";
    }

    void Update()
    {
        Vector3 temp = transform.position;
        temp.y += UpSpeed;
        transform.position = temp;

        LifeCount += Time.deltaTime;
        if (LifeCount >= LifeTime)
        {
            Destroy(this.gameObject);
        }
    }
}
