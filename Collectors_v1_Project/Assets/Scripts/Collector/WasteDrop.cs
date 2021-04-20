using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteDrop : MonoBehaviour
{
    [Header("External References")]
    [SerializeField] private GarbagePickup myGarbagePickup;
    [SerializeField] private WasteCenteManager myWasteCentreManager;
    [SerializeField] private GameManager myGameManager;
    [SerializeField] private WasteCentreLot myWasteCentreLot;

    [Header("Waste Drop-off Details")]
    [SerializeField] public bool isDroppingWaste;
    [SerializeField] private float dropoffSpeed;

    private void Start()
    {
        myGarbagePickup = GetComponentInParent<GarbagePickup>();
        myWasteCentreManager = FindObjectOfType<WasteCenteManager>();
        myGameManager = FindObjectOfType<GameManager>();
        myWasteCentreLot = FindObjectOfType<WasteCentreLot>();
    }

    private void Update()
    {
        DropWaste();
    }

    private void DropWaste()
    {
        if (isDroppingWaste && myGarbagePickup.garbageInCollector > 0)
        {
            // Debug.Log("Collector dumping waste...");
            myWasteCentreManager.generalWaste += (dropoffSpeed * Time.deltaTime) / myGameManager.garbageDivisor;
            myGarbagePickup.garbageInCollector -= (dropoffSpeed * Time.deltaTime) / myGameManager.garbageDivisor;
        }
        else if (isDroppingWaste && myGarbagePickup.garbageInCollector <= 0)
        {
            // Debug.Log("Finished dumpping waste!");
            // update Collector stats
            isDroppingWaste = false;
            myGarbagePickup.garbageInCollector = 0.0f;
            myGarbagePickup.isDeliveringWaste = false;
            myGarbagePickup.isReturningToDepot = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("WasteCentre"))
        {
            // Debug.Log("Collector left Drop Point, get next Collector...");
            myWasteCentreLot.GetNextCollector();
        }
    }
}
