using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradesStation : MonoBehaviour
{
    public TextMeshProUGUI StationsText;
    private bool isInside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            isInside = true;
            StationsText.SetText("PRESS F TO ENTER SHOP");
            StationsText.gameObject.SetActive(true);
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
