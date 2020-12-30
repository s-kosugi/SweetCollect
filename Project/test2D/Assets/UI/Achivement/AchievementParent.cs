using PlayFab.ClientModels;
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
    [SerializeField] PlayFabPlayerData playerData = default;
    [SerializeField] Button achivementButton = default;
    [SerializeField] TextMeshProUGUI descript = default;
    [SerializeField] RewordImage rewordImage = default;
    [SerializeField] Button equipButton = default;
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

            // LitJsonを使ってJsonを連想配列化する
            var jsonDic = LitJson.JsonMapper.ToObject<Dictionary<string, string>>(catalogItem.CustomData);

            //--------------------------------------------------------------------------------
            // ボタンオブジェクトの生成と初期化
            Button button = Instantiate(achivementButton, this.transform);
            AchievementButton achievementButtonScript = button.GetComponent<AchievementButton>();
            button.transform.localPosition = new Vector3(button.transform.localPosition.x, button.transform.localPosition.y - i * buttonInterval, button.transform.localPosition.z);
            button.name = store.StoreItems[i].ItemId;

            //--------------------------------------------------------------------------------
            // 実績名をセット
            TextMeshProUGUI textMesh = button.transform.Find("AchievementTitle").GetComponent<TextMeshProUGUI>();
            textMesh.text = catalogItem.DisplayName;

            //--------------------------------------------------------------------------------
            // 進捗度をセット
            textMesh = button.transform.Find("ProgressText").GetComponent<TextMeshProUGUI>();
            UserDataRecord playerRecord;
            string progressString = "0";
            // 実績内のカスタムデータからキーを取得してプレイヤーデータにアクセスする
            if (playerData.m_Data.TryGetValue(jsonDic[AchievementDataName.PROGRESS_KEY], out playerRecord))
            {

                double num;
                // 進捗度が数値ではなかった場合は実績内の該当キーと一致しているかで判断をする
                if (!double.TryParse(playerRecord.Value, out num))
                {
                    string achievementValue;
                    // 実績内のプレイヤーデータを持つキーとプレイヤーデータが一致したら達成済み(1)とする
                    if (jsonDic.TryGetValue(jsonDic[AchievementDataName.PROGRESS_KEY], out achievementValue))
                    {
                        if (playerRecord.Value == achievementValue) progressString = "1";
                    }
                }
                else
                {
                    // 数値データだったのでそのまま格納する
                    progressString = playerRecord.Value;
                }
            }
            textMesh.text = progressString + "/" + jsonDic[AchievementDataName.PROGRESS_MAX];
            // 実績達成済みなら達成済みフラグをON
            if (double.Parse(progressString) >= double.Parse(jsonDic[AchievementDataName.PROGRESS_MAX])) achievementButtonScript.ReachAchievement = true;
        }

        // ボタン生成数に応じてスワイプの移動の制限値を変える
        swipeMove.moveLimitRect.height = swipeMove.moveLimitRect.yMin + store.StoreItems.Count * buttonInterval;
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
        rewordImage.ApplyImage(achievementID);

        descriptAchievementID = achievementID;

        // 実績達成済みならボタンを有効化する
        equipButton.interactable = reachAchievement;
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
