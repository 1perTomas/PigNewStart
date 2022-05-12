﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedList : MonoBehaviour
{

    [SerializeField]
    PlayerController playerController;

    public float walkSpeed = 3.5f;
    public float runningSpeed = 6f;
    public float crawlingSpeed = 2f;
    public float slidingSpeed = 4.5f;
    public float wallSlideSpeed = 0.45f;

    public float turningRateAir = 0.1f;
    public float acceleration = 0.15f;

    internal void FlipSpeedValues()
    {
        walkSpeed *= -1;
        runningSpeed *= -1;
        crawlingSpeed *= -1;
        slidingSpeed *= -1;
    }

    internal void SpeedSet()
    {
        if (playerController.playerSurroundings.isGrounded)
        {
            if ((playerController.playerSurroundings.isTouchingWall || playerController.playerSurroundings.isTouchingLedge) && playerController.playerMovement.isStandingNew)
            {

                ChangeSpeed(0);
            }

            else
            {
                if (playerController.playerMovement.isIdleNew || playerController.playerMovement.isProneIdle)
                {
                    if (playerController.playerSurroundings.isTouchingWall)
                    {
                        ChangeSpeed(0);
                    }
                    else
                    {
                        ChangeSpeedNew(0);
                    }
                }

                else if (playerController.playerMovement.isWalkingNew)
                {
                    if (playerController.playerMovement.isFacingRight)
                    {
                        if (playerController.currentSpeed < playerController.speedList.walkSpeed)
                        {

                            ChangeSpeedNew(playerController.speedList.walkSpeed);
                        }


                        else
                        {
                            ChangeSpeedNew(playerController.speedList.walkSpeed);
                        }
                    }
                    else
                    {
                        if (playerController.currentSpeed > playerController.speedList.walkSpeed)
                        {

                            ChangeSpeedNew(playerController.speedList.walkSpeed);
                        }

                        else
                        {
                            ChangeSpeedNew(playerController.speedList.walkSpeed);
                        }
                    }
                }

                else if (playerController.playerMovement.isSprintingNew)
                {
                    ChangeSpeedNew(playerController.speedList.runningSpeed);
                }

                else if (playerController.playerMovement.isCrawlingNew)
                {
                    if (playerController.currentSpeed == 0)
                    {
                        ChangeSpeed(playerController.speedList.crawlingSpeed);
                    }

                    else
                    {
                        ChangeSpeedNew(playerController.speedList.crawlingSpeed);
                    }
                }

                else if (playerController.playerMovement.isSlidingNew)
                {
                    ChangeSpeedNew(playerController.speedList.slidingSpeed);
                }
            }
        }

        else if (playerController.playerMovement.isWallSliding || playerController.playerMovement.isHangingLedge)
        {
            if (playerController.playerMovement.wallJumpReady)
            {
                ChangeSpeed(walkSpeed);
            }
            else if ((playerController.playerMovement.isFacingRight && playerController.playerInput.isRightPressed)
                || (!playerController.playerMovement.isFacingRight && playerController.playerInput.isLeftPressed))
            {
                ChangeSpeed(walkSpeed);
            }
            else
            {
                playerController.speedList.ChangeSpeed(0);
            }
        }

        else if (!playerController.playerSurroundings.isGrounded)
        {
           //if (playerController.playerMovement.wallJumpReady)  //Test
           //{                                                   //Test
           //   // ChangeSpeed(walkSpeed);                         //Test
           //}                                                   //Test

            if (playerController.playerInput.isLeftPressed || playerController.playerInput.isRightPressed)
            {
                if (playerController.playerSurroundings.isTouchingWall
                    || (playerController.currentSpeed < 0 && playerController.playerMovement.isFacingRight && playerController.playerSurroundings.isTouchingWallBehind)
                    || (playerController.currentSpeed > 0 && !playerController.playerMovement.isFacingRight && playerController.playerSurroundings.isTouchingWallBehind))
                {
                    playerController.speedList.ChangeSpeed(0);
                }

                else if(playerController.currentSpeed == 0 && playerController.playerSurroundings.isTouchingWallBehind)
                  
                    {
                    ChangeSpeedNew(walkSpeed);
                }

                else if (playerController.playerInput.isSprintPressed && (playerController.currentSpeed > slidingSpeed || playerController.currentSpeed < slidingSpeed * -1))
                {
                    if (playerController.playerSurroundings.isTouchingWall)
                    {
                        //   playerController.speedList.ChangeSpeed(0);
                    }
                    else


                    {
                        if (playerController.rb.velocity.x > 0 && playerController.playerInput.isLeftPressed)
                        {
                            playerController.speedList.ChangeSpeedNew(walkSpeed);
                        }

                        else if (playerController.rb.velocity.x < 0 && playerController.playerInput.isRightPressed)
                        {
                            playerController.speedList.ChangeSpeedNew(walkSpeed);
                        }
                    }
                }
                else
                {
                    playerController.speedList.ChangeSpeedNew(walkSpeed);
                }
            }

            else
            {
                playerController.speedList.ChangeSpeedNew(0);
            }
        }

        else
        {
            if (!playerController.playerMovement.isMoving)
            {
                ChangeSpeedNew(0);
            }
        }
    }

    internal void ChangeSpeed(float newSpeed)
    {
        playerController.currentSpeed = newSpeed;
    }

    internal void ChangeSpeedNew(float newSpeed)
    {

        if (playerController.currentSpeed != newSpeed)
        {

            if (playerController.currentSpeed < newSpeed)
            {
                if (playerController.playerSurroundings.isGrounded)
                {
                    playerController.currentSpeed += (acceleration);
                }

                else
                {
                    playerController.currentSpeed += (turningRateAir);
                }

                if (playerController.currentSpeed > newSpeed)
                {
                    playerController.currentSpeed = newSpeed;
                }
            }

            if (playerController.currentSpeed > newSpeed)
            {
                if (playerController.playerSurroundings.isGrounded)
                {
                    playerController.currentSpeed -= (acceleration);
                }

                else
                {
                    playerController.currentSpeed -= (turningRateAir);
                }

                if (playerController.currentSpeed < newSpeed)
                {
                    playerController.currentSpeed = newSpeed;
                }
            }
        }


    }
}