using Effekseer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetAchievementReword : MonoBehaviour
{
    Button button = default;
    [SerializeField] TextMeshProUGUI text = default;
    [SerializeField] AchievementParent achievementParent = default;
    [SerializeField] PlayFabInventory inventory = default;
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    [SerializeField] PlayFabStore store = default;
    [SerializeField] Image rewordImage = default;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        // ボタンの有効無効を切り替える
        EnableChangeButton();
    }

    private void EnableChangeButton()
    {
        bool buttonActive = false;

        // 通信していない
        if (!waitConnect.IsWait())
        {
            if (inventory.m_isGet)
            {
                // 服を未所持かつデフォルト（報酬無し）でないか
                if (rewordImage.sprite != default && !inventory.IsHaveItem(rewordImage.sprite.name))
                {
                    // 解放済み
                    if (achievementParent.isNowAchievementReach)
                    {
                        buttonActive = true;
                    }
                }
            }
        }

        if (buttonActive)
        {
            // アクティブ化されていなかったらアクティブにする
            if (!text.gameObject.activeSelf) text.gameObject.SetActive(true);

            //ボタンを押せるようにする
            button.interactable = true;
        }
        else
        {
            text.gameObject.SetActive(false);
            button.interactable = false;
        }
    }

    public void PushButton()
    {
        string clothesID = rewordImage.sprite.name;
        if( inventory.IsHaveItem(clothesID)) return;

        SoundManager.Instance.PlaySE("Release");
        // ボタンが押されたので衣装を入手する
        store.BuyItem(clothesID, "HA");
    }
}
