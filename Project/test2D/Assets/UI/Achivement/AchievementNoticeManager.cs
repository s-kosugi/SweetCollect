using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementNoticeManager : MonoBehaviour
{
    [SerializeField] GameObject achievementNoticeWindow = default;
    [SerializeField] BaseScene scene = default;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (scene != default)
        {
            if (scene.fadeState == BaseScene.FADE_STATE.NONE)
            {
                // 生成していない場合
                if (!transform.Find("NoticeFrame"))
                {

                }
            }
        }
    }
}
