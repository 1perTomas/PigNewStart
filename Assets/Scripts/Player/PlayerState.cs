using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;

    internal Vector2 StandingBox;
    internal Vector2 CrawlingBox;
    internal Vector2 InteractingBox;

    internal bool isFacingRight = true;
    internal bool isMoving;
    internal bool isInteracting = false;
    internal bool isStuckInGround = false;

    internal bool canJump;

    internal int state;


    // Start is called before the first frame update
    void Start()
    {
        StandingBox = new Vector2(0.34f, 0.83f);
        CrawlingBox = new Vector2(0.34f, 0.415f);
    }

    private void BoxColliderFull()
    {
        GetComponent<BoxCollider2D>().size = StandingBox;
        GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.069f);
    }

    private void BoxColliderProne()
    {
        GetComponent<BoxCollider2D>().size = CrawlingBox; // full y size / 2
        GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.2765f); // 0 - (prone y size /2)  + (full y offset /2) 
    }

    internal void AdjustColliderBoxMovable(int direction)
    {
        playerController.GetComponent<BoxCollider2D>().offset = new Vector2(direction * ((playerController.playerState.StandingBox.x + playerController.playerInteraction.interactableObject.GetComponent<SpriteRenderer>().bounds.size.x) / 2 - (playerController.playerState.StandingBox.x / 2)), -0.069f);
        playerController.GetComponent<BoxCollider2D>().size = new Vector2(playerController.playerInteraction.interactableObject.GetComponent<SpriteRenderer>().bounds.size.x + playerController.playerState.StandingBox.x, playerController.playerState.StandingBox.y);
    }

    internal void ColliderAdjust()
    {
        // if (isInteracting)
        // {
        //     if (isFacingRight)
        //     {
        //        StartCoroutine(AdjustColliderBoxMovable(1));
        //     }
        //     else
        //     {
        //         AdjustColliderBoxMovable(1);
        //     }
        // }
        //
        // else
        // {
        if (playerController.playerMovement.isStanding)
        {
            BoxColliderFull();
        }
        else if (!playerController.playerMovement.isStanding)
        {
            BoxColliderProne();
        }
        // }
    }

    internal void SetState()
    {
        if (isInteracting)
        {
            state = 1;
        }

        else if (isStuckInGround)
        {
            state = 2;
        }

        else
        {
            state = 3;
        }

    }
}
