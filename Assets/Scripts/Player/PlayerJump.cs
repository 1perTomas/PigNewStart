﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;

    public float jumpForce = 6.6f;
    internal float jumpAntiSpam;

    internal float jumpBufferCount;
    internal float jumpBufferLength = 0.2f;

    internal float fallMultiplier = 1.2f;
    internal float riseMultiplier = 1f;

    void Update()
    {
        if (playerController.playerSurroundings.isGrounded || playerController.playerMovement.isHangingLedge || playerController.playerMovement.isWallSliding)
        {
            jumpAntiSpam = 0;
        }

        else
        {
            jumpAntiSpam += Time.deltaTime;
        }
    }

    internal void Jump()
    {
        if (jumpAntiSpam == 0)
        {
            if (playerController.playerMovement.wallJumpReady)
            {
                WallJump(jumpForce);
            }

            else
            {
                JumpNew(jumpForce);
            }
        }
    }

    internal void DropDown()
    {
        WallJump(0);
    }

    internal void JumpNew(float jumpPower)
    {
        playerController.playerTimers.hangTimeTimer = 0;
        playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, jumpPower);
        jumpBufferCount = 0;
    }

    internal void WallJump(float jumpPower) /////////CHECK THIS
    {
        playerController.playerMovement.isWallSliding = false;
        playerController.playerMovement.isHangingLedge = false;
        playerController.playerTimers.hangTimeTimer = 0;
        jumpBufferCount = 0;

        if (playerController.playerState.isFacingRight)
        {
            playerController.playerState.isFacingRight = false;
            playerController.rb.velocity = new Vector2(playerController.speedList.currentSpeed, jumpPower);
        }

        else
        {
            playerController.playerState.isFacingRight = true;
            playerController.rb.velocity = new Vector2(playerController.speedList.currentSpeed, jumpPower);
        }
    }

    internal void FallGravity()
    {
        if (playerController.rb.velocity.y < 0)
        {
            playerController.rb.velocity += Vector2.up * Physics2D.gravity.y * Time.deltaTime * fallMultiplier;
        }
        else if (playerController.rb.velocity.y < 5)
        {
            playerController.rb.velocity += Vector2.up * Physics2D.gravity.y * Time.deltaTime * riseMultiplier;
        }
    }
}
