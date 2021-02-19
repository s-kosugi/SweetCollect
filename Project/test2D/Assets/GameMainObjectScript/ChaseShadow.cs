using UnityEngine;

/// <summary>
/// 影の追従処理
/// </summary>
public class ChaseShadow : MonoBehaviour
{
    GameObject GroundObject;
    [SerializeField] float OffsetY = 14f;
    void Start()
    {
        GroundObject = GameObject.Find("Ground");
    }

    void Update()
    {
        // 地面の座標を持ってくる
        this.transform.position = new Vector3(this.transform.position.x,GroundObject.transform.position.y + OffsetY, this.transform.position.z);
    }
}
