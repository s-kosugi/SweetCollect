using System.Runtime.InteropServices;
using UnityEngine;

public class TwitterLinkButton : MonoBehaviour
{
#if UNITY_WEBGL
    // Assets/Plugins/WebGLにあるOpenNewTabPluginをインポートする
    [DllImport("__Internal")] private static extern void OpenNewTab(string URL);
#endif

    public void PushButton()
    {

        string url = "https://twitter.com/SweetCollectDev";


        // Twitter開発者画面の起動
#if UNITY_EDITOR
        Application.OpenURL(url);
#elif UNITY_WEBGL
        OpenNewTab(url);
#else
        Application.OpenURL(url);
#endif
    }
}
