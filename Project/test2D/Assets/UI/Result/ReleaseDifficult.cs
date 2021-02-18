using Effekseer;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

/// <summary>
/// 難易度解放オブジェクトクラス
/// </summary>
public class ReleaseDifficult : MonoBehaviour
{
    [SerializeField] PlayFabTitleData titleData = default;
    [SerializeField] PlayFabPlayerData playerData = default;
    [SerializeField] EffekseerEffectAsset releaseEffect = default;
    [SerializeField] GameObject normalReleaseText = default;
    [SerializeField] GameObject hardReleaseText = default;
    [SerializeField] GameObject veryhardReleaseText = default;
    [SerializeField] TextMeshProUGUI nextText = default;
    [SerializeField] NoticeAchievement noticeAchievement = default;
    [SerializeField] Vector2 appearStartPos = new Vector2(-215f,0f);
    [SerializeField] Vector2 appearGoalPos = Vector2.zero;
    [SerializeField] CameraController cameraController = default;
    private EffekseerHandle releaseEffectHandle = default;
    private ScoreManager scoreManager = default;
    [SerializeField] float AppearTime = 1.0f;
    private float appearCounter = 0f;
    public enum MESSAGE_STATE 
    {
        HIDE,
        APPEAR,
        WAIT,
        DISAPPEAR,
    };
    private MESSAGE_STATE state = default;

    void Start()
    {
        state = MESSAGE_STATE.HIDE;
        transform.localPosition = appearStartPos;

        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }


    void Update()
    {
        switch(state)
        {
            case MESSAGE_STATE.HIDE: HideMessage(); break;
            case MESSAGE_STATE.APPEAR: AppearMessage(); break;
            case MESSAGE_STATE.WAIT: WaitMessage(); break;
            case MESSAGE_STATE.DISAPPEAR: DisappearMessage(); break;
        }
    }
    private void HideMessage()
    {
        transform.localScale = Vector3.zero;
        appearCounter = 0f;

    }
    private void AppearMessage()
    {
        appearCounter += Time.deltaTime;
        if (appearCounter >= AppearTime)
        {
            state = MESSAGE_STATE.WAIT;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            transform.localPosition = appearGoalPos;
            appearCounter = 0f;
        }
        else
        {
            // outbackで出現させる
            float scale = Easing.OutBack(appearCounter, AppearTime, 1.0f, 0.0f, 0.8f);
            transform.localScale = new Vector3(scale, scale, scale);

            // 少しずつ移動する
            float positionX = Easing.OutCubic(appearCounter, AppearTime, appearGoalPos.x, appearStartPos.x);
            float positionY = Easing.OutCubic(appearCounter, AppearTime, appearGoalPos.y, appearStartPos.y);
            transform.localPosition = new Vector3(positionX,positionY);
        }
    }
    private void WaitMessage()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        transform.localPosition = appearGoalPos;
        appearCounter = 0f;
    }
    private void DisappearMessage()
    {
        appearCounter += Time.deltaTime;
        if (appearCounter >= AppearTime)
        {
            state = MESSAGE_STATE.HIDE;
            transform.localScale = Vector3.zero;
            appearCounter = 0f;
        }
        else
        {
            // Inbackで退場させる
            float scale = Easing.InBack(appearCounter, AppearTime, 0.0f, 1.0f, 0.8f);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    /// <summary>
    /// 解放メッセージを表示するかどうか
    /// </summary>
    /// <returns></returns>
    public bool isShowUnlockMessage()
    {
        bool ret = false;
        UserDataRecord record = default;
        int Threshold = 99999;
        string TargetDifficult = PlayerDataName.RELEASE_NORMAL;

        // 現在の難易度を見てスコアを超えているかどうかを確認する。
        if (playerData.data.TryGetValue(PlayerDataName.SELECTED_DIFFICULT, out record))
        {
            switch (record.Value)
            {
                case DifficultName.EASY:
                    // 解放対象難易度が解放済みかどうかをチェックする(キーが無ければ未開放)
                    if (!playerData.data.TryGetValue(PlayerDataName.RELEASE_NORMAL, out record))
                    {

                        Threshold = int.Parse(titleData.titleData[TitleDataName.RELEASE_THRESHOLD_NORMAL]);
                        TargetDifficult = PlayerDataName.RELEASE_NORMAL;
                    }
                    break;
                case DifficultName.NORMAL:
                    // 解放対象難易度が解放済みかどうかをチェックする(キーが無ければ未開放)
                    if (!playerData.data.TryGetValue(PlayerDataName.RELEASE_HARD, out record))
                    {
                        Threshold = int.Parse(titleData.titleData[TitleDataName.RELEASE_THRESHOLD_HARD]);
                        TargetDifficult = PlayerDataName.RELEASE_HARD;
                    }
                    break;
                case DifficultName.HARD:
                    // 解放対象難易度が解放済みかどうかをチェックする(キーが無ければ未開放)
                    if (!playerData.data.TryGetValue(PlayerDataName.RELEASE_VERYHARD, out record))
                    {
                        Threshold = int.Parse(titleData.titleData[TitleDataName.RELEASE_THRESHOLD_VERYHARD]);
                        TargetDifficult = PlayerDataName.RELEASE_VERYHARD;
                    }
                    break;
                default: break;
            }
        }
        // 閾値を超えたので解放する
        if (Threshold <= scoreManager.GameScore)
        {
            playerData.SetPlayerData(TargetDifficult, "RELEASED");

            // 実績通知を要求する
            noticeAchievement.RequestNotice();

            ret = true;

        }
        else if (Threshold != 99999)
        {
            // 次の解放まで届かなかった場合は何点か表示する
            nextText.gameObject.SetActive(true);
            nextText.text = nextText.text + (Threshold - scoreManager.GetCoinScore()).ToString();
        }
        else
        {
            // 開放済みまたは次がないので非表示
            nextText.gameObject.SetActive(false);
        }

        return ret; 
    }

    /// <summary>
    /// 解放エフェクトの開始
    /// </summary>
    public void StartReleaseEffect()
    {
        float size = cameraController.GetScreenRight() - cameraController.GetScreenLeft();
        size =  size / 5.0f;
        // エフェクト再生(画面の中心から1/5の位置に出す)
        releaseEffectHandle = EffekseerSystem.PlayEffect(releaseEffect,new Vector3(-size,0f));
        SoundManager.Instance.PlaySE("Release");
    }

    /// <summary>
    /// 解放エフェクトの再生が終わったかどうか
    /// </summary>
    /// <returns>true：エフェクト再生終了 false：再生中</returns>
    public bool IsReleaseEffectEnd()
    {
        if (releaseEffectHandle.exists) return false;

        return true;
    }
    /// <summary>
    /// メッセージの出現開始
    /// </summary>
    public void StartAppearMessage()
    {
        state = MESSAGE_STATE.APPEAR;

        UserDataRecord record = default;
        // 現在の難易度を見てどのオブジェクトを有効化するかを変える
        if (playerData.data.TryGetValue(PlayerDataName.SELECTED_DIFFICULT, out record))
        {
            switch (record.Value)
            {
                case DifficultName.EASY: normalReleaseText.SetActive(true); break;
                case DifficultName.NORMAL: hardReleaseText.SetActive(true); break;
                case DifficultName.HARD: veryhardReleaseText.SetActive(true); break;
                default: break;
            }
        }
    }
    /// <summary>
    /// メッセージの消失処理開始
    /// </summary>
    public void ExitMessage()
    {
        state = MESSAGE_STATE.DISAPPEAR;
    }
}
