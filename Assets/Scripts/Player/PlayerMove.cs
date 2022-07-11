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
                //playerController.playerState.isMoving = false;
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
                //playerController.playerState.isMoving = false;
                playerController.playerMovement.IdleStop();
            }
        }
    }

    internal void Move()
    {
        playerController.rb.velocity = new Vector2(playerController.speedList.currentSpeed, playerController.rb.velocity.y);
    }
}
