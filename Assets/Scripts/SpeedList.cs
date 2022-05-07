using System.Collections;
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
                        // Debug.Log("honk");
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
                ChangeSpeed(playerController.speedList.walkSpeed);
            }
            else
            {
                // ChangeSpeed(0); -----------------------------------------------------------------------
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
}
