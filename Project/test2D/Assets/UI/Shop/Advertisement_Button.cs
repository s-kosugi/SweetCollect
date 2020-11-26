using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Advertisement_Button : MonoBehaviour
{
    enum BUTTONSTATE
    {
        NONE = -1,
        DISPLAY,   //表示
        SHOW,      //確認済み
    }
    [SerializeField] BUTTONSTATE State;

    [SerializeField] Button Advertisement;

    // Start is called before the first frame update
    void Start()
    {
        State = BUTTONSTATE.DISPLAY;
        Advertisement = this.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ボタンクリック
    public void Push_Button()
    {
        if (State == BUTTONSTATE.SHOW) return;
        State = BUTTONSTATE.SHOW;

        //ここで処理を追加できるようにする

        Advertisement.enabled = false;

    }

}
