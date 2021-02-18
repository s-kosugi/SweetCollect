using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルトコインオブジェクト出現クラス
/// </summary>
public class ResultAppearCoin : MonoBehaviour
{
    [SerializeField] ResultSceneManager resultScene = default;
    [SerializeField] float AnimeTime = 1.0f;
    float AnimeCount = 0f;
    [SerializeField] float DownPosition = 50.0f;
    [SerializeField] Image CoinImage = default;
    [SerializeField] TextMeshProUGUI textMesh = default;
    private bool SoundFlag = false;


    void Start()
    {
        textMesh.color = new Color(1f,1f,1f,0f);
        CoinImage.color = new Color(1f, 1f, 1f, 0f);
    }


    void Update()
    {
        if (resultScene.state == ResultSceneManager.STATE.MAIN)
        {
            // 音を一度だけならす
            if (!SoundFlag)
            {
                SoundManager.Instance.PlaySE("Coin");
                SoundFlag = true;
            }
            AnimeCount += Time.deltaTime;
            if (AnimeTime <= AnimeCount)
            {
                AnimeCount = AnimeTime;
            }
            // イージングで移動させる
            float posY = Easing.OutCubic(AnimeCount, AnimeTime, 0f, DownPosition);
            transform.localPosition = new Vector3(transform.localPosition.x,posY,transform.localPosition.z);

            // イージングで出現させる
            float alpha = Easing.Linear(AnimeCount, AnimeTime, 1f, 0f);
            textMesh.color = new Color(1f, 1f, 1f, alpha);
            CoinImage.color = new Color(1f, 1f, 1f, alpha);
        }
    }
}
