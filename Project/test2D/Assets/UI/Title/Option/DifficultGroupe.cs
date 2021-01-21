using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class DifficultGroupe : MonoBehaviour
{
    [SerializeField] private Toggle normalToggle = default;
    [SerializeField] private Toggle hardToggle = default;
    [SerializeField] private Toggle veryhardToggle = default;
    [SerializeField] private PlayFabPlayerData playerData = default;

    public string selectedDifficult { get; private set; } = DifficultName.EASY;

    private bool isInit = false;


    void Update()
    {
        // 難易度選択
        SelectDifficult();

        // 初期選択
        SelectFirst();
    }
    private void SelectDifficult()
    {
        // 選択難易度の変更
        if (normalToggle.isOn)
        {
            selectedDifficult = DifficultName.NORMAL;
        }
        else if (hardToggle.isOn)
        {
            selectedDifficult = DifficultName.HARD;
        }
        else if (veryhardToggle.isOn)
        {
            selectedDifficult = DifficultName.VERYHARD;
        }
        else
        {
            selectedDifficult = DifficultName.EASY;
        }
    }
    private void SelectFirst()
    {
        if (playerData.m_isGet && !isInit)
        {
            UserDataRecord record = default;
            // 最初に選ばれている難易度選択チェックを入れる
            if (playerData.m_Data.TryGetValue(PlayerDataName.SELECTED_DIFFICULT, out record))
            {
                switch (record.Value)
                {
                    case DifficultName.NORMAL:
                        normalToggle.isOn = true;
                        break;
                    case DifficultName.HARD:
                        hardToggle.isOn = true;
                        break;
                    case DifficultName.VERYHARD:
                        veryhardToggle.isOn = true;
                        break;
                    default:
                        // 選択はイージーのまま(初期値)
                        break;
                }
            }
            isInit = true;
        }
    }
}
