using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverButton : MonoBehaviour
{
    // クリックされた時
    public void Click()
    {
        SoundManager.Instance.PlaySE("Tap");
    }
}
