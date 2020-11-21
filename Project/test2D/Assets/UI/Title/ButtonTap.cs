using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTap : MonoBehaviour
{
    public void Click()
    {
        SoundManager.Instance.PlaySE("Tap");
    }
}
