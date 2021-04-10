using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollectorMovement : MonoBehaviour
{
    [SerializeField] public GameObject selectedRoadHub = null;
    
    [Header("Movement")]
    [SerializeField] NavMeshAgent myAgent;
    [SerializeField] public bool isMoving = false;
    [SerializeField] private float distanceToLocation;
    [SerializeField] private float distanceToHubFromCollectorTemp;
    [SerializeField] private float distanceToHubFromDestinationTemp;
    [SerializeField] private float distanceToHubFromCollector;
    [SerializeField] private float distanceToHubFromDestination;
    [SerializeField] private float arrivalTollerance;

    [Header("RoadHubs")]
    [SerializeField] public RoadHub[] allRoadHubs;
    [SerializeField] private GameObject nextClosestHub;
    [SerializeField] private GameObject nextHubToDestination;

    // Start is called before the first frame update
    void Start()
    {
        myAgent = gameObject.GetComponent<NavMeshAgent>();
        allRoadHubs = GameObject.FindObjectsOfType<RoadHub>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedRoadHub)
        {
            // calculate distance remaining
            distanceToLocation = Vector3.Distance(gameObject.transform.position, myAgent.destination);

            // find the closest hub to the Collector
            FindNextHub();

            // check to see if the agent is close enough to the destination...
            if (myAgent.velocity.magnitude == 0 && distanceToLocation == 0)
            {
                // .. then stop all moving variables
                StopMoving();
            }
        }
    }

    private void FindNextHub()
    {
        distanceToHubFromCollectorTemp = Mathf.Infinity;
        distanceToHubFromDestinationTemp = Mathf.Infinity;

        // set distances               
        foreach (RoadHub roadHub in allRoadHubs)
        {
            // ingnore the current roadHub
            if (Vector3.Distance(roadHub.transform.position, gameObject.transform.position) <= 1)
            {
                continue;
            }

            // get the distance from the roadHub to the Collector
            distanceToHubFromCollector = Vector3.Distance(gameObject.transform.position, roadHub.transform.position);

            // get the distance from the roadHub to the Destination
            distanceToHubFromDestination = Vector3.Distance(selectedRoadHub.transform.position, roadHub.transform.position);

            // if the hub is closer to the Collector than the last hub...
            if (distanceToHubFromCollector <= distanceToHubFromCollectorTemp)
            {
                // overwrite the temp variable
                distanceToHubFromCollectorTemp = distanceToHubFromCollector;

                // ... mark this roadHub as the nextClosestHub
                nextClosestHub = roadHub.transform.gameObject;
            }
        }
    }

    public void MoveToHub(GameObject _selectedHub)
    {
        // trigger movement to initial hub
        // Debug.Log($"RoadHub selected to move to: {_selectedHub.name}.");

        if(myAgent)
        {
            isMoving = true;

            // if the Collector is not yet at the destination hub...
            if (distanceToHubFromCollector < 0)
            {
                // ... move towards the closest Hub




                // ...move the agent toward the location
                myAgent.destination = selectedRoadHub.transform.position;
            }
        }
    }

    public void StopMoving()
    {
        // debug
        // Debug.Log("Collector stopped moving.");

        // make the agent stop moving
        isMoving = false;

        // empty the GO
        selectedRoadHub = null;
    }
}
