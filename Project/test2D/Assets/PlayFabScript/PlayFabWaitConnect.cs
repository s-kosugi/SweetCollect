using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// PlayFab通信待ち管理クラス
public class PlayFabWaitConnect : MonoBehaviour
{
    private Dictionary<Transform,bool> m_WaitList = default;

    void Start()
    {
        m_WaitList = new Dictionary<Transform, bool>();
        foreach( Transform obj in transform)
        {
            Debug.Log(obj.name);
            // 通信待ちリストを作成する
            m_WaitList.Add(obj,false);
        }
    }

    void Update()
    {
        
    }

    // 通信待ちを設定する
    public void SetWait(Transform obj,bool wait)
    {
        // ウェイトリストに設定する
        if( m_WaitList.ContainsKey(obj)) m_WaitList[obj] = wait;
    }
    public bool IsWait()
    {
        // 通信待ちがあれば通信待ちを返す
        if (m_WaitList.ContainsValue(true)) return true;

        return false;
    }
}
