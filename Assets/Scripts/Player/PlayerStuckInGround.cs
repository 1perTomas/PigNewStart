using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStuckInGround : MonoBehaviour
{
    [SerializeField]
    internal PlayerController playerController;

    //internal bool isStuckInGround;
    internal float fallTimer;
    internal float stuckTimer = 0.6f;

    internal int wiggleCount;
    internal float wiggleTimer;
    internal bool wiggleLeft = false;
    internal bool wiggleRight = false;

    internal void AirTime()
    {
        if ((playerController.playerSurroundings.isGrounded && fallTimer < stuckTimer)
     || playerController.playerMovement.isWallSliding || playerController.playerMovement.isHangingLedge)
        {
            fallTimer = 0;
        }

        else if (playerController.rb.velocity.y < 0)
        {
            fallTimer += Time.deltaTime;
        }

        if (fallTimer > stuckTimer && playerController.playerSurroundings.isGrounded)
        {
            SlamIntoGround();
        }


    }

    internal void SlamIntoGround()
    {
        if (fallTimer > stuckTimer)
        {
            fallTimer = 0;
            playerController.speedList.currentSpeed = 0;
            //playerController.rb.velocity = new Vector2(0f, 0f);
            playerController.playerState.isStuckInGround = true;
        }
    }

    internal void GetUnstuck()
    {
        wiggleCount = 0;
        fallTimer = 0;
        playerController.playerState.isStuckInGround = false;
    }

    internal void WiggleWiggle()
    {
        if (playerController.playerInput.isLeftTapped)
        {
            if (!wiggleLeft)
            {
                Wiggle();
                wiggleRight = false;
                wiggleLeft = true;
            }
        }

        else if (playerController.playerInput.isRightTapped)
        {
            if (!wiggleRight)
            {
                Wiggle();
                wiggleRight = true;
                wiggleLeft = false;
            }
        }

        else
        {
            if (wiggleTimer < 0.6f)
            {
                wiggleTimer += Time.deltaTime;
            }
        }

        if (wiggleCount >= 12)
        {
            GetUnstuck();
            
        }

        if (wiggleTimer > 0.5f)
        {
            ResetWiggle();
        }
    }

    internal void Wiggle()
    {
        wiggleCount += 1;
        wiggleTimer = 0;
    }

    internal void ResetWiggle()
    {
        wiggleRight = false;
        wiggleLeft = false;
        wiggleCount = 0;
    }




}
