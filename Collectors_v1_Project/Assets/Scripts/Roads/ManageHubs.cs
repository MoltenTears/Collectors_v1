using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageHubs : MonoBehaviour
{
    [SerializeField] private List<GameObject> roadHubs;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveStep;
    [SerializeField] private float yCoordDown;
    [SerializeField] private float yCoordUp;
    [SerializeField] private float yCoordShow;
    [SerializeField] private float moveTollerance;
    [SerializeField] private bool myNewDespatch;

    [SerializeField] public bool showHubs;

    // Start is called before the first frame update
    void Start()
    {
        RoadHub[] roadHubsTemp = FindObjectsOfType<RoadHub>();
        foreach (RoadHub roadHub in roadHubsTemp)
        {
            roadHubs.Add(roadHub.transform.gameObject);

            roadHub.transform.position = new Vector3(roadHub.transform.position.x, yCoordDown, roadHub.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetStep();

        CheckNewDespatch();

        if (myNewDespatch)
        {
            ShowHubs();
        }
        else
        {
            HideHubs();
        }
    }

    private void GetStep()
    {
        if (moveSpeed > 0)
        {
            moveStep = moveSpeed * Time.deltaTime;
        }
        else
        {
            Debug.Log("moveSpeed not set in ManageHubs");
        }
    }

    public void ShowHubs()
    {
        // each raodHub...
        foreach (GameObject roadHub in roadHubs)
        {
            // Debug.Log($"{roadHub.name} wants to move from Y: {roadHub.transform.position.y} to Y: {yCoordUp}");

            // ... if the object is not yet at the new position...
            if (Vector3.Distance(roadHub.transform.position,
                new Vector3(roadHub.transform.position.x, yCoordUp, roadHub.transform.position.z)) > moveTollerance)
            {
                // ... MoveToward the Hide position
                roadHub.transform.position = Vector3.MoveTowards(roadHub.transform.position, new Vector3(roadHub.transform.position.x, yCoordUp, roadHub.transform.position.z), moveStep);
            }
        }
    }

    public void HideHubs()
    {
        // each raodHub...
        foreach (GameObject roadHub in roadHubs)
        {
            // Debug.Log($"{roadHub.name} wants to move from Y: {roadHub.transform.position.y} to Y: {yCoordDown}");

            if (Vector3.Distance(roadHub.transform.position, 
                new Vector3(roadHub.transform.position.x, yCoordDown, roadHub.transform.position.z)) > moveTollerance)
            {
                // ... MoveToward the Hide position
                roadHub.transform.position = Vector3.MoveTowards(roadHub.transform.position, new Vector3(roadHub.transform.position.x, yCoordDown, roadHub.transform.position.z), moveStep);
            }
        }
    }

    private void CheckNewDespatch()
    {
        CollectorMovement[] collectorMovements = FindObjectsOfType<CollectorMovement>();
        foreach (CollectorMovement collectorMovement in collectorMovements)
        {
            if (collectorMovement.newDespatch)
            {
                myNewDespatch = true;
                break;
            }
            else
            {
                myNewDespatch = false;
            }
        }
    }
}
