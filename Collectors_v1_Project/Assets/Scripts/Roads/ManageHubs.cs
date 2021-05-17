using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageHubs : MonoBehaviour
{
    [SerializeField] public List<GameObject> roadHubs;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveStep;
    [SerializeField] private float yCoordDown;
    [SerializeField] private float yCoordUp;
    [SerializeField] private float yCoordShow;
    [SerializeField] private float moveTollerance;
    [SerializeField] private bool myNewDespatch;

    [SerializeField] public bool confirmedHubs;
    [SerializeField] public bool showHubs;

    private void Start()
    {
        ConfirmHubs();
    }

    // Update is called once per frame
    void Update()
    {
        GetStep();

        CheckNewDespatch();

        UpdateHubs(); 
    }

    public void ConfirmHubs()
    {
        if (!confirmedHubs)
        {
            if (roadHubs.Count == 0)
            {
                InitialiseHubs();
            }
            else
            {
                Debug.Log($"RoadHub count: {roadHubs.Count}.");
            }
        }
    }

    public void InitialiseHubs()
    {
        Debug.Log("RoadHubs Initialised.");

        // flush the old list
        roadHubs.Clear();

        RoadHub[] roadHubsTemp = FindObjectsOfType<RoadHub>();

        foreach (RoadHub roadHub in roadHubsTemp)
        {
            roadHubs.Add(roadHub.transform.gameObject);

            roadHub.transform.position = new Vector3(roadHub.transform.position.x, yCoordDown, roadHub.transform.position.z);
        }

        // stop the loop
        if (roadHubs.Count > 0)
        {
            confirmedHubs = true;
        }
    }

    public void UpdateHubs()
    {
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

            if (roadHub != null)
            {
                // ... if the object is not yet at the new position...
                if (Vector3.Distance(roadHub.transform.position,
                new Vector3(roadHub.transform.position.x, yCoordUp, roadHub.transform.position.z)) > moveTollerance)
                {
                    // ... MoveToward the Hide position
                    roadHub.transform.position = Vector3.MoveTowards(roadHub.transform.position, new Vector3(roadHub.transform.position.x, yCoordUp, roadHub.transform.position.z), moveStep);
                }
            }
        }
    }

    public void HideHubs()
    {

        // each raodHub...
        foreach (GameObject roadHub in roadHubs)
        {
            if (roadHub != null)
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
    }

    private void CheckNewDespatch()
    {
        CollectorMovement[] collectorMovements = FindObjectsOfType<CollectorMovement>();
        foreach (CollectorMovement collectorMovement in collectorMovements)
        {
            if (collectorMovement.newDespatch)
            {
                // Debug.Log("New Collector Despatched!");
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
