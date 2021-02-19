using UnityEngine;

/// <summary>
/// ランキングでのレコードを表示する為の親クラスプレイヤーデータの情報とリーダーボードを保持しておく
/// </summary>
public class RankingRecord : MonoBehaviour
{
    public PlayFabLeaderBoard leaderBoard { get; private set; }= default;
    GameObject leaderBoardObject = default;
    public PlayFabStore store { get; private set; } = default;

    private GameObject playerDataObj = null;
    public PlayFabPlayerData playerData { get; private set; } = default;

    /// <summary>
    /// 現在の参照する順位
    /// </summary>
    public int rankPosition { get; set; } = -1;

    void Start()
    {
        store = GameObject.Find("PlayFabManager/PlayFabStore").GetComponent<PlayFabStore>();
    }


    void Update()
    {
        if (leaderBoardObject != default)
        {
            // プレイヤーデータのオブジェクトが作成されたら取得する
            if (playerDataObj == null)
            {
                Transform trs = leaderBoardObject.transform.Find("PlayFabPlayerDataRank" + rankPosition);
                if (trs != null) playerDataObj = trs.gameObject;
            }
            else
            {
                // プレイヤーデータを取得する
                if (playerData == default) playerData = playerDataObj.GetComponent<PlayFabPlayerData>();
            }
        }
    }

    /// <summary>
    /// リーダーボードの設定
    /// </summary>
    /// <param name="obj">リーダーボードのオブジェクト</param>
    /// <param name="leader">リーダーボードのスクリプト</param>
    public void SetLeaderBoard(GameObject obj, PlayFabLeaderBoard leader)
    {
        leaderBoardObject = obj;
        leaderBoard = leader;
    }
}
