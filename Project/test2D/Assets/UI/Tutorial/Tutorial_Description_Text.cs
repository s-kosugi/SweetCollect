using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Description_Text : MonoBehaviour
{
    [SerializeField] TutrialSceneManager Tutrialscene = null;
    [SerializeField] Tutrial_EquipFrame TutrialEquipFrame = null;
    [SerializeField] TextMeshProUGUI textmesh = default;

    [SerializeField] List<string> WordList = new List<string>();                                //説明文

    [SerializeField] int NowWordNumber = 0;                                                     //現在の説明の番号
    float DisplayTimer = 0;                                                    //表示時間
    [SerializeField] float DISPLAY_TIME = 1.0f;                                                 //表示時間
    [SerializeField] string DisplayDescription = "";                                            //表示する説明

    [SerializeField] TutrialSceneManager.TUTRIAL NextState = TutrialSceneManager.TUTRIAL.NONE;  //次に進めたい状態
    float FadeTimer = 0.0f;                                                    //フェード時間
    [SerializeField] float FADETIME = 1.0f;                                                     //フェード時間
    [SerializeField] float TextAlpha = 0.0f;                                                    //アルファ値

    public enum STATE
    {
        NONE = -1,
        CHECK = 0,   //確認
        CHANGE,      //変更
        DISPLAY,     //表示
        FADEIN,      //フェードイン
        FADEOUT,     //フェードアウト
        WAIT         //待機
    }
    [SerializeField] public STATE State = STATE.NONE;       //テキスト状態

    private void Awake()
    {
        textmesh.text = "";
    }

    void Start()
    {
        NowWordNumber = 0;
        DisplayTimer = 0;
        DisplayDescription = "";

        State = STATE.CHECK;
    }

    // Update is called once per frame
    void Update()
    {
        SetAlpha();

        switch (State)
        {
            case STATE.CHECK: Check(); break;
            case STATE.CHANGE: Change(); break;
            case STATE.DISPLAY:Display(); break;
            case STATE.FADEIN: FadeIn(); break;
            case STATE.FADEOUT: FadeOut(); break;
            case STATE.WAIT:Wait(); break;
        }

    }
    //===========================================================================================================
    //状態関連
    //確認
    private void Check()
    {
        if(TutrialEquipFrame.DisplayFlag)
        {
            if (NowWordNumber < WordList.Count)
            {
                State = STATE.CHANGE;
            }
            else
            {
                State = STATE.WAIT;
                textmesh.enabled = false;
                TutrialEquipFrame.StartVanish();
                Tutrialscene.TutrialChange(NextState);
            }
        }
    }
    //変更
    private void Change()
    {
        DisplayDescription = WordList[NowWordNumber];
        textmesh.text = DisplayDescription;
        NowWordNumber += 1;
        State = STATE.FADEIN;
    }
    //表示
    private void Display()
    {
        if(Tutrialscene.IsFadeEnd() && TutrialEquipFrame.DisplayFlag)
        {
            if(DisplayTimer < DISPLAY_TIME)
            {
                DisplayTimer += Time.deltaTime;
                textmesh.enabled = true;
            }
            else
            {
                DisplayTimer = 0.0f;
                State = STATE.FADEOUT;
            }
        }
    }
    //待機
    private void Wait()
    {

    }
    //フェードイン
    private void FadeIn()
    {
        if (TutrialEquipFrame.DisplayFlag)
        {
            if (FadeTimer < FADETIME)
            {
                TextAlpha = Easing.Linear(FadeTimer, FADETIME, 1.0f, 0.0f);
                FadeTimer += Time.deltaTime;
            }
            else
            {
                FadeTimer = 0.0f;
                TextAlpha = 1.0f;
                State = STATE.DISPLAY;
            }
        }
    }
    //フェードアウト
    private void FadeOut()
    {
        if (FadeTimer < FADETIME)
        {
            TextAlpha = Easing.Linear(FadeTimer, FADETIME, 0.0f, 1.0f);
            FadeTimer += Time.deltaTime;
        }
        else
        {
            FadeTimer = 0.0f;
            TextAlpha = 0.0f;
            State = STATE.CHECK;
        }
    }

    //アルファ値
    private void SetAlpha()
    {
        textmesh.color = new Color(textmesh.color.r, textmesh.color.g, textmesh.color.b, TextAlpha);
    }
    //===========================================================================================================
}
