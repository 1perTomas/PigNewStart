﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;
    //facing direction

    internal string objectType;
    internal RaycastHit2D canInteract; //brings back an object

    internal bool isHolding;
    internal bool canGrab;
    internal bool isCarrying;
    internal bool isDisengaging;

    internal int continueHolding;

    internal float disengageTimer;
    float angleDegrees;
    float radiusLength;

    internal bool pushingObjectRight;
    internal GameObject interactableObject;
    internal Vector2 originalObjectPosition;



    private void Start()
    {
        angleDegrees = 89f;
        radiusLength = 0.8f;
    }

    void Update()
    {
        // Physics2D.queriesStartInColliders = false;
    }

    private void FixedUpdate()
    {
        Disengage();
        //PutDown();

    }

    internal void Interactions() //
    {
        if (/*playerController.playerSurroundings.isTouchingInteractableObject &&*/ !playerController.playerState.isInteracting)
        {
            //interactableObject = playerController.playerDetectObject.objectItself.collider.gameObject;
        }

       // if (playerController.playerDetectObject.objectType == "Movable")
       // {
       //     //interactableObject = playerController.playerDetectObject.touchingObject;
       //     PushPull();
       // }

        switch (playerController.playerDetectObject.objectTypeTest)
        {
            case PlayerDetectObject.ObjectTypes.Movable:
                PushPull();

                break;

            case PlayerDetectObject.ObjectTypes.Carriable:

                if (playerController.playerInput.isUpTapped && !isCarrying)
                {
                    LiftUp();
                }

                break;
        }

       // if (playerController.playerDetectObject.objectType == "Carriable")
       // {
       //     //interactableObject = playerController.playerDetectObject.touchingObject;
       //     PushPull();
       //
       //     if (playerController.playerInput.isUpTapped && !isCarrying)
       //     {
       //         LiftUp();
       //     }
       //
       //     else if (playerController.playerInput.isDownTapped && isCarrying)
       //     {
       //         isDisengaging = true;
       //         continueHolding = 1;
       //     }
       // }

        if ((playerController.playerState.isInteracting && playerController.playerInput.isInteractTapped) /*|| !playerController.playerSurroundings.isGrounded*/)
        {
            if (isCarrying)
            {
                isDisengaging = true;
                continueHolding = 0;
            }
            else
            {
                isHolding = false;
            }


        }
    }

    //IEnumerator LetGoRaisedObject()
    //{
    //
    //    yield null;
    //}

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

    internal void PutDown() // tidy up
    {
        if (isCarrying)
        {
            if (disengageTimer >= 0.1f)
            {
                isCarrying = false;
                isDisengaging = false;
                //putting down the object
                playerController.playerDetectObject.touchingObject.GetComponent<Rigidbody2D>().transform.localPosition = originalObjectPosition;
                angleDegrees = 89f;

                if (continueHolding == 0)
                {
                    isHolding = false;
                }

                else if (continueHolding == 1)
                {
                    isHolding = true;
                }
            }
        }
    }

    internal void LiftUp()
    {
        if (!isCarrying)
        {
            angleDegrees = 89f;
            continueHolding = 2;
            disengageTimer = 0;
            playerController.playerDetectObject.touchingObject.GetComponent<Rigidbody2D>().transform.localPosition = new Vector2(0, 0.8f);
            isCarrying = true;
        }
    }

    internal void PickUp() //allows the object to be manipulated
    {
        if (!playerController.playerState.isInteracting)
        {
            isHolding = true;
            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().simulated = false;
            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().transform.SetParent(transform);
            originalObjectPosition = playerController.playerDetectObject.touchingObject.GetComponent<Rigidbody2D>().transform.localPosition;
        }
    }

    internal void LetGo() // detaches and returns object properties
    {
        if (!isHolding && playerController.playerState.isInteracting) // check if isInteracting condition needed
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

    internal void Disengage()
    {

        if (isDisengaging)
        {
            //float angleDegrees = 89;
            if (playerController.playerState.isFacingRight)
            {
                angleDegrees -= Time.deltaTime * 800;
            }

            else
            {
                angleDegrees += Time.deltaTime * 800;
            }

            if (radiusLength > 0.57f)
            {
                radiusLength -= Time.deltaTime * 3;
            }

            if (disengageTimer < 0.1f)
            {
                playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().transform.localPosition =
                     new Vector2(radiusLength * Mathf.Cos(ConvertToRadian(angleDegrees)), radiusLength * Mathf.Sin(ConvertToRadian(angleDegrees)));
            }

            else
            {
                playerController.playerDetectObject.touchingObject.GetComponent<Rigidbody2D>().transform.localPosition = originalObjectPosition;
                isDisengaging = false;
                isCarrying = false;

                if (continueHolding == 0)
                {
                    isHolding = false;
                }

                else if (continueHolding == 1)
                {
                    isHolding = true;
                }
            }

            if (angleDegrees > -1f && angleDegrees < 1f)
            {
                Debug.Log(disengageTimer);
                //takes 0.35 time to go from 90 to 0 degrees
            }

            disengageTimer += Time.deltaTime;
        }




        //basically LetGo() with a timegate for animations (4frames?)
    }

    internal float ConvertToRadian(float degrees)
    {
        return degrees * Mathf.PI / 180;
    }
}

