using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GarbagePickup : MonoBehaviour
{
    [SerializeField] public bool isCollecting = false;
    [SerializeField] public float garbageInCollector;
    [SerializeField] public float collectionSpeed;
    [SerializeField] private NavMeshAgent myNavMeshAgent;
    [SerializeField] private GameManager myGameManager;

    private void Start()
    {
        myNavMeshAgent = GetComponentInParent<NavMeshAgent>();
        myGameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider collision)
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
            // debug
            Debug.Log("Collector near house with garbage.");
            
            // ... tell the house the Collector is here
            collision.GetComponentInParent<GarbageManager>().garbageBeingCollected = true;
            
            // ... pause the movement
            myNavMeshAgent.isStopped = true;
        }
    }

    private void OnTriggerStay(Collider collision)
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
            Debug.Log("No more garbage at house, Collector not stopping.");
            
            // ... resume the movement
            myNavMeshAgent.isStopped = false;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.GetComponentInParent<GarbageManager>())
        {
            // tell the house, the collector has left
            collision.GetComponentInParent<GarbageManager>().garbageBeingCollected = false;
            collision.GetComponentInParent<GarbageManager>().garbageNeedsCollecting = false;
        }
    }
}
