﻿using System.Collections;
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

    [SerializeField]
    internal PlayerMove playerMove;

    [SerializeField]
    internal PlayerJump playerJump;

    [SerializeField]
    internal PlayerTimers playerTimers;

    [SerializeField]
    internal PlayerInteraction playerInteraction;


    internal Rigidbody2D rb;
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
        anim = GetComponent<Animator>();
        startingGravity = rb.gravityScale;

    }

    // Update is called once per frame
    void Update() //for inputs (keeps running)
    {
        speedList.SpeedSet();


        playerInput.CheckButtonInput();
        playerSurroundings.CheckSurroundings();

        playerJump.FallGravity();
        playerState.ColliderAdjust();

        if (playerState.isInteracting)
        {
            playerInteraction.PushPull();
        }

        else
        {
            playerMovement.NewMovements();
        }
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
