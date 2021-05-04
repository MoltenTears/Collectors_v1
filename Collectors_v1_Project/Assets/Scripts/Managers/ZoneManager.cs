using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [SerializeField] public GarbageManager[] myGarbageManagers;
    [SerializeField] public RoadHub[] myRoadHubs;
    [SerializeField] public GameObject currentZoneCollector;

    // Start is called before the first frame update
    void Start()
    {
        myGarbageManagers = GetComponentsInChildren<GarbageManager>();
        myRoadHubs = GetComponentsInChildren<RoadHub>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollection();
        ZoneActive();
    }

    private void CheckCollection()
    {
        int housesToBeCollected = 0;
        int totalhousesToCollect = myGarbageManagers.Length;

        foreach (GarbageManager garbageManager in myGarbageManagers)
        {
            if (!garbageManager.garbageCollected)
            {
                // increase the counter
                housesToBeCollected++;
            }
        }

        // Debug.Log($"{housesToBeCollected} left of {totalhousesToCollect} total houses in zone yet to be collected in {gameObject.name}.");
    }

    public void ResetZone()
    {
        foreach (GarbageManager garbageManager in myGarbageManagers)
        {
            garbageManager.garbageCollected = false;
        }

        currentZoneCollector = null;
    }

    public void ZoneActive()
    {
        bool zoneActive = false;

        foreach (RoadHub roadHub in myRoadHubs)
        {
            if (roadHub.isActive)
            {
                zoneActive = true;
                break;
            }
        }

        if (currentZoneCollector == null)
        {
            foreach (RoadHub roadHub in myRoadHubs)
            {
                if (zoneActive)
                {
                    roadHub.GetComponent<MeshRenderer>().material.color = roadHub.m_MouseOverColor;
                }
                else
                {
                    roadHub.GetComponent<MeshRenderer>().material.color = roadHub.m_OriginalColor;
                }
            }
        }
        else
        {
            foreach (RoadHub roadHub in myRoadHubs)
            {
                roadHub.GetComponent<MeshRenderer>().material.color = roadHub.m_DeactiveColour;
            }
        }
    }
}
