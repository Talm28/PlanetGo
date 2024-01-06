using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthPriceText;
    [SerializeField] TextMeshProUGUI sretngthPriceText;

    void Awake()
    {
        GameManager gameManager = GameManager.instance;

        healthPriceText.text = gameManager.healthPrice.ToString();
        sretngthPriceText.text = gameManager.srtengthPrice.ToString();
    }
    
}
