using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    PlayerController playerController;

    internal bool isFacingRight = true;

    internal bool isStandingNew = true;
    internal bool isProne;
    internal bool isIdleNew;
    internal bool isWalkingNew;
    internal bool isSprintingNew;
    internal bool isSlidingNew;
    internal bool isProneIdle;
    internal bool isCrawlingNew;
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
    internal bool isMoving;

    internal Vector2 hangingPosition;


    internal float velX = 0;
    internal float velY = 0;

    internal float jumpBufferCount;
    internal float jumpBufferLength = 0.2f;

    public float jumpForce = 6.6f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public float turningRateWalk;
    public float turningRateSprint;
    // public float turningRateAir;
    // public float acceleration;

    internal bool wallJumpReady = false;





    internal void WallInteraction()
    {
        if (playerController.playerSurroundings.isTouchingWall
            && !playerController.playerSurroundings.isGrounded)
        {
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
            if ((playerController.playerSurroundings.isTouchingLedge
      && playerController.rb.velocity.y < -1)) //sticks better to walljumps
            {
                isWallSliding = true;
                //ChangeSpeed(0); ---------------------------------------------------------------------------
            }
        }
        else
        {
            isHangingLedge = false;
            playerController.rb.gravityScale = playerController.startingGravity;
            isWallSliding = false;
        }
    }



    private void HangingLedge() // put into wall interaction function
    {
        if (!isClimbingLedge)
        {
            hangingPosition = new Vector2(playerController.rb.position.x, playerController.rb.position.y);
            FreezePlayerLocation(velX, velY);
            isHangingLedge = true;
            //print("i'm hanging");
        }
    }

    internal void ClimbLedge()
    {

        canMove = false;
        canTurn = false;


        if (isFacingRight && !playerController.playerInput.isLeftPressed && canClimb)
        {
            isHangingLedge = false;
            isClimbingLedge = true;

            playerController.rb.position = new Vector2(hangingPosition.x + 0.35f, hangingPosition.y + 0.43f); //standin +0.28f

        }
        else if (!isFacingRight && !playerController.playerInput.isRightPressed && canClimb)
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
    }

    internal void SpecialMovement()
    {

        if (isClimbingLedge)
        {

            DisableMovement();
            FreezePlayerLocation(velX, velY);
            playerController.climbLedgeTimer += Time.deltaTime;

            if (playerController.climbLedgeTimer > playerController.climbLedgeTimerSet || !isClimbingLedge)
            {
                EnableMovement();
                isClimbingLedge = false;
                playerController.climbLedgeTimer = 0;
            }
        }

        // else if (!isClimbingLedge)
        // {
        //    // EnableMovement();
        //     playerController.climbLedgeTimer = 0;
        // }


        if (isWallSliding)
        {
            canTurn = false;
            if (playerController.rb.velocity.y < -playerController.speedList.wallSlideSpeed)
            {
                playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, -playerController.speedList.wallSlideSpeed);
            }
        }

        //else
        //{
        //    playerController.wallSlideSpeed = 0.45f;
        //}
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

    private void NearestPixel()
    {
        if (playerController.playerSurroundings.isGrounded || playerController.playerSurroundings.isOnPlatform) // so it doesn't bug out climbing
        {
            float pixelCoord = Mathf.Round(playerController.transform.localPosition.x / 0.03125f);
            float pixelPos = (pixelCoord * 0.03125f);
            playerController.transform.localPosition = new Vector2(pixelPos, transform.localPosition.y);
        }
    }

    private void PriorityDirectionLeft()
    {
        if (canTurn)
        {

            if (playerController.playerInput.isLeftPressed)
            {
                isMoving = true;
                if (isFacingRight)
                {
                    playerController.speedList.FlipSpeedValues();

                    if (isSlidingNew)
                    {
                        CrawlNew();
                    }
                }

                isFacingRight = false;

                Move();
            }

            //---------------------------------------------------------------------------------Input Right
            else if (playerController.playerInput.isRightPressed)

            {

                isMoving = true;
                if (!isFacingRight)
                {
                    playerController.speedList.FlipSpeedValues();

                }

                isFacingRight = true;


                Move();
            }

            else
            {
                isMoving = false;
                IdleStop();
            }
        }
    }

    private void PriorityDirectionRight()
    {
        if (canTurn)
        {
            if (playerController.playerInput.isRightPressed)
            //-----------------------------------------------------------------------------------Input Left
            {
                isMoving = true;
                if (!isFacingRight)
                {
                    playerController.speedList.FlipSpeedValues();

                    if (isSlidingNew)
                    {
                        CrawlNew();
                    }
                }

                isFacingRight = true;
                Move();

            }

            //---------------------------------------------------------------------------------Input Right
            else if (playerController.playerInput.isLeftPressed)
            {

                isMoving = true;
                if (isFacingRight)
                {
                    playerController.speedList.FlipSpeedValues();

                }

                isFacingRight = false;
                Move();


            }

            else
            {
                isMoving = false;
                IdleStop();

            }
        }
        else
        {

        }
    }

    //---------------------------------------------M O V E M E N T   L O G I C-------------------------------------------------------------------------
    internal void NewMovements()
    {

        {

            if (playerController.playerInput.isLeftTapped)
            {
                Debug.Log("Left Tap");
                leftPriority = true;
            }

            else if (playerController.playerInput.isRightTapped)
            {
                Debug.Log("Right Tap");
                leftPriority = false;
            }

            if (leftPriority)
            {
                PriorityDirectionLeft();
            }

            else
            {
                PriorityDirectionRight();
            }

            if (playerController.playerInput.isJumpTapped)
            {
                jumpBufferCount = jumpBufferLength;
                if (playerController.playerSurroundings.canJump) // JUMP
                {
                    JumpNew();
                }

               // else if (wallJumpReady)
               // {
               //     WallJump(jumpForce);
               // }


            }
            else if (/*playerController.playerSurroundings.canJump &&playerController.playerInput.isJumpTapped && playerController.hangTimeTimer > 0f
            && */ jumpBufferCount > 0)
            {
                if (playerController.playerSurroundings.canJump && playerController.playerInput.isJumpPressed && playerController.playerSurroundings.isGrounded)
                {
                    JumpNew();
                }
                else if (jumpBufferCount >= 0)
                {
                    jumpBufferCount -= Time.deltaTime;
                }

                else
                {
                    // jumpBufferCount = 0;
                }
            }

            if (playerController.rb.velocity.y > 0
          && playerController.playerInput.isJumpReleased) // if space is tapped, jump is smaller
            {
                playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, playerController.rb.velocity.y * 0.5f);
                //playerController.rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

            if (playerController.playerSurroundings.isGrounded)
            {
                EnableMovement(); // CHECK

                if (isFacingRight && playerController.speedList.walkSpeed < 0)
                {
                    playerController.speedList.FlipSpeedValues();
                }

                else if (!isFacingRight && playerController.speedList.walkSpeed > 0)
                {
                    playerController.speedList.FlipSpeedValues();
                }



                if (canMove)
                {

                    if (isStandingNew) // -------------------------------------------IS STANDING ----- POSITION
                    {
                        playerController.slidingTimer = playerController.slidingTimerSet;
                        playerController.crawlTimer = playerController.crawlTimerSet; // CHECK IF WORKING CRAWL

                        if (playerController.playerSurroundings.isTouchingWall || playerController.playerSurroundings.isTouchingLedge)
                        {
                            //Debug.Log("Stop one");
                            IdleStop();
                        }

                        //--------------------SLIDE-----------------
                        if (isSprintingNew
                        && playerController.playerInput.isDownPressed
                        && playerController.currentSpeed == playerController.speedList.runningSpeed)
                        {
                            SlideNew();
                        }

                        //------------------STOP SPRINT -------------------
                        else if ((isSprintingNew && playerController.playerInput.isSprintReleased)
                            || (isSprintingNew && !playerController.playerInput.isLeftPressed && !isFacingRight)
                            || (isSprintingNew && isFacingRight && !playerController.playerInput.isRightPressed))

                        {
                            isSprintingNew = false;
                        }

                        //----------------SPRINT--------------------------
                        else if (isWalkingNew && playerController.playerInput.isSprintPressed
                                 && ((isFacingRight && playerController.currentSpeed >= playerController.speedList.walkSpeed) || (!isFacingRight && playerController.currentSpeed <= playerController.speedList.walkSpeed)))
                        {
                            //----------------------------SPRINT INTO WALL--------------------
                            if (playerController.playerSurroundings.isTouchingWall || playerController.playerSurroundings.isTouchingLedge)
                            {
                                if (isFacingRight)
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
                                SprintNew();
                            }
                        }

                        //-----------------------CRAWL----------------------
                        else if (isWalkingNew
                                && playerController.playerInput.isDownPressed)
                        {
                            CrawlNew();
                        }

                        //----------------------PRONE------------------------
                        else if (isIdleNew
                                && playerController.playerInput.isDownPressed)
                        {
                            Debug.Log("Down Pressed");
                            GoProne();
                            NearestPixel();
                            isProneIdle = true; //lets go prone if is moving into a wall
                        }

                        //---------------------WALK-------------------------
                        else if ((playerController.playerInput.isLeftPressed && !isSprintingNew)
                            || (playerController.playerInput.isRightPressed && !isSprintingNew))
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
                                WalkNew();
                            }
                        }

                        //-------------------STOP---------------------------
                        else
                        {
                            if (!isSprintingNew)
                            {
                                IdleStop();

                                if (playerController.playerSurroundings.isTouchingWall || playerController.playerSurroundings.isTouchingLedge)
                                {
                                    WallBounce();
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

                        if (isMoving && !isSlidingNew) //assigns a state if climbs up a ledge while holding direction
                        {
                            CrawlNew();
                        }

                        //-------------------------SLIDE TO CRAWL---------------------------

                        if (isSlidingNew)
                        {
                            playerController.slidingTimer -= Time.deltaTime;
                            if (!playerController.playerInput.isSprintPressed || playerController.slidingTimer < 0.05)

                            {
                                CrawlNew();
                            }

                            //------------------SLIDE TO SPRINT------------------------------
                            else if (!playerController.playerInput.isDownPressed && !playerController.playerSurroundings.isTouchingCeilingProne)
                            {
                                SprintNew();
                            }

                        }

                        else if (isCrawlingNew)
                        {
                            //------------------CRAWL TO STAND--------------------------------
                            if (!playerController.playerInput.isDownPressed && !playerController.playerSurroundings.isTouchingCeilingProne)
                            {
                                StandUp();
                                //isWalkingNew = true;
                            }

                            //-----------------IDLE PRONE-------------------------------------
                            else if ((isFacingRight && !playerController.playerInput.isRightPressed)
                                || (!isFacingRight && !playerController.playerInput.isLeftPressed)
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
                                //isIdleNew = true;
                            }


                            else if ((playerController.playerInput.isLeftPressed || playerController.playerInput.isRightPressed) && !playerController.playerSurroundings.isTouchingWall)
                            {
                                CrawlNew();
                            }
                            else                        //Check if needed
                            {                           //Check if needed
                                GoProne();              //Check if needed
                                //isProneIdle = true;     //Check if needed
                            }                           //Check if needed
                        }

                        else if (isInteracting)
                        {

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
                        //playerController.crawlTimer = 0;
                        GoProne();
                        //isProneIdle = true;
                    }
                    else
                    {
                        //  StandUp();
                    }


                }
                else
                {
                    StandUp();
                    isIdleNew = false;
                    isWalkingNew = false;
                    isSprintingNew = false;

                    if (isHangingLedge && wallJumpReady)
                    {
                        canClimb = false;
                    }

                    if (isHangingLedge || isWallSliding)
                    {

                        if ((isFacingRight && playerController.playerInput.isLeftPressed && !playerController.playerInput.isRightPressed)
                        || (!isFacingRight && playerController.playerInput.isRightPressed && !playerController.playerInput.isLeftPressed))
                        {
                            wallJumpReady = true;
                            canClimb = false;
                        }

                        else
                        {
                            wallJumpReady = false;
                            canClimb = true;
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
                            if (isFacingRight && playerController.speedList.walkSpeed > 0)
                            {
                                playerController.speedList.FlipSpeedValues();
                            }

                            else if (!isFacingRight && playerController.speedList.walkSpeed < 0)
                            {
                                playerController.speedList.FlipSpeedValues();
                            }

                            if (playerController.playerInput.isJumpTapped)
                            {

                                if (playerController.playerInput.isJumpPressed)
                                {
                                    WallJump(jumpForce);                                                                                                                        ////CHECK THIS
                                }
                            }////CHECK THIS

                            else if (playerController.playerInput.isDownPressed)
                            {

                                WallJump(0);
                            }
                        }                                                                                                                                      ////CHECK THIS
                        else
                        {
                            if (isFacingRight && playerController.speedList.walkSpeed < 0)
                            {
                                playerController.speedList.FlipSpeedValues();
                            }

                            else if (!isFacingRight && playerController.speedList.walkSpeed > 0)
                            {
                                playerController.speedList.FlipSpeedValues();
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
                                Debug.Log("S Pressed");
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
                    {  //                                                             //////////CHECK THIS
                        EnableMovement();                                           //////////CHECK THIS

                        if (playerController.playerInput.isLeftPressed || playerController.playerInput.isRightPressed)
                        {
                            if (playerController.playerInput.isSprintPressed)
                            {
                                //ChangeSpeedNew(playerController.speedList.runningSpeed);
                            }
                            else
                            {
                                playerController.speedList.ChangeSpeedNew(playerController.speedList.walkSpeed);
                            }
                        }
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

    internal void Move()
    {


        if (isFacingRight && playerController.speedList.walkSpeed < 0)
        {
            playerController.speedList.FlipSpeedValues();
        }

        else if (!isFacingRight && playerController.speedList.walkSpeed > 0)
        {
            playerController.speedList.FlipSpeedValues();
        }

        playerController.rb.velocity = new Vector2(playerController.currentSpeed, playerController.rb.velocity.y);
    }

    internal void GoProne()
    {
        Debug.Log("GoProne");
        isStandingNew = false;
        isIdleNew = false;
        isWalkingNew = false;
        isSprintingNew = false;
        isProne = true;

    }

    internal void StandUp()
    {
        isProne = false;
        isProneIdle = false;
        isCrawlingNew = false;
        isSlidingNew = false;
        isStandingNew = true;
    }

    internal void SlideNew()
    {
        GoProne();
        isSlidingNew = true;
        playerController.slideTransitionTimer = playerController.slideTransitionTimerSet;

    }

    internal void CrawlNew()
    {
        GoProne();
        isCrawlingNew = true;
        isProneIdle = false;
        isSlidingNew = false;
    }

    internal void WalkNew()
    {
        StandUp();
        isWalkingNew = true;
        isSprintingNew = false;
        isIdleNew = false;
    }

    internal void SprintNew()
    {
        StandUp();
        isSprintingNew = true;
        isWalkingNew = false;
    }

    internal void JumpNew()
    {
        if (isWallSliding || isHangingLedge)
        {
            if ((isFacingRight && playerController.playerInput.isLeftPressed) || (!isFacingRight && playerController.playerInput.isRightPressed))
            {

            }
        }

        else
        {
            playerController.hangTimeTimer = 0;

            playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, jumpForce);

            jumpBufferCount = 0;
        }
    }

    internal void WallJump(float jumpPower) /////////CHECK THIS
    {
        EnableMovement();
        playerController.hangTimeTimer = 0;

        jumpBufferCount = 0;

        if (isFacingRight)
        {
            isFacingRight = false;
            playerController.rb.velocity = new Vector2(playerController.currentSpeed, jumpPower);
        }

        else
        {
            isFacingRight = true;
            playerController.rb.velocity = new Vector2(playerController.currentSpeed, jumpPower);
        }
    }

    private void WallBounce()
    {
        if (isFacingRight)
        {
            playerController.transform.localPosition = new Vector2(transform.localPosition.x - 0.015f, transform.localPosition.y);
        }
        else
        {
            playerController.transform.localPosition = new Vector2(transform.localPosition.x + 0.015f, transform.localPosition.y);
        }
    }

    private void IdleStop()
    {
        Move();
        //Debug.Log("Stop");
        NearestPixel();
        if (isStandingNew)
        {
            isWalkingNew = false;
            isSprintingNew = false;
            isIdleNew = true;
            StandUp();
        }
        else if (isProne)
        {
            isSlidingNew = false;
            isCrawlingNew = false;
            isProneIdle = true;
            GoProne();
        }
    }


}
