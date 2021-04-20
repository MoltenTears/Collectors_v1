using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteCentreLot : MonoBehaviour
{
    [SerializeField] private WasteQueueSpot[] myWasteQueueSpots;
    [SerializeField] private GameObject[] myCollectorPositions;

    [SerializeField] public Queue<GameObject> collectorsWaiting = new Queue<GameObject>();

    [SerializeField] public List<GameObject> collectorsWaitingList = new List<GameObject>();

    [Header("Collectors Details")]
    [SerializeField] private bool lotOccupied;
    [SerializeField] public GameObject nextCollector;
    [SerializeField] private GameObject baseCollector;

    [Header("References")]
    [SerializeField] private WasteCenteManager myWasteCentreManager;

    // Start is called before the first frame update
    void Start()
    {
        myWasteCentreManager = GetComponentInParent<WasteCenteManager>();
        FindQueueSpots();
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

    // Update is called once per frame
    void Update()
    {
        UpdateQueue();

        // RollCall();

        NextCollector();
    }

    public void UpdateQueue()
    {
        for (int i = 0; i < collectorsWaitingList.Count; i++)
        {
            // turn off the NavMesh Agent
            collectorsWaitingList[i].GetComponent<CollectorMovement>().myAgent.enabled = false;

            // put it in an assigned spot
            collectorsWaitingList[i].transform.position = myCollectorPositions[i].transform.position;
            collectorsWaitingList[i].transform.rotation = myCollectorPositions[i].transform.rotation;
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

    //public void ShowCollector()
    //{
    //    // reset all the parking lots
    //    HideCollector();

    //    switch (currentCollectorDisplayed)
    //    {
    //        case CollectorTypes.CollectorType.BASE:
    //            {
    //                // show the BASE collector
    //                baseCollector.SetActive(true);
    //                break;
    //            }
    //        case CollectorTypes.CollectorType.NONE:
    //            {
    //                // do nothing, there is no collector in this spot
    //                break;
    //            }
    //        default:
    //            {
    //                // error in switch
    //                Debug.LogError("DepotLot ShowCollector() switch result default. Review Error as lot may be full");
    //                HideCollector();
    //                break;
    //            }
    //    }
    //}

    //private void HideCollector()
    //{
    //    // use this to hide all collector types for this parking lot
    //    baseCollector.SetActive(false);
    //}

    //private void RollCall()
    //{
    //    if (collectorsWaiting.Count > 0)
    //    {
    //        Debug.Log($"{collectorsWaiting.Count} Collector(s) at Waste Centre.");
    //    }
    //}
}
