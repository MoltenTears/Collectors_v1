using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageManager : MonoBehaviour
{
    [Header("House Details - garbage")]
    [SerializeField] public float myGarbageSpeed;
    [SerializeField] public float myGarbageLevel;
    [SerializeField] private GarbageBin[] binArray;
    [SerializeField] public bool garbageNeedsCollecting = false;
    [SerializeField] public bool garbageBeingCollected = false;

    [Header("External References")]
    [SerializeField] private GameManager myGameManager;
    [SerializeField] private HouseManager myHouseManager;

    // Start is called before the first frame update
    void Start()
    {
        myGameManager = FindObjectOfType<GameManager>();
        myHouseManager = GetComponent<HouseManager>();
        binArray = GetComponentsInChildren<GarbageBin>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(myHouseManager.isOccupied)
        {
            if (!garbageBeingCollected)
            {
                AccumulateGarbage();
            }
            SetOutBins();
        }
    }


    private void AccumulateGarbage()
    {
        myGarbageLevel += (myGarbageSpeed * Time.deltaTime) / myGameManager.garbageDivisor;

        // if there's at least one bin out...
        if (myGarbageLevel >= myGameManager.binSizeSmall)
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


        float tempGarbage = myGarbageLevel;
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