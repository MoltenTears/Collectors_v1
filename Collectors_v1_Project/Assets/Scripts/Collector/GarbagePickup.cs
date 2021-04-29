using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GarbagePickup : MonoBehaviour
{
    [Header("Collection Details")]
    [SerializeField] private float distanceToWaste = Mathf.Infinity;
    [SerializeField] private GarbageManager[] garbageManagers;
    [SerializeField] private float collectorSpeed;
    [SerializeField] private GameObject myCollectorDestination;

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

    private void Start()
    {
        myCollectorMovement = GetComponentInParent<CollectorMovement>();
        myNavMeshAgent = GetComponentInParent<NavMeshAgent>();
        myGameManager = FindObjectOfType<GameManager>();
        myDepotFrontGate = FindObjectOfType<DepotFrontGate>();
        myWasteCentreFrontGate = FindObjectOfType<WasteCentreFrontGate>();
    }

    private void Update()
    {
        collectorSpeed = myNavMeshAgent.velocity.magnitude;

        if (garbageInCollector >= garbageInCollectorMax && !isDeliveringWaste)
        {
            isCollecting = false;
            isDeliveringWaste = true;
        }

        if (isReturningToDepot)
        {
            ReturnToDepot();
        }

        if (isDeliveringWaste)
        {
            DeliveringWaste();
        }

        if (isCollecting)
        {
            WasteCollection();
        }

        if (isCollecting && garbageInCollector > garbageInCollectorMax && myNavMeshAgent.velocity.magnitude == 0)
        {
            Debug.Log("Reset Collector, even though isCollecting and not yet full.");
            foundWaste = false;
            FindNearestWaste();
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
            // .. turn off collection..
            isCollecting = false;
            if (garbageInCollector > 0) // ... if the collector has SOME waste in it...
            {
                isDeliveringWaste = true; // ... take it to the Waste Centre
            }
            else // .. if not...
            {
                isReturningToDepot = true; // .. go back to the depot (fail catch)
            }
        }
    }

    private void FindNearestWaste()
    {
        Debug.Log($"Collector checking for garbage...");

        // find all the GarbageManagers in the scene
        garbageManagers = FindObjectsOfType<GarbageManager>();

        // create a temporary distance variable and set it infinitely large
        distanceToWaste = Mathf.Infinity;
        float distanceToGM;

        // iterate through the GarbageManagers, recording the one with the closest distance
        foreach (GarbageManager garbageManager in garbageManagers)
        {
            distanceToGM = Vector3.Distance(
                transform.position, garbageManager.GetComponentInParent<GarbageManager>().GetComponentInChildren<CollectionPoint>().transform.position);

            // if the GM is closer than the previous distance recorded and the GM has at least one bin for collection...
            if (distanceToGM < distanceToWaste && garbageManager.garbageLevel >= myGameManager.binSizeSmall)
            {
                // ... overwrite the distance variable
                distanceToWaste = distanceToGM;

                // ... store the object as the destination
                myCollectorMovement.collectorDestination = garbageManager.GetComponentInParent<GarbageManager>().GetComponentInChildren<CollectionPoint>().gameObject;

                // ... set the foundWaste bool to true
                foundWaste = true;

                // get out of the foreach loop
                break;
            }
        }

        // pause for debugging
        if(foundWaste) Debug.Break();
    }

    private void MoveToWaste()
    {
        myCollectorMovement.gotDestination = true;
    }


    private void OnTriggerEnter(Collider collision)
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
            }
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (isCollecting && !isDeliveringWaste)
        {
            // temp variable
            float tempHouseGarbage = 0;
        
            if (collision.GetComponentInParent<GarbageManager>())
            {
                tempHouseGarbage = collision.GetComponentInParent<GarbageManager>().garbageLevel;
            }

            // IF...
            if (collision.CompareTag("Garbage") //  the collector runs into the house trigger
                && collision.GetComponentInParent<GarbageManager>().garbageNeedsCollecting // AND the house has Garbage
                && isCollecting) // AND the Collector is in pickup mode...
            {
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
}
