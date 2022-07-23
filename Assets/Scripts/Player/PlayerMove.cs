using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;

    internal bool leftPriority;

    internal void PriorityDirectionLeft()
    {
        if (playerController.playerMovement.canTurn)
        {
            if (playerController.playerInput.isLeftPressed)
            {
                playerController.playerState.isMoving = true;
                playerController.playerState.isFacingRight = false;
            }

            //---------------------------------------------------------------------------------Input Right
            else if (playerController.playerInput.isRightPressed)
            {
                playerController.playerState.isMoving = true;
                playerController.playerState.isFacingRight = true;
            }

            else
            {
                playerController.playerState.isMoving = false;
                //playerController.playerMovement.IdleStop();
            }
        }
    }

    internal void PriorityDirectionRight()
    {
        if (playerController.playerMovement.canTurn)
        {
            if (playerController.playerInput.isRightPressed)
            //-----------------------------------------------------------------------------------Input Left
            {
                playerController.playerState.isMoving = true;
                playerController.playerState.isFacingRight = true;
            }

            //---------------------------------------------------------------------------------Input Right
            else if (playerController.playerInput.isLeftPressed)
            {
                playerController.playerState.isMoving = true;
                playerController.playerState.isFacingRight = false;
            }

            else
            {
                playerController.playerState.isMoving = false;
                //playerController.playerMovement.IdleStop();
            }
        }
    }

    internal void Move()
    {
        if (playerController.playerMovement.canMove)
        {
            playerController.rb.velocity = new Vector2(playerController.speedList.currentSpeed, playerController.rb.velocity.y);
        }
    }

    internal void MoveDetection() // checks the direction that is pressed
    {
        if ((playerController.playerState.isFacingRight && playerController.speedList.walkSpeed < 0)
          || (!playerController.playerState.isFacingRight && playerController.speedList.walkSpeed > 0))
        {
            playerController.speedList.FlipSpeedValues();
        }

        if (playerController.playerInput.isLeftTapped)
        {
            leftPriority = true;
        }

        else if (playerController.playerInput.isRightTapped)
        {
            leftPriority = false;
        }

        if (leftPriority)
        {
            playerController.playerMove.PriorityDirectionLeft();
        }

        else
        {
            playerController.playerMove.PriorityDirectionRight();
        }
    }
}
