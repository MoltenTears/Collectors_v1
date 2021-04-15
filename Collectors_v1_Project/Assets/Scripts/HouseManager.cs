using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public enum HouseType
    {
        NONE,
        SINGLE,
        FAMILY,
        SHARE,
        END
    }

    [Header("External References")]
    [SerializeField] private GameManager myGameManager;

    [Header("House Details - Visuals")]
    [SerializeField] private Material myHouseMaterial;
    [SerializeField] private MeshRenderer myHouseMeshRender;

    [Header("House Details - Occupancy")]
    [SerializeField] private HouseType myHouseType;
    [SerializeField] private bool isOccupied;

    [Header("House Details - garbage")]
    [SerializeField] private float myGarbageSpeed;
    [SerializeField] public float myGarbageLevel;


    private void Start()
    {
        // get realtime references
        myGameManager = FindObjectOfType<GameManager>();
        myHouseMeshRender = GetComponentInChildren<MeshRenderer>();

        // set some variables
        RandomHouseType();
        SetHouse();
    }

    private void Update()
    {
        if (isOccupied)
        {
            AccumulateGarbage();
        }
    }

    private void AccumulateGarbage()
    {
        myGarbageLevel += (myGarbageSpeed * Time.deltaTime) / myGameManager.garbageDivisor;
    }

    private void RandomHouseType()
    {
        myHouseType = (HouseType)Random.Range((int)HouseType.NONE + 1, (int)HouseType.END);
    }

    private void SetHouse()
    {
        // figure out what kind of house this is and set some local variables
        switch (myHouseType)
        {
            case HouseType.SINGLE:
                {
                    myHouseMaterial = myGameManager.houseSingleMaterial;
                    myGarbageSpeed = myGameManager.garbageSpeedSingle;
                    break;
                }
            case HouseType.FAMILY:
                {
                    myHouseMaterial = myGameManager.houseFamilyMaterial;
                    myGarbageSpeed = myGameManager.garbageSpeedFamily;
                    break;
                }
            case HouseType.SHARE:
                {
                    myHouseMaterial = myGameManager.houseShareMaterial;
                    myGarbageSpeed = myGameManager.singleGarbageShare;
                    break;
                }
            case HouseType.NONE:
                {
                    // debug point: house type not set
                    break;
                }
            default:
                {
                    // error message
                    break;
                }

        }

        // set the house colour
        myHouseMeshRender.material = myHouseMaterial;
    }
}
