using UnityEngine;
using UnityEngine.UI;

public class BatumageController : MonoBehaviour
{
    Image BatuImage = default;
    [SerializeField] float StartScale = 3.0f;
    [SerializeField] float AnimationTime = 2.0f;
    [SerializeField] TutrialSceneManager tutrial = default;
    float animationCounter = 0f;

    void Start()
    {
        BatuImage = GetComponent<Image>();
        BatuImage.color = new Color(255f, 255f, 255f, 0f);
        BatuImage.transform.localScale = new Vector3(StartScale, StartScale);
    }


    void Update()
    {
        if (tutrial.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_03 || tutrial.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_04)
        {

            animationCounter += Time.deltaTime;
            if (animationCounter >= AnimationTime)
            {
                animationCounter = 0f;
            }

            float scale = Easing.OutQuint(animationCounter, AnimationTime, 1f, StartScale);
            BatuImage.transform.localScale = new Vector3(scale, scale);
            float alpha = Easing.OutQuint(animationCounter, AnimationTime, 1f, 0f);
            BatuImage.color = new Color(1f, 1f, 1f, alpha);
        }
    }
}
