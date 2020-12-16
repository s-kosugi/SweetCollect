using UnityEngine;
using UnityEngine.UI;

public class TwoJumpFinger : MonoBehaviour
{
    Image fingerImage = default;
    [SerializeField] float StartScale = 3.0f;
    [SerializeField] float FirstAnimationTime = 0.5f;
    [SerializeField] float SecondAnimationTime = 1.0f;
    [SerializeField] TutrialSceneManager tutrial = default;
    float animationCounter = 0f;

    enum STATE
    {
        FIRST_TAP,
        SECOND_TAP
    }

    private STATE state;

    void Start()
    {
        fingerImage = GetComponent<Image>();
        fingerImage.color = new Color(255f, 255f, 255f, 0f);
        fingerImage.transform.localScale = new Vector3(StartScale, StartScale);
    }

    void Update()
    {
        if (tutrial.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_02)
        {
            switch (state)
            {
                case STATE.FIRST_TAP: FirstTap(); break;
                case STATE.SECOND_TAP: SecondTap(); break;
            }
        }
    }
    void FirstTap()
    {
        animationCounter += Time.deltaTime;
        if (animationCounter >= FirstAnimationTime)
        {
            animationCounter = 0f;
            state = STATE.SECOND_TAP;
            return;
        }

        float scale = Easing.Linear(animationCounter, FirstAnimationTime, 1f, StartScale);
        fingerImage.transform.localScale = new Vector3(scale, scale);
        float alpha = Easing.OutQuint(animationCounter, FirstAnimationTime, 1f, 0f);
        fingerImage.color = new Color(1f, 1f, 1f, alpha);
    }
    void SecondTap()
    {
        animationCounter += Time.deltaTime;
        if (animationCounter >= SecondAnimationTime)
        {
            animationCounter = 0f;
            state = STATE.FIRST_TAP;
            return;
        }

        float scale = Easing.OutQuint(animationCounter, SecondAnimationTime, 1f, StartScale);
        fingerImage.transform.localScale = new Vector3(scale, scale);
        float alpha = Easing.OutQuint(animationCounter, SecondAnimationTime, 1f, 0f);
        fingerImage.color = new Color(1f, 1f, 1f, alpha);
    }
}
