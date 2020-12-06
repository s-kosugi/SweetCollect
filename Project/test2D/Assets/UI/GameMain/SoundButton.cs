using UnityEngine;

/// <summary>
/// ボタン用の音声再生クラス
/// </summary>
public class SoundButton : MonoBehaviour
{
    [SerializeField] string seFileName = "Tap";
    // クリックされた時
    public void Click()
    {
        SoundManager.Instance.PlaySE(seFileName);
    }
}
