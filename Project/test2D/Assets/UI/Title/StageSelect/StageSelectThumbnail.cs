using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectThumbnail : MonoBehaviour
{
    Button button = default;
    [SerializeField] StageSelectParent parent = default;
    [SerializeField] string difficultName = "EASY";
    [SerializeField] PlayFabPlayerData playerData = default;
    [SerializeField] FlashingAnimeImage buttonAnime = default;
    [SerializeField] FlashingAnimeImage thumbnailAnime = default;
    [SerializeField] FlashingAnimeText textAnime = default;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {

        // 選択されていたらアニメーションをさせる
        if (parent.difficultName == difficultName)
        {
            buttonAnime.enabled = true;
            thumbnailAnime.enabled = true;
            textAnime.enabled = true;
        }
        else
        {
            buttonAnime.enabled = false;
            buttonAnime.ResetAlpha();
            thumbnailAnime.enabled = false;
            thumbnailAnime.ResetAlpha();
            textAnime.enabled = false;
            textAnime.ResetAlpha();
        }

        // 解放されているかを確認してボタンの有効無効を決める
        if (playerData.isGet)
        {
            UserDataRecord record = default;
            switch (difficultName)
            {
                case DifficultName.EASY: return;        // イージーは無条件解放
                case DifficultName.NORMAL: playerData.data.TryGetValue(PlayerDataName.RELEASE_NORMAL, out record); break;
                case DifficultName.HARD: playerData.data.TryGetValue(PlayerDataName.RELEASE_HARD, out record); break;
                case DifficultName.VERYHARD: playerData.data.TryGetValue(PlayerDataName.RELEASE_VERYHARD, out record); break;
            }

            // 未開放の場合はボタンを無効化する
            if (record == default || record.Value != "RELEASED")
            {
                button.enabled = false;
            }
        }
    }

    /// <summary>
    /// ボタン押下時の処理
    /// </summary>
    public void PushButton()
    {
        parent.SelectDifficult(difficultName);
    }
}
