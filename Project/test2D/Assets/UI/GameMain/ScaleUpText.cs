using UnityEngine;

/// <summary>
/// 拡大テキストクラス
/// </summary>
public class ScaleUpText : MonoBehaviour
{
    [SerializeField] float StartScale = 0.0f;
    [SerializeField] float EndScale = 1.0f;
    [SerializeField] float AnimeTime = 2.0f;
    [SerializeField] float BackScale = 3.0f;
    [SerializeField] bool EndDestroy = true;
    private RectTransform rectTrans;
    private float timer = 0f;

    // スライドインが終わったかどうか
    public bool isEnd { get; private set; }

    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        rectTrans.localScale = new Vector3(StartScale,StartScale);
        timer = 0;
        isEnd = false;
    }


    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= AnimeTime)
        {
            rectTrans.localScale = new Vector3(EndScale, EndScale);
            isEnd = true;

            // 削除フラグが立っていたら削除する
            if (EndDestroy) Destroy(gameObject);
        }
        else
        {
            float scale = Easing.OutBack(timer, AnimeTime, EndScale, StartScale, BackScale);
            rectTrans.localScale = new Vector3(scale, scale);
        }
    }
}
