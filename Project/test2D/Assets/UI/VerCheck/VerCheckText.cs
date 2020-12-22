using TMPro;
using UnityEngine;

public class VerCheckText : MonoBehaviour
{
    [SerializeField]PlayFabWaitConnect waitConnect = default;
    TextMeshProUGUI textMeshProUGUI = default;
    // Start is called before the first frame update
    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waitConnect.IsWait()) textMeshProUGUI.enabled = true;
        else textMeshProUGUI.enabled = false;
    }
}
