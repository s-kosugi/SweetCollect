using TMPro;
using UnityEngine;

public class AchievementNoticeWindow : MonoBehaviour
{
    [SerializeField] float MoveTime = 0.5f;
    [SerializeField] float WaitTime = 2.0f;
    [SerializeField] float MovePosY = -145f;

    float stateCount = 0f;
    float startPos = 0f;
    TextMeshProUGUI titleTextMesh = default;

    enum State
    {
        Appear,
        Wait,
        Disappear,
    }

    private State state = State.Appear;

    void Start()
    {
        startPos = transform.localPosition.y;
        titleTextMesh = transform.Find("AchievementTitle").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        switch( state )
        {
            case State.Appear:    Appear();    break;
            case State.Wait:      Wait();      break;
            case State.Disappear: Disappear(); break;
        }
    }

    /// <summary>
    /// 出現時
    /// </summary>
    private void Appear()
    {
        stateCount += Time.deltaTime;

        float posY = Easing.OutCubic(stateCount, MoveTime, startPos + MovePosY, startPos);
        transform.localPosition = new Vector3(transform.localPosition.x, posY);

        if (stateCount >= MoveTime)
        {
            posY = startPos + MovePosY;
            stateCount = 0f;
            state = State.Wait;
        }
    }
    /// <summary>
    /// 待機時
    /// </summary>
    private void Wait()
    {
        stateCount += Time.deltaTime;

        if (stateCount >= WaitTime)
        {
            stateCount = 0f;
            state = State.Disappear;
        }
    }
    /// <summary>
    /// 待機時
    /// </summary>
    private void Disappear()
    {
        stateCount += Time.deltaTime;

        float posY = Easing.InCubic(stateCount, MoveTime, startPos, startPos + MovePosY);
        transform.localPosition = new Vector3(transform.localPosition.x, posY);

        // 完全に消えたので消滅させる
        if (stateCount >= MoveTime)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 実績名のセット
    /// </summary>
    /// <param name="text">実績名</param>
    public void SetTitleText(string text)
    {
        titleTextMesh.text = text;
    }
}
