using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateChildSweet : MonoBehaviour
{
    [SerializeField] GameObject SweetObject = null;
    [SerializeField] Vector2 SweetPos = default;
    void Start()
    {
        Vector3 pos = Vector3.zero;
        pos.x = SweetPos.x;
        pos.y = SweetPos.y;
        GameObject obj = Instantiate(SweetObject, this.transform);
        obj.transform.position += pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
