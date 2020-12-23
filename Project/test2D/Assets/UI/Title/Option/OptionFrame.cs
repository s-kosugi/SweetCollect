using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionFrame : MonoBehaviour
{
    [SerializeField] Vector3 disablePos = default;
    [SerializeField] Vector3 enablePos = default;

    void Start()
    {
        transform.localPosition = disablePos;
    }

    public void DisableFrame()
    {
        transform.localPosition = disablePos;
    }

    public void EnableFrame()
    {
        transform.localPosition = enablePos;
    }
}
