using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [SerializeField] public HouseManager[] myHouseManagers;
    [SerializeField] public RoadHub[] myRoadHubs;

    // Start is called before the first frame update
    void Start()
    {
        myHouseManagers = GetComponentsInChildren<HouseManager>();
        myRoadHubs = GetComponentsInChildren<RoadHub>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
