using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Descript_Text : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Text_Achievement; //表示テキスト

    private void Awake()
    {
        Text_Achievement = this.GetComponent<TextMeshProUGUI>();
        Text_Achievement.text = "?????????????????????????????";
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    //===========================================================================================================
    //設定関連
    public void SetAchievementText(string text)
    {
        Text_Achievement.text = text;
    }
    //===========================================================================================================

    //===========================================================================================================
    //===========================================================================================================

}
