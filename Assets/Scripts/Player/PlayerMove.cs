using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;

    internal void PriorityDirectionLeft()
    {
        if (playerController.playerMovement.canTurn)
        {
            if (playerController.playerInput.isLeftPressed)
            {
                playerController.playerMovement.isMoving = true;
                playerController.playerMovement.isFacingRight = false;
                Move();
            }

            //---------------------------------------------------------------------------------Input Right
            else if (playerController.playerInput.isRightPressed)
            {
                playerController.playerMovement.isMoving = true;
                playerController.playerMovement.isFacingRight = true;
                Move();
            }

            else
            {
                playerController.playerMovement.isMoving = false;
                playerController.playerMovement.IdleStop();
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
                playerController.playerMovement.isMoving = true;
                playerController.playerMovement.isFacingRight = true;
                Move();
            }

            //---------------------------------------------------------------------------------Input Right
            else if (playerController.playerInput.isLeftPressed)
            {
                playerController.playerMovement.isMoving = true;
                playerController.playerMovement.isFacingRight = false;
                Move();
            }

            else
            {
                playerController.playerMovement.isMoving = false;
                playerController.playerMovement.IdleStop();
            }
        }
    }

    internal void Move()
    {
        if ((playerController.playerMovement.isFacingRight && playerController.speedList.walkSpeed < 0)
            || (!playerController.playerMovement.isFacingRight && playerController.speedList.walkSpeed > 0))
        {
            playerController.speedList.FlipSpeedValues();
        }

        playerController.rb.velocity = new Vector2(playerController.speedList.currentSpeed, playerController.rb.velocity.y);
    }
}
