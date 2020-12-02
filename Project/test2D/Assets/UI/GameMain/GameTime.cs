using UnityEngine;
using TMPro;

public class GameTime : MonoBehaviour
{
    TextMeshProUGUI m_Text;
    GameMainManager m_GameMain = null;

    void Start()
    {
        m_GameMain = GameObject.Find("GameManager").GetComponent<GameMainManager>();
        m_Text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        m_Text.text = ":" + string.Format("{0:00}", (int)m_GameMain.GetGameTime());
    }
}
