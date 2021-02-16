using UnityEngine;

/// <summary>
/// GooglePlayStoreからダウンロードさせるボタン
/// </summary>
public class DownLoadButton : MonoBehaviour
{
    public void Click()
    {
        // GooglePlayStoreを表示する
        string url = "https://play.google.com/store/apps/details?id=com.Siratamasyokolife.SweetCollect";
        Application.OpenURL(url);
    }
}
