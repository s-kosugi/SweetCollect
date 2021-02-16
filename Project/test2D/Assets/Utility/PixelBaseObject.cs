using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelBaseObject : MonoBehaviour
{
    private Vector3 cashPosition;

    void LateUpdate()
    {
        // 表示する時に、表示座標の少数点を切る。
        cashPosition = transform.localPosition;
        transform.localPosition = new Vector3(
                        Mathf.RoundToInt(cashPosition.x),
                        Mathf.RoundToInt(cashPosition.y),
                        Mathf.RoundToInt(cashPosition.z)
                   );
    }

    void OnRenderObject()
    {
        transform.localPosition = cashPosition;
    }
}
