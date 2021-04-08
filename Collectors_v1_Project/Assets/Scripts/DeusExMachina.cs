using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeusExMachina : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("Fire1"))
        {
            Debug.Log("Fire1 pressed.");
        }
    }
}
