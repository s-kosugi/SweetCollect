using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTest : MonoBehaviour
{
    PlayFabStore store = null;
    // Start is called before the first frame update
    void Start()
    {
        store = GameObject.Find("PlayFabManager").GetComponent<PlayFabStore>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // アイテムリストの表示
            store.DebugShowStoreList();
        }
        if(Input.GetMouseButtonDown(1))
        {
            // ダミーアイテムの購入
            store.BuyItem("2","HA");
        }
    }
}
