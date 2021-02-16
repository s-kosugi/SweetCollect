using UnityEngine;

/// <summary>
/// 配置オブジェクトの自動削除クラス
/// </summary>
public class ArrangementAutoDelete : MonoBehaviour
{
    void Update()
    {
        // 子供がいなくなったら配置プレハブを削除する
        if (transform.childCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
