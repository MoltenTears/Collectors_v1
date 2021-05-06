using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteCentreFrontGate : MonoBehaviour
{
    [SerializeField] public GameObject roadHubAtWasteCentreFrontGate;
    [SerializeField] public WasteCenteManager myWasteCentreManager;
    [SerializeField] public WasteCentreLot myWasteCentreLot;

    private void Start()
    {
        myWasteCentreManager = GetComponentInParent<WasteCenteManager>();
        myWasteCentreLot = GetComponentInParent<WasteCentreLot>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        // if the collision is a collector and they are intending to deliver waste...
        if (collision.CompareTag("Collector") && collision.GetComponentInParent<GarbagePickup>().isDeliveringWaste == true)
        {
            // pause the collector movement
            collision.GetComponentInParent<CollectorMovement>().StopMoving();
            
            // reset the failure counter (in case it was triggered)
            if (collision.GetComponentInParent<ResetCollector>() != null)
            {
                collision.GetComponentInParent<ResetCollector>().isResetting = false;
            }

            // add it to the List at the Waste Centre
            myWasteCentreLot.collectorsWaitingList.Add(collision.transform.parent.gameObject);
        }

        // if the collision is the road hub at the front gate...
        if (collision.CompareTag("RoadHub"))
        {
            // store a reference
            roadHubAtWasteCentreFrontGate = collision.gameObject;
        }
    }
}
