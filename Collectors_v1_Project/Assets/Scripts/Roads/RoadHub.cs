using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadHub : MonoBehaviour
{
    Color m_MouseOverColor = Color.red;
    Color m_DeactiveColour = Color.grey;
    Color m_OriginalColor;
    MeshRenderer m_Renderer;

    [Header("External References")]
    [SerializeField] private RoadMap myRoadMap;

    [Header("Connected Road Hubs")]
    [SerializeField] private GameObject nodeOrigin;
    [SerializeField] public List<GameObject> ConnectedRoadHubs;

    [Header("Active Collector")]
    [SerializeField] private GameObject activeCollector;


    void Start()
    {
        m_Renderer = GetComponentInChildren<MeshRenderer>();
        m_OriginalColor = m_Renderer.material.color;
        myRoadMap = GameObject.FindObjectOfType<RoadMap>();
        nodeOrigin = gameObject;
    }

    void OnMouseOver()
    {
        // highlight the RoadHub the player has the cursor on
        m_Renderer.material.color = m_MouseOverColor;
        Debug.Log($"MouseOver RoadHub: {gameObject.name}.");
  
        // 
        if(activeCollector != null)
        {
            if (activeCollector.GetComponent<CollectorMovement>().isActive)
            {
                // Debug.Log("MouseOver and ActiveCollector");

                // only trigger if the Player is 
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    // debug
                    Debug.Log($"Active Collector sent to RoadHub: {gameObject.name}.");

                    // stop where you are
                    activeCollector.GetComponent<CollectorMovement>().myAgent.destination = activeCollector.transform.position;

                    // store a reference to this object to the RoadMap
                    activeCollector.GetComponent<CollectorMovement>().selectedRoadHub = gameObject;

                    // set this location as the destination
                    activeCollector.GetComponent<CollectorMovement>().ResetDestination();

                    // deselect the Active Collector
                    activeCollector.GetComponent<CollectorMovement>().isActive = false;
                    ForgetActiveCollector();
                }
            }
        }
    }

    void OnMouseExit()
    {
        // change it back to the original colour
        m_Renderer.material.color = m_OriginalColor;
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
