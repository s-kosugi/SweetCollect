using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleFontRotate : MonoBehaviour
{
    [SerializeField] float RotateSpeed = 10.0f;
    [SerializeField] float RotateStart = 0f;
    [SerializeField] float RotateMaxAngle = 30.0f;
    private float RotateAngle = 0f;

    void Start()
    {
        RotateAngle = RotateStart;
    }


    void Update()
    {
        RotateAngle += RotateSpeed * Time.deltaTime;
        Vector3 vec = transform.localEulerAngles;
        vec.z = Mathf.Sin(RotateAngle * Mathf.Deg2Rad) * RotateMaxAngle;
        transform.localEulerAngles = vec;
    }
}
