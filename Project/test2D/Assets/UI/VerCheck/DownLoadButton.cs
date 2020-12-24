using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownLoadButton : MonoBehaviour
{

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void Click()
    {
        // GooglePlayStoreを表示する
        string url = "https://play.google.com/store/apps/details?id=com.Siratamasyokolife.SweetCollect";
        Application.OpenURL(url);
    }
}
