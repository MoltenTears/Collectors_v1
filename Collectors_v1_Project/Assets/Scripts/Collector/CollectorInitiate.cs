using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollectorInitiate : MonoBehaviour
{
    [SerializeField] public GameObject activeHub;
    [SerializeField] private GarbagePickup myGarbagePickup;
    
        // Start is called before the first frame update
    void Start()
    {
        myGarbagePickup = GetComponentInParent<GarbagePickup>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // if the collision is a Collector
        if (other.GetComponent<RoadHub>())
        {
            //Debug.Log($"Collector triggered CollectorInitiate on {other.gameObject.name}.");

            if (other.GetComponent<RoadHub>().activeHub != null)
            {
                //Debug.Log($"{other.gameObject.name} knows that {other.GetComponent<RoadHub>().activeHub.gameObject.name} is the active RoadHub.");

                //Debug.Log($"This RoadHub's name  : {other.GetComponent<RoadHub>().activeHub.gameObject.name}.");
                //Debug.Log($"the activeHub's name : {other.gameObject.name}.");

                if (other.GetComponent<RoadHub>().activeHub.gameObject.name == other.gameObject.name)
                {
                    //Debug.Log("Collector has arrived at the activeHub.");

                    // check what the collector is doing
                    if (!myGarbagePickup.isCollecting
                        && !myGarbagePickup.isDeliveringWaste
                        && !myGarbagePickup.isReturningToDepot)
                    {
                        Debug.Log("Collector arrived at activeHub and is ready to start collecting!");

                        // if it's not doing anything, set it to collecting
                        myGarbagePickup.isCollecting = true;
                    }
                }
            }
        }
    }
}
