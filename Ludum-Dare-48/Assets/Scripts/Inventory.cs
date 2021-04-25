using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> ores = new Dictionary<string, int>();

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
