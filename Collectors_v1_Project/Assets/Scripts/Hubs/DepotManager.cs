using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepotManager : MonoBehaviour
{
    [SerializeField] public DepotLot[] myDepotLots;
    [SerializeField] public int baseCollectors; // TODO repeat for future different types of Collectors
    [SerializeField] private DepotFrontGate myDepotFrontGate;
    [SerializeField] private Transform depotFrontGateTrans;
    [SerializeField] private GameObject baseCollectorPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        myDepotLots = GetComponentsInChildren<DepotLot>();
        myDepotFrontGate = GetComponentInChildren<DepotFrontGate>();
        depotFrontGateTrans = myDepotFrontGate.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateParkingLot();
    }

    public void UpdateParkingLot()
    {
        // clear the lot
        for (int i = 0; i < myDepotLots.Length; i++)
        {
            myDepotLots[i].myCollectorInParking = CollectorTypes.CollectorType.NONE;
        }

        // storea temp variable
        int tempBaseCollectors = baseCollectors;

        // Debug.Log($"tempBaseCollectors: {tempBaseCollectors}.");

        // add to the lot
        for (int i = 0; i < myDepotLots.Length; i++)
        {
            if (tempBaseCollectors > 0)
            {
                myDepotLots[i].myCollectorInParking = CollectorTypes.CollectorType.BASE;
                --tempBaseCollectors;
            }
        }
    }

    public void DespatchCollector()
    {
        //Debug.Log("Collector despatched from Depot.");

        // create a new Collector
        GameObject newCollector = Instantiate(baseCollectorPrefab, new Vector3(depotFrontGateTrans.position.x, 0, depotFrontGateTrans.position.z), transform.rotation);
        newCollector.GetComponent<CollectorMovement>().isActive = true;

        //remove the old one from the parkign lot
        baseCollectors--;

        // Debug.Log($"Collectors at Depot: {baseCollectors}.");

        // find all the RoadHubs in the game...
        RoadHub[] roadHubs = FindObjectsOfType<RoadHub>();
        foreach (RoadHub roadHub in roadHubs)
        {
            // tell them which object is the active Collector
            roadHub.ReceiveActiveCollector(newCollector);
        }

    }
}
