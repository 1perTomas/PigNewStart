using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;
    //facing direction




    //move in interaction left right
    //exit interaction


    internal void PushPull()
    {
        if (playerController.playerInput.isLeftPressed)
        {
            if (playerController.speedList.walkSpeed > 0)
            {
                playerController.speedList.FlipSpeedValues();
            }
            else
            {
                playerController.playerMove.Move();
                playerController.playerState.isMoving = true;
            }
        }

        else if (playerController.playerInput.isRightPressed)
        {
            if (playerController.speedList.walkSpeed < 0)
            {
                playerController.speedList.FlipSpeedValues();
            }
            else
            {
                playerController.playerMove.Move();
                playerController.playerState.isMoving = true;
            }
        }

        else
        {
            playerController.playerMovement.IdleStop();
            playerController.playerState.isMoving = false;
        }
    }
}
