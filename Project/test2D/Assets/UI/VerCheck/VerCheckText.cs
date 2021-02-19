using TMPro;
using UnityEngine;

/// <summary>
/// バージョンチェック中テキストの表示
/// </summary>
public class VerCheckText : MonoBehaviour
{
    [SerializeField]PlayFabWaitConnect waitConnect = default;
    TextMeshProUGUI textMeshProUGUI = default;


    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        if (waitConnect.IsWait()) textMeshProUGUI.enabled = true;
        else textMeshProUGUI.enabled = false;
    }
}
