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
                if (playerController.playerMovement.isFacingRight)
                {
                    playerController.speedList.FlipSpeedValues();
                }
                playerController.playerMovement.isFacingRight = false;
                playerController.playerMovement.Move();
            }

            //---------------------------------------------------------------------------------Input Right
            else if (playerController.playerInput.isRightPressed)
            {
                if (!playerController.playerMovement.isFacingRight)
                {
                    playerController.speedList.FlipSpeedValues();
                }
                playerController.playerMovement.isFacingRight = true;
                playerController.playerMovement.Move();
            }

            else
            {
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
                if (!playerController.playerMovement.isFacingRight)
                {
                    playerController.speedList.FlipSpeedValues();
                }
                playerController.playerMovement.isFacingRight = true;
                playerController.playerMovement.Move();
            }

            //---------------------------------------------------------------------------------Input Right
            else if (playerController.playerInput.isLeftPressed)
            {
                if (playerController.playerMovement.isFacingRight)
                {
                    playerController.speedList.FlipSpeedValues();
                }
                playerController.playerMovement.isFacingRight = false;
                playerController.playerMovement.Move();
            }

            else
            {
                playerController.playerMovement.IdleStop();
            }
        }
    }
}
