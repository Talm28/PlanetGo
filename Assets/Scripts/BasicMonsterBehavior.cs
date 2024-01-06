using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class BasicMonsterBehavior : MonoBehaviour
{
    [SerializeField] StatsScriptableObject monsterStats;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip killSound;
    [SerializeField] int StrongProb;

    Rigidbody2D monsterBody;
    Animator monsterAnimator;
    public int moveFactor = 0;
    float leftSpawnAngel, rightSpawnAngle;
    float minSize, maxSize;
    float moveSpeed, maxSpeed;
    bool isTriggered;
    float monsterAngle;
    float triggeredDistance;

    public float clock;
    float tick;
    float maxTick, minTick;

    System.Random rnd;

    GameObject player;
    float playerAngle;

    public bool isStrong;
    public bool isHit;

    void Awake() 
    {
        rnd = new System.Random();
        
        leftSpawnAngel = monsterStats.GetItem("LeftSpawnAngle") * Mathf.Deg2Rad;
        rightSpawnAngle = monsterStats.GetItem("RightSpawnAngle") * Mathf.Deg2Rad;

        player = GameObject.FindGameObjectWithTag("Player");
        playerAngle = player.GetComponent<PlayerMovement>().playerAngle;

        isStrong = false;
        isHit = false;
    }

    void Start()
    {
        SpawnMonster();

        monsterBody = GetComponent<Rigidbody2D>();
        monsterAnimator = GetComponent<Animator>();

        minSize = monsterStats.GetItem("MinSize");
        maxSize = monsterStats.GetItem("MaxSize");
        maxTick = monsterStats.GetItem("MaxMovementTick");
        minTick = monsterStats.GetItem("MinMovementTick");
        moveSpeed = monsterStats.GetItem("Speed");
        maxSpeed = moveSpeed;
        isTriggered = false;
        triggeredDistance = monsterStats.GetItem("TriggeredDistance");

        tick = getRandomFloat(maxTick, minTick);

        // Initialize random size
        float size = getRandomFloat(maxSize,minSize);
        transform.localScale = new Vector3(size * transform.localScale.x,size,transform.localScale.z);

        monsterAngle = Mathf.Atan2(transform.position.y, transform.position.x);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0)
        {
            monsterAngle = Mathf.Atan2(transform.position.y, transform.position.x);
            if(player != null)
                playerAngle = player.GetComponent<PlayerMovement>().playerAngle;
            else
                playerAngle = Mathf.Infinity;
        
            RandomClock();

            // Check if angles between player and monster is close
            isTriggered = Mathf.Abs(monsterAngle - playerAngle) < triggeredDistance * Mathf.Deg2Rad;
            MonsterMovement();

            monsterAnimation();
            AngleRestrict();
        }
    }

    void monsterAnimation()
    {
        switch(moveFactor)
        {
            case 0:
                monsterAnimator.SetBool("IsRunning",false);
                break;
            case 1:
                monsterAnimator.SetBool("IsRunning",true);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
                break;
            case -1:
                monsterAnimator.SetBool("IsRunning",true);
                transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
                break;
        }
    }

    void MonsterMovement()
    {
        Vector2 moveDir = Vector2.Perpendicular(transform.position);
        float speed = Vector3.Project(monsterBody.velocity,moveDir).magnitude;

        if (isTriggered)
        {
            moveFactor = (int)Mathf.Sign(playerAngle - monsterAngle);
        }

        if(speed <= maxSpeed)
        {
            monsterBody.AddForce(moveDir * moveSpeed * moveFactor);
        }
    }

    // Prevent monster get out of his angle region
    void AngleRestrict()
    {
        float currAngle = HalfToFull(monsterAngle);
        
        // Vector representation of anlge borders
        Vector2 leftVec = new Vector2(Mathf.Cos(leftSpawnAngel),Mathf.Sin(leftSpawnAngel));
        Vector2 rightVec = new Vector2(Mathf.Cos(rightSpawnAngle),Mathf.Sin(rightSpawnAngle));
        Vector2 anlgeVec = new Vector2(Mathf.Cos(currAngle),Mathf.Sin(currAngle));
        if(!IsAngleInRegion(currAngle)) // If not in region
        {

            if(Vector2.Angle(anlgeVec,leftVec) < Vector2.Angle(anlgeVec,rightVec))
                moveFactor = -1;
            else
                moveFactor = 1;
        }

        //For debugging
        Debug.DrawRay(Vector2.zero, leftVec * 100, Color.blue);
        Debug.DrawRay(Vector2.zero, rightVec * 100, Color.blue);
        //Debug.DrawRay(Vector2.zero, anlgeVec * 100, Color.blue);
        //Debug.Log("Current angle: " + angle * Mathf.Rad2Deg + ". Is in region: " + IsAngleInRegion(angle));
    }

    void RandomClock()
    {
        clock += Time.deltaTime;
        if(clock >= tick)
        {
            clock = 0;
            tick = getRandomFloat(maxTick, minTick);

            moveFactor = getRandomInt();
        }
    }

    // Get random float between min and max tick
    float getRandomFloat(float max, float min)
    {
        return (float)(rnd.NextDouble() * (max - min) + min);
    }

    // Get random int in range [-1,1]
    int getRandomInt()
    {
        return rnd.Next(-1,2);
    }

    // Spawn monster between defined angle
    void SpawnMonster()
    {
        float radius = 30f;
        float angle = playerAngle;

        while(Mathf.Abs(playerAngle - angle) < 10 * Mathf.Deg2Rad) // Check if player and monster 
        {
            if(leftSpawnAngel > rightSpawnAngle) 
                angle = FullToHalf(getRandomFloat(rightSpawnAngle, leftSpawnAngel));
            else
                angle = getRandomFloat(FullToHalf(leftSpawnAngel), FullToHalf(rightSpawnAngle));
        }
        
        Vector2 newPos = new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
        transform.position = newPos;

        // Strong monster spawn
        if(GameManager.instance.PlayerStrength >= 0.75f * monsterStats.GetItem("MaxHealth"))
        {
            int num = rnd.Next(0,StrongProb + 1);
            if(num == 0)
                SpawnStrongMonster();
        }
    }
    
    // Strong monster spawn
    public void SpawnStrongMonster()
    {
        isStrong = true;
        GetComponent<HealthSystem>().StrongLife();
        GetComponent<SpriteRenderer>().color = (Color)(new Color32(255, 127, 127, 255));
    }

    // Full [0,2pi] to half [-pi,pi] angle convert
    float HalfToFull(float angle)
    {
        if(angle <0)
            return angle + 2 * Mathf.PI;
        return angle;
    }
    // Half to full angle convert
    float FullToHalf(float angle)
    {
        if(angle > Mathf.PI)
            return angle - 2 * Mathf.PI;
        return angle;
    }

    bool IsAngleInRegion(float angle)
    {
        if(rightSpawnAngle >= leftSpawnAngel) // Special case
        {
            if(angle >= leftSpawnAngel && angle <= rightSpawnAngle)
                return false;
        } 
        else // Regular case
        {
            if(angle >= leftSpawnAngel || angle <= rightSpawnAngle)
                return false;
        }
        return true;
    }

    public float GetSrength()
    {
        float strength = monsterStats.GetItem("Strength");
        if(isStrong)
            return strength * 2;
        return strength;
    }

    public void PlaySound(string soundName)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if(soundName == "Hit")
        {
            audioSource.clip = hitSound;
            audioSource.Play();
        }
        if(soundName == "Kill")
        {
            audioSource.clip = killSound;
            audioSource.Play();
        }
    }

    // Call when monster is hit
    public void MonsterHit()
    {
        isHit = true;
        StartCoroutine(HitCoolDown());
    }
    IEnumerator HitCoolDown()
    {
        yield return new WaitForSeconds(0.2f);
        isHit = false;
    }
}
