using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNode : MonoBehaviour
{
    private bool checkedHub = false;
    [SerializeField] GameObject connectedHub;
    [SerializeField] private GameObject myParentGO;

    void Start()
    {
        myParentGO = gameObject.GetComponentInParent<RoadHub>().gameObject;
    }

    private void OnTriggerStay(Collider collision)
    {
        // if the collision is a RoadHub
        if (collision.gameObject.GetComponent<RoadHub>() && !checkedHub)
        {
            Debug.Log("RoadNode at RoadHub.");

            // store a reference
            connectedHub = collision.gameObject;

            // ... if i know my parent
            if (myParentGO)
            {
                // ... give it to the parent list
                myParentGO.GetComponent<RoadHub>().ConnectedRoadHubs.Add(connectedHub);
            }

            // stop this from running again
            checkedHub = true;
        }

    }
}
