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

    // Start is called before the first frame update
    void Start()
    {
        myGameManager = FindObjectOfType<GameManager>();
        myHouseManager = GetComponent<HouseManager>();
        binArray = GetComponentsInChildren<GarbageBin>();

        SetMaxGarbage();
    }

    // Update is called once per frame
    private void Update()
    {
        if(myHouseManager.isOccupied)
        {
            if (!garbageBeingCollected && garbageLevel <= maxGarbageLevel)
            {
                AccumulateGarbage();
            }
            SetOutBins();
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