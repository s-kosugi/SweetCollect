using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 購入着替えボタンの画像切り替えクラス
/// </summary>
public class BuyButtonPicture : MonoBehaviour
{
    [SerializeField] Sprite WearSprite = default;
    [SerializeField] Sprite BuySprite = default;
    [SerializeField] Sprite AchievementSprite = default;
    BuyAndWear_Text buyAndWear_Text = default;
    private Image buttonImage;
    private GameObject coinIconObject = default;


    void Start()
    {
        buyAndWear_Text = transform.Find("BuyAndWear_Text").GetComponent<BuyAndWear_Text>();
        buttonImage = GetComponent<Image>();
        coinIconObject = transform.Find("CoinIcon").gameObject;

    }

    /// <summary>
    /// ボタン画像を着替え状態へ変更する
    /// </summary>
    public void ChangeWearState( )
    {
        buttonImage.sprite = WearSprite;
        coinIconObject.SetActive(false);
        buyAndWear_Text.SetTextFlag(false);
    }
    /// <summary>
    /// ボタン画像を購入状態へ変更する
    /// </summary>
    public void ChangeBuyState(string priceText)
    {
        buttonImage.sprite = BuySprite;
        coinIconObject.SetActive(true);
        buyAndWear_Text.SetTextFlag(true);
        buyAndWear_Text.SetBuyText("-"+priceText);
    }
    /// <summary>
    /// ボタン画像を？状態へ変更する
    /// </summary>
    public void ChangeQuestionState()
    {
        buttonImage.sprite = AchievementSprite;
        coinIconObject.SetActive(false);
        buyAndWear_Text.SetTextFlag(false);
    }
}
