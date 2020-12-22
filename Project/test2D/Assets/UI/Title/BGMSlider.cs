using UnityEngine;
using UnityEngine.UI;

public class BGMSlider : MonoBehaviour
{
    private bool isInitialize = false;
    Slider slider = default;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.enabled = false;
    }


    void Update()
    {
        // スライダー値による音量の更新
        if (isInitialize)
        {
            // BGM音量のセット
            SoundManager.Instance.SetBGMVolume(slider.value);
        }
    }
    // スライダーの初期値設定
    public void InitializeSlider()
    {
        slider.value = SoundManager.Instance.m_BGMVolume;
        slider.enabled = true;
        isInitialize = true;
    }
}
