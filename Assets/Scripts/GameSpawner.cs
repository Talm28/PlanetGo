using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum monsterNames
{
    Mushroom,
    Chicken,
    Trunk,
    Redish,
    Rino,
    Chameleon,
    Snail
}

[System.Serializable]
public struct Monster
{
    public monsterNames name;
    public GameObject obj;
}

public class GameSpawner : MonoBehaviour
{
    public Monster[] monsters;
    GameManager gameManager;

    System.Random rnd;

    // Monster index levels
    List<int> level_1;
    List<int> level_2;
    List<int> level_3;
    List<int> currentLevel;
    [SerializeField] private int strengthToLevel2;
    [SerializeField] private int strengthToLevel3;

    // Monster max amount
    [SerializeField] private int numOfTicks;
    [SerializeField] private int currentNumOfTick;
 
    List<Dictionary<monsterNames,int>> levels = new List<Dictionary<monsterNames, int>>();

    // Boolien for first two mushroom spawn
    bool hasStart;

    void Start()
    {
        gameManager = GameManager.instance;

        rnd = new System.Random();

        level_1 = new List<int>{0,2,5};
        level_2 = new List<int>{0,1,2,3,5};
        level_3 = new List<int>{0,1,2,3,4,5,6};
        currentLevel = level_1;

        currentNumOfTick = 0;

        hasStart = false;
    }

    void Update() 
    {   
        // First spawn two mushroom!
        if(!hasStart)
        {
            hasStart = true;
            SpawnMonster(0); // 3 mushrooms
            SpawnMonster(0);
            SpawnMonster(0);
            SpawnMonster(2); // 2 snails
            SpawnMonster(2);
        }
        
        UpdateClock();
        updateMonsterMaxNum();
        LevelUpdate();
    }

    private void LevelUpdate()
    {
        float strength = gameManager.PlayerStrength;
        if(strength >= strengthToLevel2)
            currentLevel = level_2;
        if(strength >= strengthToLevel3)
            currentLevel = level_3;
    }

    private void updateMonsterMaxNum()
    {
        if(currentNumOfTick >= numOfTicks)
        {
            gameManager.maxMonstersNum += 1;
            currentNumOfTick = 0;
        }
    }

    private void UpdateClock()
    {
        if(gameManager.isClockTick)
        {
            gameManager.isClockTick = false;
            ClockTick();
        }
    }

    private void ClockTick()
    {
        currentNumOfTick += 1;
        if(gameManager.currentMonsterNum < gameManager.maxMonstersNum)
        {
            int monsterIndex = rnd.Next(currentLevel.Count);
            SpawnMonster(currentLevel[monsterIndex]);
        }
        
    }

    public void SpawnMonster(int monsterIndex)
    {
        Monster monster = monsters[monsterIndex];
        Instantiate(monster.obj, Vector3.zero, Quaternion.identity);
    }

}
