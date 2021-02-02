using PlayFab;
using UnityEngine;
using UnityEngine.UI;

public class CustomIDField : MonoBehaviour
{
    [SerializeField] PlayFabLogin playFabLogin = default;
    InputField inputField = default;


    void Start()
    {
        inputField = GetComponent<InputField>();
    }

    void Update()
    {
        if ( string.IsNullOrEmpty(inputField.text) )
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                // InputFieldにカスタムIDを設定する
                inputField.text = playFabLogin.GetCustomID();
            }
        }
    }
}
