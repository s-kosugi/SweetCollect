using UnityEngine;
using UnityEngine.UI;

public class DisableSelectFadeButton : MonoBehaviour
{
    [SerializeField] private DisableSceneFadeButton disable = null;  //フェードボタン
    [SerializeField] private ClothingBuyAndWear buyandwear = null;   //服を着用または購入関連
    [SerializeField] private CurtainAnime curtainanime = null;       //カーテン
    Button button = default;                                         //ボタン

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        //開始時に有効化されたら
        if (!disable.GetStartEnable())
        {
            //条件達成でボタンが押せるようにする
            if (buyandwear.GetState() != ClothingBuyAndWear.STATE.RECEPTION || curtainanime.state != CurtainAnime.STATE.WAIT)
                button.enabled = false;
            else
                button.enabled = true;
        }
    }
}
