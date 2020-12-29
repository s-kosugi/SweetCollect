using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementParent : MonoBehaviour
{
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    [SerializeField] PlayFabInventory inventory = default;
    [SerializeField] PlayFabStore store = default;
    [SerializeField] Button achivementButton = default;
    [SerializeField] TextMeshProUGUI descript = default;
    [SerializeField] RewordImage rewordImage = default;
    [SerializeField] SwipeMove swipeMove = default;
    [SerializeField] float buttonInterval = 100f;

    public string descriptAchievementID = default;
    public string selectAchievementID { get; private set; } = default;

    public bool isCreate { get; private set; } = false;

    void Update()
    {
        if (!isCreate && !waitConnect.IsWait() && store.m_isStoreGet && store.m_isCatalogGet)
        {
            // アチーブメントボタン生成処理
            CreateAchivementButton();

            isCreate = true;
        }
    }
    private void CreateAchivementButton()
    {
        for (int i = 0; i < store.StoreItems.Count; i++)
        {
            // カタログと一致するアイテムの取得
            var catalogItem = store.CatalogItems.Find(x => x.ItemId == store.StoreItems[i].ItemId);

            // ボタンオブジェクトの生成と初期化
            Button obj = Instantiate(achivementButton, this.transform);
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y - i * buttonInterval, obj.transform.localPosition.z);
            obj.name = store.StoreItems[i].ItemId;

            // 実績名をセット
            TextMeshProUGUI textMesh = obj.transform.Find("AchievementTitle").GetComponent<TextMeshProUGUI>();
            textMesh.text = catalogItem.DisplayName;

            // 進捗MAXをセット
            textMesh = obj.transform.Find("ProgressText").GetComponent<TextMeshProUGUI>();
            textMesh.text = catalogItem.VirtualCurrencyPrices["AC"].ToString();
        }

        // ボタン生成数に応じてスワイプの移動の制限値を変える
        swipeMove.moveLimitRect.height = swipeMove.moveLimitRect.yMin + store.StoreItems.Count * buttonInterval;
    }

    /// <summary>
    /// AchievementIDからDescriptオブジェクトの内容を更新
    /// </summary>
    /// <param name="achievementID">AchievementのアイテムID</param>
    public void UpdateDescript(string achievementID)
    {
        var catalogItem = store.CatalogItems.Find(x => x.ItemId == achievementID);

        descript.text = catalogItem.Description;
        rewordImage.ApplyImage(achievementID);

        descriptAchievementID = achievementID;
    }

    /// <summary>
    /// Achievementの選択状態を変更する
    /// </summary>
    public void SelectedAchievement()
    {
        // 選択されているIDを変更
        selectAchievementID = descriptAchievementID;

        foreach (Transform item in this.transform)
        {
            // 孫を参照して選択アイコンの表示を変更する
            foreach (Transform grandChild in item)
            {
                if(grandChild.name == "SelectedIcon")
                {
                    if (item.name == descriptAchievementID)
                        grandChild.gameObject.SetActive(true);
                    else
                        grandChild.gameObject.SetActive(false);

                    break;
                }
            }
        }
    }
    /// <summary>
    /// Achievementの選択状態をIDを指定して変更する
    /// </summary>
    public void SelectedAchievement(string achievementID)
    {
        // 選択されているIDを変更
        descriptAchievementID = achievementID;

        SelectedAchievement();

    }
}
