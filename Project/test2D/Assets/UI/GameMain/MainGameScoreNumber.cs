using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainGameScoreNumber : MonoBehaviour
{
    private ScoreManager ScoreMan = null;
    private TextMeshProUGUI Text = null;
    void Start()
    {
        ScoreMan = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        Text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if( ScoreMan ) Text.text = string.Format("× {0:0000}", ScoreMan.GetCoinScore());
    }
}
