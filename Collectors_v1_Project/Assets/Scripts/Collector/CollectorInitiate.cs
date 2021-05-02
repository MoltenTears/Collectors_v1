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
        if(GetComponentInParent<CollectorMovement>())
        {
            myGameObject = GetComponentInParent<CollectorMovement>().transform.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ArrivedAtDestination(other);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    ArrivedAtDestination(other);
    //}

    private void ArrivedAtDestination(Collider other)
    {
        // if the collision is a Collector
        if (other.GetComponent<RoadHub>())
        {
            // iterate through the activeCollectorList...
            foreach (ActiveCollector activeCollector in myGameManager.activeCollectorsList)
            {
                // iterate through the active Collectors list
                for (int i = 0; i < myGameManager.activeCollectorsList.Count; i++)
                {
                    // if the Active Collector is in the list...
                    if (myGameManager.activeCollectorsList[i].collector == myGameObject)
                    {
                        // ... if there is a RoadHub in the list that matches the RoadHub that was collided with...
                        if (myGameManager.activeCollectorsList[i].destination ==  other.gameObject)
                        {
                            // commence collection
                            GameObject collector = myGameManager.activeCollectorsList[i].collector;
                            collector.GetComponentInChildren<GarbagePickup>().isCollecting = true;
                        }
                        // stop looking, we're done
                        break;
                    }
                }
            }
        }
    }
}
