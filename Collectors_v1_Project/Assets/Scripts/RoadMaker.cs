using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMaker : MonoBehaviour
{
    [SerializeField] private GameObject nodeOrigin;
    [SerializeField] private List<GameObject> roadMarkers;
    private Transform[] nodeRoadsArray;

    [SerializeField] public List<GameObject> ConnectedRoadHubs;

    // Start is called before the first frame update
    void Start()
    {
        CollectNodes();
    }

    private void CollectNodes()
    {
        // know thyself
        nodeOrigin = gameObject;

        // get all roadNodes connected to this gameobject
        nodeRoadsArray = gameObject.GetComponentsInChildren<Transform>();

        // store them in a List
        foreach (Transform node in nodeRoadsArray)
        {
            // if it's the parent gameobject
            if (node.gameObject.transform == gameObject.transform)
            {
                // do nothing
                continue;
            }

            // add it to the List
            roadMarkers.Add(node.gameObject);
        }
    }



}
