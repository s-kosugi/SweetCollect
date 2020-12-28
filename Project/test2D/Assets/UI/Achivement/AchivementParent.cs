using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchivementParent : MonoBehaviour
{
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    [SerializeField] PlayFabInventory inventory = default;
    [SerializeField] PlayFabStore store = default;
    [SerializeField] Button achivementButton = default;
    [SerializeField] TextMeshProUGUI descript = default;
    [SerializeField] RewordImage rewordImage = default;
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
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y - i * 100, obj.transform.localPosition.z);
            obj.name = store.StoreItems[i].ItemId;
            AchivementButton button = obj.GetComponent<AchivementButton>();
            // 対象オブジェクトと説明のセット
            button.Setup(descript, catalogItem.Description, rewordImage);

            // 実績名をセット
            TextMeshProUGUI textMesh = obj.transform.Find("AchivementTitle").GetComponent<TextMeshProUGUI>();
            textMesh.text = catalogItem.DisplayName;

            // 進捗MAXをセット
            textMesh = obj.transform.Find("ProgressText").GetComponentInChildren<TextMeshProUGUI>();
            textMesh.text = catalogItem.VirtualCurrencyPrices["AC"].ToString();
        }
    }
}
