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
            //interactableObject = playerController.playerDetectObject.touchingObject;
            PushPull();
        }

        if (playerController.playerDetectObject.objectType == "Carriable")
        {
            //interactableObject = playerController.playerDetectObject.touchingObject;
            PushPull();
        }

        if ((playerController.playerState.isInteracting && playerController.playerInput.isInteractTapped) /*|| !playerController.playerSurroundings.isGrounded*/)
        {
            holding = false;
        }
    }

    internal void PushPull()
    {
        Debug.Log("Pickup");
        if (playerController.playerInput.isLeftPressed)
        {
            if (playerController.speedList.walkSpeed > 0)
            {
                playerController.speedList.FlipSpeedValues();
            }

            else
            {
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
        }
    }

    internal void LetGo() // detaches and returns object properties
    {
        if (!holding && playerController.playerState.isInteracting)
        {
            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().simulated = true;
            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().transform.SetParent(null);
            playerController.playerState.isInteracting = false;
        }
    }

    internal void LockObject()
    {
        interactableObject = playerController.playerDetectObject.touchingObject;
    }

    internal void AdjustColliderBoxMovable(int direction)
    {
       // playerController.GetComponent<BoxCollider2D>().offset = new Vector2(direction * ((playerController.playerState.StandingBox.x + playerController.playerDetectObject.touchingObject.GetComponent<SpriteRenderer>().bounds.size.x) / 2 - (playerController.playerState.StandingBox.x / 2)), -0.069f);
       // playerController.GetComponent<BoxCollider2D>().size = new Vector2(playerController.playerDetectObject.touchingObject.GetComponent<SpriteRenderer>().bounds.size.x + playerController.playerState.StandingBox.x, playerController.playerState.StandingBox.y);
    }

    internal void AdjustColliderBoxCarriable(int direction)
    {
       // // needs another raycast to see if the carriable hits any obstacles, if it does - adjust collider to it's size
       // playerController.GetComponent<BoxCollider2D>().offset = new Vector2(direction * (playerController.playerState.StandingBox.x), (playerController.playerDetectObject.touchingObject.GetComponent<SpriteRenderer>().bounds.size.y) - 0.069f);
       // playerController.GetComponent<BoxCollider2D>().size = new Vector2(playerController.playerState.StandingBox.x, playerController.playerDetectObject.touchingObject.GetComponent<SpriteRenderer>().bounds.size.y + playerController.playerState.StandingBox.y);
    }

}

