using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !gameManager.isPause && !gameManager.isStoreOpen && !gameManager.isGameover)
        {
            gameManager.OpenUI("Pause");
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && gameManager.isPause)
        {
            gameManager.CloseUI("Pause");
        }
    }
}
