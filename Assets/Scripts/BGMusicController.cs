using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicController : MonoBehaviour
{
    public static BGMusicController instance;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        // Singleton stuff..
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
