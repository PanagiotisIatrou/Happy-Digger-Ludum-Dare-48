using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SellingStation : MonoBehaviour
{
    public TextMeshProUGUI StationsText;
    private GameObject PlayerGO;
    private MoneyManager playerMoney;
    private Inventory playerInventory;
    private bool isInside = false;

    private void Start()
    {
        PlayerGO = GameManager.GetCurrentPlayer();
        playerInventory = PlayerGO.GetComponent<Inventory>();
        playerMoney = PlayerGO.GetComponent<MoneyManager>();
    }

    private void Update()
    {
        if (PlayerGO == null)
        {
            PlayerGO = GameManager.GetCurrentPlayer();
            if (PlayerGO == null)
                return;
            playerInventory = PlayerGO.GetComponent<Inventory>();
            playerMoney = PlayerGO.GetComponent<MoneyManager>();
        }

        if (isInside && Input.GetKeyDown(KeyCode.F))
        {
            int silverOres = playerInventory.GetNumberOfOresWithName("Silver");
            int goldOres = playerInventory.GetNumberOfOresWithName("Gold");
            int totalEarnings = 5 * silverOres + 20 * goldOres;

            playerMoney.AddMoney(totalEarnings);
            playerInventory.RemoveAllOres();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            isInside = true;
            StationsText.SetText("PRESS F TO SELL ALL ORES\nEARNINGS: $0");
            StationsText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            int silverOres = playerInventory.GetNumberOfOresWithName("Silver");
            int goldOres = playerInventory.GetNumberOfOresWithName("Gold");
            int totalEarnings = 5 * silverOres + 20 * goldOres;
            StationsText.SetText("PRESS F TO SELL ALL ORES\nEARNINGS: $" + totalEarnings);
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
