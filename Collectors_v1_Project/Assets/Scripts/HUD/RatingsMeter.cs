using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatingsMeter : MonoBehaviour
{
    [Header("Sliders")]
    public Slider satisfactionSlider;
    public Slider wasteSlider;

    [Header("External References")]
    [SerializeField] private GameManager myGameManager;

    // Start is called before the first frame update
    void Start()
    {
        // get external references
        myGameManager = FindObjectOfType<GameManager>();


        // reset the slider at the beginning of the level
        satisfactionSlider.value = 1.0f; // full
        wasteSlider.value = 0.0f; // empty

        
    }

    // Update is called once per frame
    void Update()
    {
        SetSliders();
    }

    private void SetSliders()
    {
        // waste sliders
        wasteSlider.maxValue = myGameManager.maxCityGarbageLevel;
        wasteSlider.value = myGameManager.cityGarbageLevel;
    }
}
