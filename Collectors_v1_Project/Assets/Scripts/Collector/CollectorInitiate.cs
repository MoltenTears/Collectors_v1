using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollectorInitiate : MonoBehaviour
{
    [SerializeField] public GameObject activeHub;
    [SerializeField] private GarbagePickup myGarbagePickup;
    [SerializeField] private GameManager myGameManager;
    [SerializeField] private GameObject myGameObject;
    
        // Start is called before the first frame update
    void Start()
    {
        myGarbagePickup = GetComponentInParent<GarbagePickup>();
        myGameManager = FindObjectOfType<GameManager>();
        myGameObject = transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        ArrivedAtDestination(other);
    }

    private void OnTriggerStay(Collider other)
    {
        ArrivedAtDestination(other);
    }

    private void ArrivedAtDestination(Collider other)
    {
        // if the collision is a Collector
        if (other.GetComponent<RoadHub>())
        {
            // iterate through the activeCollectorList...
            foreach (ActiveCollector activeCollector in myGameManager.activeCollectorsList)
            {
                // Debug.Log($"Collector (List) Name: {activeCollector.collector.name}.");
                // Debug.Log($"Collector (Self) Name: {myGameObject.transform.parent.name}.");

                // if this collector is in the List...
                if (activeCollector.collector.name == myGameObject.transform.parent.name)
                {
                    // debug
                    // Debug.Log("Found this Collector in the List.");

                    // Debug.Log($"RoadHub (List) Name: {activeCollector.destination.name}.");
                    // Debug.Log($"RoadHub (Self) Name: {other.GetComponentInParent<RoadHub>().gameObject.name}.");


                    // ... if there is a RoadHub in the list that matches the RoadHub that was collided with...
                    if (activeCollector.destination.name == other.gameObject.name)
                    {
                        // Debug.Log($"Collector target: {activeCollector.destination.name}, has arrived at: {other.gameObject.name}");

                        activeCollector.collector.GetComponentInChildren<GarbagePickup>().isCollecting = true;
                    }

                }

            }
        }
    }
}
