using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatingsMeter : MonoBehaviour
{
    [Header("Satisfaction Objects")]
    public Slider satisfactionSlider;

    [Header("Waste Objects)")]
    public Slider wasteSlider;

    [Header("Happiness Ratings")]
    [SerializeField] Sprite sadFace3 = null;
    [SerializeField] Sprite sadFace2 = null;
    [SerializeField] Sprite sadFace1 = null;
    [SerializeField] Sprite neutralFace = null;
    [SerializeField] Sprite happyFace1 = null;
    [SerializeField] Sprite happyFace2 = null;
    [SerializeField] Sprite happyFace3 = null;

    // Start is called before the first frame update
    void Start()
    {
        // reset the slider at the beginning of the level
        satisfactionSlider.value = 1.0f;
        wasteSlider.value = 0.0f;

        // check the Maximum value of the ratings meter
        CheckLevelSliderMinMax();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CheckLevelSliderMinMax()
    {

    }
}
