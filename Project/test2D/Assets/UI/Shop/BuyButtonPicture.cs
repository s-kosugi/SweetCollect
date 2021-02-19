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
    BuyAndWear_Text BuyAndWearText = default;
    private Image ButtonImage;
    private GameObject CoinIconObject = default;


    void Start()
    {
        BuyAndWearText = transform.Find("BuyAndWear_Text").GetComponent<BuyAndWear_Text>();
        ButtonImage = GetComponent<Image>();
        CoinIconObject = transform.Find("CoinIcon").gameObject;

    }

    /// <summary>
    /// ボタン画像を着替え状態へ変更する
    /// </summary>
    public void ChangeWearState( )
    {
        ButtonImage.sprite = WearSprite;
        CoinIconObject.SetActive(false);
        BuyAndWearText.SetTextFlag(false);
    }
    /// <summary>
    /// ボタン画像を購入状態へ変更する
    /// </summary>
    public void ChangeBuyState(string priceText)
    {
        ButtonImage.sprite = BuySprite;
        CoinIconObject.SetActive(true);
        BuyAndWearText.SetTextFlag(true);
        BuyAndWearText.SetBuyText("-"+priceText);
    }
    /// <summary>
    /// ボタン画像を？状態へ変更する
    /// </summary>
    public void ChangeQuestionState()
    {
        ButtonImage.sprite = AchievementSprite;
        CoinIconObject.SetActive(false);
        BuyAndWearText.SetTextFlag(false);
    }
}
