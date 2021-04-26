using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CollectorDespatch : MonoBehaviour
{
    [Header("Collector Details")]
    [SerializeField] private TextMeshProUGUI collectorsAtDepot;

    [Header("Collect Button")]
    [SerializeField] private Sprite originalImage;
    [SerializeField] private Button collectButton;
    [SerializeField] private string collectTextInteractable = "SEND COLLECTOR!";
    [SerializeField] private string collectTextNotInteractable = "NO COLLECTORS!";

    [Header("External References")]
    [SerializeField] private DepotManager myDepotManager;

    // Start is called before the first frame update
    void Start()
    {
        myDepotManager = FindObjectOfType<DepotManager>();
        originalImage = collectButton.image.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        collectorsAtDepot.text = myDepotManager.baseCollectors.ToString();

        ShowCollectButton();
    }

    private void ShowCollectButton()
    {
        if (myDepotManager.baseCollectors > 0 && collectButton != null)
        {
            Debug.Log("Player could send out a Collector.");

            // allow button to be used
            collectButton.interactable = true;

            // set the text on the button
            collectButton.GetComponentInChildren<TextMeshProUGUI>().text = collectTextInteractable;
        }
        else
        {
            // "disable" button'
            collectButton.interactable = false;

            // set the text on the button
            collectButton.GetComponentInChildren<TextMeshProUGUI>().text = collectTextNotInteractable;
        }
    }

    public void DespatchCollector()
    {
        //Debug.Log("Collector Button pressed.");

        myDepotManager.DespatchCollector();
    }

    public void ResetCollectorButton()
    {
        // once the active Collector has been despatched, reset the button so another can be sent!
        Debug.Log("Collector Button reset.");
        collectButton.enabled = true;
        
    }
}
