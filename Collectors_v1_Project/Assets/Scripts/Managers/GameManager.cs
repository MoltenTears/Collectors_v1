using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("House Materials")]
    [SerializeField] public Material houseSingleMaterial;
    [SerializeField] public Material houseFamilyMaterial;
    [SerializeField] public Material houseShareMaterial;

    [Header("Household Garbage Details - speed")]
    [SerializeField] public int garbageDivisor;
    [SerializeField] public float garbageSpeedSingle;
    [SerializeField] public float garbageSpeedFamily;
    [SerializeField] public float garbageSpeedShare;

    [Header("Household Garbage Details - volume")]
    [SerializeField] public float maxGarbageSingle;
    [SerializeField] public float maxGarbageFamily;
    [SerializeField] public float maxGarbageShare;

    [Header("Bins")]
    [SerializeField] public int binSizeSmall;
    [SerializeField] public int binSizeMedium;
    [SerializeField] public int binSizeLarge;

    [Header("City Waste Levels")]
    [SerializeField] public List<GarbageManager> houseGarbage = new List<GarbageManager>();
    [SerializeField] public float cityGarbageLevel;
    [SerializeField] public float maxCityGarbageLevel;

    private void Start()
    {
        
    }

    private void Update()
    {
        GetHouses();
        GetGarbage();
    }

    private void GetHouses()
    {
        // flush the list
        houseGarbage.Clear();

        // find the houses
        GarbageManager[] garbage = GameObject.FindObjectsOfType<GarbageManager>();
        foreach (GarbageManager garbageManager in garbage)
        {
            // add them back into the list
            houseGarbage.Add(garbageManager);
        }
    }

    private void GetGarbage()
    {
        // reset the count
        cityGarbageLevel = 0.0f;
        maxCityGarbageLevel = 0.0f;

        // for each house...
        for (int i = 0; i < houseGarbage.Count; i++)
        {
            // ... add their garbage to the city value
            cityGarbageLevel += houseGarbage[i].garbageLevel;
            maxCityGarbageLevel += houseGarbage[i].maxGarbageLevel;
        }
    }
}
