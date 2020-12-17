using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectRetryWindow : MonoBehaviour
{
    [SerializeField] float ConnectTimeOut = 15.0f;
    private PlayFabWaitConnect waitConnect = default;
    private float ConnectCount = 0f;
    GameObject enableObject = default;

    void Start()
    {
        waitConnect = GameObject.Find("PlayFabManager").GetComponent<PlayFabWaitConnect>();
        enableObject = transform.Find("Frame").gameObject;
    }


    void Update()
    {
        if (waitConnect.IsWait())
        {
            ConnectCount += Time.deltaTime;
            if (ConnectCount >= ConnectTimeOut)
            {
                enableObject.SetActive(true);
            }
        }
        else
        {
            ConnectCount = 0f;
            enableObject.SetActive(false);
        }
    }
    public void ResetCounter()
    {
        ConnectCount = 0f;
        enableObject.SetActive(false);
    }
    public void GiveUpConnect()
    {
        // シーンをあきらめたらスプラッシュシーンに戻る
        SceneManager.LoadScene("SplashScene");
    }
}
