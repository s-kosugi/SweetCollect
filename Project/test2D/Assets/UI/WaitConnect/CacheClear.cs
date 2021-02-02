using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// キャッシュクリアクラス
/// </summary>
public class CacheClear : MonoBehaviour
{
    Vector3 oldPosition = default;

    void Start()
    {
        oldPosition = this.transform.localPosition;
    }

    /// <summary>
    /// キャッシュクリアの開始
    /// </summary>
    public void StartChacheClear()
    {
        PlayerPrefs.SetString(PlayFabLogin.CUSTOM_ID_SAVE_KEY, string.Empty);
        // スプラッシュシーンに戻ってデータを再ロードする
        SceneManager.LoadScene("SplashScene");
    }

    /// <summary>
    /// ウィンドウの表示
    /// </summary>
    public void EnableWindow()
    {
        this.transform.localPosition = new Vector3(0.0f, 0.0f);
    }

    /// <summary>
    /// ウィンドウの非表示
    /// </summary>
    public void DisableWindow()
    {
        this.transform.localPosition = oldPosition;
    }
}
