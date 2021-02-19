using UnityEngine;

/// <summary>
/// 広告ボタン有効化クラス
/// </summary>
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
