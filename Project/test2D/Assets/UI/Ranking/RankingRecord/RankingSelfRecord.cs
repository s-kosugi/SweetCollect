using UnityEngine;

public class RankingSelfRecord : MonoBehaviour
{
    [SerializeField] GameObject leaderBoardObject = default;
    [SerializeField] PlayFabLeaderBoard selfLeaderBoard = default;
    [SerializeField] GameObject rankingRecordObject = default;
    [SerializeField] Vector2 recordPosition = new Vector2(-290, -6);
    bool isLoad = false;


    void Update()
    {
        if (!isLoad && selfLeaderBoard.isGet)
        {
            LoadRecord();
            isLoad = true;
        }
    }
    /// <summary>
    /// レコードの読み込み
    /// </summary>
    private void LoadRecord()
    {
        RankingRecord record = default;
        GameObject obj = default;
        obj = Instantiate(rankingRecordObject, this.transform);
        obj.transform.localPosition = recordPosition;

        record = obj.GetComponent<RankingRecord>();
        record.SetLeaderBoard(leaderBoardObject, selfLeaderBoard);
        record.rankPosition = selfLeaderBoard.entries[0].Position;
    }

    /// <summary>
    /// レコードの再読み込み
    /// </summary>
    public void ReloadRecord()
    {
        // レコードの全削除
        foreach (Transform n in transform)
        {
            GameObject.Destroy(n.gameObject);
        }
        // レコードの再ロード
        isLoad = false;
    }
}
