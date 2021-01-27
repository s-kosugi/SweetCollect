using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class Tutorial_Playing_Text : MonoBehaviour
{
    [SerializeField] TutrialSceneManager Tutrialscene = null;
    [SerializeField] Tutrial_EquipFrame TutrialEquipFrame = null;
    [SerializeField] TextMeshProUGUI textmesh = default;

    [SerializeField] List<string> WordList = new List<string>();  //説明文
    [SerializeField] float FadeTimer = 0.0f;                      //フェード時間
    [SerializeField] float FADETIME  = 1.0f;                      //フェード時間
    [SerializeField] bool FadeFlag;                               //フェード実行フラグ
    [SerializeField] float TextAlpha = 0.0f;                      //アルファ値

    [SerializeField] string DisplayDescription = "";              //表示する説明

    enum TEXT
    {
        NONE = -1,
        JUMP = 0,        //ジャンプ
        DOUBLEJUMP,      //空中ジャンプ
        SYOKUDAI,        //燭台
        CHEF,            //シェフ
    }

    enum STATE
    {
        NONE = -1,
        CHECK = 0,      //確認
        DISPLAY,        //表示
        FADEIN,         //フェードイン
        FADEOUT,        //フェードアウト
        WAIT,           //待機
    }
    [SerializeField] STATE State = STATE.NONE;

    private void Awake()
    {
        textmesh.text = "";
    }

    void Start()
    {
        DisplayDescription = "";
        FadeFlag = false;
        State = STATE.CHECK;
    }

    // Update is called once per frame
    void Update()
    {
        CheckFrame();
        SetAlpha();

        switch (State)
        {
            case STATE.CHECK: ChangeDisplayText(); break;
            case STATE.DISPLAY: Dispaly(); break;
            case STATE.FADEIN: FadeIn(); break;
            case STATE.FADEOUT: FadeOut(); break;
            case STATE.WAIT:   ; break;
        }
    }
    //===========================================================================================================
    //表示テキストの変更
    private void ChangeDisplayText()
    {
        if(Tutrialscene.tutrial != TutrialSceneManager.TUTRIAL.TUTRIAL_DESCRIPTION)
        {
            if (Tutrialscene.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_JUMP)
                DisplayDescription = WordList[(int)TEXT.JUMP];
            else if (Tutrialscene.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_DOUBLEJUMP)
                DisplayDescription = WordList[(int)TEXT.DOUBLEJUMP];
            else if (Tutrialscene.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_SYOKUDAI)
                DisplayDescription = WordList[(int)TEXT.SYOKUDAI];
            else if (Tutrialscene.tutrial == TutrialSceneManager.TUTRIAL.TUTRIAL_CHEF)
                DisplayDescription = WordList[(int)TEXT.CHEF];
            else
                DisplayDescription = "";

            textmesh.text = DisplayDescription;

            State = STATE.FADEIN;
        }

    }
    //表示
    private void Dispaly()
    {
        if (FadeFlag)
        {
            State = STATE.FADEOUT;
            FadeFlag = false;
        }
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
    //===========================================================================================================
    //フレームが表示可能かどうかを確認
    private void CheckFrame()
    {
        if (TutrialEquipFrame.DisplayFlag)
        {
            textmesh.enabled = true;
        }
        else
        {
            textmesh.enabled = false;
        }
    }

    //アルファ値
    private void SetAlpha()
    {
        textmesh.color = new Color(textmesh.color.r, textmesh.color.g, textmesh.color.b, TextAlpha);
    }

    //フェード開始
    public void StartFade()
    {
        FadeFlag = true;
    }
    
    //===========================================================================================================
}
