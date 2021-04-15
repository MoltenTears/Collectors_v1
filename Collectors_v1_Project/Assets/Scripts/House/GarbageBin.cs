using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageBin : MonoBehaviour
{
    [SerializeField] public GameObject binSmall;
    [SerializeField] public GameObject binMedium;
    [SerializeField] public GameObject binLarge;

    public void ResetBins()
    {
        binSmall.SetActive(false);
        binMedium.SetActive(false);
        binLarge.SetActive(false);
    }
}
