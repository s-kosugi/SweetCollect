using UnityEngine;
using UnityEngine.UI;

public class DisableShopFadeButton : MonoBehaviour
{
    [SerializeField] private DisableSceneFadeButton disable = null;
    [SerializeField] private BuyAndWearButton buyAndWear = null;
    [SerializeField] private CurtainAnime curtainAnime = null;
    Button button = default;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!disable.GetStartEnable())
        {
            if (buyAndWear.GetState() == BuyAndWearButton.STATE.PREVIEWHINT || curtainAnime.state != CurtainAnime.STATE.WAIT)
                button.enabled = false;
            else
                button.enabled = true;
        }
    }
}
