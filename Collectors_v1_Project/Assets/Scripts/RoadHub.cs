using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadHub : MonoBehaviour
{
    Color m_MouseOverColor = Color.red;
    Color m_OriginalColor;
    MeshRenderer m_Renderer;

    [Header("External References")]
    [SerializeField] private RoadMap myRoadMap;
    [SerializeField] private CollectorMovement myCollectorMovement;

    [Header("Connected Road Hubs")]
    [SerializeField] private GameObject nodeOrigin;
    [SerializeField] public List<GameObject> ConnectedRoadHubs;


    void Start()
    {
        m_Renderer = GetComponentInChildren<MeshRenderer>();
        m_OriginalColor = m_Renderer.material.color;
        myRoadMap = GameObject.FindObjectOfType<RoadMap>();
        myCollectorMovement = GameObject.FindObjectOfType<CollectorMovement>();
        nodeOrigin = gameObject;
    }

    void OnMouseOver()
    {
        // Debug.Log($"MouseOver {gameObject.name}.");
        m_Renderer.material.color = m_MouseOverColor;
  
        // trigger movement if the Collector is not already moving
        if(!myCollectorMovement.isMoving)
        {
            // only trigger if the Player is 
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // debug
                Debug.Log("mouse0 pressed.");
                
                // store a reference to this object to the RoadMap
                myCollectorMovement.selectedRoadHub = gameObject;

                // if not already moving...
                if (!myCollectorMovement.isMoving)
                {
                    // ... trigger the function to move
                    myCollectorMovement.MoveToHub(gameObject);
                }
            }
        }
    }

    void OnMouseExit()
    {
        // Debug.Log($"MouseExit {gameObject.name}.");
        m_Renderer.material.color = m_OriginalColor;
        // myRoadMap.hoverOverRoadHub = null;
    }
}
