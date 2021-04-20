using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepotLot : MonoBehaviour
{
    [Header("Collector References")]
    [SerializeField] public GameObject baseCollector;

    [Header("Parking Details")]
    [SerializeField] public CollectorTypes.CollectorType myCollectorInParking;

    // Start is called before the first frame update
    void Start()
    {
        HideCollectors();
    }

    // Update is called once per frame
    void Update()
    {
        ShowCollector();
    }

    public void ShowCollector()
    {
        // reset all the parking lots
        HideCollectors();

        switch (myCollectorInParking)
        {
            case CollectorTypes.CollectorType.BASE:
                {
                    // show the BASE collector
                    baseCollector.SetActive(true);
                    break;
                }
            case CollectorTypes.CollectorType.NONE:
                {
                    // do nothing, there is no collector in this spot
                    break;
                }
            default:
                {
                    // error in switch
                    Debug.LogError("DepotLot ShowCollector() switch result default. Review Error as lot may be full");
                    HideCollectors();
                    break;
                }
        }
    }

    private void HideCollectors()
    {
        // use this to hide all collector types for this parking lot
        baseCollector.SetActive(false);
    }
}
