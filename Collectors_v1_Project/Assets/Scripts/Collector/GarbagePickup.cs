using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GarbagePickup : MonoBehaviour
{
    [Header("Collection Details")]
    [SerializeField] private float distanceToWaste = Mathf.Infinity;
    [SerializeField] private GarbageManager[] garbageManagers;
    [SerializeField] private int minimumBinsOut;

    [SerializeField] public bool isCollecting = false;
    [SerializeField] public bool foundWaste = false;
    [SerializeField] public bool isDeliveringWaste = false;
    [SerializeField] public bool isReturningToDepot = false;
    [SerializeField] public float garbageInCollector;
    [SerializeField] public float garbageInCollectorMax;
    [SerializeField] public float collectionSpeed;
    [SerializeField] private CollectorMovement myCollectorMovement;
    [SerializeField] private NavMeshAgent myNavMeshAgent;
    [SerializeField] private GameManager myGameManager;
    [SerializeField] private DepotFrontGate myDepotFrontGate;
    [SerializeField] private WasteCentreFrontGate myWasteCentreFrontGate;
    [SerializeField] private AudioManager myAudioManager;

    [SerializeField] public ZoneManager collectionZone;

    private void Awake()
    {
        myCollectorMovement = GetComponentInParent<CollectorMovement>();
        myNavMeshAgent = GetComponentInParent<NavMeshAgent>();
        myGameManager = FindObjectOfType<GameManager>();
        myDepotFrontGate = FindObjectOfType<DepotFrontGate>();
        myWasteCentreFrontGate = FindObjectOfType<WasteCentreFrontGate>();
        myAudioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {

        // check to see if the Collector is full and needs to go to the Waste Depot
        if (garbageInCollector >= garbageInCollectorMax && !isDeliveringWaste)
        {
            isCollecting = false;
            isDeliveringWaste = true;
        }

        // check which action is occuring. Depending on priority, turn off the other two
        // TODO convert to ENUM/Switch
        if (isCollecting)
        {
            isDeliveringWaste = false;
            isReturningToDepot = false;
            WasteCollection();
        }
        else if (isDeliveringWaste)
        {
            isCollecting = false;
            isReturningToDepot = false;
            collectionZone.ResetZone();
            DeliveringWaste();
        }
        else if (isReturningToDepot)
        {
            isDeliveringWaste = false;
            isCollecting = false;
            ReturnToDepot();
        }

        // catch for NAvMesh error
        if (isCollecting && garbageInCollector > garbageInCollectorMax && myNavMeshAgent.velocity.magnitude == 0)
        {
            Debug.Log($"{myCollectorMovement.transform.name} is stationary and should be collecting. Delivery waste, then return to the Depot (you broken!).");

            isCollecting = false;
            isDeliveringWaste = true;
        }

        // if not a new despatch...
        if (!myCollectorMovement.newDespatch)
        {
            // is not moving but has a destination it should be going to...
            if ((!myCollectorMovement.isMoving && !isCollecting && !isDeliveringWaste && !isReturningToDepot)
                && myCollectorMovement.myAgent.velocity.magnitude == 0
                && myCollectorMovement.collectorDestination != null)
            {
                // Debug.Log("Collector Lost, looking for waste.");
                WasteCollection();
            }

            // ... thinks it's moving, isn't, and has a destination to go to...
            if (!myCollectorMovement.isMoving
                && myCollectorMovement.myAgent.velocity.magnitude == 0
                && myCollectorMovement.collectorDestination != null)
            {
                // Debug.Log($"Collector {transform.parent.name} Reset, got stuck.");
                myCollectorMovement.ResetDestination();
            }
        }
    }

    private void WasteCollection()
    {
        if (!foundWaste)
        {
            FindNearestWaste();
        }
        if (foundWaste) // if there is waste to collect...
        {
            // .. move to that location to trigger collection
            MoveToWaste();
        }
        else // .. if there is no waste found...
        {
            // reset the Collector
            FinaliseCollection();
        }
    }

    private void FindNearestWaste()
    {
        // create a temporary distance variable and set it infinitely large
        distanceToWaste = Mathf.Infinity;
        float distanceToGM;

        // iterate through the GarbageManagers, recording the one with the closest distance
        foreach (GarbageManager garbageManager in collectionZone.myGarbageManagers)
        {
            // Debug.Log($"{garbageManager.name} being assessed for waste to collect.");

            // if garbage has not been collected this run...
            if (!garbageManager.garbageCollected)
            {
                // get the distance to the Collection Point
                distanceToGM = Vector3.Distance(
                    transform.position, garbageManager.GetComponentInParent<GarbageManager>().GetComponentInChildren<CollectionPoint>().transform.position);

                // Debug.Log($"Distance from {gameObject.GetComponentInParent<CollectorMovement>().name} to {garbageManager.name} is: {distanceToGM}.");

                // set the minimum bins out value
                if (distanceToGM < distanceToWaste && minimumBinsOut > 0 ? garbageManager.garbageLevel >= myGameManager.binSizeSmall * minimumBinsOut : garbageManager.garbageLevel >= myGameManager.binSizeSmall)

                    // if the new object is closer than the last object (or infinity) and the Collector has a minimum bins out requirement
                    if (distanceToGM < distanceToWaste && minimumBinsOut > 0)
                    { 
                        // ... overwrite the distance variable
                        distanceToWaste = distanceToGM;

                        // ... store the object as the destination
                        myCollectorMovement.collectorDestination = garbageManager.GetComponentInParent<GarbageManager>().GetComponentInChildren<CollectionPoint>().gameObject;

                        // ... get collecting!
                        foundWaste = true;
                        isCollecting = true;
                    }
            }
        }

        // Debug.Log($"{GetComponentInParent<CollectorMovement>().gameObject.name}'s nearest house is : {myCollectorMovement.collectorDestination}.");
    }

    private void MoveToWaste()
    {
        myCollectorMovement.gotDestination = true;
    }


    private void OnTriggerEnter(Collider collision)
    {
        CollectGarbage(collision);
    }

    private void OnTriggerStay(Collider collision)
    {
        CollectGarbage(collision);
    }

    private void CollectGarbage(Collider collision)
    {
        if (isCollecting && !isDeliveringWaste)
        {
            if (collision.CompareTag("Garbage") // if the collector runs into the house trigger
                && collision.GetComponentInParent<GarbageManager>().garbageNeedsCollecting // AND the house has Garbage
                && isCollecting) // AND the Collector is in pickup mode...
            {
                // ... tell the house the Collector is here
                collision.GetComponentInParent<GarbageManager>().garbageBeingCollected = true;

                // ... pause the movement
                myNavMeshAgent.isStopped = true;

                // ... get a reference to the house's Garbage Manager
                GarbageManager tempGM = collision.GetComponentInParent<GarbageManager>();

                // temp variable for collecting garbage
                float garbageCollected = (collectionSpeed * Time.deltaTime) * myGameManager.garbageMultipler;

                // ... reduce the garbage in the house at a given speed
                tempGM.garbageLevel -= garbageCollected;

                // ... and add it to the Collector
                garbageInCollector += garbageCollected;

                // if the house has run out of garbage
                if (tempGM.garbageLevel <= 0)
                {
                    tempGM.garbageNeedsCollecting = false;
                    tempGM.garbageBeingCollected = false;
                    tempGM.garbageLevel = 0.0f;
                    tempGM.garbageCollected = true;

                    myAudioManager.GarbageCollectedSFX();
                }
            }
            else if (collision.CompareTag("Garbage")
                && !collision.GetComponentInParent<GarbageManager>().garbageNeedsCollecting
                && isCollecting)
            {
                // debug
                // Debug.Log("No more garbage at house, Collector not stopping.");

                // reset the bool to look for more garbage
                foundWaste = false;

                // ... resume the movement
                myNavMeshAgent.isStopped = false;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (isCollecting && !isDeliveringWaste)
        {
            if (collision.GetComponentInParent<GarbageManager>())
            {              
                // tell the house, the collector has left
                collision.GetComponentInParent<GarbageManager>().garbageBeingCollected = false;
                collision.GetComponentInParent<GarbageManager>().garbageNeedsCollecting = false;   
            }
        }
    }

    public void ReturnToDepot()
    {
        if (myCollectorMovement)
        {
            // store a reference of the Waste Centre
            myCollectorMovement.collectorDestination = myDepotFrontGate.roadHubAtDepotFrontGate;

            // set this location as the destination
            myCollectorMovement.ResetDestination();
        }
    }

    public void DeliveringWaste()
    {
        if (myCollectorMovement)
        {
            // store a reference of the Waste Centre
            myCollectorMovement.collectorDestination = myWasteCentreFrontGate.roadHubAtWasteCentreFrontGate;

            // set this location as the destination
            myCollectorMovement.ResetDestination();
        }
    }

    public void FinaliseCollection()
    {
        Debug.LogWarning($"FinaliseCollection() called.");

        // .. turn off collection..
        isCollecting = false;
        if (garbageInCollector > 0) // ... if the collector has SOME waste in it...
        {
            // Debug.Log($"Collector {myCollectorMovement.transform.name} is going to Waste Centre with partial load, not more waste available.");
            isDeliveringWaste = true; // ... take it to the Waste Centre
            this.collectionZone.currentZoneCollector = null;
        }
        else // .. if not...
        {
            // Debug.Log($"Collector {myCollectorMovement.transform.name} is going to Collector Depot, insufficient Waste to collect.");
            isReturningToDepot = true; // .. go back to the depot (fail catch)
            this.collectionZone.currentZoneCollector = null;

        }
    }
}
