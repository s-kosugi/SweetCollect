using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 拡大しながらのフェードアウトイメージクラス
/// </summary>
public class ImageScaleUpFadeOut : MonoBehaviour
{
    Image image = default;
    [SerializeField] float MaxScale = 2.0f;
    [SerializeField] float AnimationTime = 1.5f;
    private float AnimationCounter = 0f;
    private float sorceScale = default;

    void Start()
    {
        image = GetComponent<Image>();
        sorceScale = image.transform.localScale.x;
    }
    

    void Update()
    {
        AnimationCounter += Time.deltaTime;
        if (AnimationCounter >= AnimationTime)
        {
            AnimationCounter = 0f;
        }
        float scale = Easing.Linear(AnimationCounter, AnimationTime, MaxScale * sorceScale, sorceScale);
        image.transform.localScale = new Vector3(scale, scale, scale);
        float alpha = Easing.Linear(AnimationCounter, AnimationTime, 0.0f, 1.0f);
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }
}
