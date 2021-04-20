using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteDrop : MonoBehaviour
{
    [Header("External References")]
    [SerializeField] private GarbagePickup myGarbagePickup;
    [SerializeField] private WasteCenteManager myWasteCentreManager;
    [SerializeField] private GameManager myGameManager;

    [Header("Waste Drop-off Details")]
    [SerializeField] public bool isDroppingWaste;
    [SerializeField] private float dropoffSpeed;

    private void Start()
    {
        myGarbagePickup = GetComponent<GarbagePickup>();
        myWasteCentreManager = FindObjectOfType<WasteCenteManager>();
        myGameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        DropWaste();
    }

    private void DropWaste()
    {
        if (isDroppingWaste & myGarbagePickup.garbageInCollector > 0)
        {
            Debug.Log("Collector dumping waste...");
            myWasteCentreManager.generalWaste += (dropoffSpeed * Time.deltaTime) / myGameManager.garbageDivisor;
            myGarbagePickup.garbageInCollector -= (dropoffSpeed * Time.deltaTime) / myGameManager.garbageDivisor;
        }
        else if (isDroppingWaste & myGarbagePickup.garbageInCollector <= 0)
        {
            isDroppingWaste = false;
        }
    }
}
