using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialMonsterUIController : MonoBehaviour
{
    [SerializeField] StatsScriptableObject[] monsterStats;
    [SerializeField] TextMeshProUGUI[] monsterHealth;
    [SerializeField] TextMeshProUGUI[] monsterStrength;
    [SerializeField] TextMeshProUGUI[] monsterCoins;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0 ; i < monsterStats.Length ; i++ )
        {
            monsterHealth[i].text = monsterStats[i].GetItem("MaxHealth").ToString();
            monsterStrength[i].text = monsterStats[i].GetItem("Strength").ToString();
            monsterCoins[i].text = monsterStats[i].GetItem("CoinsValue").ToString();
        }
    }
}
