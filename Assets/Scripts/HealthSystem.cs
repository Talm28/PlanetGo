using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] StatsScriptableObject stats;
    [SerializeField] Slider healthBar;

    public float health;
    bool isDead;

    void Awake()
    {
        health = stats.GetItem("MaxHealth");
        healthBar.maxValue = health;
        healthBar.minValue = 0;
        healthBar.value = health;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health;
        if (health <= 0)
        {
            StartCoroutine(DeathCoroutine());
        }
    }

    IEnumerator DeathCoroutine ()
    {
        BasicMonsterBehavior basicMonsterScript = GetComponent<BasicMonsterBehavior>();
        if(!isDead)
        {
            basicMonsterScript.PlaySound("Kill");
        }
        isDead = true;
        yield return new WaitForSeconds(0.5f);

        GameManager gameManager = GameManager.instance;
        float score = stats.GetItem("CoinsValue");
        if(basicMonsterScript.isStrong) // Double score if monster is strong
            score *= 2;
        gameManager.PlayerCoins += score; // Coins update
        gameManager.PlayerScore += stats.GetItem("ScoreValue"); // Score update
        
        Destroy(this.gameObject);
    }

    public void TakeHealth(float amount)
    {
        health -= amount;
    }

    public void GiveHealth(float amount)
    {
        health += amount;
    }

    public float GetHealth()
    {
        return health;
    }

    public void StrongLife()
    {
        health *= 10;
        healthBar.maxValue = health;
        healthBar.minValue = 0;
        healthBar.value = health;
    }
}
