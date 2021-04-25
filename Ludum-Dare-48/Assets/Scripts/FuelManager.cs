using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FuelManager : MonoBehaviour
{
    public TextMeshProUGUI fuelText;
    public Slider fuelBar;
    private float maxFuel = 10f;
    private float fuel;

    private void Start()
    {
        fuel = maxFuel;
    }

    public void SetFuel(float amount)
    {
        fuel = amount;
        fuel = Mathf.Clamp(fuel, 0f, maxFuel);
        fuelText.SetText("FUEL: " + Mathf.RoundToInt(fuel));
        fuelBar.value = (fuel / maxFuel);
    }

    public void DecreaseFuel(float amount)
    {
        fuel -= amount;
        fuel = Mathf.Clamp(fuel, 0f, maxFuel);
        fuelText.SetText("FUEL: " + Mathf.RoundToInt(fuel));
        fuelBar.value = (fuel / maxFuel);
    }

    public void AddFuel(float amount)
    {
        fuel += amount;
        fuel = Mathf.Clamp(fuel, 0f, maxFuel);
        fuelText.SetText("FUEL: " + Mathf.RoundToInt(fuel));
        fuelBar.value = (fuel / maxFuel);
    }

    public void FillFuel()
    {
        fuel = maxFuel;
        fuel = Mathf.Clamp(fuel, 0f, maxFuel);
        fuelText.SetText("FUEL: " + Mathf.RoundToInt(fuel));
        fuelBar.value = (fuel / maxFuel);
    }
}
