using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FuelStation : MonoBehaviour
{
    public TextMeshProUGUI StationsText;
    public GameObject PlayerGO;
    private FuelManager playerFuel;
    private MoneyManager playerMoney;
    private bool isInside = false;

    private void Start()
    {
        playerFuel = PlayerGO.GetComponent<FuelManager>();
        playerMoney = PlayerGO.GetComponent<MoneyManager>();
    }

    private void Update()
    {
        if (isInside && Input.GetKeyDown(KeyCode.F))
        {
            float fuelToBuy = playerFuel.GetMissingFuel();
            int cost = Mathf.CeilToInt(fuelToBuy);
            if (playerMoney.GetMoney() >= cost)
            {
                playerMoney.DecreaseMoney(cost);
                playerFuel.FillFuel();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            isInside = true;
            StationsText.SetText("PRESS F TO REFUEL\nCOST: $0");
            StationsText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            float fuelToBuy = playerFuel.GetMissingFuel();
            int cost = Mathf.CeilToInt(fuelToBuy);
            StationsText.SetText("PRESS F TO REFUEL\nCOST: $" + cost);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            isInside = false;
            StationsText.gameObject.SetActive(false);
        }
    }
}
