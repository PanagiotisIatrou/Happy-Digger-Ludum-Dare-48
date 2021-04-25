using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListeners : MonoBehaviour
{
    public void OnXButtonListener()
    {
        UpgradesStation.SetShopState(false);
    }
}
