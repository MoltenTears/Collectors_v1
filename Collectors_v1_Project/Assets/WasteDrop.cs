using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteDrop : MonoBehaviour
{
    private void OnTriggerStay(Collider collision)
    {
        Debug.Log($"Collision by: {collision.name}");

        //if (collision.GetComponent<GarbagePickup>().isDeliveringWaste == true)
        //{
            // debug
            Debug.Log("Collector in Dropoff Zone and commenced waste dump.");
        //}
    }
}
