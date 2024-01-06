using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Animator swordAnimator;
    [SerializeField] float recoilAmount = 5f;
    [SerializeField] StatsScriptableObject playerStats;
    [SerializeField] Slider attackBar;
    [SerializeField] float attackBarSpeed;
    [SerializeField] float attackBarDecrease;
    
    [SerializeField] AudioClip [] swordSound;
    AudioSource audioSource;

    System.Random rnd;

    GameManager gameManager;

    public bool isAttack;
    private float attackTimer;
    private float attackTime;
    
    // Start is called before the first frame update
    void Start()
    {
        isAttack = false;
        gameManager = GameManager.instance;
        audioSource = GetComponent<AudioSource>();

        rnd = new System.Random();

        attackTime = 0.3f;
        attackTimer = 0;

        attackBar.value = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameManager.isPause && !gameManager.isStoreOpen && !gameManager.isGameover)
        {
            Attack();
            AttackBarUpdate();
        }
    }

    void Attack()
    {
        if(Input.GetButton("Attack")) // Attack button pres
        {
            if(attackBar.value >= attackBarDecrease) // Bar value check
            {
                if(swordAnimator.GetCurrentAnimatorStateInfo(0).IsName("Sword idle")) // Check if not in attack animation
                {
                    attackTimer = attackTime;
                    swordAnimator.SetBool("IsAttack", true); // Sword animation
                    attackBar.value -= attackBarDecrease; // Attack bar decrease value on attack
                    PlaySordSound();
                }
                else if(attackTimer <= 0) // If attack when timer is over (continus press)
                {
                    attackTimer = attackTime;
                    attackBar.value -= attackBarDecrease;
                    PlaySordSound();
                }
            }
            
        }

        if(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            isAttack = true;
        }
        else
        {
            attackTimer = 0;
            isAttack = false;
            swordAnimator.SetBool("IsAttack", false);
        }
    }

    // Attack bar update
    void AttackBarUpdate()
    {
        if(attackBar.value < 1f)
        {
            attackBar.value += Time.deltaTime * attackBarSpeed;
        }
    }

    // Monster attack
    void OnTriggerStay2D(Collider2D other) {
        if(isAttack && other.tag == "Monster")
        {
            BasicMonsterBehavior monsterBasicScript =  other.GetComponent<BasicMonsterBehavior>();
            if(!monsterBasicScript.isHit)
            {
                monsterBasicScript.MonsterHit(); // Start monster hit clock

                Rigidbody2D monsterRigidBody = other.GetComponent<Rigidbody2D>();

                // Get the recoil vec and normalize it
                Vector2 recoilVec = transform.position - other.gameObject.transform.position;
                recoilVec = recoilVec/recoilVec.magnitude;

                monsterBasicScript.moveFactor = 0; // Make the monster stop
                monsterBasicScript.clock = 0; // Reset monster move clock
                monsterBasicScript.PlaySound("Hit");
                monsterRigidBody.velocity = Vector2.zero;
                monsterRigidBody.AddForce(-recoilVec * recoilAmount);
                other.GetComponent<Animator>().SetTrigger("IsHit");

                HealthSystem healthBar = other.GetComponent<HealthSystem>();
                if(healthBar != null) healthBar.TakeHealth(gameManager.PlayerStrength);
            }
        }
    }

    // Player random attack sound
    public void PlaySordSound()
    {
        int soundIndex = rnd.Next(0,3);
        audioSource.clip = swordSound[soundIndex];
        audioSource.Play();
    }
}
