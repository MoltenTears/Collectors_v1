using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollectorMovement : MonoBehaviour
{
    [SerializeField] public GameObject selectedRoadHub = null;
    
    [Header("Movement")]
    [SerializeField] public NavMeshAgent myAgent;
    [SerializeField] public bool gotDestination = false;
    [SerializeField] public bool isMoving = false;
    [SerializeField] private bool isRotating = false;
    [SerializeField] private float rotationAngle;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float distanceToLocation;
    [SerializeField] private float arrivalTollerance;
    [SerializeField] private float angleTollerance;
    [SerializeField] private Vector3 agentDestination;

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
    void FixedUpdate()
    {
        agentDestination = myAgent.destination;
        float velocity = myAgent.velocity.magnitude;
        // Debug.Log($"myAgent velocity == {velocity}.");

        if (gotDestination && (myAgent.velocity.magnitude > 0))
        {
            rotationAngle = Vector3.Angle(transform.position, myAgent.steeringTarget);
        }
        else
        {
            rotationAngle = 0.0f;
        }

        if (selectedRoadHub && gotDestination)
        {
            GotDestination();
        }
    }

    private void GotDestination()
    {
        // Debug.Log($"angle to roate == {Vector3.Angle(transform.position, myAgent.steeringTarget)}");

        // if destination is set and the angle toward the first intersection is greater than zero
        if (selectedRoadHub && (Mathf.Abs(rotationAngle) > angleTollerance))
        {
            // rotate before moving
            isRotating = true;
            FaceForward(myAgent.steeringTarget);
        }
        // if not rotating, and has destination set...
        else if (!isRotating && selectedRoadHub)
        {
            // Debug.Log("started moving toward destination.");

            // ... move toward destination
            MoveToHub(selectedRoadHub);
        }
        // if is moving and close enough to destination...
        else if (isMoving && (distanceToLocation <= arrivalTollerance))
        {
            // ... stop moving
            StopMoving();
        }
    }

    private void FaceForward(Vector3 _facingTarget)
    {
        // if destination is set and the 
        if (selectedRoadHub && (Mathf.Abs(rotationAngle) > 0))
        {
            // debug
            // Debug.Log("Collector needs to rotate before moving.");

            // Determine which direction to rotate towards
            Vector3 targetDirection = _facingTarget - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = rotationSpeed * Time.deltaTime;

            // rotate the forward direction toward the target direction by one step
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDir, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDir);
        }
        else
        {
            isRotating = false;
        }
    }

    public void MoveToHub(GameObject _selectedHub)
    {
        // trigger movement to initial hub
        // Debug.Log($"RoadHub selected to move to: {_selectedHub.name}.");

        if(myAgent)
        {
            isMoving = true;
            myAgent.destination = _selectedHub.transform.position;
        }
    }

    public void ResetDestination()
    {
        // update bools
        isRotating = false;
        isMoving = false;
        gotDestination = true;

        rotationAngle = 0.0f;
    }

    public void StopMoving()
    {
        // debug
        Debug.Log("Collector stopped moving.");

        // update bools
        gotDestination = false;
        isRotating = false;
        isMoving = false;

        rotationAngle = 0.0f;

        // empty the GO
        selectedRoadHub = null;
    }
}
