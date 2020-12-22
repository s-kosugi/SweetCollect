using UnityEngine;
using UnityEngine.SceneManagement;

public class VerCheckSceneManager : MonoBehaviour
{
#if UNITY_ANDROID
    [SerializeField] PlayFabTitleData titleData = default;
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    [SerializeField] GameObject ErrorText = default;
#endif

    void Start()
    {
        
    }


    void Update()
    {

#if UNITY_ANDROID
        if (!waitConnect.IsWait() && titleData.m_isGet)
        {
            if (titleData.titleData["GameVersion"] == Application.version)
            {
                Debug.Log("バージョンチェックOK : " + Application.version);
                SceneManager.LoadScene("SplashScene");
            }
            else
            {
                // エラーテキストを表示
                ErrorText.SetActive(true);
            }
        }
#else
        // ANDROID以外は無条件でスプラッシュシーンへ
        SceneManager.LoadScene("SplashScene");
#endif
    }
}
