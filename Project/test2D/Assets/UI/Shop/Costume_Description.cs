using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Costume_Description : MonoBehaviour
{
    [SerializeField] string Description;  //説明
    [SerializeField] TextMeshProUGUI Text_Description; //表示テキスト

    // Start is called before the first frame update
    void Start()
    {
        Text_Description = this.GetComponent<TextMeshProUGUI>();
        Description = "";

        Text_Description.text = Description;
    }

    // Update is called once per frame
    void Update()
    {
    }

    //===========================================================================================================
    //描画関連
    private void PreviewDescription()
    {
        Text_Description.text = Description;
    }

    //===========================================================================================================

    //===========================================================================================================
    //対応する説明文を設定
    public void SetDescription(string catalog )
    {
        Description = catalog;

        PreviewDescription();
    }

    //===========================================================================================================

    //===========================================================================================================
    //設定(Setter)

    //===========================================================================================================
    //===========================================================================================================
    //取得(Getter)
    //===========================================================================================================

}
