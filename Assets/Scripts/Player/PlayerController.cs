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
    internal PlayerState playerState;

    [SerializeField]
    internal SpeedList speedList;

    [SerializeField]
    internal PlayerMove playerMove;

    [SerializeField]
    internal PlayerJump playerJump;

    [SerializeField]
    internal PlayerTimers playerTimers;

    [SerializeField]
    internal PlayerDetectObject playerDetectObject;

    [SerializeField]
    internal PlayerInteraction playerInteraction;

    [SerializeField]
    internal PlayerStuckInGround playerStuckInGround;

    [SerializeField]
    internal PlayerColliders playerColliders;

    [SerializeField]
    internal TakeDamage takeDamage;


    internal Rigidbody2D rb;
    internal BoxCollider2D bc;
    internal SpriteRenderer spriteRenderer;
    private Animator anim;
    internal float startingGravity;

    public float fallMultiplier = 1.2f;
    public float riseMultiplier = 1f;

    internal string currentState;

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 1;
        //Application.targetFrameRate = 144;
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();

        anim = GetComponent<Animator>();
        startingGravity = rb.gravityScale;

    }

    internal IEnumerator Knockback()
    {
        yield return new WaitForSeconds(0.4f);

        if (playerSurroundings.isGrounded)
        {
            playerState.controlMode = PlayerState.ControlMode.FreeMovement;
        }
    }

    // Update is called once per frame
    void Update() //for inputs (keeps running)
    {
        playerStuckInGround.AirTime();
        // speedList.SpeedSet();
        speedList.SpeedAdjust();



        switch (playerState.controlMode)
        {
            case PlayerState.ControlMode.FreeMovement:
                playerMovement.NewMovements();

                break;

            case PlayerState.ControlMode.Damaged:

                StartCoroutine(Knockback());

                //playerTimers.knockBackTimer -= Time.deltaTime;
                //
                //if (playerSurroundings.isGrounded && playerTimers.knockBackTimer <0)
                //{
                //    playerTimers.knockBackTimer = playerTimers.knockBackTimerSet;
                //    playerState.controlMode = PlayerState.ControlMode.FreeMovement; // finesse
                //}


                break;

            case PlayerState.ControlMode.Interaction:

                playerInteraction.Interactions();

                break;

            default:
                playerState.controlMode = PlayerState.ControlMode.FreeMovement;
                break;

        }




        //  if (playerState.isInteracting)
        //  {
        //      playerInteraction.Interactions();
        //  }
        //
        //  else if (playerState.isStuckInGround)
        //  {
        //      playerStuckInGround.WiggleWiggle();
        //  }
        //
        //  else
        //  {
        //      playerMovement.NewMovements();
        //  }
        //
        //

        //
        //
        //
        //  // playerState.ColliderAdjust();
        playerInput.CheckButtonInput();

    }



    private void FixedUpdate() //for physics (after button inputs)?
    {
        playerState.Invincibility();


        playerJump.FallGravity();
        playerMove.Move();
        playerSurroundings.CheckSurroundings();
        playerInteraction.LetGo();

        playerDetectObject.CheckForObjects();

        // playerMovement.SpecialMovement();
        // playerMovement.WallInteraction();
        playerColliders.ColliderAdjust();


        PlayerAnimationManager.AnimationManagerStateTest();
        //PlayerAnimationManager.AnimationManager();

        //playerState.GetHit();
        switch (playerState.controlMode)
        {
            case PlayerState.ControlMode.FreeMovement:

                playerMovement.WallInteraction();
                break;
        }

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

    // private void FallGravity()
    // {
    //     if (rb.velocity.y < 0)
    //     {
    //         rb.velocity += Vector2.up * Physics2D.gravity.y * Time.deltaTime * fallMultiplier;
    //     }
    //     else if (rb.velocity.y < 5)
    //     {
    //         rb.velocity += Vector2.up * Physics2D.gravity.y * Time.deltaTime * riseMultiplier;
    //     }
    // }


}
