using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutrial_Target : MonoBehaviour
{
    [SerializeField] TutrialSceneManager m_TutrialManager = null;

    [SerializeField] string TargetTagName = "Player";
    [SerializeField] TutrialSceneManager.TUTRIAL m_StateID = default;         //チュートリアルID
    private void Start()
    {
        m_TutrialManager = GameObject.Find("TutrialSceneManager").GetComponent<TutrialSceneManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == TargetTagName)
        {
            m_TutrialManager.TutrialChange(m_StateID);
        }
    }
}
