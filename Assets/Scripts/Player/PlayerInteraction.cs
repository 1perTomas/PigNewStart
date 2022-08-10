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


    internal bool isHolding;
    internal bool canGrab;
    internal bool isCarrying;

    internal bool isDisengaging;
    internal bool isEngaging;

    internal int continueHolding;

    internal float disengageTimer;
    internal float engageTimer;
    internal float angleDegrees;
    float radiusLength;

    internal float startRadius;


    internal bool pushingObjectRight;
    internal GameObject interactableObject;
    internal Vector3 originalObjectPosition;



    private void Start()
    {
        radiusLength = 0.8f;
    }


    internal void Interactions() //
    {
        switch (playerController.playerDetectObject.objectTypeTest)
        {
            case PlayerDetectObject.ObjectTypes.Movable:

                if (playerController.playerInput.isInteractTapped)
                {
                    isHolding = false;
                    LetGo();
                }
                else
                {
                    playerController.playerColliders.AdjustColliderBoxMovable();
                    PushPull();
                }

                break;

            case PlayerDetectObject.ObjectTypes.Carriable:

                if (!isCarrying)
                {
                    if (playerController.playerInput.isUpTapped)
                    {
                        disengageTimer = 0;
                        isEngaging = true;
                        StartCoroutine("RaiseUp");
                        // LiftUp();
                    }
                    else if (playerController.playerInput.isInteractTapped && !isEngaging)
                    {
                        //playerController.playerColliders.BoxColliderFull();
                        isHolding = false;
                        LetGo();
                    }
                    else
                    {
                        playerController.playerColliders.AdjustColliderBoxMovable();
                        //StopCoroutine("PutDownLetGo");
                        PushPull();
                    }
                }
                else
                {
                    playerController.playerJump.NewJump();

                    if (playerController.playerInput.isInteractTapped && !isDisengaging)
                    {
                        isDisengaging = true;
                        continueHolding = 0;
                        StartCoroutine("PutDownLetGo");
                    }
                    else if (playerController.playerInput.isDownTapped)
                    {
                        isDisengaging = true;
                        continueHolding = 1;
                        StartCoroutine("PutDownLetGo");
                    }
                    else
                    {
                        //StopCoroutine("RaiseUp");

                        playerController.playerMove.MoveDetection(); // moves too fast
                    }
                }

                break;

            case PlayerDetectObject.ObjectTypes.Lever:

                if (playerController.playerInput.isLeftTapped)
                {

                    //trigger effect left
                }
                else if (playerController.playerInput.isRightTapped)
                {
                    //trigger effect right
                }
                else if (playerController.playerInput.isInteractTapped)
                {
                    playerController.playerState.controlMode = PlayerState.ControlMode.FreeMovement;
                }

                break;
        }
    }

    internal void PushPull()
    {
        Debug.Log("Pickup");
        if (playerController.playerInput.isLeftPressed
            && ((playerController.playerState.isFacingRight && !playerController.playerSurroundings.isTouchingWallBehind)
            || (!playerController.playerState.isFacingRight && !playerController.playerSurroundings.isTouchingWall)))
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

    IEnumerator PutDownLetGo()
    {
        while (isDisengaging)
        {
            Disengage();
            yield return new WaitForFixedUpdate();
        }

        if (!isHolding)
        {
            playerController.playerColliders.BoxColliderFull();
            LetGo();
            yield break;
        }

        yield break;
    }

    IEnumerator RaiseUp()
    {
        Debug.Log("bogos)");

        while (isEngaging)
        {
            Engage();

            // yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(0.1f);
        }

        playerController.playerColliders.AdjustColliderBoxCarriable();
        yield break;
    }

    internal void PutDown() // tidy up
    {
        if (isCarrying)
        {
            // if (disengageTimer >= 0.1f)
            // {
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
            //}
        }
    }

    internal void FindRadius()
    {
        if (playerController.playerState.isFacingRight)
        {
            startRadius = playerController.playerDetectObject.touchingObject.GetComponent<Rigidbody2D>().transform.localPosition.x - playerController.rb.transform.position.x;
            Debug.Log(startRadius);
        }
        else
        {
            startRadius = playerController.rb.transform.position.x - playerController.playerDetectObject.touchingObject.GetComponent<Rigidbody2D>().transform.localPosition.x;
            Debug.Log(startRadius);
        }
    }

    internal void LiftUp()
    {
        if (!isCarrying)
        {
            angleDegrees = 89f;
            continueHolding = 2;
            disengageTimer = 0;
            playerController.playerDetectObject.touchingObject.GetComponent<Rigidbody2D>().transform.localPosition = new Vector3(0, 0.8f, 0);
            isCarrying = true;
        }
    }

    internal void PickUp() //allows the object to be manipulated
    {


        if (!playerController.playerState.isInteracting)
        {
            FindRadius();

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
            playerController.playerColliders.BoxColliderFull();
            playerController.playerState.isInteracting = false;
            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().simulated = true;
            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().transform.SetParent(null);

            playerController.playerState.controlMode = PlayerState.ControlMode.FreeMovement;
        }
    }

    internal void LockObject()
    {
        interactableObject = playerController.playerDetectObject.touchingObject;
    }

    internal void Engage()
    {
        playerController.playerMovement.DisableMovement();

        if (playerController.playerState.isFacingRight)
        {
            //angleDegrees += Time.deltaTime * 800;
            angleDegrees += 90 / 4;

        }

        else if (!playerController.playerState.isFacingRight)
        {
            angleDegrees -= Time.deltaTime * 800;
        }

        if (radiusLength < 0.8f)
        {
            radiusLength += Time.deltaTime * 3;
        }

        if (/*engageTimer < 0.1f*/ angleDegrees < 90)
        {

            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().transform.localPosition =
                 new Vector2(radiusLength * Mathf.Cos(ConvertToRadian(angleDegrees)), radiusLength * Mathf.Sin(ConvertToRadian(angleDegrees)));
        }

        else
        {
            playerController.playerMovement.EnableMovement();
            playerController.playerDetectObject.touchingObject.GetComponent<Rigidbody2D>().transform.localPosition = new Vector3(0, 0.8f, 0); ;
            isEngaging = false;
            angleDegrees = 90;
            isCarrying = true;
            engageTimer = 0;

        }
        engageTimer += Time.deltaTime;

    }

    internal void Disengage()
    {
        playerController.playerMovement.DisableMovement();

        // if (isDisengaging)
        // {
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

        //  if (disengageTimer < 0.1f) 
        if ((playerController.playerState.isFacingRight && angleDegrees > 0) || (!playerController.playerState.isFacingRight && angleDegrees < 180))
        {
            playerController.playerDetectObject.objectItself.collider.gameObject.GetComponent<Rigidbody2D>().transform.localPosition =
                 new Vector2(radiusLength * Mathf.Cos(ConvertToRadian(angleDegrees)), radiusLength * Mathf.Sin(ConvertToRadian(angleDegrees)));
        }

        else
        {
            playerController.playerMovement.EnableMovement();
            //playerController.playerDetectObject.touchingObject.GetComponent<Rigidbody2D>().transform.localPosition = originalObjectPosition;

            isDisengaging = false;
            isCarrying = false;
            disengageTimer = 0;
            playerController.playerColliders.AdjustColliderBoxMovable();

            if (playerController.playerState.isFacingRight)
            {
                playerController.playerDetectObject.touchingObject.GetComponent<Rigidbody2D>().transform.localPosition = new Vector3(+startRadius, -0.109f, 0);
                angleDegrees = 0;
                ContinueHolding();
            }
            else
            {
                playerController.playerDetectObject.touchingObject.GetComponent<Rigidbody2D>().transform.localPosition = new Vector3(-startRadius, -0.109f, 0);
                angleDegrees = 180;
                ContinueHolding();
            }

            // if (continueHolding == 0)
            // {
            //     isHolding = false;
            // }
            //
            // else if (continueHolding == 1)
            // {
            //     isHolding = true;
            // }
        }

        if (angleDegrees > -1f && angleDegrees < 1f)
        {
            Debug.Log(disengageTimer);
            //takes 0.35 time to go from 90 to 0 degrees
        }

        disengageTimer += Time.deltaTime;
        // }

        //basically LetGo() with a timegate for animations (4frames?)
    }

    internal void ContinueHolding()
    {
        if (continueHolding == 0)
        {
            isHolding = false;
        }

        else if (continueHolding == 1)
        {
            isHolding = true;
        }
    }

    internal float ConvertToRadian(float degrees)
    {
        return degrees * Mathf.PI / 180;
    }

    internal void InitiateInteraction()
    {
        PickUp();
        playerController.playerState.isInteracting = true;

        if (playerController.playerState.isFacingRight)
        {
            angleDegrees = 0;
        }

        else
        {
            angleDegrees = 180;
        }
        playerController.playerState.controlMode = PlayerState.ControlMode.Interaction;
        playerController.playerState.currentState = PlayerState.CharacterMovement.Interacting;
    }
}

