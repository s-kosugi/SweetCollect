using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingRecordParent : MonoBehaviour
{
    [SerializeField] GameObject rankingRecord = default;
    [SerializeField] PlayFabLeaderBoard leaderBoard = default;
    [SerializeField] float HeightInterval = 80;
    [SerializeField] Vector2 startPos = default;
    [SerializeField] SwipeMove swipe = default;
    [SerializeField] float SwipeMoveAdjustment = 360;


    void Start()
    {
        RankingRecord record = default;
        GameObject obj = default;

        // レコード表示用オブジェクトの生成
        for (int i = 0; i < leaderBoard.GetMaxRecord(); i++)
        {
            obj = Instantiate(rankingRecord, this.transform);
            record = obj.GetComponent<RankingRecord>();
            record.rankPosition = i;

            obj.transform.localPosition = new Vector3(startPos.x, i * -HeightInterval + startPos.y);
        }
        // スワイプ移動の制限
        swipe.moveLimitRect = new Rect(0,0,0,HeightInterval * leaderBoard.GetMaxRecord() - SwipeMoveAdjustment);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
