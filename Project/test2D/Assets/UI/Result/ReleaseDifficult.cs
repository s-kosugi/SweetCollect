using Effekseer;
using PlayFab.ClientModels;
using UnityEngine;

public class ReleaseDifficult : MonoBehaviour
{
    [SerializeField] int NormalReleaseThreshold = 80;
    [SerializeField] int HardReleaseThreshold = 150;
    [SerializeField] PlayFabPlayerData playerData = default;
    [SerializeField] EffekseerEffectAsset releaseEffect = default;
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
        }
        else
        {
            // outbackで出現させる
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
        }else
        {
            // Inbackで退場させる
        }
    }
    public bool isShowUnlockMessage()
    {
        bool ret = false;
        UserDataRecord record = default;
        int Threshold = 99999;
        // 現在の難易度を見てスコアを超えているかどうかを確認する。
        if (playerData.m_Data.TryGetValue(PlayerDataName.SELECTED_DIFFICULT, out record))
        {
            switch (record.Value)
            {
                case DifficultName.EASY: Threshold = NormalReleaseThreshold; break;
                case DifficultName.NORMAL: Threshold = HardReleaseThreshold; break;
                //case DifficultName.HARD: break;
                default: break;
            }
        }
        // 閾値を超えたので解放する
        if (Threshold <= scoreManager.GetScore())
            ret = true;

        return ret; 
    }
    public void StartReleaseEffect()
    {
        // 再生中の為重複して表示しない
        if (releaseEffectHandle.enabled) return;

        // エフェクト再生
        releaseEffectHandle = EffekseerSystem.PlayEffect(releaseEffect,this.transform.position);
    }

    /// <summary>
    /// 解放エフェクトの再生が終わったかどうか
    /// </summary>
    /// <returns>true：エフェクト再生終了 false：再生中</returns>
    public bool IsReleaseEffectEnd()
    {
        if (releaseEffectHandle.enabled) return false;
        return true;
    }
}
