using TMPro;
using UnityEngine;

public class ConnectWaitUI : MonoBehaviour
{
    private PlayFabWaitConnect WaitConnect = null;
    private TextMeshProUGUI text;
    [SerializeField] float ConnectAnimeTime = 1.0f;
    private float ConnectAnimeCounter = 0;

    void Start()
    {
        WaitConnect = GameObject.Find("PlayFabManager").GetComponent<PlayFabWaitConnect>();
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (WaitConnect.IsWait())
        {
            ConnectAnimeCounter += Time.deltaTime;
            if (ConnectAnimeCounter <= ConnectAnimeTime / 3.0f)
            {
                text.text = "ちょっとまってね・";
            }
            else if (ConnectAnimeCounter <= ConnectAnimeTime / 3.0f * 2.0f)
            {
                text.text = "ちょっとまってね・・";
            }
            else
            {
                text.text = "ちょっとまってね・・・";
            }
            // カウンターリセット
            if (ConnectAnimeCounter >= ConnectAnimeTime)
            {
                ConnectAnimeCounter = 0f;
            }
        }
        else
        {
            ConnectAnimeCounter = 0f;
            text.text = "";
        }
    }
}
