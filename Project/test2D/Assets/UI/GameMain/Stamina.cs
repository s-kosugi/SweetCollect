using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] GameObject ShowObject = null;
    CalcDamage m_ShowTarget;
    TextMeshProUGUI m_Text;

    void Start()
    {
        m_ShowTarget = ShowObject.GetComponent<CalcDamage>();
        m_Text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        m_Text.text = "スタミナ : " + string.Format("{0:0000}",m_ShowTarget.hp);
    }
}
