using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultVC : MonoBehaviour
{
    private PlayFabVirtualCurrency playFabVirtualCurrency= null;
    private TextMeshProUGUI text = null;

    void Start()
    {
        playFabVirtualCurrency = GameObject.Find("PlayFabVirtualCurrency").GetComponent<PlayFabVirtualCurrency>();
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (playFabVirtualCurrency && text)
        {
            // 仮想通貨情報が取得済みかどうか
            if (playFabVirtualCurrency.isGet)
            {
                if (playFabVirtualCurrency.VirtualCurrency.ContainsKey("HA"))
                {
                    text.text = playFabVirtualCurrency.VirtualCurrency["HA"].ToString();
                }
            }
        }
    }
}
