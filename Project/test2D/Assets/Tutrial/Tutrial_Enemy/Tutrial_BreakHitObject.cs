﻿using Effekseer;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class Tutrial_BreakHitObject : MonoBehaviour
{
    [SerializeField] int SubScore = -100;
    [SerializeField] EffekseerEffectAsset Effect = null;
    [SerializeField] string BreakSEName = default;
    [SerializeField] GameObject MinusCoinUIObject = default;
    private GameObject CanvasObject = default;
    private Camera cameraObject = default;

    [SerializeField] TutrialSceneManager m_TutrialManager = null;           //チュートリアルマネージャー

    private void Start()
    {
        CanvasObject = GameObject.Find("Canvas");
        cameraObject = GameObject.Find("Main Camera").GetComponent<Camera>();
        m_TutrialManager = GameObject.Find("TutrialSceneManager").GetComponent<TutrialSceneManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーに当たったら破壊される
        if (collision.gameObject.tag == "Player")
        {
            // 破壊エフェクトの再生
            EffekseerSystem.PlayEffect(Effect, transform.position);

            // SEの再生
            SoundManager.Instance.PlaySE(BreakSEName);

            // コイン減算UIを表示
            GameObject obj = Instantiate(MinusCoinUIObject, CanvasObject.transform);
            obj.transform.position = RectTransformUtility.WorldToScreenPoint(cameraObject, this.transform.position);
            obj.GetComponent<TextMeshProUGUI>().text = SubScore.ToString();
            
            if(m_TutrialManager.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_03)
            {
                //チュートリアルの変更
                m_TutrialManager.TutrialChange();
            }

            Destroy(this.gameObject);

        }
    }
}