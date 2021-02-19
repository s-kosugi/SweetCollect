using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ユーザー名入力テキストボックス
/// </summary>
public class UserNameTextBox : MonoBehaviour
{
    [SerializeField] TitleManager titleManager = default;
    [SerializeField] PlayFabUserProfiel playFabUserProfiel = default;
    InputField inputField = default;
    private bool isNameSet = false;

    void Start()
    {
        inputField = GetComponent<InputField>();
    }

    void Update()
    {
        // PlayFabから名前未設定の場合はテキストに名前をセットする
        if (!isNameSet)
        {
            if (playFabUserProfiel.isGet)
            {
                string displayName = playFabUserProfiel.DisplayName;
                if (displayName != "")
                {
                    // ログインしてたらユーザーネームをセットする
                    inputField.text = displayName;
                    isNameSet = true;
                }
            }
        }
        // メイン状態以外は入力を受け付けない
        if (titleManager.fadeState != BaseScene.FADE_STATE.NONE)
        {
            inputField.enabled = false;
        }
        else
        {
            inputField.enabled = true;
        }
    }
}
