using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSoundObject : MonoBehaviour
{
    [SerializeField] string seFileName = "";

    void Start()
    {
        SoundManager.Instance.PlaySE(seFileName);
    }
}
