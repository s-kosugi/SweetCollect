using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementParent : MonoBehaviour
{
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    [SerializeField] PlayFabStore store = default;
    [SerializeField] PlayFabPlayerData playerData = default;
    [SerializeField] Button achivementButton = default;
    [SerializeField] TextMeshProUGUI descript = default;
    [SerializeField] SwipeMove swipeMove = default;
    [SerializeField] float buttonVerticalInterval = 100f;
    [SerializeField] float buttonHorizonInterval = 400.0f;
    [SerializeField] int buttonHorizonNum = 2;
    [SerializeField] ReachAchievement reach = default;

    public string descriptAchievementID = default;      // Descriptに表示中のID
    public string descriptAchievementName { get; private set; } = default;    // Descriptに表示中の実績名
    public string selectAchievementID { get; private set; } = default;      // 装備中の称号ID

    public bool isCreate { get; private set; } = false;
    public bool isNowAchievementReach { get; private set; } = false;          // 現在表示中の実績が解放済みかどうか

    void Update()
    {
        if (!isCreate && !waitConnect.IsWait() && store.m_isStoreGet && store.m_isCatalogGet &&
            playerData.m_isGet && reach.isSet)
        {
            // アチーブメントボタン生成処理
            CreateAchivementButton();

            isCreate = true;
        }
    }

    /// <summary>
    /// 実績ボタンの作成
    /// </summary>
    private void CreateAchivementButton()
    {
        for (int i = 0; i < store.StoreItems.Count; i++)
        {
            // カタログと一致するアイテムの取得
            var catalogItem = store.CatalogItems.Find(x => x.ItemId == store.StoreItems[i].ItemId);

            var info = reach.GetInfo(store.StoreItems[i].ItemId);

            //--------------------------------------------------------------------------------
            // ボタンオブジェクトの生成と初期化
            Button button = Instantiate(achivementButton, this.transform);
            AchievementButton achievementButtonScript = button.GetComponent<AchievementButton>();
            button.transform.localPosition = new Vector3(button.transform.localPosition.x + i % buttonHorizonNum * buttonHorizonInterval, button.transform.localPosition.y - (int)(i / buttonHorizonNum) * buttonVerticalInterval, button.transform.localPosition.z);
            button.name = store.StoreItems[i].ItemId;

            //--------------------------------------------------------------------------------
            // 実績名をセット
            TextMeshProUGUI textMesh = button.transform.Find("AchievementTitle").GetComponent<TextMeshProUGUI>();
            textMesh.text = catalogItem.DisplayName;

            //--------------------------------------------------------------------------------
            // 進捗度をセット
            textMesh = button.transform.Find("ProgressText").GetComponent<TextMeshProUGUI>();
            textMesh.text = info.progressValue + "/" + info.progressMax;

            // 実績達成済みなら達成済みフラグをON
            if (info.reach) achievementButtonScript.ReachAchievement = true;
        }

        // ボタン生成数に応じてスワイプの移動の制限値を変える
        swipeMove.moveLimitRect.height = swipeMove.moveLimitRect.yMin + store.StoreItems.Count / buttonHorizonNum * buttonVerticalInterval;
    }

    /// <summary>
    /// AchievementIDからDescriptオブジェクトの内容を更新
    /// </summary>
    /// <param name="achievementID">AchievementのアイテムID</param>
    /// <param name="reachAchievement">実績達成ずみか</param>
    public void UpdateDescript(string achievementID,bool reachAchievement)
    {
        var catalogItem = store.CatalogItems.Find(x => x.ItemId == achievementID);

        descript.text = catalogItem.Description;

        descriptAchievementID = achievementID;

        descriptAchievementName = catalogItem.DisplayName;

        // 現在表示中の実績が解放済みかどうか
        isNowAchievementReach = reachAchievement;

    }

    /// <summary>
    /// Achievementの選択状態を変更する
    /// </summary>
    public void SelectedAchievement()
    {
        // 開放済みの物だけ選択できる
        if (isNowAchievementReach)
        {
            // 選択されているIDを変更
            selectAchievementID = descriptAchievementID;

            foreach (Transform item in this.transform)
            {
                // 孫を参照して選択アイコンの表示を変更する
                foreach (Transform grandChild in item)
                {
                    if (grandChild.name == "SelectedIcon")
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
    }
    /// <summary>
    /// Achievementの選択状態をIDを指定して変更する
    /// </summary>
    public void SelectedAchievement(string achievementID)
    {
        // 選択されているIDを変更
        descriptAchievementID = achievementID;

        // Descriptの内容を更新する
        UpdateDescript(achievementID,true);

        SelectedAchievement();

    }
}
