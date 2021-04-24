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
    [SerializeField] private GarbageManager myGarbageManager;
    [SerializeField] private SatisfactionManager mySatisfactionManager;
    [SerializeField] private SphereCollider mySatisfactionTrigger;

    [Header("House Details - Visuals")]
    [SerializeField] private Material myHouseMaterial;
    [SerializeField] private MeshRenderer myHouseMeshRender;

    [Header("House Details - Occupancy")]
    [SerializeField] public HouseType houseType;
    [SerializeField] public bool isOccupied;


    private void Awake()
    {
        // get realtime references
        myGameManager = FindObjectOfType<GameManager>();
        myGarbageManager = GetComponent<GarbageManager>();
        mySatisfactionManager = GetComponentInChildren<SatisfactionManager>();
        if (mySatisfactionManager.GetComponent<SphereCollider>())
        {
            mySatisfactionTrigger = mySatisfactionManager.GetComponent<SphereCollider>();
        }
        myHouseMeshRender = GetComponentInChildren<MeshRenderer>();

        // set some variables
        RandomHouseType();
    }
    private void Start()
    {
        
        SetHouse();
    }

    private void Update()
    {

    }



    private void RandomHouseType()
    {
        houseType = (HouseType)Random.Range((int)HouseType.NONE + 1, (int)HouseType.END);
    }

    private void SetHouse()
    {
        // figure out what kind of house this is and set some local variables
        switch (houseType)
        {
            case HouseType.SINGLE:
                {
                    myHouseMaterial = myGameManager.houseSingleMaterial;
                    myGarbageManager.garbageSpeed = myGameManager.garbageSpeedSingle;
                    mySatisfactionTrigger.radius = myGameManager.satisfactionRadiusSingle;
                    break;
                }
            case HouseType.FAMILY:
                {
                    myHouseMaterial = myGameManager.houseFamilyMaterial;
                    myGarbageManager.garbageSpeed = myGameManager.garbageSpeedFamily;
                    mySatisfactionTrigger.radius = myGameManager.satisfactionRadiusFamily;
                    break;
                }
            case HouseType.SHARE:
                {
                    myHouseMaterial = myGameManager.houseShareMaterial;
                    myGarbageManager.garbageSpeed = myGameManager.garbageSpeedShare;
                    mySatisfactionTrigger.radius = myGameManager.satisfactionRadiusShare;
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
