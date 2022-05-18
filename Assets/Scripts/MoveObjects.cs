using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveObjects : MonoBehaviour
{

    [SerializeField]
    PlayerController playerController;

    public float distance = 1f;
    public LayerMask boxMask;
    internal bool isInteracting = false;
    internal bool canGrab;
    private float extraSize;
    internal bool pushRight;
    private float interactCount;
    private float interactTime = 0.15f;

    RaycastHit2D canInteract;
    private GameObject box;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.queriesStartInColliders = false;
        canInteract = playerController.playerSurroundings.isTouchingMovableObject;

        if (playerController.playerSurroundings.isGrounded && canInteract && canInteract.collider.CompareTag("MovableObject"))
        {
            box = canInteract.collider.gameObject;

            if (!isInteracting
                && playerController.playerInput.isInteractTapped
                && playerController.playerMovement.isStandingNew
                && !playerController.playerInput.isJumpPressed) // can't jump and drag object upwards
            {

                isInteracting = true;

                if (playerController.playerMovement.isFacingRight)
                {
                    pushRight = true;
                }

                else
                {
                    pushRight = false;
                }
            }
        }
        else if (isInteracting && playerController.playerInput.isInteractTapped)
        {
            isInteracting = false;
            //playerController.playerMovement.isInteracting = false;

        }
    }

    private void LateUpdate()
    {

        if (isInteracting)
        {
            //playerController.playerMovement.isInteracting = true;
            box.GetComponent<Rigidbody2D>().isKinematic = true;
            box.GetComponent<Rigidbody2D>().simulated = false;
            box.GetComponent<Rigidbody2D>().transform.SetParent(transform);

            if (pushRight)
            {

                playerController.GetComponent<BoxCollider2D>().offset = new Vector2((playerController.playerState.StandingBox.x + box.GetComponent<SpriteRenderer>().bounds.size.x) / 2 - (playerController.playerState.StandingBox.x / 2), -0.069f);
                playerController.GetComponent<BoxCollider2D>().size = new Vector2(box.GetComponent<SpriteRenderer>().bounds.size.x + playerController.playerState.StandingBox.x, playerController.playerState.StandingBox.y);

            }
            else
            {

                playerController.GetComponent<BoxCollider2D>().offset = new Vector2(-((playerController.playerState.StandingBox.x + box.GetComponent<SpriteRenderer>().bounds.size.x) / 2 - (playerController.playerState.StandingBox.x / 2)), -0.069f);
                playerController.GetComponent<BoxCollider2D>().size = new Vector2((box.GetComponent<SpriteRenderer>().bounds.size.x + playerController.playerState.StandingBox.x), playerController.playerState.StandingBox.y);

            }

        }

        else if (box != null)
        {
            // box.GetComponent<Rigidbody2D>().isKinematic = false;
            //playerController.playerMovement.isInteracting = false;
            box.GetComponent<Rigidbody2D>().simulated = true;
            box.GetComponent<Rigidbody2D>().transform.SetParent(null);
        }

    }


}