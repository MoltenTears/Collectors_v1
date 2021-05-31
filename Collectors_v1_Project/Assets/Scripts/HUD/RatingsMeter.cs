using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatingsMeter : MonoBehaviour
{
    [Header("Sliders")]
    public Slider satisfactionSlider;
    public Slider wasteSlider;

    [Header("Slider Variants")]
    [SerializeField] private Sprite normalFillSpirte;
    [SerializeField] private Sprite normalButtonSpirte;
    [SerializeField] private Sprite winningFillSpirte;
    [SerializeField] private Sprite winningButtonSpirte;

    [Header("Slider Components")]
    [SerializeField] private Image satisfactionFillImage;
    [SerializeField] private Image satisfactionHandleImage;


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
        // waste slider value
        wasteSlider.maxValue = myGameManager.maxCityGarbageLevel;
        wasteSlider.value = myGameManager.cityGarbageLevel;

        // satisfaction slider value
        satisfactionSlider.maxValue = myGameManager.houseSatisfaction.Count;
        satisfactionSlider.value = myGameManager.citySatisfactionLevel;

        // satisfaction slider colour
        if (myGameManager.citySatisfactionLevel/myGameManager.maxCitySatisfactionLevel >= myGameManager.satisfactionToWin/100)
        {
            //Debug.Log("Currently Winning!");
            satisfactionFillImage.sprite = winningFillSpirte;
            satisfactionHandleImage.sprite = winningButtonSpirte;
        }
        else
        {
            //Debug.Log("Currently Losing!");
            satisfactionFillImage.sprite = normalFillSpirte;
            satisfactionHandleImage.sprite = normalButtonSpirte;
        }
    }
}
