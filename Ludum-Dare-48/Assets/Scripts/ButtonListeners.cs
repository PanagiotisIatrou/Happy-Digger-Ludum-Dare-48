using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListeners : MonoBehaviour
{
    public void OnXUpgradesButtonListener()
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

    public void OnInventoryButtonListener()
    {
        GameManager.GetCurrentPlayer().GetComponent<Inventory>().Open();
    }

    public void OnXInventoryButtonListener()
    {
        GameManager.GetCurrentPlayer().GetComponent<Inventory>().Close();
    }

    public void OnPauseButtonListener()
    {

    }
}
