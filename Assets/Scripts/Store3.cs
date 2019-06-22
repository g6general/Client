using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store3 : MonoBehaviour
{
    private void OnMouseUp()
    {
        const double price = 0.75;
        const int value = 700;

        var main = GameObject.Find("Main Camera");
        if (main.GetComponent<PayInStore>().SendMoneyToStore(price))
        {
            var coins = main.GetComponent<ProfileManager>().mProfile.GetCoins();
            coins += value;
            
            main.GetComponent<ProfileManager>().mProfile.SetCoins(coins);
        }
    }

}
