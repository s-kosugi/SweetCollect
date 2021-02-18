using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClotingNameText : MonoBehaviour
{
    [SerializeField] AchievementEquipFrame achievementequipframe = null;    //表示フレーム
    [SerializeField] TextMeshProUGUI textmesh = default;                    //テキスト
    // Start is called before the first frame update
    void Start()
    {
        textmesh.text = "???????????????";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //服の名前の取得
    //name : 服の名前
    public void GetClotingName(string name)
    {
        textmesh.text = name;
        achievementequipframe.StartAppear();
    }
}
