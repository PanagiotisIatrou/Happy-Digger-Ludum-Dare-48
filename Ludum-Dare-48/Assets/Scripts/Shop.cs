using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private float[] engineAccelerationLevels = { 1100, 1200, 1300, 1400 };
    private float[] engineMaxSpeedLevels = { 5.8f, 6.6f, 7.4f, 8.2f };

    private int currentFuelTankLevel = 0;
    private int currentDrillLevel = 0;
    private int currentEngineLevel = 0;

    public static void UpgradeFuelTank()
    {
        if (Instance.currentFuelTankLevel == Instance.fuelTankPrices.Length)
             return;

        MoneyManager moneyManager = GameManager.GetCurrentPlayer().GetComponent<MoneyManager>();
        int price = Instance.fuelTankPrices[Instance.currentFuelTankLevel];
        if (moneyManager.GetMoney() >= price)
        {
            Instance.currentFuelTankLevel++;
            moneyManager.DecreaseMoney(price);
            AudioSource.PlayClipAtPoint(GameManager.Instance.CoinPickupSound, GameManager.GetCurrentPlayer().transform.position);
            GameManager.GetCurrentPlayer().GetComponent<FuelManager>().SetMaxFuel(Instance.fuelTankLevels[Instance.currentFuelTankLevel - 1]);

            if (Instance.currentFuelTankLevel != Instance.fuelTankPrices.Length)
            {
                Instance.FuelTankUpgradeText.SetText("UPGRADE $" + Instance.fuelTankPrices[Instance.currentFuelTankLevel]);
            }
            else
            {
                Instance.FuelTankUpgradeText.SetText("MAXED OUT");
                Instance.FuelTankUpgradeText.transform.parent.GetComponent<Button>().interactable = false;
            }
        }
    }

    public static void UpgradeDrill()
    {
        if (Instance.currentDrillLevel == Instance.drillPrices.Length)
            return;

        MoneyManager moneyManager = GameManager.GetCurrentPlayer().GetComponent<MoneyManager>();
        int price = Instance.drillPrices[Instance.currentDrillLevel];
        if (moneyManager.GetMoney() >= price)
        {
            Instance.currentDrillLevel++;
            moneyManager.DecreaseMoney(price);
            AudioSource.PlayClipAtPoint(GameManager.Instance.CoinPickupSound, GameManager.GetCurrentPlayer().transform.position);
            GameManager.GetCurrentPlayer().GetComponent<PlayerMovement>().SetDrillingTime(Instance.drillLevels[Instance.currentDrillLevel - 1]);

            if (Instance.currentDrillLevel != Instance.drillPrices.Length)
            {
                Instance.DrillUpgradeText.SetText("UPGRADE $" + Instance.drillPrices[Instance.currentDrillLevel]);
            }
            else
            {
                Instance.DrillUpgradeText.SetText("MAXED OUT");
                Instance.DrillUpgradeText.transform.parent.GetComponent<Button>().interactable = false;
            }
        }
    }

    public static void UpgradeEngine()
    {
        if (Instance.currentEngineLevel == Instance.enginePrices.Length)
            return;

        MoneyManager moneyManager = GameManager.GetCurrentPlayer().GetComponent<MoneyManager>();
        int price = Instance.enginePrices[Instance.currentEngineLevel];
        if (moneyManager.GetMoney() >= price)
        {
            Instance.currentEngineLevel++;
            moneyManager.DecreaseMoney(price);
            AudioSource.PlayClipAtPoint(GameManager.Instance.CoinPickupSound, GameManager.GetCurrentPlayer().transform.position);
            GameManager.GetCurrentPlayer().GetComponent<PlayerMovement>().SetMaxSpeedAndAcceleration(Instance.engineMaxSpeedLevels[Instance.currentEngineLevel - 1], Instance.engineAccelerationLevels[Instance.currentEngineLevel - 1]);
            if (Instance.currentEngineLevel != Instance.enginePrices.Length)
            {
                Instance.EngineUpgradeText.SetText("UPGRADE $" + Instance.enginePrices[Instance.currentEngineLevel]);
            }
            else
            {
                Instance.EngineUpgradeText.SetText("MAXED OUT");
                Instance.EngineUpgradeText.transform.parent.GetComponent<Button>().interactable = false;
            }
        }
    }
}
