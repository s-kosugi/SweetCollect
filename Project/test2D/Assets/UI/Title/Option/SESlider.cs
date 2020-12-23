using UnityEngine;
using UnityEngine.UI;

public class SESlider : MonoBehaviour
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
            // SE音量のセット
            SoundManager.Instance.SetSEVolume(slider.value);
        }
    }

    // スライダーの初期値設定
    public void InitializeSlider()
    {
        slider.value = SoundManager.Instance.m_SEVolume;
        slider.enabled = true;
        isInitialize = true;
    }
}
