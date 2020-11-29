using Effekseer;
using UnityEngine;

public class BreakHitObject : MonoBehaviour
{
    [SerializeField] int SubScore = -100;
    [SerializeField] EffekseerEffectAsset Effect = null;
    [SerializeField] string BreakSEName = default;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーに当たったら破壊される
        if (collision.gameObject.tag == "Player")
        {
            // スコアの減算
            transform.root.GetComponent<GameMainManager>().m_ScoreManager.AddScore(SubScore);

            // 破壊エフェクトの再生
            EffekseerSystem.PlayEffect(Effect, transform.position);

            // SEの再生
            SoundManager.Instance.PlaySE(BreakSEName);


            Destroy(this.gameObject);
        }
    }
}
