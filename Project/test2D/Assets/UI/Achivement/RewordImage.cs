using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 報酬イメージ表示クラス
/// </summary>
public class RewordImage : MonoBehaviour
{
    // 報酬プレビュー用
    private Dictionary<string, Sprite> previewDictionary = new Dictionary<string, Sprite>();
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    [SerializeField] PlayFabStore store = default;
    private bool isInit = false;
    private Image image = default;
    void Start()
    {
        image = GetComponent<Image>();
    }


    void Update()
    {
        // 初回のデータ読み込み
        if (!isInit && !waitConnect.IsWait() && store.m_isCatalogGet && store.m_isStoreGet)
        {
            LoadImage();
            isInit = true;
        }
    }

    /// <summary>
    /// イメージのロード
    /// </summary>
    private void LoadImage()
    {
        for (int i = 0; i < store.StoreItems.Count; i++)
        {
            // カタログと一致するアイテムの取得
            var catalogItem = store.CatalogItems.Find(x => x.ItemId == store.StoreItems[i].ItemId);

            if (catalogItem.CustomData != null)
            {
                // LitJsonを使ってJsonを連想配列化する
                var jsonlist = LitJson.JsonMapper.ToObject<Dictionary<string, string>>(catalogItem.CustomData);

                // 連想配列からデータを読み込み
                previewDictionary.Add(store.StoreItems[i].ItemId, Resources.Load<Sprite>("Player\\" + jsonlist["REWORD"]));
            }
        }
    }

    /// <summary>
    /// 報酬衣装の切り替え
    /// </summary>
    /// <param name="achivementID"></param>
    public void ApplyImage(string achivementID)
    {
        Sprite sprite = default;
        if (previewDictionary.TryGetValue(achivementID, out sprite))
        {
            image.sprite = sprite;
            image.color = new Color(0.3f, 0.3f, 0.3f, 1f);
        }
        else
        {
            // 報酬設定無し
            // 透明度を上げて見た目を隠す
            image.color = new Color(1f, 1f, 1f, 0f);
        }
    }
}
