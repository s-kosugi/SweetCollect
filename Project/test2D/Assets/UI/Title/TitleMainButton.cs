using UnityEngine;
using UnityEngine.UI;

public class TitleMainButton : MonoBehaviour
{
    [SerializeField] TitleManager titleManager = default;
    Button button = default;
    void Start()
    {
        button = GetComponent<Button>();
    }


    void Update()
    {
        // タイトルがメイン状態以外の時はボタンを無効化する
        if (titleManager.state != TitleManager.STATE.MAIN)
        {
            button.enabled = false;
        }
        else
        {
            button.enabled = true;
        }
    }
}
