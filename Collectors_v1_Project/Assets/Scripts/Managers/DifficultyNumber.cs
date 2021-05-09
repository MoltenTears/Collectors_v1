using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyNumber : MonoBehaviour
{
    public int difficultyNo = 0;

    // Singleton
    public static DifficultyNumber DNInstance { get; private set; }

    private void Awake()
    {
        // Singleton
        if (DNInstance == null)
        {
            DNInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
