using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> ores = new Dictionary<string, int>();
    private GameObject InventoryGO;
    private TextMeshProUGUI ironText;
    private TextMeshProUGUI goldText;
    private TextMeshProUGUI emeraldText;
    private TextMeshProUGUI redIronText;
    private TextMeshProUGUI lapisText;
    private TextMeshProUGUI fossilsText;

    private bool isOpen = false;

    private void Start()
    {
        InventoryGO = GameManager.Instance.InventoryGO;
        ironText = InventoryGO.transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
        goldText = InventoryGO.transform.GetChild(0).GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>();
        emeraldText = InventoryGO.transform.GetChild(0).GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>();
        redIronText = InventoryGO.transform.GetChild(0).GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>();
        lapisText = InventoryGO.transform.GetChild(0).GetChild(7).GetChild(1).GetComponent<TextMeshProUGUI>();
        fossilsText = InventoryGO.transform.GetChild(0).GetChild(8).GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (isOpen)
            UpdateInventoryValues();
    }

    public void Open()
    {
        isOpen = true;
        InventoryGO.SetActive(true);
        UpdateInventoryValues();
    }

    public void Close()
    {
        isOpen = false;
        InventoryGO.SetActive(false);
    }

    public bool IsOpen()
    {
        return isOpen;
    }

    private void UpdateInventoryValues()
    {
        int ironOres;
        ores.TryGetValue("Iron", out ironOres);
        ironText.SetText("IRON ORE: " + ironOres);

        int goldOres;
        ores.TryGetValue("Gold", out goldOres);
        goldText.SetText("GOLD ORE: " + goldOres);

        int emeraldOres;
        ores.TryGetValue("Emerald", out emeraldOres);
        emeraldText.SetText("EMERALD ORE: " + emeraldOres);

        int redIronOres;
        ores.TryGetValue("Red Iron", out redIronOres);
        redIronText.SetText("RED IRON ORE: " + redIronOres);

        int lapisOres;
        ores.TryGetValue("Lapis", out lapisOres);
        lapisText.SetText("LAPIS ORES: " + lapisOres);

        int fossiles;
        ores.TryGetValue("Fossile", out fossiles);
        fossilsText.SetText("FOSSILS: " + fossiles);
    }

    public int GetNumberOfOresWithName(string name)
    {
        if (!ores.ContainsKey(name))
            return 0;
        else
            return ores[name];
    }

    public void AddOre(string name)
    {
        if (!ores.ContainsKey(name))
            ores.Add(name, 1);
        else
            ores[name]++;
    }

    public void RemoveOre(string name)
    {
        ores[name]--;
    }

    public void RemoveAllOresWithName(string name)
    {
        ores.Remove(name);
    }

    public void RemoveAllOres()
    {
        ores = new Dictionary<string, int>();
    }
}
