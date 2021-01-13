using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingSelfRecord : MonoBehaviour
{
    [SerializeField] PlayFabLeaderBoard selfLeaderBoard = default;
    [SerializeField] RankingRecord rankingRecord = default;


    void Start()
    {
        
    }


    void Update()
    {
        if (selfLeaderBoard.isGet)
        {
            rankingRecord.rankPosition = selfLeaderBoard.entries[0].Position+1;
        }
    }
}
