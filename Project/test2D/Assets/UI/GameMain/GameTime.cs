using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    TextMeshProUGUI m_Text;
    GameMainManager m_GameMain = null;
    [SerializeField] Color textChangeColor = default;
    [SerializeField] Image clockIcon = default;
    [SerializeField] GameObject scaleBigTextObject = default;
    private Color textSorceColor = default;
    private bool oldFrameisRed = false;

    void Start()
    {
        m_GameMain = GameObject.Find("GameManager").GetComponent<GameMainManager>();
        m_Text = GetComponent<TextMeshProUGUI>();
        textSorceColor = m_Text.color;
    }

    void Update()
    {
        m_Text.text = ":" + string.Format("{0:00}", (int)m_GameMain.GameTimer);

        // 一定秒数以下になったらゲーム時間文字と時計アイコンを赤くする
        if (m_GameMain.GameDengerTime >= m_GameMain.GameTimer)
        {
            m_Text.color = textChangeColor;
            clockIcon.color = textChangeColor;

            // 文字が赤くなった瞬間
            if (!oldFrameisRed)
            {
                oldFrameisRed = true;
                scaleBigTextObject.SetActive(true);
            }
        } else
        {
            m_Text.color = textSorceColor;
            clockIcon.color = textSorceColor;
            oldFrameisRed = false;
        }
    }
}
