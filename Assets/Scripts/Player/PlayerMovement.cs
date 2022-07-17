using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    PlayerController playerController;





    //internal bool isFacingRight = true;

    internal bool isStanding; //sets the hit box
                              //internal bool isProne;
                              //internal bool isIdle;
                              // internal bool isWalking;
                              //internal bool isSprinting;
                              // internal bool isSliding;
                              // internal bool isProneIdle;
                              // internal bool isCrawling;
                              // internal bool isTurning;

    internal bool isPushing;

    // internal bool isHangingLedge;
    internal bool isClimbingLedge;
    // internal bool isWallSliding;

    internal bool leftPriority;


    internal bool canTurn = true;
    internal bool canMove = true;

    internal bool canJump;



    internal Vector2 hangingPosition;

    internal float velX = 0;
    internal float velY = 0;

    internal bool wallJumpReady = false;

    public float wallSlideSense;


    internal void NewJump()
    {
        if (playerController.playerInput.isJumpTapped)
        {
            playerController.playerJump.jumpBufferCount = playerController.playerJump.jumpBufferLength;

            playerController.playerJump.Jump();
        }

        else if (playerController.playerJump.jumpBufferCount > 0)
        {
            if (playerController.playerInput.isJumpPressed && playerController.playerSurroundings.isGrounded)
            {
                playerController.playerJump.Jump();
            }
            else if (playerController.playerJump.jumpBufferCount >= 0)
            {
                playerController.playerJump.jumpBufferCount -= Time.deltaTime;
            }
        }

        if (playerController.rb.velocity.y > 0
      && playerController.playerInput.isJumpReleased) // if space is tapped, jump is smaller
        {
            playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, playerController.rb.velocity.y * 0.65f);
        }
    }

    internal void Movement()
    {

        if (playerController.playerSurroundings.isTouchingWall
        || (playerController.playerSurroundings.isTouchingLedge && isStanding))
        {
            IdleStop();
        }

        NewJump();



        //if (isGrounded) and else for wall and air movement

        if (playerController.playerSurroundings.isGrounded)
        {
            EnableMovement();

            switch (playerController.playerState.currentState)
            {



                //-----------------------------------------------------------------I D L E-------------------------------------------
                case PlayerState.CharacterMovement.Idle:

                    Debug.Log("I'm Idle");


                    if (playerController.playerState.isMoving)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Walking;
                    }

                    if (playerController.playerInput.isDownPressed)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Prone;
                    }

                    isStanding = true;
                    playerController.playerState.canJump = true;

                    canTurn = true;
                    IdleStop();
                    break;
                //-----------------------------------------------------------------W A L K I N G-------------------------------------------
                case PlayerState.CharacterMovement.Walking:
                    Debug.Log("I'm walking");



                    if (!playerController.playerState.isMoving)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Idle;
                    }

                    if (playerController.playerInput.isDownPressed)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Crawling;
                    }

                    if (playerController.playerInput.isSprintPressed
                    && ((playerController.playerState.isFacingRight && playerController.speedList.currentSpeed >= playerController.speedList.walkSpeed)
                    || (!playerController.playerState.isFacingRight && playerController.speedList.currentSpeed <= playerController.speedList.walkSpeed)))
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Sprinting;
                    }

                    isStanding = true;
                    playerController.playerState.canJump = true;


                    break;
                //-----------------------------------------------------------------S P R I N T I N G-------------------------------------------
                case PlayerState.CharacterMovement.Sprinting:
                    Debug.Log("I'm Sprinting");


                    if (playerController.playerInput.isDownPressed
                    && playerController.speedList.currentSpeed == playerController.speedList.runningSpeed)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Sliding;
                    }

                    if (!playerController.playerInput.isSprintPressed
                    || playerController.playerState.isFacingRight && playerController.rb.velocity.x < 0
                    || !playerController.playerState.isFacingRight && playerController.rb.velocity.x > 0)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Walking;
                    }

                    if (!playerController.playerState.isMoving)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Idle;
                    }


                    playerController.playerTimers.slidingTimer = playerController.playerTimers.slidingTimerSet;
                    isStanding = true;
                    playerController.playerState.canJump = true;


                    break;
                //-----------------------------------------------------------------S L I D I N G-------------------------------------------
                case PlayerState.CharacterMovement.Sliding:
                    Debug.Log("I'm Sliding");


                    if (!playerController.playerInput.isDownPressed && !playerController.playerSurroundings.isTouchingCeilingProne)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Sprinting;
                    }

                    if (!playerController.playerInput.isSprintPressed
                    || playerController.playerTimers.slidingTimer < 0.05
                    || (playerController.playerState.isFacingRight && playerController.playerInput.isLeftPressed)
                    || (!playerController.playerState.isFacingRight && playerController.playerInput.isRightPressed))
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Crawling;
                    }

                    if (!playerController.playerState.isMoving)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Prone;
                    }

                    isStanding = false;
                    playerController.playerState.canJump = true;
                    Slide();

                    break;
                //-----------------------------------------------------------------P R O N E-------------------------------------------
                case PlayerState.CharacterMovement.Prone:
                    Debug.Log("I'm Prone");


                    if (playerController.playerState.isMoving)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Crawling;
                    }

                    if (!playerController.playerInput.isDownPressed && !playerController.playerSurroundings.isTouchingCeilingProne)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Idle;
                    }

                    isStanding = false;
                    playerController.playerState.canJump = false;
                    IdleStop();

                    break;
                //-----------------------------------------------------------------C R A W L I N G-------------------------------------------
                case PlayerState.CharacterMovement.Crawling:
                    Debug.Log("I'm Crawling");


                    if (!playerController.playerState.isMoving)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Prone;
                    }

                    if (!playerController.playerInput.isDownPressed && !playerController.playerSurroundings.isTouchingCeilingProne)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Walking;
                    }

                    isStanding = false;
                    playerController.playerState.canJump = false;


                    break;


                //-----------------------------------------------------------------D E F A U L T  O N  G R O U N D-------------------------------------------
                default:

                    if (isClimbingLedge)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Climbing;
                    }

                    if (!playerController.playerInput.isLeftPressed
                    || !playerController.playerInput.isRightPressed)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Idle;
                    }

                    if (playerController.playerInput.isDownPressed
                    && playerController.speedList.currentSpeed == playerController.speedList.runningSpeed)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Sliding;
                    }

                    if (playerController.playerInput.isSprintPressed
                    && playerController.playerState.isMoving
                    && playerController.speedList.currentSpeed >= playerController.speedList.runningSpeed)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Sprinting;
                    }

                    if (playerController.playerState.isMoving
                    && playerController.speedList.currentSpeed == playerController.speedList.walkSpeed)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Walking;
                    }

                    //stuck

                    Debug.Log("Unassigned State on ground");

                    break;

                //-----------------------------------------------------------------C L I M B I N G-------------------------------------------
                case PlayerState.CharacterMovement.Climbing:

                    FreezePlayerLocation(velX, velY);
                    playerController.playerTimers.climbLedgeTimer += Time.deltaTime;

                    if (playerController.playerTimers.climbLedgeTimer < playerController.playerTimers.climbLedgeTimerSet)
                    {

                    }

                    else
                    {
                        EnableMovement();
                        isClimbingLedge = false;
                        playerController.playerTimers.climbLedgeTimer = 0;
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Idle;
                    }

                    playerController.playerState.canJump = false;
                    break;
            }
        }

        else
        {
            switch (playerController.playerState.currentState)
            {
                //-----------------------------------------------------------------J U M P I N G-------------------------------------------
                case PlayerState.CharacterMovement.Jumping:
                    Debug.Log("I'm Jumping");

                    if (playerController.rb.velocity.y < 0)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Falling;
                    }

                    if (playerController.rb.velocity.y < 1
                    && playerController.playerSurroundings.isTouchingWall)
                    {
                        if (!playerController.playerSurroundings.isTouchingLedge)
                        {
                            playerController.playerState.currentState = PlayerState.CharacterMovement.HangingLedge;
                        }

                        else
                        {
                            playerController.playerState.currentState = PlayerState.CharacterMovement.Wallsliding;
                        }
                    }


                    isStanding = true;
                    playerController.playerState.canJump = false;
                    break;

                //-----------------------------------------------------------------F A L L I N G-------------------------------------------
                case PlayerState.CharacterMovement.Falling:
                    if (playerController.playerSurroundings.isTouchingWall)
                    {
                        if (!playerController.playerSurroundings.isTouchingLedge)
                        {
                            playerController.playerState.currentState = PlayerState.CharacterMovement.HangingLedge;
                        }

                        else
                        {
                            playerController.playerState.currentState = PlayerState.CharacterMovement.Wallsliding;
                        }
                    }

                    EnableMovement();
                    Debug.Log("I'm Falling");

                    //fall timer for stuck

                    //increase fall velocity

                    isStanding = true;
                    playerController.playerState.canJump = false;
                    break;
                //-----------------------------------------------------------------W A L L S L I D I N G-------------------------------------------
                case PlayerState.CharacterMovement.Wallsliding:
                    Debug.Log("I'm Wallsliding");



                    if ((playerController.playerState.isFacingRight && playerController.playerInput.isLeftPressed && !playerController.playerInput.isRightPressed)
                    || (!playerController.playerState.isFacingRight && playerController.playerInput.isRightPressed && !playerController.playerInput.isLeftPressed))
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.WallJump;
                    }

                    if (!playerController.playerSurroundings.isTouchingWall)
                    {
                        EnableMovement();
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Falling;
                    }

                    else
                    {
                        DisableMovement();
                    }

                    if (!playerController.playerSurroundings.isTouchingLedge)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.HangingLedge;
                    }

                    if (playerController.playerInput.isDownPressed)
                    {
                        //increase slide speed
                    }

                    // canTurn = false;
                    isStanding = true;
                    playerController.playerState.canJump = false;
                    break;

                //-----------------------------------------------------------------W A L L J U M P-------------------------------------------
                case PlayerState.CharacterMovement.WallJump:
                    Debug.Log("I'm ready to slide jump");

                    if ((playerController.playerState.isFacingRight && playerController.speedList.walkSpeed > 0)
                    || (!playerController.playerState.isFacingRight && playerController.speedList.walkSpeed < 0))
                    {
                        playerController.speedList.FlipSpeedValues();
                    }

                    if (playerController.playerSurroundings.isTouchingWall 
                    && !playerController.playerInput.isJumpPressed 
                    && ((playerController.playerState.isFacingRight && !playerController.playerInput.isLeftPressed)
                    || (!playerController.playerState.isFacingRight && !playerController.playerInput.isRightPressed)))
                    {

                        if (playerController.playerSurroundings.isTouchingLedge)
                        {
                            {
                                playerController.playerState.currentState = PlayerState.CharacterMovement.Wallsliding;
                            }
                        }

                        else
                        {
                            {
                                playerController.playerState.currentState = PlayerState.CharacterMovement.HangingLedge;
                            }
                        }
                    }



                    if (playerController.playerInput.isJumpTapped && playerController.speedList.currentSpeed != 0)
                    {
                        if (playerController.playerState.isFacingRight)
                        {
                            playerController.playerState.isFacingRight = false;
                        }
                        else
                        {
                            playerController.playerState.isFacingRight = true;
                        }

                        EnableMovement();
                        playerController.playerJump.WallJump();

                        playerController.playerState.currentState = PlayerState.CharacterMovement.Jumping;

                    }

                    else if (playerController.playerInput.isDownTapped)
                    {
                        if (playerController.playerState.isFacingRight)
                        {
                            playerController.playerState.isFacingRight = false;
                        }
                        else
                        {
                            playerController.playerState.isFacingRight = true;
                        }
                    }

                    else if (!playerController.playerSurroundings.isTouchingWall) //no auto movement after sliding to the bottom
                    {
                        Debug.Log("ENTER");
                        EnableMovement();
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Falling;
                    }

                    else
                    {
                        DisableMovement();
                    }


                    isStanding = true;
                    playerController.playerState.canJump = true;
                    break;

                //-----------------------------------------------------------------H A N G I N G  L E D G E-------------------------------------------
                case PlayerState.CharacterMovement.HangingLedge:
                    Debug.Log("I'm Hanging");


                    if (playerController.playerInput.isDownPressed)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Wallsliding;
                    }

                    if (!playerController.playerSurroundings.isTouchingWall)
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Falling;
                    }

                    if ((playerController.playerState.isFacingRight && playerController.playerInput.isLeftPressed && !playerController.playerInput.isRightPressed)
                    || (!playerController.playerState.isFacingRight && playerController.playerInput.isRightPressed && !playerController.playerInput.isLeftPressed))
                    {
                        playerController.playerState.currentState = PlayerState.CharacterMovement.WallJump;
                    }

                    if (playerController.playerInput.isUpTapped
                    || playerController.playerInput.isJumpTapped)

                    {
                        // if (canClimb)
                        //  {
                        ClimbLedgeTest();
                        playerController.playerState.currentState = PlayerState.CharacterMovement.Climbing;
                        // }

                    }

                    HangingLedge();
                    DisableMovement();
                    isStanding = true;
                    playerController.playerState.canJump = false;
                    break;

                //-----------------------------------------------------------------D E F A U L T  I N  A I R-------------------------------------------
                default:
                    Debug.Log("Unassigned State in air");

                    if (!isClimbingLedge)
                    {
                        if (playerController.rb.velocity.y > 0)
                        {
                            playerController.playerState.currentState = PlayerState.CharacterMovement.Jumping;
                        }

                        else
                        {
                            playerController.playerState.currentState = PlayerState.CharacterMovement.Falling;
                        }
                    }

                    break;
            }
        }
    }


    internal void MoveDetection() // checks the direction that is pressed
    {
        if ((playerController.playerState.isFacingRight && playerController.speedList.walkSpeed < 0)
          || (!playerController.playerState.isFacingRight && playerController.speedList.walkSpeed > 0))
        {
            playerController.speedList.FlipSpeedValues();
        }

        if (playerController.playerInput.isLeftTapped)
        {
            leftPriority = true;
        }

        else if (playerController.playerInput.isRightTapped)
        {
            leftPriority = false;
        }

        if (leftPriority)
        {
            playerController.playerMove.PriorityDirectionLeft();
        }

        else
        {
            playerController.playerMove.PriorityDirectionRight();
        }
    }

    // private void PriorityDirectionLeft() - now in PlayerDirectionPriority
    // private void PriorityDirectionRight() - now in PlayerDirectionPriority

    //---------------------------------------------M O V E M E N T   L O G I C-------------------------------------------------------------------------
    internal void NewMovements()
    {

        // if (playerController.playerSurroundings.isTouchingWall || playerController.playerSurroundings.isTouchingLedge)
        // {
        //     WallBounce();
        //     IdleStop();
        // }

        MoveDetection();
        Movement();

        {
            // MoveDetection();

            //jump    if (playerController.playerInput.isJumpTapped)
            //jump    {
            //jump        playerController.playerJump.jumpBufferCount = playerController.playerJump.jumpBufferLength;
            //jump
            //jump        if (playerController.playerState.canJump) // JUMP
            //jump        {
            //jump            playerController.playerJump.Jump();
            //jump        }
            //jump    }
            //jump
            //jump    else if (playerController.playerJump.jumpBufferCount > 0)
            //jump    {
            //jump        if (playerController.playerState.canJump && playerController.playerInput.isJumpPressed && playerController.playerSurroundings.isGrounded)
            //jump        {
            //jump            playerController.playerJump.Jump();
            //jump        }
            //jump        else if (playerController.playerJump.jumpBufferCount >= 0)
            //jump        {
            //jump            playerController.playerJump.jumpBufferCount -= Time.deltaTime;
            //jump        }
            //jump    }
            //jump
            //jump    if (playerController.rb.velocity.y > 0
            //jump  && playerController.playerInput.isJumpReleased) // if space is tapped, jump is smaller
            //jump    {
            //jump        playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, playerController.rb.velocity.y * 0.65f);
            //jump    }

            if (playerController.playerSurroundings.isGrounded)
            {
                // wallJumpReady = false;
                // EnableMovement(); // CHECK

                if (canMove)
                {


                    if (isStanding)  // -------------------------------------------IS STANDING ----- POSITION
                    {
                        if (playerController.playerDetectObject.canInteract)
                        {
                            if (playerController.playerInput.isInteractTapped && !playerController.playerInput.isJumpPressed)
                            {
                                playerController.playerInteraction.PickUp();
                                playerController.playerState.isInteracting = true;
                            }

                        }


                        playerController.playerTimers.slidingTimer = playerController.playerTimers.slidingTimerSet;
                        playerController.playerTimers.crawlTimer = 0; // CHECK IF WORKING CRAWL

                        if (playerController.playerSurroundings.isTouchingWall || playerController.playerSurroundings.isTouchingLedge)
                        {
                            // WallBounce();
                            // IdleStop();
                        }

                        //--------------------SLIDE----------------- // Transfered
                        // if (isSprinting
                        // && playerController.playerInput.isDownPressed
                        // && playerController.speedList.currentSpeed == playerController.speedList.runningSpeed)
                        // {
                        //     // Slide();
                        // }

                        //------------------STOP SPRINT ------------------- // transfered
                        // else if ((isSprinting && playerController.playerInput.isSprintReleased)
                        //     || (isSprinting && !playerController.playerInput.isLeftPressed && !playerController.playerState.isFacingRight)
                        //     || (isSprinting && playerController.playerState.isFacingRight && !playerController.playerInput.isRightPressed))
                        // {
                        //     //isSprinting = false;
                        // }

                        //----------------SPRINT-------------------------- // transfered
                        // else if (isWalking && playerController.playerInput.isSprintPressed
                        //          && ((playerController.playerState.isFacingRight && playerController.speedList.currentSpeed >= playerController.speedList.walkSpeed)
                        //          || (!playerController.playerState.isFacingRight && playerController.speedList.currentSpeed <= playerController.speedList.walkSpeed)))
                        // {
                        //     //----------------------------SPRINT INTO WALL--------------------
                        //     if (playerController.playerSurroundings.isTouchingWall || playerController.playerSurroundings.isTouchingLedge)
                        //     {
                        //         // if (playerController.playerState.isFacingRight)
                        //         // {
                        //         //     WallBounce();
                        //         // }
                        //         // else
                        //         // {
                        //         //     WallBounce();
                        //         // }
                        //         // IdleStop();
                        //     }
                        //     else
                        //     {
                        //         //Sprint();
                        //     }
                        // }
                        //
                        // //-----------------------CRAWL---------------------- // transfered
                        // else if (isWalking
                        //         && playerController.playerInput.isDownPressed)
                        // {
                        //     //Crawl();
                        // }

                        //----------------------PRONE------------------------ // transfered
                        // else if (isIdle
                        //         && playerController.playerInput.isDownPressed)
                        // {
                        //     // GoProne();
                        //     // NearestPixel();
                        //     // isProneIdle = true; //lets go prone if is moving into a wall
                        // }

                        //---------------------WALK-------------------------
                        // else if ((playerController.playerInput.isLeftPressed && !isSprinting)
                        //     || (playerController.playerInput.isRightPressed && !isSprinting))
                        // {
                        //     //--------------------HIT WALL----------------------
                        //     // if (playerController.playerSurroundings.isTouchingWall || playerController.playerSurroundings.isTouchingLedge)
                        //     // {
                        //     //     WallBounce();
                        //     //     IdleStop();
                        //     //
                        //     // }
                        //     // //------------------WALK--------------------- //transfered
                        //     // else
                        //     // {
                        //     //     Walk();
                        //     // }
                        // }

                        //-------------------STOP--------------------------- // transfered
                        // else
                        // {
                        //     if (!isSprinting)
                        //     {
                        //         //  IdleStop();
                        //
                        //         if (playerController.playerSurroundings.isTouchingWall || playerController.playerSurroundings.isTouchingLedge)
                        //         {
                        //             //   WallBounce();
                        //         }
                        //     }
                        // }

                    }
                    //else if (isProne) // ----------------------------------------IS PRONE ----- POSITION // transfered
                    //{
                    //
                    //
                    //    if (playerController.playerSurroundings.isTouchingWall || (!playerController.playerInput.isRightPressed && !playerController.playerInput.isLeftPressed)) //if touches wall - stop
                    //    {
                    //        //IdleStop();
                    //    }
                    //
                    //    if (playerController.playerState.isMoving && !isSliding) //assigns a state if climbs up a ledge while holding direction
                    //    {
                    //        //Crawl();
                    //    }
                    //
                    //    //-------------------------SLIDE TO CRAWL---------------------------
                    //
                    //    if (isSliding)
                    //    {
                    //        // //playerController.playerTimers.slidingTimer -= Time.deltaTime;
                    //        // if (!playerController.playerInput.isSprintPressed
                    //        //     || playerController.playerTimers.slidingTimer < 0.05
                    //        //     || (playerController.playerState.isFacingRight && playerController.speedList.currentSpeed < playerController.speedList.slidingSpeed)
                    //        //     || (!playerController.playerState.isFacingRight && playerController.speedList.currentSpeed > playerController.speedList.slidingSpeed))
                    //        // {
                    //        //     // Crawl();
                    //        // }
                    //        //
                    //        // //------------------SLIDE TO SPRINT------------------------------
                    //        // else if (!playerController.playerInput.isDownPressed && !playerController.playerSurroundings.isTouchingCeilingProne)
                    //        // {
                    //        //     Sprint();
                    //        // }
                    //    }
                    //
                    //    else if (isCrawling)
                    //    {
                    //        //------------------CRAWL TO STAND-------------------------------- transfered
                    //        if (!playerController.playerInput.isDownPressed && !playerController.playerSurroundings.isTouchingCeilingProne)
                    //        {
                    //            // StandUp();
                    //        }
                    //
                    //        //-----------------IDLE PRONE------------------------------------- // transfered
                    //        else if ((playerController.playerState.isFacingRight && !playerController.playerInput.isRightPressed)
                    //            || (!playerController.playerState.isFacingRight && !playerController.playerInput.isLeftPressed)
                    //            || (playerController.playerSurroundings.isTouchingWall))
                    //        {
                    //            //  IdleStop();
                    //        }
                    //    }
                    //
                    //    else if (isProneIdle)
                    //    {
                    //        //---------------------------PRONE TO STAND------------------------------- transfered
                    //        if (!playerController.playerInput.isDownPressed && !playerController.playerSurroundings.isTouchingCeilingProne)
                    //        {
                    //            //StandUp();
                    //        }
                    //
                    //        else if ((playerController.playerInput.isLeftPressed || playerController.playerInput.isRightPressed) && !playerController.playerSurroundings.isTouchingWall)
                    //        {
                    //            // Crawl();
                    //        }
                    //        else
                    //        {
                    //            //GoProne();
                    //        }
                    //    }
                    //}
                }
            }
            else if (!playerController.playerSurroundings.isGrounded)
            {


                //  if (isClimbingLedge)
                //  {
                //      if (playerController.playerSurroundings.isAbleToClimb)
                //      {
                //          //GoProne();
                //      }
                //      else
                //      {
                //      }
                //
                //  }
                //  else
                //  {
                //      StandUp();
                //      //isIdle = false;
                //      //isWalking = false;
                //      //isSprinting = false;
                //
                //      if (isHangingLedge && wallJumpReady)
                //      {
                //          canClimb = false;
                //      }
                //
                //      if (isHangingLedge || isWallSliding)
                //      {
                //          if ((playerController.playerState.isFacingRight && playerController.playerInput.isLeftPressed && !playerController.playerInput.isRightPressed)
                //          || (!playerController.playerState.isFacingRight && playerController.playerInput.isRightPressed && !playerController.playerInput.isLeftPressed))
                //          {
                //
                //              // wallJumpReady = true;
                //              // canClimb = false;
                //          }
                //
                //          else
                //          {
                //              // wallJumpReady = false;
                //              // canClimb = true;
                //              //slideBlock = false;
                //          }
                //
                //         // if (playerController.playerSurroundings.isAbleToClimb)
                //         // {
                //         //     //StandUp();
                //         //     //GoProne(); <---------------------------------------------------FIX TO GO PRONE ONLY CLIMBING IN SMALL ENTRANCES
                //         //     //isProneIdle = true;
                //         // }
                //         // else
                //         // {
                //         //     //StandUp();
                //         // }
                //
                //          if (wallJumpReady)
                //          {
                //              if ((playerController.playerState.isFacingRight && playerController.speedList.walkSpeed > 0)
                //              || (!playerController.playerState.isFacingRight && playerController.speedList.walkSpeed < 0))
                //              {
                //                  playerController.speedList.FlipSpeedValues();
                //              }
                //
                //              // else if (!playerController.playerState.isFacingRight && playerController.speedList.walkSpeed < 0)
                //              // {
                //              //     playerController.speedList.FlipSpeedValues();
                //              // }
                //
                //              else
                //              {
                //
                //              }
                //
                //              if (playerController.playerInput.isJumpTapped)
                //              {
                //                  if (playerController.playerInput.isJumpPressed)
                //                  {
                //                      //  playerController.playerJump.Jump();                                                                                                                        ////CHECK THIS
                //                  }
                //              }////CHECK THIS
                //
                //              else if (playerController.playerInput.isDownTapped)
                //              {
                //                  // playerController.playerJump.DropDown();
                //              }
                //          }
                //
                //
                //          if (isHangingLedge)
                //          {
                //              // DisableMovement();
                //              // GoProne();
                //              if (playerController.playerInput.isJumpTapped
                //                  ||
                //                  playerController.playerInput.isUpTapped)
                //              {
                //                  if (canClimb)
                //                  {
                //                      //   ClimbLedge();
                //                  }
                //              }
                //
                //              else if (playerController.playerInput.isDownTapped)
                //              {
                //                  // playerController.rb.gravityScale = default;
                //                  // playerController.playerSurroundings.canHangLedge = false;
                //                  // EnableMovement();
                //              }
                //          }
                //
                //          else if (isWallSliding)
                //          {
                //              //DisableMovement();
                //              if (playerController.playerInput.isDownPressed)
                //              {
                //                  if (playerController.speedList.wallSlideSpeed < 6f)
                //                  {
                //                      playerController.speedList.wallSlideSpeed += 0.1f;
                //                  }
                //                  else
                //                  {
                //                      playerController.speedList.wallSlideSpeed = 6f;
                //                  }
                //              }
                //
                //              else
                //              {
                //                  //playerController.wallSlideSpeed = 0.45f;
                //                  WallSlideSpeedIdle();
                //              }
                //          }
                //          else
                //          {
                //              // EnableMovement();
                //          }
                //      }
                //
                //      else if (!playerController.playerSurroundings.isGrounded)
                //      {
                //          wallJumpReady = false;
                //          // EnableMovement();
                //      }
                //
                //      else
                //      {
                //          //EnableMovement();
                //      }
                //  }
            }
        }
    }
    //-----------------------------------------S T A T E   F U N C T I O N S-----------------------------------------------------------------

    internal void GoProne()
    {
        Debug.Log("I lie down");
        isStanding = false;
        // isIdle = false;
        //isWalking = false;
        //isSprinting = false;
        //isProne = true;
    }

    internal void StandUp()
    {
        Debug.Log("I stand");

        //isProne = false;
        // isProneIdle = false;
        // isCrawling = false;
        // isSliding = false;
        isStanding = true;
        playerController.playerTimers.crawlTimer = 0;
        playerController.playerTimers.slidingTimer = playerController.playerTimers.slidingTimerSet;
    }

    internal void Slide()
    {
        playerController.playerTimers.slidingTimer -= Time.deltaTime;
        //GoProne();
        // isSliding = true;
        playerController.playerTimers.slideTransitionTimer = playerController.playerTimers.slideTransitionTimerSet;
    }

    internal void WallInteraction()
    {
        if (playerController.playerSurroundings.isTouchingWall
            && !playerController.playerSurroundings.isGrounded)
        {
            // playerController.rb.gravityScale = 3;
            if (!playerController.playerSurroundings.isTouchingLedge
                && playerController.playerSurroundings.canHangLedge
                && playerController.rb.velocity.y < 2)
            {
                HangingLedge();
            }

            else
            {
                //isHangingLedge = false;
            }
            if (playerController.playerSurroundings.isTouchingLedge || isClimbingLedge
      && (playerController.rb.velocity.y < wallSlideSense/*-1*/ )) //sticks better to walljumps
            {
                // isWallSliding = true;
                //playerController.rb.gravityScale = 1;
            }
        }
        else
        {

            //isHangingLedge = false;
            playerController.rb.gravityScale = playerController.startingGravity;
            //isWallSliding = false;
        }
    }

    private void WallBounce()
    {
        if (playerController.playerState.isFacingRight)
        {
            playerController.transform.localPosition = new Vector2(transform.localPosition.x - 0.015f, transform.localPosition.y);
        }

        else
        {
            playerController.transform.localPosition = new Vector2(transform.localPosition.x + 0.015f, transform.localPosition.y);
        }
    }

    private void HangingLedge() // put into wall interaction function
    {

        hangingPosition = new Vector2(playerController.rb.position.x, playerController.rb.position.y);
        FreezePlayerLocation(velX, velY);


    }

    internal void ClimbLedgeTest()
    {
        isClimbingLedge = true;
        FreezePlayerLocation(velX, velY);

        if (playerController.playerState.isFacingRight)
        {
            playerController.rb.position = new Vector2(hangingPosition.x + 0.35f, hangingPosition.y + 0.28f);
        }
        else
        {
            playerController.rb.position = new Vector2(hangingPosition.x - 0.35f, hangingPosition.y + 0.28f);
        }
    }


    internal void ClimbLedge()
    {


        playerController.playerTimers.climbLedgeTimer += Time.deltaTime;

        // DisableMovement();
        //
        // if (playerController.playerState.isFacingRight && !playerController.playerInput.isLeftPressed && canClimb)
        // {
        //     isHangingLedge = false;
        //     isClimbingLedge = true;
        playerController.rb.position = new Vector2(hangingPosition.x + 0.35f, hangingPosition.y + 0.28f); //standin +0.28f
        // }
        // else if (!playerController.playerState.isFacingRight && !playerController.playerInput.isRightPressed && canClimb)
        // {
        //     isHangingLedge = false;
        //     isClimbingLedge = true;
        //     playerController.rb.position = new Vector2(hangingPosition.x - 0.35f, hangingPosition.y + 0.43f);
        // }
        // else
        // {
        //     isHangingLedge = true;
        //     isClimbingLedge = false;
        // }

    } // climbing logic

    internal void SpecialMovement()
    {
        // if (isClimbingLedge)
        // {
        //     //DisableMovement();
        //     FreezePlayerLocation(velX, velY);
        //     playerController.playerTimers.climbLedgeTimer += Time.deltaTime;
        //
        //     if (playerController.playerTimers.climbLedgeTimer > playerController.playerTimers.climbLedgeTimerSet || !isClimbingLedge)
        //     {
        //         //EnableMovement();
        //         isClimbingLedge = false;
        //         playerController.playerTimers.climbLedgeTimer = 0;
        //     }
        // }
    }

    internal void IdleStop()
    {

        playerController.playerState.isMoving = false;
        // playerController.playerMove.Move();
        NearestPixel();
        // if (isStanding)
        // {
        //     //isWalking = false;
        //     //isSprinting = false;
        //     //isIdle = true;
        //     StandUp();
        // }
        // else
        // {
        //    // isSliding = false;
        //    // isCrawling = false;
        //    // isProneIdle = true;
        //     GoProne();
        // }
    }

    private void FreezePlayerLocation(float velocityX, float velocityY)
    {
        playerController.rb.velocity = new Vector2(velocityX, velocityY);
        playerController.rb.gravityScale = 0f;
    }

    private void DisableMovement()
    {
        canMove = false;
        canTurn = false;
    }

    private void EnableMovement()
    {
        Debug.Log("Bogos");
        canMove = true;
        canTurn = true;
    }

    internal void WallSlideSpeedIdle()
    {
        if (playerController.speedList.wallSlideSpeed > 0.45f)
        {
            playerController.speedList.wallSlideSpeed *= 0.925f;
        }

        else if (playerController.speedList.wallSlideSpeed < 0.45f)
        {
            playerController.speedList.wallSlideSpeed = 0.45f;
        }
    }

    private void NearestPixel()  // sets the player to the closes pixel for the resolution
    {
        if (playerController.playerSurroundings.isGrounded || playerController.playerSurroundings.isOnPlatform) // so it doesn't bug out climbing
        {
            float pixelCoord = Mathf.Round(playerController.transform.localPosition.x / 0.03125f);
            float pixelPos = (pixelCoord * 0.03125f);
            playerController.transform.localPosition = new Vector2(pixelPos, transform.localPosition.y);
        }
    }

}
