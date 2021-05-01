using UnityEngine;
using UnityEngine.AI;

public class CollectorMovement : MonoBehaviour
{
    [SerializeField] public GameObject collectorDestination = null;
    
    [Header("References")]
    [SerializeField] public NavMeshAgent myAgent;
    [SerializeField] public CollectorTypes.CollectorType myCollectorType;

    [Header("Movement")]
    [SerializeField] public bool newDespatch = true;
    [SerializeField] public bool isMoving = false;
    [SerializeField] private float agentSpeed;

    [Header("Rotation Details")]
    [SerializeField] private bool isRotating = false;
    [SerializeField] private float angleTollerance;
    [SerializeField] private float rotationAngle;
    [SerializeField] private float rotationSpeed;

    [Header("Destination Details")]
    [SerializeField] public bool gotDestination = false;
    [SerializeField] private float arrivalTollerance;
    [SerializeField] private float distanceToLocation;
    [SerializeField] private Vector3 agentDestination;

    [Header("Depot Despatch Details")]
    [SerializeField] public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        myAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (collectorDestination && gotDestination)
        {
            newDespatch = false;
            GotDestination();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetRotationAngle();

        agentSpeed = myAgent.velocity.magnitude;
        agentDestination = myAgent.destination;
        distanceToLocation = Vector3.Distance(transform.position, myAgent.destination);
    }

    private void GetRotationAngle()
    {
        if (gotDestination && (agentSpeed > 0))
        {
            rotationAngle = Vector3.Angle(transform.position, myAgent.steeringTarget);
        }
        else
        {
            rotationAngle = 0.0f;
        }
    }

    public void GotDestination()
    {
        // if destination is set and the angle toward the first intersection is greater than zero
        if (collectorDestination && (Mathf.Abs(rotationAngle) > angleTollerance))
        {
            // rotate before moving
            isRotating = true;
            FaceForward(myAgent.steeringTarget);
        }
        // if has destination set, got destination and is not rotating...
        else if (collectorDestination && gotDestination && !isRotating)
        {
            // ... move toward destination
            MoveToHub(collectorDestination);
        }
        // if is moving and close enough to destination...
        else if (isMoving && (distanceToLocation <= arrivalTollerance))
        {
            StopMoving();
        }
    }


    public void MoveToHub(GameObject _selectedHub)
    {
        if(myAgent && myAgent.enabled)
        {
            // allow the NavMeshAgent to move again
            myAgent.isStopped = false;

            isMoving = true;
            myAgent.destination = _selectedHub.transform.position;
        }
    }

    private void FaceForward(Vector3 _facingTarget)
    {
        // if destination is set and the 
        if (collectorDestination && (Mathf.Abs(rotationAngle) > 0))
        {
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
        // update bools
        gotDestination = false;
        isRotating = false;
        isMoving = false;
        myAgent.isStopped = true;

        rotationAngle = 0.0f;

        // empty the GO
        //collectorDestination = null;
    }
}
