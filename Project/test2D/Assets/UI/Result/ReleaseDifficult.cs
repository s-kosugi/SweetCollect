using Effekseer;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class ReleaseDifficult : MonoBehaviour
{
    [SerializeField] int NormalReleaseThreshold = 80;
    [SerializeField] int HardReleaseThreshold = 150;
    [SerializeField] PlayFabPlayerData playerData = default;
    [SerializeField] EffekseerEffectAsset releaseEffect = default;
    [SerializeField] GameObject normalReleaseText = default;
    [SerializeField] GameObject hardReleaseText = default;
    [SerializeField] GameObject veryhardReleaseText = default;
    [SerializeField] TextMeshProUGUI nextText = default;
    [SerializeField] NoticeAchievement noticeAchievement = default;
    private EffekseerHandle releaseEffectHandle = default;
    private ScoreManager scoreManager = default;
    [SerializeField] float AppearTime = 1.0f;
    private float AppearCounter = 0f;
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
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        state = MESSAGE_STATE.HIDE;
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
        AppearCounter = 0f;

    }
    private void AppearMessage()
    {
        AppearCounter += Time.deltaTime;
        if (AppearCounter >= AppearTime)
        {
            state = MESSAGE_STATE.WAIT;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            AppearCounter = 0f;
        }
        else
        {
            // outbackで出現させる
            float scale = Easing.OutBack(AppearCounter, AppearTime, 1.0f, 0.0f, 0.8f);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
    private void WaitMessage()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        AppearCounter = 0f;
    }
    private void DisappearMessage()
    {
        AppearCounter += Time.deltaTime;
        if (AppearCounter >= AppearTime)
        {
            state = MESSAGE_STATE.HIDE;
            transform.localScale = Vector3.zero;
            AppearCounter = 0f;
        }
        else
        {
            // Inbackで退場させる
            float scale = Easing.InBack(AppearCounter, AppearTime, 0.0f, 1.0f, 0.8f);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
    public bool isShowUnlockMessage()
    {
        bool ret = false;
        UserDataRecord record = default;
        int Threshold = 99999;
        string TargetDifficult = PlayerDataName.RELEASE_NORMAL;

        // 現在の難易度を見てスコアを超えているかどうかを確認する。
        if (playerData.m_Data.TryGetValue(PlayerDataName.SELECTED_DIFFICULT, out record))
        {
            switch (record.Value)
            {
                case DifficultName.EASY:
                    // 解放対象難易度が解放済みかどうかをチェックする(キーが無ければ未開放)
                    if (!playerData.m_Data.TryGetValue(PlayerDataName.RELEASE_NORMAL, out record))
                    {
                        Threshold = NormalReleaseThreshold;
                        TargetDifficult = PlayerDataName.RELEASE_NORMAL;
                    }
                    break;
                case DifficultName.NORMAL:
                    // 解放対象難易度が解放済みかどうかをチェックする(キーが無ければ未開放)
                    if (!playerData.m_Data.TryGetValue(PlayerDataName.RELEASE_HARD, out record))
                    {
                        Threshold = HardReleaseThreshold;
                        TargetDifficult = PlayerDataName.RELEASE_HARD;
                    }
                    break;
                //case DifficultName.HARD: 
                        // 解放対象難易度が解放済みかどうかをチェックする(キーが無ければ未開放)
                //    if (!playerData.m_Data.TryGetValue(PlayerDataName.RELEASE_VERYHARD, out record))
                //    {
                //        Threshold = VeryHardReleaseThreshold;
                //        TargetDifficult = PlayerDataName.RELEASE_VERYHARD;
                //    }
                //    break;
                default: break;
            }
        }
        // 閾値を超えたので解放する
        if (Threshold <= scoreManager.GameScore)
        {
            playerData.SetPlayerData(TargetDifficult, "RELEASED");
            // オプションに通知を送る
            playerData.SetPlayerData(PlayerDataName.NOTICE_OPTION, "TRUE");

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
    public void StartReleaseEffect()
    {
        // エフェクト再生
        releaseEffectHandle = EffekseerSystem.PlayEffect(releaseEffect,new Vector3(0f,0f,0f));
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
        if (playerData.m_Data.TryGetValue(PlayerDataName.SELECTED_DIFFICULT, out record))
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
