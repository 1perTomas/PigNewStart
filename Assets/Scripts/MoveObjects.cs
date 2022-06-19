using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveObjects : MonoBehaviour
{

    [SerializeField]
    PlayerController playerController;

    public float distance = 1f;
    public LayerMask boxMask;
    //internal bool isInteracting = false;
    internal bool canGrab;
    private float extraSize;
    internal bool pushRight;
    private float interactCount;
    private float interactTime = 0.15f;

    internal string objectType;

    internal RaycastHit2D canInteract;
    private GameObject box;


   // Update is called once per frame
    void Update()
    {
        Physics2D.queriesStartInColliders = false;
        canInteract = playerController.playerSurroundings.isTouchingInteractableObject;
   
        if (playerController.playerSurroundings.isGrounded && canInteract)
        {
            box = playerController.playerSurroundings.interactableObject;
            canGrab = true;
            if (canInteract.collider.CompareTag("MovableObject"))
            {
                objectType = "Movable";
            }
   
            else if (canInteract.collider.CompareTag("CarriableObject"))
            {
                objectType = "Carriable";
            }
        }
        else if ((playerController.playerState.isInteracting && playerController.playerInput.isInteractTapped) || !playerController.playerSurroundings.isGrounded)
        {
            playerController.playerState.isInteracting = false;
   
   
            box.GetComponent<Rigidbody2D>().transform.SetParent(null);
            //box.GetComponent<Rigidbody2D>().position = new Vector2(box.GetComponent<Rigidbody2D>().position.x - 0.15f, box.GetComponent<Rigidbody2D>().position.y);
            //playerController.rb.position = new Vector2(playerController.rb.position.x+0.05f, playerController.rb.position.y + 0.05f);
        }
   
        else
        {
            canGrab = false;
        }
    }
   
   // private void LateUpdate()
   // {
   //
   //     if (playerController.playerState.isInteracting)
   //     {
   //         box.GetComponent<Rigidbody2D>().isKinematic = true;
   //         box.GetComponent<Rigidbody2D>().simulated = false;
   //         box.GetComponent<Rigidbody2D>().transform.SetParent(transform);
   //
   //         if (objectType == "Movable")
   //         {
   //             if (playerController.playerState.isFacingRight)
   //             {
   //                 AdjustColliderBoxMovable(1);
   //             }
   //             else
   //             {
   //                 AdjustColliderBoxMovable(-1);
   //             }
   //         }
   //
   //         else if (objectType == "Carriable")
   //         {
   //             
   //                 box.GetComponent<Rigidbody2D>().transform.localPosition = new Vector2(0, 0.8f);
   //             AdjustColliderBoxCarriable();
   //         }
   //
   //         if (playerController.playerInput.isInteractTapped)
   //         {
   //             LetGo();
   //         }
   //
   //     }
   //
   //     else if (box != null)
   //     {
   //     
   //         box.GetComponent<Rigidbody2D>().simulated = true;
   //         box.GetComponent<Rigidbody2D>().isKinematic = false;
   //         playerController.playerState.isInteracting = false;
   //         // playerController.BoxColliderFull();
   //         // box.GetComponent<Rigidbody2D>().simulated = true;
   //         // 
   //         // box.GetComponent<Rigidbody2D>().transform.SetParent(null);
   //     }
   //
   // }
   
    internal void LetGo()
    {
         box.GetComponent<Rigidbody2D>().simulated = true;
         box.GetComponent<Rigidbody2D>().isKinematic = false;
         playerController.playerState.isInteracting = false;
        box.GetComponent<Rigidbody2D>().transform.SetParent(null);
    }

   // internal void AdjustColliderBoxMovable(int direction)
   // {
   //     playerController.GetComponent<BoxCollider2D>().offset = new Vector2(direction * ((playerController.playerState.StandingBox.x + box.GetComponent<SpriteRenderer>().bounds.size.x) / 2 - (playerController.playerState.StandingBox.x / 2)), -0.069f);
   //     playerController.GetComponent<BoxCollider2D>().size = new Vector2(box.GetComponent<SpriteRenderer>().bounds.size.x + playerController.playerState.StandingBox.x, playerController.playerState.StandingBox.y);
   // }
   //
   // internal void AdjustColliderBoxCarriable()
   // {
   //     playerController.GetComponent<BoxCollider2D>().offset = new Vector2(0.015f, -0.069f + (box.GetComponent<SpriteRenderer>().bounds.size.y) / 2);
   //     playerController.GetComponent<BoxCollider2D>().size = new Vector2(box.GetComponent<SpriteRenderer>().bounds.size.x, box.GetComponent<SpriteRenderer>().bounds.size.y + playerController.playerState.StandingBox.y+0.13f); // figure out last number
   // }

}