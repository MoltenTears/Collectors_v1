using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepotManager : MonoBehaviour
{
    [SerializeField] public DepotLot[] myDepotLots;
    [SerializeField] public int baseCollectors; // TODO repeat for future different types of Collectors
    

    // Start is called before the first frame update
    void Start()
    {
        myDepotLots = GetComponentsInChildren<DepotLot>();    
    }

    // Update is called once per frame
    void Update()
    {
        UpdateParkingLot();
    }

    public void UpdateParkingLot()
    {
        int tempBaseCollectors = baseCollectors;

        for (int i = 0; i < myDepotLots.Length; i++)
        {
            if (tempBaseCollectors > 0)
            {
                myDepotLots[i].myCollectorInParking = DepotLot.CollectorInParking.BASE;
                --tempBaseCollectors;
            }
        }
    }
}
