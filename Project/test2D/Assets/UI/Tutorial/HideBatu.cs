using UnityEngine;
using UnityEngine.UI;

public class HideBatu : MonoBehaviour
{
    Image fingerImage = default;
    [SerializeField] TutrialSceneManager tutrial = default;

    void Start()
    {
        fingerImage = GetComponent<Image>();
    }


    void Update()
    {

        if (tutrial.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_JUMP ||
            tutrial.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_DOUBLEJUMP ||
            tutrial.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_FINISHDESCRIPTION
            )
        {
            // 透明にする
            fingerImage.color = new Color(1f, 1f, 1f, 0f);
        }
    }
}
