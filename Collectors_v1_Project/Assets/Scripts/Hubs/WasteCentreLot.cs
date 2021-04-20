using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteCentreLot : MonoBehaviour
{
    [SerializeField] private WasteQueueSpot[] myWasteQueueSpots;
    [SerializeField] private GameObject[] myCollectorPositions;
    [SerializeField] private WasteCentreFrontGate myWasteCentreFrontGate;

    [SerializeField] public Queue<GameObject> collectorsWaiting = new Queue<GameObject>();

    [SerializeField] public List<GameObject> collectorsWaitingList = new List<GameObject>();

    [Header("Collectors Details")]
    [SerializeField] private bool lotOccupied;
    [SerializeField] public GameObject nextCollector;

    [Header("References")]
    [SerializeField] private WasteCenteManager myWasteCentreManager;

    void Start()
    {
        myWasteCentreManager = GetComponentInParent<WasteCenteManager>();
        myWasteCentreFrontGate = GetComponentInChildren<WasteCentreFrontGate>();
        FindQueueSpots();
    }

    void Update()
    {
        UpdateQueue();

        NextCollector();

        ReadyToDumpWaste();

        ReleaseCollector();
    }

    private void ReleaseCollector()
    {
        if (nextCollector && nextCollector.GetComponentInChildren<GarbagePickup>().isReturningToDepot)
        {
            // turn back on NAvMeshAgent
            nextCollector.GetComponent<CollectorMovement>().myAgent.enabled = true;

            // remove Collector from from of List
            lotOccupied = false;
            nextCollector = null;
            collectorsWaitingList.RemoveAt(0);
        }
    }

    public void GetNextCollector()
    {
        // TODO actions to take after last Collector left dumping spot
    }

    private void ReadyToDumpWaste()
    {
        if (nextCollector)
        {
            // Debug.Log($"Collector name {nextCollector.name} notified to drop waste.");
            nextCollector.GetComponentInChildren<WasteDrop>().isDroppingWaste = true;
        }
    }

    public void UpdateQueue()
    {
        for (int i = 0; i < collectorsWaitingList.Count; i++)
        {
            // turn off the NavMesh Agent
            collectorsWaitingList[i].GetComponentInParent<CollectorMovement>().myAgent.enabled = false;

            // put it in an assigned spot
            collectorsWaitingList[i].transform.position = myCollectorPositions[i].transform.position;
            collectorsWaitingList[i].transform.rotation = myCollectorPositions[i].transform.rotation;
        }
    }

    private void FindQueueSpots()
    {
        myWasteQueueSpots = new WasteQueueSpot[9];
        myCollectorPositions = new GameObject[9];
        myWasteQueueSpots = GetComponentsInChildren<WasteQueueSpot>();
        for (int i = 0; i < myWasteQueueSpots.Length; i++)
        {
            myCollectorPositions[i] = myWasteQueueSpots[i].gameObject;
        }
    }

    public void NextCollector()
    {
        if (collectorsWaitingList.Count > 0 && !lotOccupied)
        {
            lotOccupied = true;

            nextCollector = collectorsWaitingList[0];

        }
    }
}
