using UnityEngine;
using UnityEngine.UI;

public class DisableShopFadeButton : MonoBehaviour
{
    [SerializeField] private DisableSceneFadeButton disable = null;    //シーンフェード
    [SerializeField] private ClothingBuyAndWear buyandwear = null;     //着用購入関連処理
    [SerializeField] private CurtainAnime curtainanime = null;         //カーテン
    Button button = default;                                           //ボタン

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
            if (buyandwear.GetState() == ClothingBuyAndWear.STATE.PREVIEWHINT 
                || curtainanime.state != CurtainAnime.STATE.WAIT)
                button.enabled = false;
            else
                button.enabled = true;
        }
    }
}
