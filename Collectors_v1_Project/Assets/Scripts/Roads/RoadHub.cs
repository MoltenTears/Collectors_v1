using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadHub : MonoBehaviour
{
    public Color m_MouseOverColor = Color.red;
    public Color m_DeactiveColour;
    public Color m_OriginalColor;
    MeshRenderer m_Renderer;

    [Header("Active RoadHub")]
    [SerializeField] public bool isActive;
    [SerializeField] public GameObject activeHub;

    [Header("External References")]
    [SerializeField] private RoadMap myRoadMap;
    [SerializeField] private CollectorInitiate myCollectorInitate;
    [SerializeField] private GameManager myGameManager;
    [SerializeField] private DepotManager myDepotManager;
    [SerializeField] private AudioManager myAudioManager;

    [Header("Zone Management Details")]  
    [SerializeField] private ZoneManager myZoneManager;
    [SerializeField] private List<RoadHub> myZoneRoadHubs;

    [Header("Connected Road Hubs")]
    [HideInInspector] private GameObject nodeOrigin;
    [HideInInspector] public List<GameObject> ConnectedRoadHubs;

    [Header("Active Collector")]
    [SerializeField] private GameObject activeCollector;

    void Start()
    {
        m_Renderer = GetComponentInChildren<MeshRenderer>();
        m_OriginalColor = m_Renderer.material.color;
        myRoadMap = GameObject.FindObjectOfType<RoadMap>();
        nodeOrigin = gameObject;
        myCollectorInitate = FindObjectOfType<CollectorInitiate>();
        myDepotManager = FindObjectOfType<DepotManager>();
        myGameManager = FindObjectOfType<GameManager>();
        myAudioManager = FindObjectOfType<AudioManager>();
        myZoneManager = GetComponentInParent<ZoneManager>();
        foreach (RoadHub roadHub in myZoneManager.myRoadHubs)
        {
            myZoneRoadHubs.Add(roadHub);
        }
    }

    private void Update()
    {
        RaycastRoadHub();
        SelectRoadHub();
    }

    private void SelectRoadHub()
    {
        // if this RoadHub isActive
        if (isActive)
        {
            // if there is an Active Collector
            if (activeCollector != null)
            {
                // re-check if the Active Collector is indeed Active
                if (activeCollector.GetComponent<CollectorMovement>().newDespatch)
                {
                    // only trigger if the Player presses the right key AND there is not already a Collector in the Zone
                    if (Input.GetKeyDown(KeyCode.Mouse0) && this.myZoneManager.currentZoneCollector == null)
                    {
                        // play SFX
                        myAudioManager.RoadHubClickSFX();

                        // add the combo to the List in GameManager
                        myGameManager.AddCollectorDestination(activeCollector, this.gameObject);

                        // store a reference to this object in the collector
                        activeCollector.GetComponent<CollectorMovement>().collectorDestination = activeHub;

                        // set this location as the destination
                        activeCollector.GetComponent<CollectorMovement>().ResetDestination();

                        // tell the Collector which zone their working in
                        activeCollector.GetComponentInChildren<GarbagePickup>().collectionZone = myZoneManager;
                        myZoneManager.currentZoneCollector = activeCollector;

                        // deselect the Active Collector
                        activeCollector.GetComponent<CollectorMovement>().isActive = false;
                        ForgetActiveCollector();
                    }
                    // else if the player presses the key and there is already a Collector in the Zone
                    else if (Input.GetKeyDown(KeyCode.Mouse0) && this.myZoneManager.currentZoneCollector != null)
                    {
                        // Debug.Log($"Cannot send collector to {myZoneManager.gameObject.name}; Collector already present. Returned to Depot.");

                        // destroy the instantiation
                        Destroy(activeCollector);

                        // add it back into the Depot
                        myDepotManager.baseCollectors++;
                    }
                }
            }
        }
    }

    private void RaycastRoadHub()
    {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));

        foreach (RaycastHit hit in hits)
        {
            // if there is an ActiveCollector
            if (hit.collider.GetComponent<RoadHub>())
            {
                // store a reference for later
                activeHub = hit.collider.transform.gameObject;

                // activate the RoadHub
                activeHub.GetComponent<RoadHub>().isActive = true;

                // for each RoadHub in the Zone...
                foreach (RoadHub roadHub in activeHub.GetComponent<RoadHub>().myZoneRoadHubs)
                {
                    // change colour to indicate to the player the hub is activated
                    hit.transform.GetComponent<MeshRenderer>().material.color = m_MouseOverColor;
                }

                // break the foreach (because we found what we're looking for)
                break;
            }
            else
            {
                // deactiveate the RoadHub
                isActive = false;

                // if there was a reference to an ActiveHub
                if (activeHub != null)
                {
                    // creset the RoadHub

                    // for each RoadHub in the Zone...
                    foreach (RoadHub roadHub in activeHub.GetComponent<RoadHub>().myZoneRoadHubs)
                    {
                        // change colour to indicate to the player the hub is activated
                        roadHub.transform.GetComponent<MeshRenderer>().material.color = m_OriginalColor;
                    }
                    
                    // deactivate this RoadHub
                    activeHub.GetComponent<RoadHub>().isActive = false;
                }
            }
        }
    }

    private void ForgetActiveCollector()
    {
        // find all the RoadHubs in the game...
        RoadHub[] roadHubs = FindObjectsOfType<RoadHub>();
        foreach (RoadHub roadHub in roadHubs)
        {
            // tell them to forget about the activeCollector
            roadHub.activeCollector = null;
        }
    }

    public void ReceiveActiveCollector(GameObject _activeCollector)
    {
        activeCollector = _activeCollector;
    }
}
