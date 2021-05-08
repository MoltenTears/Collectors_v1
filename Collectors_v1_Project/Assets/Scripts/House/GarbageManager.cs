using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageManager : MonoBehaviour
{
    [Header("House Details - garbage")]
    [SerializeField] public float garbageSpeed;
    [SerializeField] public float garbageLevel;
    [SerializeField] public float maxGarbageLevel;
    [SerializeField] private GarbageBin[] binArray;
    [SerializeField] public bool garbageNeedsCollecting = false;
    [SerializeField] public bool garbageBeingCollected = false;
    [SerializeField] public bool garbageCollected = false;

    [Header("External References")]
    [SerializeField] private GameManager myGameManager;
    [SerializeField] private HouseManager myHouseManager;

    [Header("Stink Lines")]
    [SerializeField] private ParticleSystem stinkParticles;
    [SerializeField] [Range(0, 1)] private float stinkPercentageLow;
    [SerializeField] [Range(0, 1)] private float stinkPercentageMedium;
    [SerializeField] [Range(0, 1)] private float stinkPercentageHigh;
    [SerializeField] private float stinkVolumeLow;
    [SerializeField] private float stinkVolumeMedium;
    [SerializeField] private float stinkVolumeHigh;
    [SerializeField] private float stinkVolumeTotal;
    [SerializeField] private Vector3 stinkSizeLow;
    [SerializeField] private Vector3 stinkSizeMedium;
    [SerializeField] private Vector3 stinkSizeHigh;
    [SerializeField] private Vector3 stinkSizeTotal;
    [SerializeField] private Color stinkColourLow;
    [SerializeField] private Color stinkColourMedium;
    [SerializeField] private Color stinkColourHigh;
    [SerializeField] private Color stinkColourTotal;

    // Start is called before the first frame update
    void Start()
    {
        myGameManager = FindObjectOfType<GameManager>();
        myHouseManager = GetComponent<HouseManager>();
        binArray = GetComponentsInChildren<GarbageBin>();

        stinkParticles = GetComponentInChildren<ParticleSystem>();
        UpdateParticles(stinkParticles, 0.0f, new Vector3(0,0,0), Color.gray);

        SetMaxGarbage();
    }

    // Update is called once per frame
    private void Update()
    {
        if (myHouseManager.isOccupied)
        {
            if (!garbageBeingCollected && garbageLevel <= maxGarbageLevel)
            {
                AccumulateGarbage();
            }
            SetOutBins();
        }

        SetStink();
    }

    private void UpdateParticles(ParticleSystem _particleSystem, float _emitterValue, Vector3 _stinkSize, Color _stinkColour)
    {
        // get the emitter module
        var particleEmitter = _particleSystem.emission;
        
        // change the rateOverTime
        particleEmitter.rateOverTime = _emitterValue;

        // set the size of the particle
        var main = _particleSystem.main;
        main.startSizeX = _stinkSize.x;
        main.startSizeY = _stinkSize.y;
        main.startSizeZ = _stinkSize.z;

        // set the colour of the particle
        main.startColor = _stinkColour;
    }

    private void SetStink()
    {
        if (garbageLevel >= maxGarbageLevel)
        {
            // Debug.Log($"{gameObject.name} emitter set to TOTAL.");
            UpdateParticles(stinkParticles, stinkVolumeTotal, stinkSizeTotal, stinkColourTotal);
        }
        else if (garbageLevel/maxGarbageLevel >= stinkPercentageHigh)
        {
            // Debug.Log($"{gameObject.name} emitter set to HIGH.");
            UpdateParticles(stinkParticles, stinkVolumeHigh, stinkSizeHigh, stinkColourHigh);
        }
        else if (garbageLevel / maxGarbageLevel >= stinkPercentageMedium)
        {
            // Debug.Log($"{gameObject.name} emitter set to MEDIUM.");
            UpdateParticles(stinkParticles, stinkVolumeMedium, stinkSizeMedium, stinkColourMedium);
        }
        else if (garbageLevel / maxGarbageLevel >= stinkPercentageLow)
        {
            // Debug.Log($"{gameObject.name} emitter set to LOW.");
            UpdateParticles(stinkParticles, stinkVolumeLow, stinkSizeLow, stinkColourLow);
        }
        else
        {
            // Debug.Log($"{gameObject.name} emitter set to NONE.");
            UpdateParticles(stinkParticles, 0.0f, new Vector3(0, 0, 0), Color.gray);
        }

    }

    private void SetMaxGarbage()
    {
        switch (myHouseManager.houseType)
        {
            case HouseManager.HouseType.SINGLE:
                {
                    maxGarbageLevel = myGameManager.maxGarbageSingle;
                    break;
                }
            case HouseManager.HouseType.FAMILY:
                {
                    maxGarbageLevel = myGameManager.maxGarbageFamily;
                    break;
                }
            case HouseManager.HouseType.SHARE:
                {
                    maxGarbageLevel = myGameManager.maxGarbageShare;
                    break;
                }
            case HouseManager.HouseType.NONE:
                {
                    Debug.LogWarning("House does not have HouseType set in HouseManager.cs");
                    break;
                }
            default:
                {
                    Debug.LogError("Switch in HouseManager.SetMaxGarbage() has defaulted, review and correct.");
                    break;
                }
        }
    }

    private void AccumulateGarbage()
    {
        // accumulate some garbage
        float garbage = (garbageSpeed * Time.deltaTime) * myGameManager.garbageMultipler;

        // add to the house level
        garbageLevel += garbage;

        // add the same amount to the Game Manager
        myGameManager.totalWasteGenerated += garbage;

        // if there's at least one bin out...
        if (garbageLevel >= myGameManager.binSizeSmall)
        {
            // ... Collector may collect
            garbageNeedsCollecting = true;
        }
        else
        {
            // ... don't stop at the house
            garbageNeedsCollecting = false;
        }
    }

    private void SetOutBins()
    {
        // reset out the bins
        foreach (GarbageBin Bin in binArray)
        {
            Bin.ResetBins();
        }


        float tempGarbage = garbageLevel;
        for (int i = 0; i <binArray.Length; i++)
        {
            // if there's more garbage than a large bin...
            if (tempGarbage >= myGameManager.binSizeLarge)
            {
                // show the bin
                binArray[i].binLarge.SetActive(true);

                // reduce the temp value
                tempGarbage -= myGameManager.binSizeLarge;
            }

            // if there's More garbage than a medium bin...
            else if (tempGarbage >= myGameManager.binSizeMedium)
            {
                // show the bin
                binArray[i].binMedium.SetActive(true);

                // reduce the temp value
                tempGarbage -= myGameManager.binSizeMedium;
            }

            // if there's more garbage than a small bin...
            else if (tempGarbage >= myGameManager.binSizeSmall)
            {
                // show the bin
                binArray[i].binSmall.SetActive(true);

                // reduce the temp value
                tempGarbage -= myGameManager.binSizeSmall;
            }
        }
    }
}