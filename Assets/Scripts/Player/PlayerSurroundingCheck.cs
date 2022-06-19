using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSurroundingCheck : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;



    internal bool isColliding;

    public Transform GroundCheck;
    public Transform wallCheck;
    public Transform wallSlideCheck;
    public Transform CeilingCheck;
    public Transform CeilingCheckCrawling;
    public Transform LedgeCheck;
    public Transform ClimbCheck;


    public float groundCheckRadius;
    public float wallCheckDistance;
    public float CeilingCheckRadius;
    public float facingDirection;

    internal bool touchingGround;

    internal bool isGroundedLeft;
    internal bool isGroundedMiddle;
    internal bool isGroundedRight;



    internal bool isGrounded;
    internal bool isTouchingWall;
    internal bool isTouchingWallHigh;
    internal bool isTouchingLedge;
    internal bool isTouchingLedgeJump;
    internal bool isTouchingCeiling;
    internal bool isTouchingCeilingProne;
    internal bool isTouchingRectangleFloor;
    internal bool isAbleToClimb;

    internal bool canInteract;

    internal bool isTouchingWallBehind;


    internal GameObject interactableObject;
    internal RaycastHit2D isTouchingInteractableObject;
    internal string objectType;

    //internal bool canJump;
    internal bool canHangLedge;

    internal bool isOnPlatform;

    public LayerMask whatIsGround;


    internal void CheckSurroundings()
    {
        CheckLayerSurroundings(whatIsGround);
        CheckIfCanHangLedge();
        CheckIfCanJump();



        // AirGlitch();
    }
    internal void CheckIfCanJump()
    {

        if (playerController.playerState.isInteracting)
        {
            playerController.playerState.canJump = false;
        }

        else if

         (!playerController.playerMovement.isCrawling
         && !playerController.playerMovement.isProneIdle
         && !playerController.playerMovement.isWallSliding)
        {
            if (isGrounded && /*playerController.rb.velocity.y < 0 &&*/ playerController.playerTimers.hangTimeTimer != playerController.playerTimers.hangTime)
            {
                playerController.playerTimers.hangTimeTimer = playerController.playerTimers.hangTime; // can jump if presses jump a bit over the ledge
                playerController.playerState.canJump = true;
                if (playerController.playerTimers.hangTimeTimer <= 0f)
                {

                }
            }

            else
            {
                playerController.playerTimers.hangTimeTimer -= Time.deltaTime;

                if (playerController.playerTimers.hangTimeTimer > 0f)
                {
                    playerController.playerState.canJump = true;
                }
                else
                {
                    playerController.playerTimers.hangTimeTimer = 0f;
                    playerController.playerState.canJump = false;
                }
            }
        }
        else
        {
            playerController.playerState.canJump = false;
        }
    }

    internal void CheckIfCanHangLedge()
    {
        if (!isTouchingWall
            ||
            isGrounded)
        {
            canHangLedge = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {
        isColliding = true;
        //if(Collision.gameObject.CompareTag("Ground"))
        //{
        //    touchingGround = true;
        //}

        if (Collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D Collision)
    {
        isColliding = false;
        Debug.Log("Leave");

        if (Collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = false;
        }

        // if (Collision.gameObject.CompareTag("Ground"))
        // {
        //     touchingGround = false;
        // }
    }

    internal void CanGrabObject(LayerMask layer)
    {
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, layer);
    }

    private void CheckLayerSurroundings(LayerMask layer)
    {
        if (playerController.playerMovement.isProne)
        {
            isTouchingCeilingProne = Physics2D.OverlapCircle(CeilingCheckCrawling.position, CeilingCheckRadius, layer);
        }

        else
        {
            isTouchingCeilingProne = false;
        }

        if (playerController.playerState.isInteracting)
        {
            if (playerController.playerState.isFacingRight)
            {
                DetectSurroundings(1, layer);
            }

            else
            {
                DetectSurroundings(-1, layer);
            }
        }
        else
        {
            if (playerController.playerState.isFacingRight)
            {
                DetectSurroundings(1, layer);
            }

            else
            {
                DetectSurroundings(-1, layer);
            }
        }
    }





    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(GroundCheck.position, new Vector3(GroundCheck.position.x, GroundCheck.position.y + 0.1f));

        //Gizmos.DrawWireSphere(GroundCheck.position, groundCheckRadius;
        // Gizmos.DrawLine(GroundCheck.position, new Vector3(GroundCheck.position.x + 0.28f, GroundCheck.position.y, GroundCheck.position.z));

        Gizmos.DrawWireSphere(CeilingCheck.position, CeilingCheckRadius);
        Gizmos.DrawWireSphere(CeilingCheckCrawling.position, CeilingCheckRadius);



        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));

        Gizmos.DrawLine(LedgeCheck.position, new Vector3(LedgeCheck.position.x + wallCheckDistance, LedgeCheck.position.y, LedgeCheck.position.z));
    }

    private void DetectSurroundings(int direction, LayerMask layer)
    {
        isAbleToClimb = Physics2D.Raycast(ClimbCheck.position, transform.right, wallCheckDistance * direction, layer);
        isGrounded = Physics2D.OverlapArea(new Vector2(GroundCheck.position.x - 0.1f, GroundCheck.position.y), new Vector2(GroundCheck.position.x + 0.1f, GroundCheck.position.y + 0.1f), layer);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance * direction, layer);
        isTouchingWallBehind = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance * -direction, layer);
        isTouchingWallHigh = Physics2D.Raycast(wallSlideCheck.position, transform.right, wallCheckDistance * direction, layer);
        isTouchingLedge = Physics2D.Raycast(LedgeCheck.position, transform.right, wallCheckDistance * direction, layer);
        isTouchingLedgeJump = Physics2D.Raycast(LedgeCheck.position, transform.right, wallCheckDistance * direction, layer);
        isTouchingCeiling = Physics2D.OverlapCircle(CeilingCheck.position, CeilingCheckRadius, layer);
        isTouchingInteractableObject = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance * direction, layer);
    }


    private void AirGlitch()
    {
        if (isColliding
            && !isGrounded
            && !playerController.playerMovement.isWallSliding
            && !playerController.playerMovement.isHangingLedge
            && playerController.playerState.canJump)
        {
            playerController.transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - 0.01f);
        }
    }
}