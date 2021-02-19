using UnityEngine;
using UnityEngine.SceneManagement;

public class VerCheckSceneManager : MonoBehaviour
{
#if UNITY_ANDROID
    [SerializeField] PlayFabTitleData titleData = default;
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    [SerializeField] GameObject DownloadObj = default;
    [SerializeField] GameObject ContinueObj = default;

#endif
    public enum STATE
    {
        CONNECTING,
        DOWNLOAD,
        PLAY,
    };
    public STATE state { get; private set; }

    void Start()
    {
        state = STATE.CONNECTING;
    }


    void Update()
    {

#if UNITY_ANDROID
        switch (state)
        {
            case STATE.CONNECTING: VerConnecting(); break;
            case STATE.DOWNLOAD:   VerDownload(); break;
            case STATE.PLAY:       VerPlay(); break;
        }

#else
        // ANDROID以外は無条件でスプラッシュシーンへ
        NextScene();
#endif
    }
#if UNITY_ANDROID
    private void VerConnecting()
    {
        if (!waitConnect.IsWait() && titleData.isGet)
        {
            if (titleData.titleData["GameVersion"] == Application.version)
            {
                Debug.Log("バージョンチェックOK : " + Application.version);
                NextScene();
            }
            else
            {
                // バージョン違いの場合GooglePlayStoreへ誘導
                state = STATE.DOWNLOAD;
            }
        }
    }
    private void VerDownload()
    {
        // GooglePlayStoreへ誘導
        DownloadObj.SetActive(true);
        ContinueObj.SetActive(false);
    }
    private void VerPlay()
    {
        // Playする際の注意喚起
        DownloadObj.SetActive(false);
        ContinueObj.SetActive(true);
    }
#endif
    public void ChangeDownloadState()
    {
        state = STATE.DOWNLOAD;
    }
    public void ChangePlayState()
    {
        state = STATE.PLAY;
    }
    public void NextScene()
    {
        SceneManager.LoadScene("SplashScene");
    }
}
