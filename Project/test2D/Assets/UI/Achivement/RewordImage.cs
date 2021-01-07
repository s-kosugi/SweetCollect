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
    [SerializeField] PlayFabInventory inventory = default;
    [SerializeField] float AnimationTime = 0.5f;
    [SerializeField] Color disableColor = new Color( 0.3f, 0.3f, 0.3f, 1f );
    private Image image = default;
    private float AnimationCount = 0f;

    enum State
    {
        LOAD,
        IDLE,
        ANIMATION_GET
    }

    private State state = State.LOAD;

    void Start()
    {
        image = GetComponent<Image>();
    }


    void Update()
    {
        switch( state)
        {
            case State.LOAD: LoadImage(); break;
            case State.IDLE: break;
            case State.ANIMATION_GET: AnimationGet(); break;
        }
        
    }

    /// <summary>
    /// イメージのロード（初回）
    /// </summary>
    private void LoadImage()
    {
        if (!waitConnect.IsWait() && store.m_isCatalogGet && store.m_isStoreGet)
        {
            for (int i = 0; i < store.StoreItems.Count; i++)
            {
                // カタログと一致するアイテムの取得
                var catalogItem = store.CatalogItems.Find(x => x.ItemId == store.StoreItems[i].ItemId);

                if (catalogItem.CustomData != null)
                {
                    // LitJsonを使ってJsonを連想配列化する
                    var jsonDic = LitJson.JsonMapper.ToObject<Dictionary<string, string>>(catalogItem.CustomData);
                    string record;
                    // 報酬が存在しない場合はデータ読み込みをしない
                    if (!jsonDic.TryGetValue(AchievementDataName.REWORD, out record)) continue;

                    // 連想配列からデータを読み込み
                    previewDictionary.Add(store.StoreItems[i].ItemId, Resources.Load<Sprite>("Player\\" + record));
                }
            }
            state = State.IDLE;
        }
    }
    /// <summary>
    /// 入手アニメーションの再生
    /// </summary>
    private void AnimationGet()
    {
        AnimationCount += Time.deltaTime;

        float scaleRate = Mathf.Sin(AnimationCount / AnimationTime * 180.0f * Mathf.Deg2Rad);
        // スケーリングでアニメーションさせる
        transform.localScale = new Vector3(1.0f + scaleRate, 1.0f + scaleRate, 1.0f + scaleRate);
        // 少しずつ色を出す
        float colorvalue = Easing.OutCubic(AnimationCount, AnimationTime, 1f, disableColor.r);
        image.color = new Color(colorvalue, colorvalue, colorvalue,1f);

        // アニメーション終了
        if( AnimationCount >= AnimationTime)
        {
            state = State.IDLE;
            transform.localScale = new Vector3(1f, 1f, 1f);
            image.color = new Color(1f, 1f, 1f, 1f);
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
            // 取得済みかどうかで表示色を変える
            if (inventory.IsHaveItem(sprite.name))
            {
                image.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                image.color = disableColor;
            }
        }
        else
        {
            // 報酬設定無し
            // 透明度を上げて見た目を隠す
            image.color = new Color(1f, 1f, 1f, 0f);
            // スプライトを初期状態にしておく
            image.sprite = default;
        }

        // 大きさを戻しておく
        transform.localScale = new Vector3(1f,1f,1f);

        // 状態を待機状態に強制的に変える
        state = State.IDLE;
    }

    /// <summary>
    /// 入手時のアニメーションの開始
    /// </summary>
    public void StartGetAnimation()
    {
        state = State.ANIMATION_GET;
    }
}
