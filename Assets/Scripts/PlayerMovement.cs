using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float maxMoveSpeed = 5f;
    [SerializeField] int jumpNumber = 2;
    [SerializeField] float recoil = 5f;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip hitSound;

    Rigidbody2D playyerBody;
    Animator playerAnimator;
    Collider2D playerCollider;
    AudioSource audioSource;

    int jumpsRemains;

    public float playerAngle;

    // Start is called before the first frame update
    void Start()
    {
        playyerBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

        jumpsRemains = jumpNumber;

        playerAngle = Mathf.Atan2(transform.position.y, transform.position.x);

        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameManager.isPause && !gameManager.isStoreOpen && !gameManager.isGameover)
        {
            Jumping();
            Movement();
        }
    }

    void Movement()
    {
        // The vector direction for player move
        Vector2 moveDir = Vector2.Perpendicular(transform.position);
        moveDir = moveDir/moveDir.magnitude;

        // Calculate velocity projection on move direction
        Vector2 speed = Vector3.Project(playyerBody.velocity,moveDir);

        //For debugging
        Debug.DrawRay(transform.position, speed, Color.green);

        // Left and right movement
        if(Input.GetKey(KeyCode.LeftArrow) && Mathf.Abs(speed.magnitude) < maxMoveSpeed)
        {
            playyerBody.AddForce(moveDir * moveSpeed);
            transform.localScale = new Vector3(-1,1,1); // Set sprite side
        }
        else if(Input.GetKey(KeyCode.RightArrow) && Mathf.Abs(speed.magnitude) < maxMoveSpeed)
        {
            playyerBody.AddForce(-moveDir * moveSpeed);
            transform.localScale = new Vector3(1,1,1); // Set sprite side
        }

        // Run animation
        if(speed.magnitude > 0.2f)
            playerAnimator.SetBool("IsRunning", true);
        else
            playerAnimator.SetBool("IsRunning", false);
        
        // Update player angle
        playerAngle = Mathf.Atan2(transform.position.y, transform.position.x);
    }   

    void Jumping()
    {
        if(Input.GetKeyDown(KeyCode.Space) && jumpsRemains > 1)
        {
            jumpsRemains -=1;

            audioSource.clip = jumpSound;
            audioSource.Play();

            // Fix jump amont with current jump speed of the player - always jump with the same force!
            Vector2 jumpDir = transform.position/transform.position.magnitude;
            float currJumpSpeed = Vector3.Project(playyerBody.velocity,jumpDir).magnitude;
            float jumpSpeedAngle = Vector2.Angle(jumpDir,playyerBody.velocity);
            float jumpAddForce;
            if(jumpSpeedAngle == 0)
                jumpAddForce = -currJumpSpeed;
            else
                jumpAddForce = currJumpSpeed;

            playyerBody.velocity += jumpDir * (jumpSpeed + jumpAddForce);
        }

        // Reset remaining jump counter on planet touch
        if(playerCollider.IsTouchingLayers(LayerMask.GetMask("Planet")))
            jumpsRemains = jumpNumber;

        // Fix for jumping to high
        if(playyerBody.velocity.y > 10)
        {
            Vector2 newVelocity = new Vector2(playyerBody.velocity.x,10);
            playyerBody.velocity = newVelocity;
        }
        
    }

    // Player hit
    void OnCollisionEnter2D(Collision2D other) {
        
        //Monster collide
        if(other.gameObject.tag == "Monster")
        {
            Vector2 monsterPos = other.gameObject.transform.position;
            Vector2 playerPos = transform.position;
            Vector2 recoilDir = monsterPos - playerPos; // Direction between player and monster
            recoilDir = recoilDir/recoilDir.magnitude; // Normelize vector

            audioSource.clip = hitSound;
            audioSource.Play();

            playyerBody.AddForce(-recoilDir * recoil);

            playerAnimator.SetTrigger("IsDamage");

            gameManager.PlayerHealth -= other.gameObject.GetComponent<BasicMonsterBehavior>().GetSrength();

            if(gameManager.PlayerHealth <= 0)
            {
                gameManager.PlayerHealth = 0;
                StartCoroutine(PlayerDead());
            }
        }

    }

    IEnumerator PlayerDead()
    {
        GameManager.instance.isGameover = true;
        playerAnimator.SetTrigger("IsDead");

        yield return new WaitForSeconds(0.5f);

        GameManager.instance.SummonTomb(transform);

        Destroy(this.gameObject);
    }
}
