using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayInStore : MonoBehaviour
{
    public bool SendMoneyToStore(double value)
    {
        Debug.Log($"Send money to store: ${value}");
        return true;
    }
}
