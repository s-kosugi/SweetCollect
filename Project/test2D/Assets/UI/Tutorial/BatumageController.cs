using UnityEngine;
using UnityEngine.UI;

public class BatumageController : MonoBehaviour
{
    Image BatuImage = default;
    [SerializeField] float StartScale = 3.0f;
    [SerializeField] float AnimationTime = 2.0f;
    float animationCounter = 0f;
    bool DisplayFlag = false;                                   //表示フラグ

    void Start()
    {
        BatuImage = GetComponent<Image>();
        BatuImage.color = new Color(255f, 255f, 255f, 0f);
        BatuImage.transform.localScale = new Vector3(StartScale, StartScale);
        DisplayFlag = false;
    }


    void Update()
    {
        DisplayUi();
    }

    //表示
    private void DisplayUi()
    {
        if (DisplayFlag)
        {
            animationCounter += Time.deltaTime;
            if (animationCounter >= AnimationTime)
            {
                animationCounter = 0f;
                DisplayFlag = false;
            }

            float scale = Easing.OutQuint(animationCounter, AnimationTime, 1f, StartScale);
            BatuImage.transform.localScale = new Vector3(scale, scale);
            float alpha = Easing.OutQuint(animationCounter, AnimationTime, 1f, 0f);
            BatuImage.color = new Color(1f, 1f, 1f, alpha);
        }
    }

    //UI表示開始
    public void StartDisplay()
    {
        DisplayFlag = true;
    }

}
