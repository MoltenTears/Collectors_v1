using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GarbagePickup : MonoBehaviour
{
    [SerializeField] public bool isCollecting = false;
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

        // if the collector runs into the house trigger AND the house has Garbage AND the Collector is in pickup mode...
        if (collision.CompareTag("Garbage") && tempHouseGarbage > 0 && isCollecting)
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

        // if the collector runs into the house trigger AND the house has Garbage AND the Collector is in pickup mode...
        if (collision.CompareTag("Garbage") && tempHouseGarbage > 0 && isCollecting)
        {
            // ... get a reference to the house's Garbage Manager
            GarbageManager tempGM = collision.GetComponentInParent<GarbageManager>();

            // ... reduce the garbage in the house at a given speed
            tempGM.myGarbageLevel -= (collectionSpeed * Time.deltaTime) / myGameManager.garbageDivisor;
        }
        else if (collision.CompareTag("Garbage") && tempHouseGarbage <= 0 && isCollecting)
        {
            // debug
            Debug.Log("No more garbage at house, Collector resumed collection.");
            
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
        }
    }
}
