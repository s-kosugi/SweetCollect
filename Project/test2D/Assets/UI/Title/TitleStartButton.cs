using UnityEngine;
using UnityEngine.UI;

public class TitleStartButton : MonoBehaviour
{
    [SerializeField] PlayFabPlayerData playerData = default;
    [SerializeField] TitleManager titleManager = default;
    [SerializeField] TitlePlayerController playerController = default;
    [SerializeField] StageSelectParent stageSelect = default;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// スタートボタン押下
    /// </summary>
    public void PushButton()
    {
        // ボタンを押したら他ボタンを無効化する
        if (playerData.m_isGet)
        {
            // シーンをステージセレクト操作状態にする
            titleManager.TapStageSelect();

            // チュートリアル終了済みでなかったら名前入力へ飛ばす
            if (!playerData.m_Data.ContainsKey(PlayerDataName.TUTORIAL) || playerData.m_Data[PlayerDataName.TUTORIAL].Value != "End")
            {
                StartNextScene("InputNameScene");
            }
            else
            {
                // チュートリアル終了時はステージセレクトを表示する
                stageSelect.StartAppear();
            }
        }
    }

    /// <summary>
    /// 次シーン開始処理
    /// </summary>
    private void StartNextScene(string sceneName)
    {
        titleManager.NextScene(sceneName);
        playerController.StartJump();
    }
}
