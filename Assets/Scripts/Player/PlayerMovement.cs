using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    PlayerController playerController;

    //internal bool isFacingRight = true;

    internal bool isStanding = true;
    internal bool isProne;
    internal bool isIdle;
    internal bool isWalking;
    internal bool isSprinting;
    internal bool isSliding;
    internal bool isProneIdle;
    internal bool isCrawling;
    internal bool isTurning;


    internal bool isPushing;

    internal bool isHangingLedge;
    internal bool isClimbingLedge;
    internal bool isWallSliding;


    internal bool isInteracting;

    internal bool leftPriority;

    internal bool canClimb = false;
    internal bool canTurn = true;
    internal bool canMove = true;
    internal bool canSlide = false;
    //internal bool isMoving;

    internal Vector2 hangingPosition;


    internal float velX = 0;
    internal float velY = 0;

    // internal float jumpBufferCount;
    // internal float jumpBufferLength = 0.2f;

    //internal float jumpAntiSpam;

    internal bool wallJumpReady = false;

    public float wallSlideSense;



    private void HangingLedge() // put into wall interaction function
    {
        if (!isClimbingLedge)
        {
            hangingPosition = new Vector2(playerController.rb.position.x, playerController.rb.position.y);
            FreezePlayerLocation(velX, velY);
            isHangingLedge = true;
        }
    }

    internal void ClimbLedge()
    {
        DisableMovement();

        if (playerController.playerState.isFacingRight && !playerController.playerInput.isLeftPressed && canClimb)
        {
            isHangingLedge = false;
            isClimbingLedge = true;
            playerController.rb.position = new Vector2(hangingPosition.x + 0.35f, hangingPosition.y + 0.43f); //standin +0.28f
        }
        else if (!playerController.playerState.isFacingRight && !playerController.playerInput.isRightPressed && canClimb)
        {
            isHangingLedge = false;
            isClimbingLedge = true;
            playerController.rb.position = new Vector2(hangingPosition.x - 0.35f, hangingPosition.y + 0.43f);
        }
        else
        {
            isHangingLedge = true;
            isClimbingLedge = false;
        }
    } // climbing logic

    internal void SpecialMovement()
    {
        if (isClimbingLedge)
        {
            //DisableMovement();
            FreezePlayerLocation(velX, velY);
            playerController.playerTimers.climbLedgeTimer += Time.deltaTime;

            if (playerController.playerTimers.climbLedgeTimer > playerController.playerTimers.climbLedgeTimerSet || !isClimbingLedge)
            {
                //EnableMovement();
                isClimbingLedge = false;
                playerController.playerTimers.climbLedgeTimer = 0;
            }
        }
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
        canMove = true;
        canTurn = true;
    }

    //internal void ChangeSpeed(float newSpeed) - now in SpeedList
    //internal void ChangeSpeedNew(float newSpeed) - now in SpeedList

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
    private void CheckInteraction()
    {
        // if (playerController.playerState.isInteracting)
        // {
        //     isInteracting = true;
        // }
        //
        // else
        // {
        //     isInteracting = false;
        // }
    }

    // private void PriorityDirectionLeft() - now in PlayerDirectionPriority
    // private void PriorityDirectionRight() - now in PlayerDirectionPriority

    //---------------------------------------------M O V E M E N T   L O G I C-------------------------------------------------------------------------
    internal void NewMovements()
    {
        {
            MoveDetection();
            CheckInteraction();

            if (playerController.playerInput.isJumpTapped)
            {
                playerController.playerJump.jumpBufferCount = playerController.playerJump.jumpBufferLength;

                if (playerController.playerState.canJump) // JUMP
                {
                    playerController.playerJump.Jump();
                }
            }

            else if (playerController.playerJump.jumpBufferCount > 0)
            {
                if (playerController.playerState.canJump && playerController.playerInput.isJumpPressed && playerController.playerSurroundings.isGrounded)
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

            if (playerController.playerSurroundings.isGrounded)
            {
                wallJumpReady = false;
                EnableMovement(); // CHECK

                if (canMove)
                {


                    if (isStanding)  // -------------------------------------------IS STANDING ----- POSITION
                    {
                        if (playerController.moveObject.canGrab)
                        {
                            if (playerController.playerInput.isInteractTapped)
                            {
                                playerController.playerState.isInteracting = true;
                            }

                        }
                        else
                        {
                            playerController.playerTimers.slidingTimer = playerController.playerTimers.slidingTimerSet;
                            playerController.playerTimers.crawlTimer = playerController.playerTimers.crawlTimerSet; // CHECK IF WORKING CRAWL

                            if (playerController.playerSurroundings.isTouchingWall || playerController.playerSurroundings.isTouchingLedge)
                            {
                                IdleStop();
                            }

                            //--------------------SLIDE-----------------
                            if (isSprinting
                            && playerController.playerInput.isDownPressed
                            && playerController.speedList.currentSpeed == playerController.speedList.runningSpeed)
                            {
                                Slide();
                            }

                            //------------------STOP SPRINT -------------------
                            else if ((isSprinting && playerController.playerInput.isSprintReleased)
                                || (isSprinting && !playerController.playerInput.isLeftPressed && !playerController.playerState.isFacingRight)
                                || (isSprinting && playerController.playerState.isFacingRight && !playerController.playerInput.isRightPressed))
                            {
                                isSprinting = false;
                            }

                            //----------------SPRINT--------------------------
                            else if (isWalking && playerController.playerInput.isSprintPressed
                                     && ((playerController.playerState.isFacingRight && playerController.speedList.currentSpeed >= playerController.speedList.walkSpeed) || (!playerController.playerState.isFacingRight && playerController.speedList.currentSpeed <= playerController.speedList.walkSpeed)))
                            {
                                //----------------------------SPRINT INTO WALL--------------------
                                if (playerController.playerSurroundings.isTouchingWall || playerController.playerSurroundings.isTouchingLedge)
                                {
                                    if (playerController.playerState.isFacingRight)
                                    {
                                        WallBounce();
                                    }
                                    else
                                    {
                                        WallBounce();
                                    }
                                    IdleStop();
                                }
                                else
                                {
                                    Sprint();
                                }
                            }

                            //-----------------------CRAWL----------------------
                            else if (isWalking
                                    && playerController.playerInput.isDownPressed)
                            {
                                Crawl();
                            }

                            //----------------------PRONE------------------------
                            else if (isIdle
                                    && playerController.playerInput.isDownPressed)
                            {
                                GoProne();
                                NearestPixel();
                                isProneIdle = true; //lets go prone if is moving into a wall
                            }

                            //---------------------WALK-------------------------
                            else if ((playerController.playerInput.isLeftPressed && !isSprinting)
                                || (playerController.playerInput.isRightPressed && !isSprinting))
                            {
                                //--------------------HIT WALL----------------------
                                if (playerController.playerSurroundings.isTouchingWall || playerController.playerSurroundings.isTouchingLedge)
                                {
                                    IdleStop();
                                    WallBounce();
                                }
                                //------------------WALK---------------------
                                else
                                {
                                    Walk();
                                }
                            }

                            //-------------------STOP---------------------------
                            else
                            {
                                if (!isSprinting)
                                {
                                    IdleStop();

                                    if (playerController.playerSurroundings.isTouchingWall || playerController.playerSurroundings.isTouchingLedge)
                                    {
                                        WallBounce();
                                    }
                                }
                            }
                        }
                    }
                    else if (isProne) // ----------------------------------------IS PRONE ----- POSITION
                    {


                        if (playerController.playerSurroundings.isTouchingWall || (!playerController.playerInput.isRightPressed && !playerController.playerInput.isLeftPressed)) //if touches wall - stop
                        {
                            IdleStop();
                        }

                        if (playerController.playerState.isMoving && !isSliding) //assigns a state if climbs up a ledge while holding direction
                        {
                            Crawl();
                        }

                        //-------------------------SLIDE TO CRAWL---------------------------

                        if (isSliding)
                        {
                            playerController.playerTimers.slidingTimer -= Time.deltaTime;
                            if (!playerController.playerInput.isSprintPressed
                                || playerController.playerTimers.slidingTimer < 0.05
                                || (playerController.playerState.isFacingRight && playerController.speedList.currentSpeed < playerController.speedList.slidingSpeed)
                                || (!playerController.playerState.isFacingRight && playerController.speedList.currentSpeed > playerController.speedList.slidingSpeed))
                            {
                                Crawl();
                            }

                            //------------------SLIDE TO SPRINT------------------------------
                            else if (!playerController.playerInput.isDownPressed && !playerController.playerSurroundings.isTouchingCeilingProne)
                            {
                                Sprint();
                            }
                        }

                        else if (isCrawling)
                        {
                            //------------------CRAWL TO STAND--------------------------------
                            if (!playerController.playerInput.isDownPressed && !playerController.playerSurroundings.isTouchingCeilingProne)
                            {
                                StandUp();
                            }

                            //-----------------IDLE PRONE-------------------------------------
                            else if ((playerController.playerState.isFacingRight && !playerController.playerInput.isRightPressed)
                                || (!playerController.playerState.isFacingRight && !playerController.playerInput.isLeftPressed)
                                || (playerController.playerSurroundings.isTouchingWall))
                            {
                                IdleStop();
                            }
                        }

                        else if (isProneIdle)
                        {
                            //---------------------------PRONE TO STAND-------------------------------
                            if (!playerController.playerInput.isDownPressed && !playerController.playerSurroundings.isTouchingCeilingProne)
                            {
                                StandUp();
                            }

                            else if ((playerController.playerInput.isLeftPressed || playerController.playerInput.isRightPressed) && !playerController.playerSurroundings.isTouchingWall)
                            {
                                Crawl();
                            }
                            else
                            {
                                GoProne();
                            }
                        }
                    }
                }
            }
            else if (!playerController.playerSurroundings.isGrounded)
            {


                if (isClimbingLedge)
                {
                    if (playerController.playerSurroundings.isAbleToClimb)
                    {
                        GoProne();
                    }
                    else
                    {
                    }

                }
                else
                {
                    StandUp();
                    isIdle = false;
                    isWalking = false;
                    isSprinting = false;

                    if (isHangingLedge && wallJumpReady)
                    {
                        canClimb = false;
                    }

                    if (isHangingLedge || isWallSliding)
                    {
                        if ((playerController.playerState.isFacingRight && playerController.playerInput.isLeftPressed && !playerController.playerInput.isRightPressed)
                        || (!playerController.playerState.isFacingRight && playerController.playerInput.isRightPressed && !playerController.playerInput.isLeftPressed))
                        {
                            
                            wallJumpReady = true;
                            canClimb = false;
                        }

                        else
                        {
                            wallJumpReady = false;
                            canClimb = true;
                            //slideBlock = false;
                        }

                        if (playerController.playerSurroundings.isAbleToClimb)
                        {
                            //StandUp();
                            //GoProne(); <---------------------------------------------------FIX TO GO PRONE ONLY CLIMBING IN SMALL ENTRANCES
                            isProneIdle = true;
                        }
                        else
                        {
                            StandUp();
                        }

                        if (wallJumpReady)
                        {
                            if ((playerController.playerState.isFacingRight && playerController.speedList.walkSpeed > 0) 
                            || (!playerController.playerState.isFacingRight && playerController.speedList.walkSpeed < 0))
                            {
                                playerController.speedList.FlipSpeedValues();
                            }

                           // else if (!playerController.playerState.isFacingRight && playerController.speedList.walkSpeed < 0)
                           // {
                           //     playerController.speedList.FlipSpeedValues();
                           // }

                            else
                            {

                            }

                            if (playerController.playerInput.isJumpTapped)
                            {
                                if (playerController.playerInput.isJumpPressed)
                                {
                                    playerController.playerJump.Jump();                                                                                                                        ////CHECK THIS
                                }
                            }////CHECK THIS

                            else if (playerController.playerInput.isDownTapped)
                            {
                                playerController.playerJump.DropDown();
                            }
                        }


                        if (isHangingLedge)
                        {
                            DisableMovement();
                            GoProne();
                            if (playerController.playerInput.isJumpTapped
                                ||
                                playerController.playerInput.isUpTapped)
                            {
                                if (canClimb)
                                {
                                    ClimbLedge();
                                }
                            }

                            else if (playerController.playerInput.isDownTapped)
                            {
                                playerController.rb.gravityScale = default;
                                playerController.playerSurroundings.canHangLedge = false;
                                EnableMovement();
                            }
                        }

                        else if (isWallSliding)
                        {
                            DisableMovement();
                            if (playerController.playerInput.isDownPressed)
                            {
                                if (playerController.speedList.wallSlideSpeed < 6f)
                                {
                                    playerController.speedList.wallSlideSpeed += 0.1f;
                                }
                                else
                                {
                                    playerController.speedList.wallSlideSpeed = 6f;
                                }
                            }

                            else
                            {
                                //playerController.wallSlideSpeed = 0.45f;
                                WallSlideSpeedIdle();
                            }
                        }
                        else
                        {
                            EnableMovement();
                        }
                    }

                    else if (!playerController.playerSurroundings.isGrounded)       //////////CHECK THIS
                    {

                        wallJumpReady = false;
                        //jumpAntiSpam += Time.deltaTime; //////////CHECK THIS
                        EnableMovement();                                           //////////CHECK THIS
                    }

                    else
                    {
                        EnableMovement();
                    }
                }
            }
        }
    }

    // internal void SpeedSet() - now in SpeedList

    //-----------------------------------------S T A T E   F U N C T I O N S-----------------------------------------------------------------

    internal void GoProne()
    {
        isStanding = false;
        isIdle = false;
        isWalking = false;
        isSprinting = false;
        isProne = true;
    }

    internal void StandUp()
    {
        isProne = false;
        isProneIdle = false;
        isCrawling = false;
        isSliding = false;
        isStanding = true;
    }

    internal void Slide()
    {
        GoProne();
        isSliding = true;
        playerController.playerTimers.slideTransitionTimer = playerController.playerTimers.slideTransitionTimerSet;
    }

    internal void Crawl()
    {
        GoProne();
        isCrawling = true;
        isProneIdle = false;
        isSliding = false;
    }

    internal void Walk()
    {
        StandUp();
        isWalking = true;
        isSprinting = false;
        isIdle = false;
    }

    internal void Sprint()
    {
        StandUp();
        isSprinting = true;
        isWalking = false;
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
                isHangingLedge = false;
            }
            if (playerController.playerSurroundings.isTouchingLedge
      && (playerController.rb.velocity.y < wallSlideSense/*-1*/ )) //sticks better to walljumps
            {
                isWallSliding = true;
                //playerController.rb.gravityScale = 1;
            }
        }
        else
        {
            isHangingLedge = false;
            playerController.rb.gravityScale = playerController.startingGravity;
            isWallSliding = false;
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

    internal void IdleStop()
    {
       // playerController.playerMove.Move();
        NearestPixel();
        if (isStanding)
        {
            isWalking = false;
            isSprinting = false;
            isIdle = true;
            StandUp();
        }
        else if (isProne)
        {
            isSliding = false;
            isCrawling = false;
            isProneIdle = true;
            GoProne();
        }
    }
}
