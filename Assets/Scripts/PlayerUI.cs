using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField]
    TextMeshProUGUI healthText;
    [SerializeField]
    TextMeshProUGUI strengthText;
    [SerializeField]
    TextMeshProUGUI coinsText;
    [SerializeField]
    TextMeshProUGUI scoreText;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        healthText.text = gameManager.PlayerHealth.ToString();;
        strengthText.text = gameManager.PlayerStrength.ToString();
        coinsText.text = gameManager.PlayerCoins.ToString();
        scoreText.text = gameManager.PlayerScore.ToString();
    }
}
