using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListeners : MonoBehaviour
{
    public void OnXButtonListener()
    {
        UpgradesStation.SetShopState(false);
    }

    public void OnUpgradeFuelTankButtonListener()
    {
        Shop.UpgradeFuelTank();
    }

    public void OnUpgradeDrillButtonListener()
    {
        Shop.UpgradeDrill();
    }

    public void OnUpgradeEngineButtonListener()
    {
        Shop.UpgradeEngine();
    }
}
