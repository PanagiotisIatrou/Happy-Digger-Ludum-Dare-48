using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    private int money = 100;

    public void SetMoney(int amount)
    {
        money = amount;
        moneyText.SetText("$" + money);
    }

    public void DecreaseMoney(int amount)
    {
        money -= amount;
        moneyText.SetText("$" + money);
    }

    public void AddMoney(int amount)
    {
        money += amount;
        moneyText.SetText("$" + money);
    }
}
