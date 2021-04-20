using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepotFrontGate : MonoBehaviour
{
    [SerializeField] public GameObject roadHubAtDepotFrontGate;

    [SerializeField] private DepotManager myDepotManager;

    private void Start()
    {
        myDepotManager = FindObjectOfType<DepotManager>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        // if the collision is a collector and they are intending to return to the depot...
        if (collision.CompareTag("Collector") && collision.GetComponentInChildren<GarbagePickup>().isReturningToDepot == true)
        {
            // debug
            Debug.Log("Collector returned to Depot");

            // ... destroy the game object
            Destroy(collision.transform.parent.gameObject);

            // add the truck back to the tally of trucks at the depot
            int tempCount = myDepotManager.baseCollectors;
            ++myDepotManager.baseCollectors;
            Debug.Log($"Count of base Collectors at Depot was: {tempCount}, is now: {myDepotManager.baseCollectors}.");

        }

        // if the collision is the road hub at the front gate...
        if (collision.CompareTag("RoadHub"))
        {
            // store a reference
            roadHubAtDepotFrontGate = collision.gameObject;
        }
    }
}
