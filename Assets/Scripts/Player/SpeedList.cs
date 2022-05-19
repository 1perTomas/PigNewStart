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

    public float turningRateAir = 0.08f;
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
        // if (playerController.playerMovement.isInteracting)
        // {
        //     ChangeSpeedNew(2);
        // }

        // different acceleration for ground and air
        if (playerController.playerSurroundings.isGrounded) // on ground
        {
            if (playerController.playerMovement.isStandingNew) //standing
            {
                if ((playerController.playerSurroundings.isTouchingWall || playerController.playerSurroundings.isTouchingLedge) && playerController.currentSpeed != 0) // stops when hitting wall
                {
                    ChangeSpeed(0);
                }

                if (!playerController.playerMovement.isMoving) // slows to stop when nothing is pressed
                {
                    ChangeSpeedNew(0);
                }

                if (playerController.playerMovement.isWalkingNew)
                {
                    ChangeSpeedNew(walkSpeed);
                }

                if (playerController.playerMovement.isSprintingNew)
                {
                    ChangeSpeedNew(runningSpeed);
                }

            }

            else // prone
            {
                if (playerController.playerSurroundings.isTouchingWall && playerController.currentSpeed != 0) // stops when hitting wall
                {
                    ChangeSpeed(0);
                }

                if (!playerController.playerMovement.isMoving) // slows to stop when nothing is pressed
                {
                    ChangeSpeedNew(0);
                }

                if (playerController.playerMovement.isCrawlingNew) // crawl speed
                {
                    ChangeSpeedNew(crawlingSpeed);
                }

                if (playerController.playerMovement.isSlidingNew) // slide speed
                {
                    ChangeSpeedNew(slidingSpeed);
                }
            }
        }

        else if (playerController.playerMovement.isWallSliding)
        {

            ChangeSpeed(0);
            //canTurn = false;

            if (playerController.playerMovement.wallJumpReady)
            {
                playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, -0.25f);
            }

            if (playerController.rb.velocity.y < -playerController.speedList.wallSlideSpeed)
            {
                playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, -playerController.speedList.wallSlideSpeed);
            }

            if (playerController.rb.velocity.y > playerController.speedList.wallSlideSpeed)
            {
                playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, -playerController.speedList.wallSlideSpeed);
            }

        }

        else // in air
        {
            if (!playerController.playerMovement.isMoving) // slows to stop when nothing is pressed
            {
                turningRateAir = 0.04f;
                ChangeSpeedNew(0);
            }

            else
            {
                turningRateAir = 0.08f;
            }

            if (playerController.playerMovement.isMoving && playerController.currentSpeed != runningSpeed) // speed in air 
            {
                ChangeSpeedNew(walkSpeed);
            }

            

            if (playerController.playerMovement.wallJumpReady)
            {
                ChangeSpeed(walkSpeed);
            }

            else if (playerController.playerMovement.isWallSliding || playerController.playerMovement.isHangingLedge)
            {
                ChangeSpeed(0);
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
