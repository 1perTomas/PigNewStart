using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;
    //facing direction

    internal string objectType;
    internal RaycastHit2D canInteract; //brings back an object

    internal bool holding;
    internal bool canGrab;

    internal bool pushingObjectRight;
    internal GameObject interactableObject;

    void Update()
    {
        // Physics2D.queriesStartInColliders = false;
    }

    internal void Interactions() //
    {
        if (/*playerController.playerSurroundings.isTouchingInteractableObject &&*/ !playerController.playerState.isInteracting)
        {
            //interactableObject = playerController.playerDetectObject.objectItself.collider.gameObject;
        }

        if (playerController.playerDetectObject.objectType == "Movable")
        {
            PushPull();
        }

        else if (objectType == "Carriable")
        {
            PushPull();
        }

        if ((playerController.playerState.isInteracting && playerController.playerInput.isInteractTapped) || !playerController.playerSurroundings.isGrounded)
        {
            holding = false;
        }
    }

    //internal void WhatObject()
    //{
    //    if (playerController.playerSurroundings.isTouchingInteractableObject)
    //    {
    //        interactableObject = playerController.playerDetectObject.objectItself.collider.gameObject;
    //    }
    //}

    internal void PushPull()
    {
        if (playerController.playerInput.isLeftPressed) //turn around work around, otherwise cant move in another direction if hitting wall
        {
            pushingObjectRight = false;
        }

        else if (playerController.playerInput.isRightPressed)
        {
            pushingObjectRight = true;
        }

        if (playerController.playerInput.isLeftPressed && !playerController.playerSurroundings.isTouchingWall
            && !playerController.playerSurroundings.isTouchingLedge)
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


        else if (playerController.playerInput.isRightPressed && !playerController.playerSurroundings.isTouchingWall
            && !playerController.playerSurroundings.isTouchingLedge && pushingObjectRight)
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

    internal void PickUp() //allows the object to be manipulated
    {
        if (!playerController.playerState.isInteracting)
        {
            holding = true;
            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().simulated = false;
            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().transform.SetParent(transform);
            playerController.playerSurroundings.wallCheckDistance = 2.21f;
        }
    }

    internal void LetGo() // detaches and returns object properties
    {
        if (!holding && playerController.playerState.isInteracting)
        {
            // if (interactableObject != null && !holding && playerController.playerState.isInteracting)
            // {
            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().simulated = true;
            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().transform.SetParent(null);
            playerController.playerState.isInteracting = false;
            playerController.playerSurroundings.wallCheckDistance = 0.21f;
            // }}
        }

    }

}

