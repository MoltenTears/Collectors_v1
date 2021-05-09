using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum DifficultySetting
    {
        NONE,
        EASY,
        MEDIUM,
        HARD
    }

    [Header("Gameplay Variables")]
    [SerializeField] public DifficultySetting difficultySetting;
    [SerializeField] public bool isDifficultySet;
    [SerializeField] public int daysToPlay;
    [SerializeField] public int daysLeftToPlay;
    [SerializeField] public bool isGameOver;
    [SerializeField] public float totalWasteGenerated;
    [SerializeField] public float satisfactionToWin = 0;
    [SerializeField] public int startingBaseCollectors;
    
    [Header("General Variables")]
    [Tooltip("Larger value means garbage accumulates quicker in the city.")] [SerializeField] [Range(0, 1)] public float garbageMultipler;

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

    [Header("External References")]
    [SerializeField] private DepotManager myDepotManager;

    [Header("Difficultly - EASY")]
    [SerializeField] private int daysEasy;
    [SerializeField] private float garbageSpeedEasy;
    [SerializeField] private int startingCollectorsEasy;
    [SerializeField] private float satisfactionToWinEasy;

    [Header("Difficultly - MEDIUM")]
    [SerializeField] private int daysMedium;
    [SerializeField] private float garbageSpeedMedium;
    [SerializeField] private int startingCollectorsMedium;
    [SerializeField] private float satisfactionToWinMedium;

    [Header("Difficultly - HARD")]
    [SerializeField] private int daysHard;
    [SerializeField] private float garbageSpeedHard;
    [SerializeField] private int startingCollectorsHard;
    [SerializeField] private float satisfactionToWinHard;

    [Header("Zone Game Objects")]
    [SerializeField] private GameObject zoneEssential;
    [SerializeField] private GameObject zoneEasy;
    [SerializeField] private GameObject zoneMedium;
    [SerializeField] private GameObject zoneHard;

    [Header("Lists")]
    [SerializeField] public List<GarbageManager> houseGarbage = new List<GarbageManager>();
    [SerializeField] public List<SatisfactionManager> houseSatisfaction = new List<SatisfactionManager>();
    [SerializeField] public List<ActiveCollector> activeCollectorsList = new List<ActiveCollector>();

    [SerializeField] private DifficultyNumber myDifficultyNumber;

// Singleton
    public static GameManager GMInstance { get; private set; }

    private void Awake()
    {
        // Singleton
        if (GMInstance  == null)
        {
            GMInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        myDepotManager = FindObjectOfType<DepotManager>();
        myDifficultyNumber = FindObjectOfType<DifficultyNumber>();
        isDifficultySet = false;
        Debug.Log($"Diffculty started with: {difficultySetting}");
    }

    private void Start()
    {


        daysLeftToPlay = daysToPlay;
    }

    private void Update()
    {
        if (!isDifficultySet)
        {
            SetZones();
            SetDifficulty();
        }

        CheckGameOver();
        if (myDepotManager == null) FindDepotManager();
    }


    private void FixedUpdate()
    {
        GetHouses();
        GetGarbage();
        GetSatisfaction();
    }

    public void SetZones()
    {
        Debug.Log($"SetZones() called.");

        // find the zones in the level
        zoneEssential = GameObject.FindGameObjectWithTag("ZoneEssential");
        zoneEasy = GameObject.FindGameObjectWithTag("ZoneEasy");
        zoneMedium = GameObject.FindGameObjectWithTag("ZoneMedium");
        zoneHard = GameObject.FindGameObjectWithTag("ZoneHard");

        // deactivate all the zones
        if (zoneEssential != null) zoneEssential.SetActive(false);
        if (zoneEasy != null) zoneEasy.SetActive(false);
        if (zoneMedium != null) zoneMedium.SetActive(false);
        if (zoneHard != null) zoneHard.SetActive(false);
    }

    public void SetDifficulty()
    {
        // Debug.Log($"SetDifficulty() called.");

        if (myDifficultyNumber.difficultyNo == 1)
        {
            Debug.Log("Difficulty set to EASY (DifficultyNumber)");
            if (zoneEssential != null) zoneEssential.SetActive(true);
            if (zoneEasy != null) zoneEasy.SetActive(true);

            // update the settings
            daysToPlay = daysEasy;
            daysLeftToPlay = daysEasy;
            garbageMultipler = garbageSpeedEasy;
            myDepotManager.baseCollectors = startingCollectorsEasy;
            satisfactionToWin = satisfactionToWinEasy;
        }
        else if (myDifficultyNumber.difficultyNo == 2)
        {
            Debug.Log("Difficulty set to MEDIUM (DifficultyNumber)");
            if (zoneEssential != null) zoneEssential.SetActive(true);
            if (zoneEasy != null) zoneEasy.SetActive(true);
            if (zoneMedium != null) zoneMedium.SetActive(true);

            // update the settings
            daysToPlay = daysMedium;
            daysLeftToPlay = daysMedium;
            garbageMultipler = garbageSpeedMedium;
            myDepotManager.baseCollectors = startingCollectorsMedium;
            satisfactionToWin = satisfactionToWinMedium;
        }
        else if (myDifficultyNumber.difficultyNo == 3)
        {
            Debug.Log("Difficulty set to HARD (DifficultyNumber)");
            if (zoneEssential != null) zoneEssential.SetActive(true);
            if (zoneEasy != null) zoneEasy.SetActive(true);
            if (zoneMedium != null) zoneMedium.SetActive(true);
            if (zoneHard != null) zoneHard.SetActive(true);

            // update the settings
            daysToPlay = daysHard;
            daysLeftToPlay = daysHard;
            garbageMultipler = garbageSpeedHard;
            myDepotManager.baseCollectors = startingCollectorsHard;
            satisfactionToWin = satisfactionToWinHard;
        }



        //// based on the difficulty setting, activate selected zones
        //switch (difficultySetting)
        //{
        //    case DifficultySetting.EASY:
        //        {
        //            Debug.Log($"Difficultly confirmed as: {difficultySetting}.");
        //            // activate related zones
        //            if (zoneEssential != null) zoneEssential.SetActive(true);
        //            if (zoneEasy != null) zoneEasy.SetActive(true);

        //            // update the settings
        //            daysToPlay = daysEasy;
        //            daysLeftToPlay = daysEasy;
        //            garbageMultipler = garbageSpeedEasy;
        //            myDepotManager.baseCollectors = startingCollectorsEasy;
        //            satisfactionToWin = satisfactionToWinEasy;

        //            break;
        //        }
        //    case DifficultySetting.MEDIUM:
        //        {
        //            Debug.Log($"Difficultly confirmed as: {difficultySetting}.");
        //            // activate related zones
        //            if (zoneEssential != null) zoneEssential.SetActive(true);
        //            if (zoneEasy != null) zoneEasy.SetActive(true);
        //            if (zoneMedium != null) zoneMedium.SetActive(true);

        //            // update the settings
        //            daysToPlay = daysMedium;
        //            daysLeftToPlay = daysMedium;
        //            garbageMultipler = garbageSpeedMedium;
        //            myDepotManager.baseCollectors = startingCollectorsMedium;
        //            satisfactionToWin = satisfactionToWinMedium;

        //            break;
        //        }
        //    case DifficultySetting.HARD:
        //        {
        //            Debug.Log($"Difficultly confirmed as: {difficultySetting}.");
        //            // activate related zones
        //            if (zoneEssential != null) zoneEssential.SetActive(true);
        //            if (zoneEasy != null) zoneEasy.SetActive(true);
        //            if (zoneMedium != null) zoneMedium.SetActive(true);
        //            if (zoneHard != null) zoneHard.SetActive(true);

        //            // update the settings
        //            daysToPlay = daysHard;
        //            daysLeftToPlay = daysHard;
        //            garbageMultipler = garbageSpeedHard;
        //            myDepotManager.baseCollectors = startingCollectorsHard;
        //            satisfactionToWin = satisfactionToWinHard;

        //            break;
        //        }
        //    case DifficultySetting.NONE:
        //        {
        //            // do nothing, it's ok :-)
        //            break;
        //        }
        //    default:
        //        {
        //            Debug.LogError("GameManager.SetZones() switch failed.");
        //            break;
        //        }
        //}

        isDifficultySet = true;
    }

    private void FindDepotManager()
    {
        myDepotManager = FindObjectOfType<DepotManager>();
        if (myDepotManager) myDepotManager.baseCollectors = startingBaseCollectors;
    }

    private void CheckGameOver()
    {
        if (daysLeftToPlay == 0)
        {
            isGameOver = true; // trigger GameOver state
            garbageMultipler = 0.0f; // stop garbage accumulation
        }
        else
        {
            isGameOver = false;
        }
    }

    public void AddCollectorDestination(GameObject _collector, GameObject _roadHub)
    {
        ActiveCollector instance = ActiveCollector.CreateInstance<ActiveCollector>();
        
        instance.collector = _collector;
        instance.destination = _roadHub;

        activeCollectorsList.Add(instance);
    }

    public void RemoveCollectorDestination(GameObject _collector)
    {
        // iterate through the list
        for (int i = 0; i < activeCollectorsList.Count; i++)
        {
            if (activeCollectorsList[i].collector = _collector)
            {
                // remove the one you found
                activeCollectorsList.Remove(activeCollectorsList[i]);

                // stop looking
                break;
            }
        }
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
