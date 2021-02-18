using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayModeFadeButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_WEBGL
        transform.gameObject.SetActive(true);
#endif
#if UNITY_ANDROID
        transform.gameObject.SetActive(false);
#endif
    }
}
