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
        Falling,
        KnockBack,
        Interacting
    };

    public enum ControlMode
    {
        FreeMovement,
        Interaction,
        Damaged,
        Dialogue
    };

    internal CharacterMovement currentState;
    internal ControlMode controlMode;

    internal bool isFacingRight = true;
    internal bool isStanding; //collider size
    internal bool isMoving;
    internal bool isInteracting = false;
    internal bool isStuckInGround = false;
    internal bool isDamaged = false;
    public bool invulnerability;

    internal float damagedFall;

    internal bool canJump;
    


    internal int health = 3;


    internal void Invincibility()
    {
        if (invulnerability)
        {
            playerController.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }

    }

}
