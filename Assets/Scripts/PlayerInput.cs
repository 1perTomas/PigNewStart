using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;


    internal bool isLeftPressed;
    internal bool isLeftTapped;
    internal bool isRightPressed;
    internal bool isRightTapped;
    internal bool isDownPressed;
    internal bool isDownTapped;
    internal bool isUpPressed;
    internal bool isUpTapped;
    internal bool isSprintPressed;
    internal bool isSprintReleased;
    internal bool isJumpPressed;
    internal bool isJumpTapped;
    internal bool isJumpReleased;
    internal bool isInteractTapped;

    internal void CheckButtonInput()
    {

        if (!PauseMenu.gameIsPaused)
        {


            //-------------------------------------------------A / Left
            if (Input.GetKey(KeyCode.A))
            {
                isLeftPressed = true;
            }
            else
            {
                isLeftPressed = false;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                isLeftTapped = true;
            }
            else
            {
                isLeftTapped = false;
            }
            //-------------------------------------------------D / Right
            if (Input.GetKey(KeyCode.D))
            {
                isRightPressed = true;
            }
            else
            {
                isRightPressed = false;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                isRightTapped = true;
            }
            else
            {
                isRightTapped = false;
            }
            //-------------------------------------------------S / Down
            if (Input.GetKey(KeyCode.S))
            {
                isDownPressed = true;
            }
            else
            {
                isDownPressed = false;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                isDownTapped = true;
            }
            else
            {
                isDownTapped = false;
            }
            //-------------------------------------------------W / Up
            if (Input.GetKey(KeyCode.W))
            {
                isUpPressed = true;
            }
            else
            {
                isUpPressed = false;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                isUpTapped = true;
            }
            else
            {
                isUpTapped = false;
            }
            //-------------------------------------------------Space / Jump

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumpTapped = true;
            }

            else
            {
                isJumpTapped = false;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                isJumpPressed = true;
            }
            else
            {
                isJumpPressed = false;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumpReleased = true;
            }
            else
            { isJumpReleased = false; }

            //-------------------------------------------------Left Shift / Sprint
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isSprintPressed = true;
                isSprintReleased = false;
            }
            else
            {
                isSprintPressed = false;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isSprintReleased = true;
            }
            //-------------------------------------------------E / Interact
            if (Input.GetKeyDown(KeyCode.E))
            {
                isInteractTapped = true;
            }

            else
            {
                isInteractTapped = false;
            }
        }
    }
}
