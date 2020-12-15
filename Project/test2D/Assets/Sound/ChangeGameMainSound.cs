using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGameMainSound : MonoBehaviour
{
    [SerializeField] GameMainManager mainManager = default;
    [SerializeField] float FastSpeed = 1.5f;
    [SerializeField] AdsBonus adsBonus = default; 

    void Start()
    {
        SoundManager.Instance.PlayBGM("MainGame");
    }


    void Update()
    {
        // ゲームが危険ラインになったら
        if (mainManager.GameDengerTime >= mainManager.GameTimer)
        {
            // ボーナス加算していない(ボーナスタイム中以外）場合
            if( !adsBonus.isAdd )
            // ゲームのBGMを早くする。
            SoundManager.Instance.SetBGMSpeed(FastSpeed);
        }

        // メイン状態以外になったら通常速度に戻す
        if (mainManager.state != GameMainManager.STATE.MAIN)
        {
            // ゲームのBGMを元に戻す。
            SoundManager.Instance.SetBGMSpeed(1.0f);
        }

        // リスタート時にBGMを変更する
        if (mainManager.state == GameMainManager.STATE.RESTART)
        {
            SoundManager.Instance.PlayBGM("BonusTime");
        }
    }
}
