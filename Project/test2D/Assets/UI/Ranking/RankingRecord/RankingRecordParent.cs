using UnityEngine;

/// <summary>
/// ランキングレコード生成クラス
/// </summary>
public class RankingRecordParent : MonoBehaviour
{
    [SerializeField] GameObject rankingRecord = default;
    [SerializeField] PlayFabLeaderBoard leaderBoard = default;
    [SerializeField] GameObject leaderBoardObject = default;
    [SerializeField] float HeightInterval = 80;
    [SerializeField] Vector2 startPos = default;
    [SerializeField] SwipeMove swipe = default;
    [SerializeField] float SwipeMoveAdjustment = 360;
    bool isLoadChild = false;


    void Update()
    {
        if (!isLoadChild)
        {
            if (leaderBoard.isGet)
            {
                // 子のロード
                LoadChild();
                isLoadChild = true;
            }
        }
    }

    /// <summary>
    /// 子の再ロード　
    /// </summary>
    public void ReloadChild()
    {
        // 子の全削除
        foreach (Transform n in transform)
        {
            GameObject.Destroy(n.gameObject);
        }
        // 子の再ロード
        isLoadChild = false;
    }

    /// <summary>
    /// 子のロード
    /// </summary>
    public void LoadChild()
    {
        // レコード表示用オブジェクトの生成
        for (int i = 0; i < leaderBoard.entries.Count; i++)
        {
            RankingRecord record = default;
            GameObject obj = default;

            obj = Instantiate(rankingRecord, this.transform);
            record = obj.GetComponent<RankingRecord>();
            record.SetLeaderBoard(leaderBoardObject, leaderBoard);

            record.rankPosition = i;

            obj.transform.localPosition = new Vector3(startPos.x, i * -HeightInterval + startPos.y);
        }
        // スワイプ移動の制限
        swipe.moveLimitRect = new Rect(0, 0, 0, HeightInterval * leaderBoard.entries.Count - SwipeMoveAdjustment);
    }
}
