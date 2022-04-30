using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    internal PlayerAnimationManager PlayerAnimationManager;

    [SerializeField]
    internal PlayerInput playerInput;

    [SerializeField]
    internal PlayerMovement playerMovement;

    [SerializeField]
    internal PlayerSurroundingCheck playerSurroundings;

    [SerializeField]
    internal MoveObjects moveObject;

    [SerializeField]
    internal PlayerState playerState;

    [SerializeField]
    internal SpeedList speedList;


    //private float movementInputDirection;
    internal float currentSpeed;

    //timers

    internal float turnTimer = 0;
    public float turnTimerSet = 0.1f;

    internal float crawlTimer = 0.15f;
    internal float crawlTimerSet = 0.15f;

    internal float standUpTimer = 0;
    internal float standUpTimerSet = 0.15f;

    internal float slidingTimer;
    internal float slidingTimerSet = 0.8f;

    internal float climbLedgeTimer = 0;
    public float climbLedgeTimerSet = 0.5f;

    internal float hangTime = 0.1f;
    internal float hangTimeTimer;

    // internal float jumpBufferLength = .05f;
    // internal float jumpBufferCount;

    internal float slideTransitionTimer;
    internal float slideTransitionTimerSet = 0.5f;

    internal Rigidbody2D rb;
    private Animator anim;
    internal float startingGravity;



    public float walkSpeed = 4f;
    // public float runningSpeed = 6f;
    internal float CrawlingSpeed = 2f;
    internal float SlidingSpeed = 4.5f;
    //private float jumpForce = 6.75f;
    public float fallMultiplier = 1.2f;
    public float riseMultiplier = 1f;


    //internal float wallSlideSpeed = 0.45f;

    internal string currentState;




    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 1;
        //Application.targetFrameRate = 144;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startingGravity = rb.gravityScale;

    }

    // Update is called once per frame
    void Update() //for inputs (keeps running)
    {
        // if (rb.velocity.y < 0)
        // {
        //     rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier -1) * Time.deltaTime;
        // }



        playerMovement.SpeedSet();
        playerMovement.NewMovements();

        playerInput.CheckButtonInput();
        playerSurroundings.CheckSurroundings();
        playerSurroundings.CheckIfCanJump();
        playerSurroundings.CheckIfCanHangLedge();



        // if (playerMovement.isStanding)
        // {
        //     slidingTimer = slidingTimerSet;
        // }

        FallGravity();
        ColliderAdjust();

    }

    private void FixedUpdate() //for physics (after button inputs)?
    {
        playerMovement.SpecialMovement();
        playerMovement.WallInteraction();

        PlayerAnimationManager.AnimationManager();
    }


    [SerializeField]
    internal void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if (currentState == newState) return;

        //play the animation
        anim.Play(newState);

        //reassing the current state
        currentState = newState;
    }

    private void BoxColliderFull()
    {
        GetComponent<BoxCollider2D>().size = new Vector2(0.28f, 0.83f);
        if (playerMovement.isFacingRight)
        {
            GetComponent<BoxCollider2D>().offset = new Vector2(0.015f, -0.069f);
        }
        else
        {
            GetComponent<BoxCollider2D>().offset = new Vector2(-0.015f, -0.069f);
        }
    }

    private void BoxColliderProne()
    {
        GetComponent<BoxCollider2D>().size = new Vector2(0.28f, 0.415f); // full y size / 2

        if (playerMovement.isFacingRight)
        {
            GetComponent<BoxCollider2D>().offset = new Vector2(0.015f, -0.2765f);
        }
        else
        {
            GetComponent<BoxCollider2D>().offset = new Vector2(-0.015f, -0.2765f);
        }
        //GetComponent<BoxCollider2D>().offset = new Vector2(0.015f, -0.2765f); // 0 - (prone y size /2)  + (full y offset /2) 
    }

    private void ColliderAdjust()
    {
        if (playerMovement.isStandingNew && !moveObject.isInteracting)
        {
            BoxColliderFull();
        }
        else if (!playerMovement.isStandingNew && !moveObject.isInteracting)
        {
            BoxColliderProne();
        }
    }

    private void FallGravity()
    {

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * Time.deltaTime * fallMultiplier;
        }
        else if (rb.velocity.y < 5)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * Time.deltaTime * riseMultiplier;
        }
    }
}
