using UnityEngine;

public class TwitterLinkButton : MonoBehaviour
{
    public void PushButton()
    {
        string url = "https://twitter.com/SweetCollectDev";

        //Twitter開発者画面の起動
        Application.OpenURL(url);
    }
}
