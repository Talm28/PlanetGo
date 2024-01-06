using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isPause;
    public bool isGameover;
    public bool isStoreOpen;
    [SerializeField] GameObject storeUI;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] TextMeshProUGUI gameOverScore;
    public StatsScriptableObject playerStats;
    [SerializeField] GameObject player;
    [SerializeField] GameObject tomb;

    // Player variables
    public float PlayerHealth {get; set;}
    public float PlayerMaxHealth {get; set;}
    public float PlayerStrength {get; set;}
    public float PlayerCoins {get; set;}
    public float PlayerScore {get; set;}

    // Prices
    [SerializeField] public int healthPrice;
    [SerializeField] public int srtengthPrice;

    // Game clock
    [SerializeField] float tick;
    [SerializeField] float clock;
    [SerializeField] float globalClock;
    [SerializeField] TextMeshProUGUI clockText;
    public bool isClockTick;
    
    // Monsters coiunters
    [SerializeField] public int maxMonstersNum;
    [SerializeField] public int currentMonsterNum;

    void Awake() 
    {
        // Singleton stuff..
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
    }  

    void Start()
    {
        isPause = false;
        isGameover = false;
        isStoreOpen = false;
        PlayerHealth = playerStats.GetItem("MaxHealth");
        PlayerMaxHealth = playerStats.GetItem("MaxHealth");
        PlayerStrength = playerStats.GetItem("Strength");
        PlayerCoins = 0;

        clock = 0;
        globalClock = 0;
        isClockTick = false;

        currentMonsterNum = 0;

    }

    void Update()
    {
        if(!isGameover)
            ClockUpdate();

        currentMonsterNum = GameObject.FindGameObjectsWithTag("Monster").Length;
    }

    public void ClockUpdate()
    {
        globalClock += Time.deltaTime;
        clock += Time.deltaTime;
        if(clock >= tick)
        {
            clock = 0;
            isClockTick = true;
        }

        clockText.text = ((int)Mathf.Floor(globalClock)).ToString();
    }


    public void OpenUI(string ui)
    {
        GameObject obj;
        switch (ui)
        {
            case "Store":
                obj = storeUI;
                isStoreOpen = true;
                break;
            case "Pause":
                obj = pauseUI;
                isPause = true;
                break;
            case "Game over":
                obj = gameOverUI;
                isGameover = true;
                float score = PlayerStrength + PlayerCoins + PlayerScore + globalClock;
                gameOverScore.text = Mathf.Floor(score).ToString();
                break;
            default:
                obj = null;
                break;
        }
        obj.SetActive(true);
        if(ui != "Game over")
            Time.timeScale = 0;
        
    }

    public void CloseUI(string ui)
    {
        GameObject obj;
        switch (ui)
        {
            case "Store":
                obj = storeUI;
                isStoreOpen = false;
                break;
            case "Pause":
                obj = pauseUI;
                isPause = false;
                break;
            default:
                obj = null;
                break;
        }
        obj.SetActive(false);
        Time.timeScale = 1;
    }

    public void BuyHealth(int amount)
    {
        if(PlayerCoins >= healthPrice)
        {
            PlayerHealth += amount;
            PlayerCoins -= healthPrice;
        }
        
    }

    public void BuyStrength(int amount)
    {
        if(PlayerCoins >= srtengthPrice)
        {
            PlayerStrength += amount;
            PlayerCoins -= srtengthPrice;
        }
    }

    public void SummonTomb(Transform playerTransform)
    {
        StartCoroutine(SummonTombCoroutine(playerTransform));
    }
    IEnumerator SummonTombCoroutine(Transform playerTransform)
    {
        Vector3 tombPosition = playerTransform.position;

        Instantiate(tomb, tombPosition, Quaternion.identity);

        yield return new WaitForSeconds(1f);

        OpenUI("Game over");
    }
}
