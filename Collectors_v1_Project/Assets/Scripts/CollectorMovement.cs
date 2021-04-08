using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollectorMovement : MonoBehaviour
{
    [SerializeField] public GameObject selectedRoadHub = null;
    
    [SerializeField] public bool isMoving = false;
    [SerializeField] NavMeshAgent myAgent;

    [SerializeField] private float distanceToLocation;
    [SerializeField] private float arrivalTollerance;

    // Start is called before the first frame update
    void Start()
    {
        myAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(selectedRoadHub)
        {
            // calculate distance remaining
            distanceToLocation = Vector3.Distance(gameObject.transform.position, myAgent.destination);

            // check to see if the agent is close enough to the destination
            if (distanceToLocation <= arrivalTollerance)
            {
                StopMoving();
            }
        }
    }

    public void MoveToHub(GameObject _selectedHub)
    {
        // trigger movement to initial hub
        Debug.Log($"RoadHub selected to move to: {_selectedHub.name}.");

        if(myAgent)
        {
            isMoving = true;

            // move the agent toward the location
            myAgent.destination = selectedRoadHub.transform.position;

        }
    }

    public void StopMoving()
    {
        // debug
        Debug.Log("Collector stopped moving.");

        // make the agent stop moving
        myAgent.isStopped = true;
        isMoving = false;

        // empty the GO
        selectedRoadHub = null;
    }
}
