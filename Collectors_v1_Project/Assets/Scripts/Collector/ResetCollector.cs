using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCollector : MonoBehaviour
{
    [SerializeField] private CollectorMovement myCollectorMovement;
    [SerializeField] private GarbagePickup myGarbagePickup;

    [SerializeField] private float minSpeed;
    [SerializeField] private float waitTimeSecondsMax;
    [SerializeField] private float waitTimeSecondsRemaining;
    [SerializeField] public bool isResetting;


    [SerializeField] private float navMeshAgentVelocity;

    // Start is called before the first frame update
    void Start()
    {
        myCollectorMovement = GetComponent<CollectorMovement>();
        myGarbagePickup = GetComponentInChildren<GarbagePickup>();
        waitTimeSecondsRemaining = waitTimeSecondsMax;
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (myCollectorMovement != null)
        {
            navMeshAgentVelocity = myCollectorMovement.myAgent.velocity.sqrMagnitude;

            if (!myCollectorMovement.newDespatch && !myGarbagePickup.isDeliveringWaste && !isResetting)
            {
                // if there is still time on the clock
                if (waitTimeSecondsRemaining > 0)
                {
                    if (navMeshAgentVelocity < minSpeed)
                    {
                        waitTimeSecondsRemaining -= Time.deltaTime; // reduce the counter
                    }
                    else
                    {
                        waitTimeSecondsRemaining = waitTimeSecondsMax; // reset the counter
                    }
                }
                else if (waitTimeSecondsRemaining <= 0 && !myCollectorMovement.newDespatch && !myGarbagePickup.isDeliveringWaste)
                {
                    isResetting = true;
                    // Debug.LogWarning($"{gameObject.name} has been idle for {waitTimeSecondsMax}.");
                    myGarbagePickup.FinaliseCollection(); // trigger the reset
                }
            }
            else
            {
                waitTimeSecondsRemaining = waitTimeSecondsMax;
            }
        }
    }
}
