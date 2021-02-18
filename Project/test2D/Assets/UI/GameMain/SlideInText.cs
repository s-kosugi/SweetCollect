using UnityEngine;

/// <summary>
/// スライドインするテキストクラス
/// </summary>
public class SlideInText : MonoBehaviour
{
    [SerializeField] float StartPositionX = -1000.0f;
    [SerializeField] float StartPositionY = 0;
    [SerializeField] float EndPositionX = 0;
    [SerializeField] float EndPositionY = 0;
    [SerializeField] float SlideTime = 2.0f;
    [SerializeField] bool EndDestroy = true;
    private RectTransform rectTrans;
    private float timer = 0f;

    // スライドインが終わったかどうか
    public bool isEnd { get; private set; }

    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        rectTrans.localPosition = new Vector3(StartPositionX, StartPositionY);
        timer = 0;
        isEnd = false;
    }


    void Update()
    {
        timer += Time.deltaTime;

        if( timer >= SlideTime)
        {
            rectTrans.localPosition = new Vector3(EndPositionX, EndPositionY);
            isEnd = true;

            // 削除フラグが立っていたら削除する
            if (EndDestroy) Destroy(gameObject);
        }
        else
        {
            float positionX = Easing.OutExp(timer, SlideTime, EndPositionX, StartPositionX);
            float positionY = Easing.OutExp(timer, SlideTime, EndPositionY, StartPositionY);
            rectTrans.localPosition = new Vector3(positionX, positionY);
        }
    }
}
