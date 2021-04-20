using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GarbagePickup : MonoBehaviour
{
    [SerializeField] public bool isCollecting = false;
    [SerializeField] public bool isReturningToDepot = false;
    [SerializeField] public float garbageInCollector;
    [SerializeField] public float garbageInCollectorMax;
    [SerializeField] public float collectionSpeed;
    [SerializeField] private CollectorMovement myCollectorMovement;
    [SerializeField] private NavMeshAgent myNavMeshAgent;
    [SerializeField] private GameManager myGameManager;
    [SerializeField] private DepotFrontGate myDepotFrontGate;

    private void Start()
    {
        myCollectorMovement = GetComponentInParent<CollectorMovement>();
        myNavMeshAgent = GetComponentInParent<NavMeshAgent>();
        myGameManager = FindObjectOfType<GameManager>();
        myDepotFrontGate = FindObjectOfType<DepotFrontGate>();
    }

    private void LateUpdate()
    {
        if (garbageInCollector >= garbageInCollectorMax && !isReturningToDepot)
        {
            ReturnToDepot();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (isCollecting && !isReturningToDepot)
        {
            // temp variable
            float tempHouseGarbage = 0;
            if (collision.GetComponentInParent<GarbageManager>())
            {
                tempHouseGarbage = collision.GetComponentInParent<GarbageManager>().myGarbageLevel;
            }

            if (collision.CompareTag("Garbage") // if the collector runs into the house trigger
                && collision.GetComponentInParent<GarbageManager>().garbageNeedsCollecting // AND the house has Garbage
                && isCollecting) // AND the Collector is in pickup mode...
            {
                // ... tell the house the Collector is here
                collision.GetComponentInParent<GarbageManager>().garbageBeingCollected = true;
            
                // ... pause the movement
                myNavMeshAgent.isStopped = true;
            }
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (isCollecting && !isReturningToDepot)
        {
            // temp variable
            float tempHouseGarbage = 0;
        
            if (collision.GetComponentInParent<GarbageManager>())
            {
                collision.GetComponentInParent<GarbageManager>().garbageBeingCollected = true;

                tempHouseGarbage = collision.GetComponentInParent<GarbageManager>().myGarbageLevel;
            }
            // IF...
            if (collision.CompareTag("Garbage") //  the collector runs into the house trigger
                && collision.GetComponentInParent<GarbageManager>().garbageNeedsCollecting // AND the house has Garbage
                && isCollecting) // AND the Collector is in pickup mode...
            {
                // indicate the Collector is collecting
                myCollectorMovement.isAtHouse = true;

                // ... get a reference to the house's Garbage Manager
                GarbageManager tempGM = collision.GetComponentInParent<GarbageManager>();

                // temp variable for collecting garbage
                float garbageCollected = (collectionSpeed * Time.deltaTime) / myGameManager.garbageDivisor;

                // ... reduce the garbage in the house at a given speed
                tempGM.myGarbageLevel -= garbageCollected;

                // ... and add it to the Collector
                garbageInCollector += garbageCollected;
            }
            else if (collision.CompareTag("Garbage") 
                && !collision.GetComponentInParent<GarbageManager>().garbageNeedsCollecting
                && isCollecting)
            {
                // debug
                // Debug.Log("No more garbage at house, Collector not stopping.");
            
                // ... resume the movement
                myNavMeshAgent.isStopped = false;
            }
        }
        else if (!isCollecting)
        {
            myCollectorMovement.isAtHouse = false;
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        if (isCollecting && !isReturningToDepot)
        {
            if (collision.GetComponentInParent<GarbageManager>())
            {
                // tell the house, the collector has left
                collision.GetComponentInParent<GarbageManager>().garbageBeingCollected = false;
                collision.GetComponentInParent<GarbageManager>().garbageNeedsCollecting = false;

                // tell the collector it is no longer collecting
                myCollectorMovement.isAtHouse = false;
            }
        }
    }

    public void ReturnToDepot()
    {
        // reset bools
        isCollecting = false;
        isReturningToDepot = true;

        // store a reference to this object to the RoadMap
        myCollectorMovement.selectedRoadHub = myDepotFrontGate.roadHubAtDepotFrontGate;

        // set this location as the destination
        myCollectorMovement.ResetDestination();
    }
}
