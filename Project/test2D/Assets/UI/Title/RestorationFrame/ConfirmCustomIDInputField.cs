using UnityEngine;
using UnityEngine.UI;

public class ConfirmCustomIDInputField : MonoBehaviour
{
    [SerializeField] InputField inputField = default;
    [SerializeField] InputField sorceInputField = default;

    void Update()
    {
        inputField.text = sorceInputField.text;
    }
}
