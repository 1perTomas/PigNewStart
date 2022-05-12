using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectionPriority : MonoBehaviour
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
                if (playerController.playerMovement.isFacingRight)
                {
                    playerController.speedList.FlipSpeedValues();

                    if (playerController.playerMovement.isSlidingNew) //if changes direction when sliding, stops sliding, crawls other direction
                    {
                        playerController.playerMovement.CrawlNew();
                    }
                }
                playerController.playerMovement.isFacingRight = false;
                playerController.playerMovement.Move();
            }

            //---------------------------------------------------------------------------------Input Right
            else if (playerController.playerInput.isRightPressed)
            {
                playerController.playerMovement.isMoving = true;

                if (!playerController.playerMovement.isFacingRight)
                {
                    playerController.speedList.FlipSpeedValues();

                }
                playerController.playerMovement.isFacingRight = true;
                playerController.playerMovement.Move();
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
                if (!playerController.playerMovement.isFacingRight)
                {
                    playerController.speedList.FlipSpeedValues();

                    if (playerController.playerMovement.isSlidingNew) //if changes direction when sliding, stops sliding, crawls other direction
                    {
                        playerController.playerMovement.CrawlNew();
                    }
                }
                playerController.playerMovement.isFacingRight = true;
                playerController.playerMovement.Move();
            }

            //---------------------------------------------------------------------------------Input Right
            else if (playerController.playerInput.isLeftPressed)
            {
                playerController.playerMovement.isMoving = true;

                if (playerController.playerMovement.isFacingRight)
                {
                    playerController.speedList.FlipSpeedValues();
                }
                playerController.playerMovement.isFacingRight = false;
                playerController.playerMovement.Move();
            }

            else
            {
                playerController.playerMovement.isMoving = false;
                playerController.playerMovement.IdleStop();
            }
        }
    }
}
