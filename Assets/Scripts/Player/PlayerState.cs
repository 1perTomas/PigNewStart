using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;

    public enum CharacterMovement
    {
        Idle,
        Walking,
        Sprinting,
        Sliding,
        Prone,
        Crawling,
        HangingLedge,
        Wallsliding,
        WallJump,
        Climbing,
        Jumping,
        Falling
    };

    public enum CharacterState
    {
        FreeMovement,
        Interaction,
        DamageRecoil,
        Dialogue
    };

    internal CharacterMovement currentState;

    internal bool isFacingRight = true;
    internal bool isMoving;
    internal bool isInteracting = false;
    internal bool isStuckInGround = false;
    internal bool isDamaged = false;

      internal float damagedFall;

    internal bool canJump;
    internal int state;


    internal int health = 3;

    internal void TakeDamage()
    {
        
        if (health > 0)
        {
            health -= 1;
            Debug.Log($"you took damage, your health is {health}");
        }
    }

    internal void GetHit()
    {
        if (playerController.playerSurroundings.isTouchingWall)
        {
            isDamaged = true;
            playerController.rb.velocity = new Vector2(-20f, 3f);
            TakeDamage();
        }
    }

    internal void SetState()
    {
        if (isInteracting)
        {
            state = 1;
        }

        else if (isStuckInGround)
        {
            state = 2;
        }

        else
        {
            state = 3;
        }

    }
}
