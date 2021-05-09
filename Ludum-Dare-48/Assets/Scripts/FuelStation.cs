using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FuelStation : MonoBehaviour
{
    public TextMeshProUGUI StationsText;
    private GameObject PlayerGO;
    private FuelManager playerFuel;
    private MoneyManager playerMoney;
    private bool isInside = false;

    private void Start()
    {
        playerMoney = GameManager.Instance.MoneyManagerSC;
    }

    private void Update()
    {
        if (isInside)
        {
            float fuelToBuy = playerFuel.GetMissingFuel();
            int cost = Mathf.CeilToInt(fuelToBuy);
            StationsText.SetText("PRESS F TO REFUEL\nCOST: $" + cost);
        }

        if (PlayerGO == null)
        {
            PlayerGO = GameManager.GetCurrentPlayer();
            if (PlayerGO == null)
                return;
            playerFuel = PlayerGO.GetComponent<FuelManager>();
        }

        if (isInside && Input.GetKeyDown(KeyCode.F))
        {
            float fuelToBuy = playerFuel.GetMissingFuel();
            int cost = Mathf.CeilToInt(fuelToBuy);
            if (cost > 0 && playerMoney.GetMoney() > 0)
            {
                float perc = (float)playerMoney.GetMoney() / cost;
                AudioSource.PlayClipAtPoint(GameManager.Instance.CoinPickupSound, PlayerGO.transform.position);
                if (playerMoney.GetMoney() >= cost)
                {
                    playerFuel.FillFuel();
                    playerMoney.DecreaseMoney(cost);
                }
                else if (playerMoney.GetMoney() > 0)
                {
                    playerFuel.AddFuel(fuelToBuy * perc);
                    playerMoney.DecreaseMoney(playerMoney.GetMoney());
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            isInside = true;
            AudioSource.PlayClipAtPoint(GameManager.Instance.ClickSound, collision.transform.position);
            StationsText.SetText("PRESS F TO REFUEL\nCOST: $0");
            StationsText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            isInside = false;
            AudioSource.PlayClipAtPoint(GameManager.Instance.ClickSound, collision.transform.position);
            StationsText.gameObject.SetActive(false);
        }
    }
}
