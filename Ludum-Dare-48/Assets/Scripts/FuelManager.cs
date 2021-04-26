using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FuelManager : MonoBehaviour
{
    private Slider fuelSlider;
    private TextMeshProUGUI fuelText;
    private float maxFuel = 8f;
    private float fuel;

    private void Start()
    {
        fuelSlider = GameManager.Instance.FuelSlider;
        fuelText = fuelSlider.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        SetFuel(maxFuel);
    }

    public float GetFuel()
    {
        return fuel;
    }

    public float GetMissingFuel()
    {
        return maxFuel - fuel;
    }

    public void SetFuel(float amount)
    {
        fuel = amount;
        fuel = Mathf.Clamp(fuel, 0f, maxFuel);
        fuelText.SetText("FUEL: " + Mathf.RoundToInt(fuel));
        fuelSlider.value = (fuel / maxFuel);
    }

    public void DecreaseFuel(float amount)
    {
        fuel -= amount;
        fuel = Mathf.Clamp(fuel, 0f, maxFuel);
        fuelText.SetText("FUEL: " + Mathf.RoundToInt(fuel));
        fuelSlider.value = (fuel / maxFuel);
    }

    public void AddFuel(float amount)
    {
        fuel += amount;
        fuel = Mathf.Clamp(fuel, 0f, maxFuel);
        fuelText.SetText("FUEL: " + Mathf.RoundToInt(fuel));
        fuelSlider.value = (fuel / maxFuel);
    }

    public void FillFuel()
    {
        fuel = maxFuel;
        fuel = Mathf.Clamp(fuel, 0f, maxFuel);
        fuelText.SetText("FUEL: " + Mathf.RoundToInt(fuel));
        fuelSlider.value = (fuel / maxFuel);
    }

    public void SetMaxFuel(float amount)
    {
        maxFuel = amount;
        fuelSlider.value = (fuel / maxFuel);
    }
}
