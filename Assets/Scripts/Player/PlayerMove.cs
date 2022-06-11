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
                //Move();
            }

            //---------------------------------------------------------------------------------Input Right
            else if (playerController.playerInput.isRightPressed)
            {
                playerController.playerState.isMoving = true;
                playerController.playerState.isFacingRight = true;
                //Move();
            }

            else
            {
                playerController.playerState.isMoving = false;
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

              //  Move();
            }

            //---------------------------------------------------------------------------------Input Right
            else if (playerController.playerInput.isLeftPressed)
            {
                playerController.playerState.isMoving = true;
                playerController.playerState.isFacingRight = false;
               // Move();
            }

            else
            {
                playerController.playerState.isMoving = false;
                playerController.playerMovement.IdleStop();
            }
        }
    }

    internal void Move()
    {
       // if ((playerController.playerState.isFacingRight && playerController.speedList.walkSpeed < 0)
       //     || (!playerController.playerState.isFacingRight && playerController.speedList.walkSpeed > 0))
       // {
       //     playerController.speedList.FlipSpeedValues();
       // }

       //if (playerController.playerState.isMoving)
       //{
            playerController.rb.velocity = new Vector2(playerController.speedList.currentSpeed, playerController.rb.velocity.y);
        //}

            }
}
