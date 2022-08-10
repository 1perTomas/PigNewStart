using System.Collections;
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

    internal float hangTime = 0.1f;
    internal float hangTimeTimer;

    internal float fallMultiplier = 1.2f;
    internal float riseMultiplier = 1f;

    internal bool coyoteTimeActive;
    internal float coyoteTimer;
    public float coyoteTimerWindow;


    void Update()
    {
        if (playerController.playerSurroundings.isGrounded)
        {
            jumpAntiSpam = 0;
        }
        else
        {
            jumpAntiSpam += Time.deltaTime;
        }

        // CoyoteTime();
    }

    internal void NewJump() // transfer to jump
    {

        if (playerController.playerInput.isJumpTapped && playerController.playerState.canJump)
        {
            jumpBufferCount = jumpBufferLength;
            if (playerController.playerState.canJump)
            {
                playerController.playerJump.Jump();
            }
        }

        else if (jumpBufferCount > 0)
        {
            if (playerController.playerInput.isJumpPressed && playerController.playerSurroundings.isGrounded)
            {
                playerController.playerJump.Jump();
            }
            else if (jumpBufferCount >= 0)
            {
                jumpBufferCount -= Time.deltaTime;
            }
        }


        if (playerController.rb.velocity.y > 0
        && playerController.playerInput.isJumpReleased) // if space is tapped, jump is smaller
        {
            playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, playerController.rb.velocity.y * 0.65f);
        }
    }

    internal void CoyoteTime()
    {
        if (playerController.playerState.currentState == PlayerState.CharacterMovement.Falling && coyoteTimer < coyoteTimerWindow)
        {
            coyoteTimer += Time.deltaTime;
            playerController.playerState.canJump = true;
        }

        else if (playerController.playerState.currentState == PlayerState.CharacterMovement.Jumping)
        {
            coyoteTimer = coyoteTimerWindow;
        }

        else if (playerController.playerSurroundings.isGrounded)
        {
            coyoteTimer = 0;
        }


        else if (coyoteTimer >= coyoteTimerWindow)
        {
            playerController.playerState.canJump = false;
        }
        // if (playerController.playerState.currentState == PlayerState.CharacterMovement.Falling && coyoteTimer < coyoteTimerWindow)
        // {
        //     coyoteTimer += Time.deltaTime;
        // }
        // else
        // {
        //     coyoteTimer = 0;
        // }

        StartCoroutine("Coyote");

        // internal void CheckIfCanJump()
        // {
        //
        //     if (playerController.playerState.isInteracting)
        //     {
        //         playerController.playerState.canJump = false;
        //     }
        //
        //     else if
        //
        //      (!playerController.playerMovement.isCrawling
        //      && !playerController.playerMovement.isProneIdle
        //      && !playerController.playerMovement.isWallSliding)
        //     {
        //         if (isGrounded && /*playerController.rb.velocity.y < 0 &&*/ playerController.playerTimers.hangTimeTimer != playerController.playerTimers.hangTime)
        //         {
        //             playerController.playerTimers.hangTimeTimer = playerController.playerTimers.hangTime; // can jump if presses jump a bit over the ledge
        //             playerController.playerState.canJump = true;
        //             if (playerController.playerTimers.hangTimeTimer <= 0f)
        //             {
        //
        //             }
        //         }
        //
        //         else
        //         {
        //             playerController.playerTimers.hangTimeTimer -= Time.deltaTime;
        //
        //             if (playerController.playerTimers.hangTimeTimer > 0f)
        //             {
        //                 playerController.playerState.canJump = true;
        //             }
        //             else
        //             {
        //                 playerController.playerTimers.hangTimeTimer = 0f;
        //                 playerController.playerState.canJump = false;
        //             }
        //         }
        //     }
        //     else
        //     {
        //         playerController.playerState.canJump = false;
        //     }
        // }
    }

    IEnumerator Coyote()
    {

        while (playerController.playerState.currentState == PlayerState.CharacterMovement.Falling && coyoteTimer < coyoteTimerWindow)
        {
            coyoteTimer += Time.deltaTime;
            yield return null;
        }

        if (playerController.playerSurroundings.isGrounded)
        {
            coyoteTimer = 0;
            yield break;
        }
    }

    internal void Jump()
    {
        if (jumpAntiSpam == 0 || coyoteTimer < coyoteTimerWindow)
        {
            if (playerController.playerState.currentState == PlayerState.CharacterMovement.WallJump)
            {
                //WallJump(jumpForce);
            }
            else
            {
                JumpNew(jumpForce);
            }
        }
    }

    internal void DropDown()
    {
        playerController.rb.velocity = new Vector2(playerController.speedList.wallSlideSpeed, 0);
        playerController.playerTimers.hangTimeTimer = 0;
        jumpBufferCount = 0;
    }

    internal void JumpNew(float jumpPower)
    {
        playerController.playerTimers.hangTimeTimer = 0;
        playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, jumpPower);
        jumpBufferCount = 0;
    }

    internal void JumpTest()
    {
        Debug.Log("bababooey");
        playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, jumpForce);
    }

    internal void WallJump() /////////CHECK THIS
    {
        playerController.rb.velocity = new Vector2(playerController.speedList.walkSpeed, jumpForce);
        playerController.playerTimers.hangTimeTimer = 0;
        jumpBufferCount = 0;
    }

    internal void FallGravity()
    {

        if (playerController.playerState.currentState == PlayerState.CharacterMovement.Interacting)
        {
            jumpForce = 6;
        }
        else
        {
            jumpForce = 6.6f;
        }

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
