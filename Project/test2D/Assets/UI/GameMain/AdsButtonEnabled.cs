using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsButtonEnable : MonoBehaviour
{
    void Start()
    {
        // AndroidとiOSの場合広告ボタンを非表示
        if (Application.platform != RuntimePlatform.Android && Application.platform == RuntimePlatform.IPhonePlayer)
        {
            GameObject adsbutton = GameObject.Find("AdsButton");
            if (adsbutton) adsbutton.SetActive(false);
        }
    }
}
