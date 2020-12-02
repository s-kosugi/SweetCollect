using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
