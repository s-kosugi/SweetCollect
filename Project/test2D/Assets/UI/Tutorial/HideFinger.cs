using UnityEngine;
using UnityEngine.UI;

public class HideFinger : MonoBehaviour
{
    Image fingerImage = default;
    [SerializeField] TutrialSceneManager tutrial = default;

    void Start()
    {
        fingerImage = GetComponent<Image>();
    }


    void Update()
    {

        if (tutrial.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_03 ||
            tutrial.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_04 ||
            tutrial.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_END
            )
        {
            // 透明にする
            fingerImage.color = new Color(1f, 1f, 1f, 0f);
        }
    }
}
