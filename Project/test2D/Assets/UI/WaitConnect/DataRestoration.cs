using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// データ引継ぎクラス
/// </summary>
public class DataRestoration : MonoBehaviour
{
    [SerializeField] InputField inputField = default;
    PlayFabLogin playfabLogin = default;
    Vector3 oldPosition = default;

    void Start()
    {
        oldPosition = this.transform.localPosition;
        GameObject manager = GameObject.Find("PlayFabManager");
        if( manager != null)
            playfabLogin = manager.GetComponent<PlayFabLogin>(); ;
    }

    /// <summary>
    /// データ復旧の開始
    /// </summary>
    public void StartRestoration()
    {
        // 文字数を満たしていない場合はロードできない
        if (inputField.text.Length >= playfabLogin.idLength)
        {
            PlayerPrefs.SetString(PlayFabLogin.CUSTOM_ID_SAVE_KEY, inputField.text);
            // スプラッシュに戻す前にログアウト処理を行う
            if (playfabLogin)
                playfabLogin.LogOut();
            // スプラッシュシーンに戻ってデータを再ロードする
            SceneManager.LoadScene("SplashScene");
        }
    }

    /// <summary>
    /// ウィンドウの表示
    /// </summary>
    public void EnableWindow()
    {
        this.transform.localPosition = new Vector3(0.0f,0.0f);
    }

    /// <summary>
    /// ウィンドウの非表示
    /// </summary>
    public void DisableWindow()
    {
        this.transform.localPosition = oldPosition;
    }

}
