using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("General Variables")]
    
    [Tooltip("Larger means garbage accumulates quicker in the city")] [SerializeField] [Range(0, 1)] public float garbageMultipler;

    [Header("Bins")]
    [SerializeField] public int binSizeSmall;
    [SerializeField] public int binSizeMedium;
    [SerializeField] public int binSizeLarge;

    [Header("Display Meter Levels")]
    [SerializeField] public float cityGarbageLevel;
    [SerializeField] public float maxCityGarbageLevel;
    [SerializeField] public float citySatisfactionLevel;
    [SerializeField] public float maxCitySatisfactionLevel;
    [SerializeField] public float houseSatisfactionNone;
    [SerializeField] public float houseSatisfactionLow;
    [SerializeField] public float houseSatisfactionMedium;
    [SerializeField] public float houseSatisfactionHigh;

    [Header("House Type: SINGLE")]
    [SerializeField] public Material houseSingleMaterial;
    [SerializeField] public float garbageSpeedSingle;
    [SerializeField] public float maxGarbageSingle;
    [SerializeField] public float satisfactionRadiusSingle;
    [SerializeField] public float wasteTolleranceSingleLow;
    [SerializeField] public float wasteTolleranceSingleMedium;
    [SerializeField] public float wasteTolleranceSingleHigh;

    [Header("House Type: FAMILY")]
    [SerializeField] public Material houseFamilyMaterial;
    [SerializeField] public float garbageSpeedFamily;
    [SerializeField] public float maxGarbageFamily;
    [SerializeField] public float satisfactionRadiusFamily;
    [SerializeField] public float wasteTolleranceFamilyLow;
    [SerializeField] public float wasteTolleranceFamilyMedium;
    [SerializeField] public float wasteTolleranceFamilyHigh;

    [Header("House Type: SHARE")]
    [SerializeField] public Material houseShareMaterial;
    [SerializeField] public float garbageSpeedShare;
    [SerializeField] public float maxGarbageShare;
    [SerializeField] public float satisfactionRadiusShare;
    [SerializeField] public float wasteTolleranceShareLow;
    [SerializeField] public float wasteTolleranceShareMedium;
    [SerializeField] public float wasteTolleranceShareHigh;

    [Header("Lists")]
    [SerializeField] public List<GarbageManager> houseGarbage = new List<GarbageManager>();
    [SerializeField] public List<SatisfactionManager> houseSatisfaction = new List<SatisfactionManager>();
    [SerializeField] public List<ActiveCollector> activeCollectorsList = new List<ActiveCollector>();

    private void FixedUpdate()
    {
        GetHouses();
        GetGarbage();
        GetSatisfaction();
    }

    public void AddCollectorDestination(GameObject _collector, GameObject _roadHub)
    {
        ActiveCollector instance = ActiveCollector.CreateInstance<ActiveCollector>();
        
        instance.collector = _collector;
        instance.destination = _roadHub;

        activeCollectorsList.Add(instance);
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

        maxCitySatisfactionLevel = houseGarbage.Count;

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

    private void GetSatisfaction()
    {
        // flush the list
        houseSatisfaction.Clear();

        // temp variables
        citySatisfactionLevel = 0;

        // find the houses
        SatisfactionManager[] satisfaction = GameObject.FindObjectsOfType<SatisfactionManager>();
        foreach (SatisfactionManager satisfactionManager in satisfaction)
        {
            // add them back into the list
            houseSatisfaction.Add(satisfactionManager);

            switch (satisfactionManager.mySatisfcationLevel)
            {
                case SatisfactionManager.SatisfactionLevel.HIGH:
                    {
                        citySatisfactionLevel += houseSatisfactionHigh;
                        break;
                    }
                case SatisfactionManager.SatisfactionLevel.MEDIUM:
                    {
                        citySatisfactionLevel += houseSatisfactionMedium;
                        break;
                    }
                case SatisfactionManager.SatisfactionLevel.LOW:
                    {
                        citySatisfactionLevel += houseSatisfactionLow;
                        break;
                    }
                case SatisfactionManager.SatisfactionLevel.NONE:
                    {
                        citySatisfactionLevel += houseSatisfactionNone;
                        break;
                    }
                default:
                    {
                        Debug.LogError("GameManager.GetSatisfaction() switch defaulted, review for correction.");
                        break;
                    }
            }
        }
    }
}
