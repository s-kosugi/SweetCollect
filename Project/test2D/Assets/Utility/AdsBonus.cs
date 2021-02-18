using UnityEngine;

/// <summary>
/// 広告ボーナスクラス
/// </summary>
public class AdsBonus : MonoBehaviour
{
    private Ads ads = default;
    [SerializeField] GameMainManager gameMainManager = default;
    [SerializeField] float AddTime = 30f;
    public bool isAdd { get; private set; } = false;     // ボーナス加算済みかどうか

    void Start()
    {
        ads = gameObject.GetComponent<Ads>();
        isAdd = false;
    }

    void Update()
    {
        // 広告表示済みなら時間を延長させる
        if (ads && ads.isShow  && !isAdd)
        {
            Debug.Log("Add:AdsBonus");
            gameMainManager.AddGameTime(AddTime);
            isAdd = true;
        }
    }

}
