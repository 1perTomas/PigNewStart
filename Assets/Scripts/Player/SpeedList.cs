using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedList : MonoBehaviour
{

    [SerializeField]
    PlayerController playerController;

    internal float currentSpeed;

    public float walkSpeed = 3.5f;
    public float runningSpeed = 6f;
    public float crawlingSpeed = 2f;
    public float slidingSpeed = 4.5f;
    public float wallSlideSpeed = 0.45f;

    internal float heavyObjectSpeed = 1.25f;
    internal float lightObjectSpeed = 1.75f;
    internal float carryingLightObjectSpeed = 2.75f;

    public float turningRateAir = 0.08f;
    public float acceleration = 0.18f;

    internal void FlipSpeedValues()
    {
        walkSpeed *= -1;
        runningSpeed *= -1;
        crawlingSpeed *= -1;
        slidingSpeed *= -1;
        heavyObjectSpeed *= -1;
        lightObjectSpeed *= -1;
        carryingLightObjectSpeed *= -1;
    }


    internal void SpeedAdjust()
    {
        // if (playerController.playerSurroundings.isTouchingWall
        // || (playerController.playerSurroundings.isTouchingLedge && playerController.playerMovement.isStanding))
        // {
        //     ChangeSpeed(0);
        // }

        switch (playerController.playerState.controlMode)
        {
            case PlayerState.ControlMode.FreeMovement:

                switch (playerController.playerState.currentState)
                {
                    case PlayerState.CharacterMovement.Idle:
                        if (playerController.playerSurroundings.isTouchingWall
               || playerController.playerSurroundings.isTouchingLedge)
                        {
                            ChangeSpeed(0);
                        }
                        else
                        {
                            ChangeSpeedNew(0);
                        }
                        break;

                    case PlayerState.CharacterMovement.Walking:
                        ChangeSpeedNew(walkSpeed);
                        break;

                    case PlayerState.CharacterMovement.Sprinting:
                        ChangeSpeedNew(runningSpeed);
                        break;

                    case PlayerState.CharacterMovement.Prone:
                        ChangeSpeedNew(0);
                        break;

                    case PlayerState.CharacterMovement.Crawling:
                        ChangeSpeedNew(crawlingSpeed);
                        break;

                    case PlayerState.CharacterMovement.Sliding:
                        ChangeSpeedNew(slidingSpeed);
                        break;

                    case PlayerState.CharacterMovement.Jumping:

                        if (playerController.playerSurroundings.isTouchingWallBehind
                        && ((playerController.playerState.isFacingRight && currentSpeed < 0) || (!playerController.playerState.isFacingRight && currentSpeed > 0))
                        && playerController.playerTimers.wallJumpTimer == 0)
                        {
                            ChangeSpeed(0);
                        }
                        else
                        {

                            if (!playerController.playerState.isMoving) // slows to stop when nothing is pressed
                            {
                                turningRateAir = 0.04f;
                                ChangeSpeedNew(0);
                            }
                            else
                            {
                                if (playerController.playerState.isMoving && currentSpeed != runningSpeed && currentSpeed != slidingSpeed) // speed in air 
                                {
                                    turningRateAir = 0.08f;
                                    ChangeSpeedNew(walkSpeed);
                                }
                            }
                        }
                        break;

                    case PlayerState.CharacterMovement.Falling:

                        // if (playerController.playerSurroundings.isTouchingWallBehind && currentSpeed < 0)
                        // {
                        //     ChangeSpeed(0);
                        // }

                        if (!playerController.playerState.isMoving) // slows to stop when nothing is pressed
                        {
                            turningRateAir = 0.04f;
                            ChangeSpeedNew(0);
                        }
                        else
                        {
                            if (playerController.playerState.isMoving && currentSpeed != runningSpeed && currentSpeed != slidingSpeed) // speed in air 
                            {
                                turningRateAir = 0.08f;
                                ChangeSpeedNew(walkSpeed);
                            }
                        }
                        break;

                    case PlayerState.CharacterMovement.Wallsliding:
                        ChangeSpeed(0);

                        if (playerController.rb.velocity.y < -playerController.speedList.wallSlideSpeed)
                        {
                            playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, -playerController.speedList.wallSlideSpeed);
                        }

                        if (playerController.rb.velocity.y > playerController.speedList.wallSlideSpeed)
                        {
                            playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, -playerController.speedList.wallSlideSpeed);
                        }

                        break;

                    case PlayerState.CharacterMovement.HangingLedge:
                        ChangeSpeed(0); //if is !=0, then climbing spot is moving
                        break;

                    case PlayerState.CharacterMovement.WallJump:
                        if (!playerController.playerSurroundings.isTouchingWall)
                        {
                            ChangeSpeed(0);
                        }
                        else
                        {
                            ChangeSpeed(walkSpeed); // doesn't have to build up speed when wall jumping
                        }
                        playerController.rb.velocity = new Vector2(0, -0.25f); //slow slide down

                        break;

                    default:
                        break;

                }

                break;

            case PlayerState.ControlMode.Interaction:

                if (playerController.playerState.isMoving)
                {
                    if (playerController.playerDetectObject.objectType == "Carriable")
                    {
                        if (playerController.playerInteraction.isCarrying)
                        {
                            ChangeSpeed(carryingLightObjectSpeed);
                        }
                        else
                        {

                            if ((playerController.playerState.isFacingRight && walkSpeed > 0) || (!playerController.playerState.isFacingRight && walkSpeed < 0))
                            {
                                ChangeSpeed(lightObjectSpeed);
                            }

                            else
                            {
                                ChangeSpeed(heavyObjectSpeed);
                            }
                        }
                    }

                    else
                    {
                        ChangeSpeed(heavyObjectSpeed);
                    }
                }
                else
                {
                    ChangeSpeed(0);
                }

                break;
        }


    }


    internal void SpeedSet()
    {
        if (playerController.playerSurroundings.isGrounded) // on ground
        {
            if (playerController.playerState.isInteracting)
            {
                if (playerController.playerState.isMoving)
                {
                    if (playerController.playerDetectObject.objectType == "Carriable")
                    {
                        if (playerController.playerInteraction.isCarrying)
                        {
                            ChangeSpeed(walkSpeed);
                        }
                        else
                        {

                            if ((playerController.playerState.isFacingRight && walkSpeed > 0) || (!playerController.playerState.isFacingRight && walkSpeed < 0))
                            {
                                ChangeSpeed(lightObjectSpeed);
                            }

                            else
                            {
                                ChangeSpeed(heavyObjectSpeed);
                            }
                        }
                    }

                    else
                    {
                        ChangeSpeed(heavyObjectSpeed);
                    }
                }
                else
                {
                    ChangeSpeed(0);
                }
            }

            else if (playerController.playerState.isStanding) //standing
            {
                if ((playerController.playerSurroundings.isTouchingWall || playerController.playerSurroundings.isTouchingLedge) && playerController.speedList.currentSpeed != 0) // stops when hitting wall
                {
                    ChangeSpeed(0);
                }

                if (!playerController.playerState.isMoving) // slows to stop when nothing is pressed
                {
                    ChangeSpeedNew(0);
                }

                //  if (playerController.playerMovement.isWalking)
                //  {
                //      ChangeSpeedNew(walkSpeed);
                //  }

                // if (playerController.playerMovement.isSprinting)
                // {
                //     ChangeSpeedNew(runningSpeed);
                // }
            }

            else // prone
            {
                if (playerController.playerSurroundings.isTouchingWall && currentSpeed != 0) // stops when hitting wall
                {
                    ChangeSpeed(0);
                }

                if (!playerController.playerState.isMoving) // slows to stop when nothing is pressed
                {
                    ChangeSpeedNew(0);
                }

                // if (playerController.playerMovement.isCrawling) // crawl speed
                // {
                //     ChangeSpeedNew(crawlingSpeed);
                // }
                //
                // if (playerController.playerMovement.isSliding) // slide speed
                // {
                //     ChangeSpeedNew(slidingSpeed);
                // }
            }
        }

        // else if (playerController.playerMovement.isWallSliding)
        // {
        //     ChangeSpeed(0);
        //
        //     if (playerController.playerMovement.wallJumpReady)
        //     {
        //         playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, -0.25f);
        //     }
        //
        //     if (playerController.rb.velocity.y < -playerController.speedList.wallSlideSpeed)
        //     {
        //         playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, -playerController.speedList.wallSlideSpeed);
        //     }
        //
        //     if (playerController.rb.velocity.y > playerController.speedList.wallSlideSpeed)
        //     {
        //         playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, -playerController.speedList.wallSlideSpeed);
        //     }
        //
        // }

        else // in air
        {
            //  if (!playerController.playerState.isMoving) // slows to stop when nothing is pressed
            //  {
            //      turningRateAir = 0.04f;
            //      ChangeSpeedNew(0);
            //  }
            //
            //  else
            //  {
            //      turningRateAir = 0.08f;
            //  }
            //
            //  if (playerController.playerState.isMoving && currentSpeed != runningSpeed && currentSpeed != slidingSpeed) // speed in air 
            //  {
            //      ChangeSpeedNew(walkSpeed);
            //  }
            //
            //  if (playerController.playerMovement.wallJumpReady)
            //  {
            //      ChangeSpeed(walkSpeed);
            //  }
            //
            //  else if (playerController.playerMovement.isWallSliding || playerController.playerMovement.isHangingLedge)
            //  {
            //      ChangeSpeed(0);
            //  }
        }
    }

    internal void ChangeSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
    }

    internal void ChangeSpeedNew(float newSpeed)
    {
        if (currentSpeed != newSpeed)
        {
            if (currentSpeed < newSpeed)
            {
                if (playerController.playerSurroundings.isGrounded)
                {
                    currentSpeed += (acceleration);
                }

                else
                {
                    currentSpeed += (turningRateAir);
                }

                if (currentSpeed > newSpeed)
                {
                    currentSpeed = newSpeed;
                }
            }

            if (currentSpeed > newSpeed)
            {
                if (playerController.playerSurroundings.isGrounded)
                {
                    currentSpeed -= (acceleration);
                }

                else
                {
                    currentSpeed -= (turningRateAir);
                }

                if (currentSpeed < newSpeed)
                {
                    currentSpeed = newSpeed;
                }
            }
        }
    }
}
