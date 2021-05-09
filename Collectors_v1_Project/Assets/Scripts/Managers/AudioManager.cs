using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [Header("Audio Objects")]
    [SerializeField] private AudioSource backgroundMusicCity1;
    [SerializeField] private AudioSource desaptchCollector;
    [SerializeField] private AudioSource collectorReturnToDepot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DespatchCollectorSFX()
    {
        if (desaptchCollector != null) desaptchCollector.Play();
    }

    public void CollectorReturnSFX()
    {
        if (collectorReturnToDepot != null) collectorReturnToDepot.Play();
    }
}
