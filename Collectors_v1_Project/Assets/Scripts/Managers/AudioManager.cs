using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [SerializeField] private SceneType mySceneType;

    [Header("Audio Objects")]
    [SerializeField] private AudioSource backgroundMusicTitle;
    [SerializeField] private AudioSource backgroundMusicCity1;
    [SerializeField] private AudioSource desaptchCollector;
    [SerializeField] private AudioSource collectorReturnToDepot;
    [SerializeField] private AudioSource trashCans;
    [SerializeField] private AudioSource cheer;
    [SerializeField] private AudioSource menuButtonClick;
    [SerializeField] private AudioSource roadHubClick;

    // Start is called before the first frame update
    void Start()
    {
        mySceneType = FindObjectOfType<SceneType>();

        if (mySceneType.isTitle)
        {
            backgroundMusicTitle.Play();
        }
        else if (mySceneType.isCity1)
        {
            backgroundMusicCity1.Play();
        }
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

    public void GarbageCollectedSFX()
    {
        if (trashCans != null) trashCans.PlayOneShot(trashCans.clip);
    }

    public void CheerSFX()
    {
        if (cheer != null) cheer.PlayOneShot(cheer.clip);
    }

    public void MenuButtonClickSFX()
    {
        if (menuButtonClick != null) menuButtonClick.PlayOneShot(menuButtonClick.clip);
    }

    public void RoadHubClickSFX()
    {
        if (roadHubClick != null) roadHubClick.PlayOneShot(roadHubClick.clip);
    }
}
