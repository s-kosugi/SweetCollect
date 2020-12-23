﻿using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class DifficultRadioButton : MonoBehaviour
{
    [SerializeField] PlayFabPlayerData playerData = default;
    [SerializeField] string ReleaseKey = PlayerDataName.RELEASE_NORMAL;
    private Toggle toggle = default;
    private bool isInit = false;


    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.interactable = false;
    }


    void Update()
    {
        if (playerData.m_isGet && !isInit)
        {
            UserDataRecord record = default;
            // 該当難易度が開放済みかどうかをチェックしてボタンの有効性を決める
            if (playerData.m_Data.TryGetValue(ReleaseKey, out record))
            {
                if( record.Value == "RELEASED")
                    toggle.interactable = true;
            }

            isInit = true;
        }
    }
}
