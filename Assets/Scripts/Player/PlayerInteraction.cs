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
    internal bool isCarrying;
    internal bool isDisengaging;

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
        PutDown();
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

            if (playerController.playerInput.isUpTapped && !isCarrying)
            {
                LiftUp();
            }

            else if (playerController.playerInput.isDownTapped && isCarrying)
            {
                PutDown();
            }
        }

        if ((playerController.playerState.isInteracting && playerController.playerInput.isInteractTapped) /*|| !playerController.playerSurroundings.isGrounded*/)
        {
            if (isCarrying)
            {
                isDisengaging = true;
               // if (disengageTimer >= 0.1f)
               // {
               //     PutDown();
               //     holding = false;
               //     isDisengaging = false;
               // }
            }

            
        }

        else if ((playerController.playerState.isInteracting && playerController.playerInput.isSprintPressed))
        {
            Disengage();
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

    internal void PutDown() // tidy up
    {
        if (isCarrying)
        {
            if (disengageTimer >= 0.1f)
            {

                holding = false;
                isDisengaging = false;


                //putting down the object
                playerController.playerDetectObject.touchingObject.GetComponent<Rigidbody2D>().transform.localPosition = originalObjectPosition;

                isCarrying = false;
                disengageTimer = 0;
                angleDegrees = 89f;
            }
        }
    }

    internal void LiftUp()
    {
        if (!isCarrying)
        {
            playerController.playerDetectObject.touchingObject.GetComponent<Rigidbody2D>().transform.localPosition = new Vector2(0, 0.8f);
            isCarrying = true;
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
            originalObjectPosition = playerController.playerDetectObject.touchingObject.GetComponent<Rigidbody2D>().transform.localPosition;
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
            //float decreasingDegrees;

            if (disengageTimer < 0.1f)
            {
                playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().transform.localPosition =
                     new Vector2(radiusLength * Mathf.Cos(ConvertToRadian(angleDegrees)), radiusLength * Mathf.Sin(ConvertToRadian(angleDegrees)));
                //new Vector2(radiusLength * Mathf.Cos(angleDegrees * Mathf.PI / 180), radiusLength * Mathf.Sin(angleDegrees * Mathf.PI / 180));
            }

            if (angleDegrees > -1f && angleDegrees < 1f)
            {
                Debug.Log(disengageTimer);
                //takes 0.35 time to go from 90 to 0 degrees
            }

            //Vector2 newPosition2 = new Vector2(0.8f * Mathf.Cos(angleDegrees * Mathf.PI / 180), 0.8f * Mathf.Sin(angleDegrees * Mathf.PI / 180));

            //Debug.Log(0.8f * Mathf.Sin((89*Mathf.PI/180)) + "+" + 0.8f * Mathf.Cos(89 * Mathf.PI / 180));
            //Vector2 newPosition = new Vector2(0.8f * Mathf.Sin(90), 0.8f * Mathf.Cos(90));

            disengageTimer += Time.deltaTime;
        }


        //basically LetGo() with a timegate for animations (4frames?)
    }

    internal float ConvertToRadian (float degrees)
    {
        return degrees * Mathf.PI / 180;
    }
}

