using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advertisements_Bonus : MonoBehaviour
{
    private Advertisements Advertisements = null;
    private PlayFabVirtualCurrency PlayFabVirtualCurrency = null;
    private Money_Text money = null;
    bool IsAddBonus = false;     //加算済みかどうか
    // Start is called before the first frame update
    void Start()
    {
        Advertisements = gameObject.GetComponent<Advertisements>();
        PlayFabVirtualCurrency = GameObject.Find("PlayFabVirtualCurrency").GetComponent<PlayFabVirtualCurrency>();
        money = GameObject.Find("ShopCanvas/Player_Money/Money_Buck/Money_Text").GetComponent<Money_Text>();
        IsAddBonus = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Advertisements && Advertisements.isShow && Advertisements && !IsAddBonus)
        {
            Debug.Log("Add:AdsBonus");
            PlayFabVirtualCurrency.AddUserVirtualCurrency("HA", Advertisements.plusmoney);
            IsAddBonus = true;

            money.RequestMoney();
        }
    }
}
