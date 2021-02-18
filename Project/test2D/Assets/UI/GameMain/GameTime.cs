using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    TextMeshProUGUI textMesh;
    [SerializeField] GameMainManager gameMain = default;
    [SerializeField] Color textChangeColor = default;
    [SerializeField] Image clockIcon = default;
    [SerializeField] GameObject scaleBigTextObject = default;
    private Color textSorceColor = default;
    private bool oldFrameisRed = false;

    void Start()
    {
        if(gameMain == default)
            gameMain = GameObject.Find("GameManager").GetComponent<GameMainManager>();

        textMesh = GetComponent<TextMeshProUGUI>();
        textSorceColor = textMesh.color;
    }

    void Update()
    {
        textMesh.text = ":" + string.Format("{0:00}", (int)gameMain.GameTimer);

        // 一定秒数以下になったらゲーム時間文字と時計アイコンを赤くする
        if (gameMain.GameDengerTime >= gameMain.GameTimer)
        {
            textMesh.color = textChangeColor;
            clockIcon.color = textChangeColor;

            // 文字が赤くなった瞬間
            if (!oldFrameisRed)
            {
                oldFrameisRed = true;
                scaleBigTextObject.SetActive(true);
            }
        } 
        else
        {
            textMesh.color = textSorceColor;
            clockIcon.color = textSorceColor;
            oldFrameisRed = false;
        }
    }
}
