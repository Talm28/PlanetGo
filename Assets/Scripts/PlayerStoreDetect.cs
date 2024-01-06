using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStoreDetect : MonoBehaviour
{
    bool isNearStore;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        isNearStore = false;
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) && isNearStore && !gameManager.isPause && !gameManager.isGameover)
        {
            if(!gameManager.isStoreOpen)
            {
                GameManager.instance.OpenUI("Store");
            }
            else
            {
                GameManager.instance.CloseUI("Store");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Store")
        {
            isNearStore = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Store")
        {
            isNearStore = false;
        }
    }
}
