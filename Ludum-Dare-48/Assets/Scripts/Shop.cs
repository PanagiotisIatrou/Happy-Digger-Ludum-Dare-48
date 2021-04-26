using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    // Singleton
    private static Shop _instance;
    public static Shop Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Shop>();
            }

            return _instance;
        }

    }

    public TextMeshProUGUI FuelTankUpgradeText;
    public TextMeshProUGUI DrillUpgradeText;
    public TextMeshProUGUI EngineUpgradeText;

    private int[] fuelTankPrices = { 50, 200, 500, 1000 };
    private int[] drillPrices = { 50, 200, 500, 1000 };
    private int[] enginePrices = { 50, 200, 500, 1000 };

    private float[] fuelTankLevels = { 15, 30, 50, 100 };
    private float[] drillLevels = { 0.75f, 0.562f, 0.4215f, 0.3161f };
    private float[] engineAccelerationLevels = { 1150, 1300, 1450, 1600 };
    private float[] engineMaxSpeedLevels = { 6f, 7f, 8f, 9f };

    private int currentFuelTankLevel = 0;
    private int currentDrillLevel = 0;
    private int currentEngineLevel = 0;

    public static void UpgradeFuelTank()
    {
        MoneyManager moneyManager = GameManager.GetCurrentPlayer().GetComponent<MoneyManager>();
        int price = Instance.fuelTankPrices[Instance.currentFuelTankLevel];
        if (Instance.currentFuelTankLevel < Instance.fuelTankPrices.Length
         && moneyManager.GetMoney() >= price)
        {
            Instance.currentFuelTankLevel++;
            moneyManager.DecreaseMoney(price);
            AudioSource.PlayClipAtPoint(GameManager.Instance.CoinPickupSound, GameManager.GetCurrentPlayer().transform.position);
            Instance.FuelTankUpgradeText.SetText("UPGRADE $" + Instance.fuelTankPrices[Instance.currentFuelTankLevel]);
            GameManager.GetCurrentPlayer().GetComponent<FuelManager>().SetMaxFuel(Instance.fuelTankLevels[Instance.currentFuelTankLevel - 1]);
        }
    }

    public static void UpgradeDrill()
    {
        MoneyManager moneyManager = GameManager.GetCurrentPlayer().GetComponent<MoneyManager>();
        int price = Instance.drillPrices[Instance.currentDrillLevel];
        if (Instance.currentDrillLevel < Instance.drillPrices.Length
         && moneyManager.GetMoney() >= price)
        {
            Instance.currentDrillLevel++;
            moneyManager.DecreaseMoney(price);
            AudioSource.PlayClipAtPoint(GameManager.Instance.CoinPickupSound, GameManager.GetCurrentPlayer().transform.position);
            Instance.DrillUpgradeText.SetText("UPGRADE $" + Instance.drillPrices[Instance.currentDrillLevel]);
            GameManager.GetCurrentPlayer().GetComponent<PlayerMovement>().SetDrillingTime(Instance.drillLevels[Instance.currentDrillLevel - 1]);
        }
    }

    public static void UpgradeEngine()
    {
        MoneyManager moneyManager = GameManager.GetCurrentPlayer().GetComponent<MoneyManager>();
        int price = Instance.enginePrices[Instance.currentEngineLevel];
        if (Instance.currentEngineLevel < Instance.enginePrices.Length
         && moneyManager.GetMoney() >= price)
        {
            Instance.currentEngineLevel++;
            moneyManager.DecreaseMoney(price);
            AudioSource.PlayClipAtPoint(GameManager.Instance.CoinPickupSound, GameManager.GetCurrentPlayer().transform.position);
            Instance.EngineUpgradeText.SetText("UPGRADE $" + Instance.enginePrices[Instance.currentEngineLevel]);
            GameManager.GetCurrentPlayer().GetComponent<PlayerMovement>().SetMaxSpeedAndAcceleration(Instance.engineMaxSpeedLevels[Instance.currentEngineLevel - 1], Instance.engineAccelerationLevels[Instance.currentEngineLevel - 1]);
        }
    }
}
