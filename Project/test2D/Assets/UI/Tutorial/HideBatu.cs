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

        if (tutrial.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_01 ||
            tutrial.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_02 ||
            tutrial.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_END
            )
        {
            // 透明にする
            fingerImage.color = new Color(1f, 1f, 1f, 0f);
        }
    }
}
