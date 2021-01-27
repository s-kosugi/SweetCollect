using UnityEngine;
using UnityEngine.UI;

public class OneJumpFinger : MonoBehaviour
{
    Image fingerImage = default;
    [SerializeField] float StartScale = 3.0f;
    [SerializeField] float AnimationTime = 2.0f;
    [SerializeField] TutrialSceneManager tutrial = default;
    float animationCounter = 0f;

    void Start()
    {
        fingerImage = GetComponent<Image>();
        fingerImage.color = new Color(255f, 255f, 255f, 0f);
        fingerImage.transform.localScale = new Vector3(StartScale, StartScale);
    }


    void Update()
    {
        if (tutrial.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_JUMP)
        {

            animationCounter += Time.deltaTime;
            if (animationCounter >= AnimationTime)
            {
                animationCounter = 0f;
            }

            float scale = Easing.OutQuint(animationCounter, AnimationTime, 1f, StartScale);
            fingerImage.transform.localScale = new Vector3(scale, scale);
            float alpha = Easing.OutQuint(animationCounter, AnimationTime, 1f, 0f);
            fingerImage.color = new Color(1f, 1f, 1f, alpha);
        }
    }
}
