using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store1 : MonoBehaviour
{
    private void OnMouseUp()
    {
        const double price = 0.25;
        const int value = 200;

        var main = GameObject.Find("Main Camera");
        if (main.GetComponent<PayInStore>().SendMoneyToStore(price))
        {
            var coins = main.GetComponent<ProfileManager>().mProfile.GetCoins();
            coins += value;
            
            main.GetComponent<ProfileManager>().mProfile.SetCoins(coins);
        }
    }

}
