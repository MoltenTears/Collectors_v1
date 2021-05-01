using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadHub : MonoBehaviour
{
    Color m_MouseOverColor = Color.red;
    Color m_DeactiveColour = Color.grey;
    Color m_OriginalColor;
    MeshRenderer m_Renderer;

    [Header("Active RoadHub")]
    [SerializeField] public bool isActive;
    [SerializeField] public GameObject activeHub;

    [Header("External References")]
    [SerializeField] private RoadMap myRoadMap;
    [SerializeField] private CollectorInitiate myCollectorInitate;
    [SerializeField] private GameManager myGameManager;

    [Header("Connected Road Hubs")]
    [SerializeField] private GameObject nodeOrigin;
    [SerializeField] public List<GameObject> ConnectedRoadHubs;

    [Header("Active Collector")]
    [SerializeField] private GameObject activeCollector;

    

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
                    // only trigger if the Player presses the right key
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        // add the combo to the List in GameManager
                        myGameManager.AddCollectorDestination(activeCollector, this.gameObject);

                        // store a reference to this object to the RoadMap
                        activeCollector.GetComponent<CollectorMovement>().collectorDestination = activeHub;

                        // set this location as the destination
                        activeCollector.GetComponent<CollectorMovement>().ResetDestination();

                        // deselect the Active Collector
                        activeCollector.GetComponent<CollectorMovement>().isActive = false;
                        ForgetActiveCollector();
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

                // change colour to indicate to the player the hub is activated
                hit.transform.GetComponent<MeshRenderer>().material.color = m_MouseOverColor;

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
                    activeHub.GetComponent<MeshRenderer>().material.color = m_OriginalColor;
                    activeHub.GetComponent<RoadHub>().isActive = false;
                }
            }
        }
    }


    void Start()
    {
        m_Renderer = GetComponentInChildren<MeshRenderer>();
        m_OriginalColor = m_Renderer.material.color;
        myRoadMap = GameObject.FindObjectOfType<RoadMap>();
        nodeOrigin = gameObject;
        myCollectorInitate = FindObjectOfType<CollectorInitiate>();
        myGameManager = FindObjectOfType<GameManager>();
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
