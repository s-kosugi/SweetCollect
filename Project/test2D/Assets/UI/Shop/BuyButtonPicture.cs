using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 購入着替えボタンの画像切り替えクラス
/// </summary>
public class BuyButtonPicture : MonoBehaviour
{
    [SerializeField] Sprite WearSprite = default;
    [SerializeField] Sprite BuySprite = default;
    private bool IsWear = true;
    BuyAndWear_Text buyAndWear_Text = default;
    private Image buttonImage;
    private GameObject coinIconObject = default;


    void Start()
    {
        buyAndWear_Text = transform.Find("BuyAndWear_Text").GetComponent<BuyAndWear_Text>();
        buttonImage = GetComponent<Image>();
        coinIconObject = transform.Find("CoinIcon").gameObject;


        SetWear(true);
    }

    /// <summary>
    /// 着替える状態か買う状態かをセットする
    /// </summary>
    /// <param name="flag">true = 着替えボタン false = 購入ボタン</param>
    public void SetWear( bool flag )
    {
        if (flag)
        {
            buttonImage.sprite = WearSprite;
            coinIconObject.SetActive(false);
            buyAndWear_Text.SetTextFlag(true);
        }
        else
        {
            buttonImage.sprite = BuySprite;
            coinIconObject.SetActive(true);
            buyAndWear_Text.SetTextFlag(false);
        }
    }
}
