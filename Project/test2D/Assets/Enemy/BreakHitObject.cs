using Effekseer;
using UnityEngine;
using TMPro;

public class BreakHitObject : MonoBehaviour
{
    [SerializeField] int SubScore = -100;
    [SerializeField] EffekseerEffectAsset Effect = null;
    [SerializeField] string BreakSEName = default;
    [SerializeField] GameObject MinusCoinUIObject = default;
    private GameObject CanvasObject = default;
    private Camera cameraObject = default;

    private void Start()
    {
        CanvasObject = GameObject.Find("Canvas");
        cameraObject = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーに当たったら破壊される
        if (collision.gameObject.tag == "Player")
        {
            // スコアの減算
            transform.root.GetComponent<GameMainManager>().scoreManager.AddScore(SubScore);

            // 破壊エフェクトの再生
            EffekseerSystem.PlayEffect(Effect, transform.position);

            // SEの再生
            SoundManager.Instance.PlaySE(BreakSEName);

            // コイン減算UIを表示
            GameObject obj = Instantiate(MinusCoinUIObject, CanvasObject.transform);
            obj.transform.position = RectTransformUtility.WorldToScreenPoint(cameraObject, this.transform.position);
            obj.GetComponent<TextMeshProUGUI>().text = SubScore.ToString();
            obj.transform.SetAsFirstSibling(); //一番上(uGUIなら背面)

            Destroy(this.gameObject);
        }
    }
}
