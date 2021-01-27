using Effekseer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutrial_chefAngry : MonoBehaviour
{
    private bool isAngry = false;
    [SerializeField] float MinusTime = 5.0f;
    [SerializeField] SpriteRenderer sprite = default;
    [SerializeField] EffekseerEffectAsset effect = default;
    [SerializeField] GameObject MinusTimeUIObject = default;
    EffekseerHandle effectHandle = default;
    private GameObject CanvasObject = default;
    private Camera cameraObject = default;
    [SerializeField] TutrialSceneManager m_TutrialManager = null;
    [SerializeField] TutrialSceneManager.TUTRIAL m_StateID = default;         //チュートリアルID

    [SerializeField] BatumageController batumage = null;                      //バツイメージUi
    void Start()
    {
        CanvasObject = GameObject.Find("Canvas");
        cameraObject = GameObject.Find("Main Camera").GetComponent<Camera>();
        m_TutrialManager = GameObject.Find("TutrialSceneManager").GetComponent<TutrialSceneManager>();
        batumage = GameObject.Find("Canvas").GetComponentInChildren<BatumageController>();
    }

    void Update()
    {
        if (isAngry)
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

                //チュートリアルの変更
                m_TutrialManager.TutrialChange(m_StateID);
                batumage.StartDisplay();
            }
        }
    }
}
