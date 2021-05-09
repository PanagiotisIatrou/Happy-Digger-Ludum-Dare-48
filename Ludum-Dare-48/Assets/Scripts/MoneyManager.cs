using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    private TextMeshProUGUI moneyText;
    private static int money = 50;

    private void Start()
    {
        moneyText = GameManager.Instance.MoneyText;
    }

    public int GetMoney()
    {
        return money;
    }

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
