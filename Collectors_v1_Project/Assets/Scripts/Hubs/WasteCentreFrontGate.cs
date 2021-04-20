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
        // if the collision is a collector and they are intending to return to the depot...
        if (collision.CompareTag("Collector") && collision.GetComponentInChildren<GarbagePickup>().isDeliveringWaste == true)
        {
            // pause the collector movement
            collision.GetComponentInParent<CollectorMovement>().StopMoving();


            // myWasteCentreLot.collectorsWaiting.Enqueue(collision.transform.parent.gameObject);
            myWasteCentreLot.collectorsWaitingList.Add(collision.transform.parent.gameObject);

            


            // Debug.Log($"Next Collector in queue is: {myWasteCentreLot.collectorsWaiting.Peek().name}.");



            // display a fake truck "unloading" waste


            // like collection, remove waste at a steady rate into the Waste Centre

        }

        // if the collision is the road hub at the front gate...
        if (collision.CompareTag("RoadHub"))
        {
            // store a reference
            roadHubAtWasteCentreFrontGate = collision.gameObject;
        }
    }

    private void HideCollector(GameObject _collector)
    {
        // stop moving
        _collector.GetComponent<CollectorMovement>().StopMoving();

        _collector.GetComponentInChildren<MeshRenderer>().enabled = false;

    }
}
