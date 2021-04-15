using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("House Materials")]
    [SerializeField] public Material houseSingleMaterial;
    [SerializeField] public Material houseFamilyMaterial;
    [SerializeField] public Material houseShareMaterial;

    [Header("Household Garbage Details")]
    [SerializeField] public int garbageDivisor;
    [SerializeField] public float garbageSpeedSingle;
    [SerializeField] public float garbageSpeedFamily;
    [SerializeField] public float singleGarbageShare;
}
