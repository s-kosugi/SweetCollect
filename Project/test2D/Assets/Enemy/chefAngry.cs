using UnityEngine;
using Effekseer;
using TMPro;

public class chefAngry : MonoBehaviour
{
    private bool isAngry = false;
    private GameMainManager GameMain;
    [SerializeField] float MinusTime = 5.0f;
    [SerializeField] SpriteRenderer sprite = default;
    [SerializeField] EffekseerEffectAsset effect = default;
    [SerializeField] GameObject MinusTimeUIObject = default;
    EffekseerHandle effectHandle = default;
    private GameObject CanvasObject = default;
    private Camera cameraObject = default;

    void Start()
    {
        GameMain = transform.root.GetComponent<GameMainManager>();
        CanvasObject = GameObject.Find("Canvas");
        cameraObject = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if(isAngry)
        {
            // 怒りエフェクトを追従させる
            if (effectHandle.enabled)
            {
                effectHandle.SetLocation(this.transform.position);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAngry)
        {
            if (collision.tag == "Player")
            {
                isAngry = true;
                // 店員に当たったらゲームプレイ時間を減らす
                GameMain.AddGameTime(-MinusTime);

                // 店員の色を変える
                Color color = sprite.color;
                color.b = 0.5f;
                color.g = 0.5f;
                sprite.color = color;

                // 怒りエフェクトを再生
                effectHandle = EffekseerSystem.PlayEffect(effect, this.transform.position);

                // 怒り音を再生
                SoundManager.Instance.PlaySE("Angry");

                // 時間減算UIを表示
                GameObject obj = Instantiate(MinusTimeUIObject, CanvasObject.transform);
                obj.transform.position = RectTransformUtility.WorldToScreenPoint(cameraObject, this.transform.position);
                obj.GetComponent<TextMeshProUGUI>().text = (-MinusTime).ToString();
            }
        }
    }
}
