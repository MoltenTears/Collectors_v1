using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepotFrontGate : MonoBehaviour
{
    [SerializeField] public GameObject roadHubAtDepotFrontGate;

    private void OnTriggerEnter(Collider collision)
    {
        // if the collision is a collector and they are intending to return to the depot...
        if (collision.CompareTag("Collector") && collision.GetComponentInChildren<GarbagePickup>().isReturningToDepot == true)
        {
            // debug
            Debug.Log("Collector returned to Depot");

            // ... destroy the game object
            Destroy(collision.transform.parent.gameObject);
        }

        // if the collision is the road hub at the front gate...
        if (collision.CompareTag("RoadHub"))
        {
            // debug
            // Debug.Log("Front Gate has RoadHub reference.");

            // store a reference
            roadHubAtDepotFrontGate = collision.gameObject;
        }
    }
}
