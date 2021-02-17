using UnityEngine;

/// <summary>
/// 特定タグに当った時に消去する
/// </summary>
public class CollisionEnterDestroy : MonoBehaviour
{
    [SerializeField] string TargetTagName = "Player";


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == TargetTagName)
        {
            Destroy(this.gameObject);
        }
    }
}
