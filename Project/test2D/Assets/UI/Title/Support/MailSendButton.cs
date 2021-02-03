using UnityEngine;

public class MailSendButton : MonoBehaviour
{
    public void PushButton()
    {
        string url = "mailto:sweetcollectdev@gmail.com";

        //メールの起動
        Application.OpenURL(url);
    }
}
